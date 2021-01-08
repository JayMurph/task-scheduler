﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using task_scheduler_application.UseCases.CreateTask;
using task_scheduler_application.UseCases.ViewTasks;
using task_scheduler_application.DTO;

using task_scheduler_presentation.Views;
using task_scheduler_presentation.Models;

namespace task_scheduler_presentation.Controllers {
    public class UserController {
        private CreateTaskUseCaseFactory CreateTaskUseCaseFactory;
        private ViewTasksUseCaseFactory ViewTasksUseCaseFactory;

        //protected event EventHandler<TaskItemModel> TaskCreated;
        //protected void OnTaskCreated(TaskItemModel taskItem) {
        //    TaskCreated?.Invoke(this, taskItem);
        //}

        //TODO : abstract these strings out of the presentation layer
        public IEnumerable<string> FrequencyTypeStrings { 
            get => new List<string>(){ "Daily", "Every Other Day", "Review", "Custom"};
        }

        public UserController(
            CreateTaskUseCaseFactory createTaskUseCaseFactory,
            ViewTasksUseCaseFactory viewTasksUseCaseFactory
            ) {
            CreateTaskUseCaseFactory = createTaskUseCaseFactory;
            ViewTasksUseCaseFactory = viewTasksUseCaseFactory;
        }

        public void ViewTasks(ITasksView view) {

            //create view tasks use case, pass input, execute, then get output
            var uc = ViewTasksUseCaseFactory.New();

            uc.Execute();

            //add taskItemDTOs from UseCase to observable collection for view
            foreach(TaskItemDTO taskItemDTO in uc.Output.TaskItems) {

                TaskItemModel taskItemModel = new TaskItemModel() {
                    Title = taskItemDTO.Title,
                    Desciption = taskItemDTO.Description,
                    FrequencyType = taskItemDTO.FrequencyType,
                    NotificationFrequency = taskItemDTO.CustomFrequency,
                    StartTime = taskItemDTO.StartTime,
                    Color = new Windows.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, taskItemDTO.R, taskItemDTO.G, taskItemDTO.B))
                };

                view.TaskItems.Add(taskItemModel);
            }
        }

        public void CreateTask(IAddTaskView view) {

            //get and VALIDATE input values from view
            CreateTaskInput input = new CreateTaskInput {
                Title = view.Title,
                Description = view.Description,
                StartTime = view.StartTime,
                R = view.Color.R,
                G = view.Color.G,
                B = view.Color.B,
                FrequencyType = view.FrequencyType
            };

            //check for "Custom" frequency and handle that
            //TODO : abstract away 'magic' string
            if(input.FrequencyType == "Custom") {
                input.CustomFrequency = view.CustomFrequency;
            }

            //create UseCase instance and assign input structure to its input port
            var uc = App.UserController.CreateTaskUseCaseFactory.New();
            uc.Input = input;

            //run the use case
            uc.Execute();

            //get Use Case output and handle errors
            CreateTaskOutput output = uc.Output;

            if (output.Success) {
                //need to close the view from here somehow, and clear the values
                view.CloseSelf();
            }
            else {
                view.Error = output.Error;
            }
        }
    }
}
