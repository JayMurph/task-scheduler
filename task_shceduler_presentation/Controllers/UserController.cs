using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UC = task_scheduler_application.UseCases;
using task_scheduler_application.Frequencies;
using task_scheduler_application.DTO;
using task_scheduler_presentation.Views;

namespace task_scheduler_presentation.Controllers {
    public class UserController {
        private UC.CreateTask.CreateTaskUseCaseFactory CreateTaskUseCaseFactory;
        private UC.ViewTasks.ViewTasksUseCaseFactory ViewTasksUseCaseFactory;

        //this mapping should probably go elsewhere
        private Dictionary<string, FrequencyTypes> stringToFrequencyMap =
            new Dictionary<string, FrequencyTypes>() {
                { "Daily", FrequencyTypes.Daily},
                { "Every Other Day", FrequencyTypes.Every_Other_Day},
                { "Review", FrequencyTypes.Review},
                { "Custom", FrequencyTypes.Custom},
            };

        public IEnumerable<string> FrequencyTypeStrings { get => stringToFrequencyMap.Keys; }

        public UserController(
            UC.CreateTask.CreateTaskUseCaseFactory createTaskUseCaseFactory,
            UC.ViewTasks.ViewTasksUseCaseFactory viewTasksUseCaseFactory
            ) {
            this.CreateTaskUseCaseFactory = createTaskUseCaseFactory;
            this.ViewTasksUseCaseFactory = viewTasksUseCaseFactory;
        }

        public void ViewTasks(ITasksView view) {

            //create view tasks use case, pass input, execute, then get output
            var uc = ViewTasksUseCaseFactory.New();

            uc.Execute();

            //could maybe hook the view up to the TaskAdded callback here???????????????

            //apply the output to the view
            foreach(TaskItemDTO item in uc.Output.TaskItems) {
                view.TaskItems.Add(new TaskItemModel() { Title = item.Title });
            }
        }

        public void CreateTask(IAddTaskView view) {

            //get and VALIDATE input values from view
            UC.CreateTask.CreateTaskInput input = new UC.CreateTask.CreateTaskInput {
                Title = view.Title,
                Description = view.Description,
                StartTime = view.StartTime,
                R = view.Color.R,
                G = view.Color.G,
                B = view.Color.B,
                FrequencyType = stringToFrequencyMap[view.FrequencyType]
            };

            //check for "Custom" frequency and handle that
            if(input.FrequencyType == FrequencyTypes.Custom) {
                input.CustomFrequency = view.CustomFrequency;
            }

            //create UseCase instance and assign input structure to its input port
            var uc = App.UserController.CreateTaskUseCaseFactory.New();
            uc.Input = input;

            //run the use case
            uc.Execute();

            //get Use Case output and handle errors
            UC.CreateTask.CreateTaskOutput output = uc.Output;

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
