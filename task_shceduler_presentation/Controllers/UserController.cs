using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task_scheduler_application.UseCases.CreateTask;
using task_scheduler_application.Frequencies;

namespace task_scheduler_presentation.Controllers {
    public class UserController {
        private CreateTaskUseCaseFactory CreateTaskUseCaseFactory;

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
            CreateTaskUseCaseFactory createTaskUseCaseFactory
            ) {
            this.CreateTaskUseCaseFactory = createTaskUseCaseFactory;
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
                FrequencyType = stringToFrequencyMap[view.FrequencyType]
            };

            //check for "Custom" frequency and handle that
            //abstract away the magic "Custom" string
            if(input.FrequencyType == FrequencyTypes.Custom) {
                input.CustomFrequency = view.CustomFrequency;
            }

            //create UseCase instance and assign input structure to its input port
            var uc = App.UserController.CreateTaskUseCaseFactory.New();
            uc.Input = input;

            //run the use case
            uc.Execute();

            //get Use Case output and handle errors
            CreateTaskOutput output = uc.Output;
            if (!output.Success) {
                view.Error = output.Error;
            }
        }
    }
}
