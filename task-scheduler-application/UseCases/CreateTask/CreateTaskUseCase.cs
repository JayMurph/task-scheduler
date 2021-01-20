using System;
using task_scheduler_application.DTO;
using task_scheduler_application.NotificationFrequencies;
using task_scheduler_data_access.DataObjects;
using task_scheduler_data_access.Repositories;
using task_scheduler_entities;
using task_scheduler_utility;

namespace task_scheduler_application.UseCases.CreateTask {
    /// <summary>
    /// Encapsulates the logic required for creating a new TaskItem
    /// </summary>
    public class CreateTaskUseCase : IUseCase<CreateTaskInput, CreateTaskOutput> {

        #region Properties
        /// <summary>
        /// Holds all currently active TaskItem's in the application. After the Use-Case creates a
        /// new TaskItem, it will give it to the taskManager.
        /// </summary>
        private readonly ITaskManager taskManager;

        /// <summary>
        /// Receives and maintains Notifications produced by TaskItem's. Required for the
        /// construction of a new TaskItem.
        /// </summary>
        private readonly INotificationManager notificationManager;

        /// <summary>
        /// Retrieves the current time. Required for the creation of a new TaskItem.
        /// </summary>
        private readonly IClock clock;

        /// <summary>
        /// Produces <see cref="TaskItemRepository"/>s, allowing the Use-Case to add newly created
        /// TaskItems to a database
        /// </summary>
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        public CreateTaskInput Input { set; private get; } = null;

        public CreateTaskOutput Output { get; private set; } = null;

        #endregion 

        #region AddTaskUseCase Constructor

        public CreateTaskUseCase(
            ITaskManager taskManager,
            INotificationManager notificationManager,
            IClock clock,
            ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        #endregion


        public void Execute() {

            if(Input == null) {
                Output = new CreateTaskOutput { Success = false, Error = "No input data found for creating a new Task" };
                return;
            }

            //validate the Input data
            if (!Input.IsValid()) {
                Output = new CreateTaskOutput { Success = false , Error = Input.GetErrorMessage()};
                return;
            }

            //create new Domain TaskItem from the supplied Input data
            TaskItem newTask = GetInputAsTaskItem();

            //add task to domain TaskManager
            if (!taskManager.Add(newTask)) {
                //unable to add new TaskItem to domain TaskManager
                //fill output data and return
                Output = new CreateTaskOutput { Success = false, Error = "Unable to process the new Task." };
                return;
            }

            //Create a TaskItemDAL to save to the database
            TaskItemDAL taskItemDAL = ConvertTaskItemToDAL(newTask);

            ITaskItemRepository taskItemRepo = taskItemRepositoryFactory.New();

            //add the new TaskItemDAL to the database
            if(!taskItemRepo.Add(taskItemDAL)) {
                //remove taskItem from domain TaskManager
                if (!taskManager.Remove(newTask)) {
                    //TaskItem could not be removed. we're now screwed . . .
                    //TODO: decide what to do here
                }

                //create error Output
                Output = new CreateTaskOutput { Success = false, Error = "Unable to save the new Task." };
                return;
            }

            //save the changed make to the TaskItemRepository
            if (!taskItemRepo.Save()) {
                //remove taskItem from domain TaskManager
                if (!taskManager.Remove(newTask)) {
                    //TaskItem could not be removed. we're now screwed . . .
                    //TODO: decide what to do here
                }

                //create error Output
                Output = new CreateTaskOutput { Success = false, Error = "Unable to save the new Task." };
                return;
            }

            //create DTO to return as Output data
            TaskItemDTO taskItemDTO = GetInputAsTaskItemDTO();

            //fill output data and return
            Output = new CreateTaskOutput { Success = true , TaskItemDTO = taskItemDTO };
        }

        /// <summary>
        /// Converts the fields of the CreateTaskUseCase's Input property to a TaskItem and returns
        /// it
        /// </summary>
        /// <returns>
        /// TaskItem created with the data contained in the CreateTaskUseCase's Input property
        /// </returns>
        private TaskItem GetInputAsTaskItem() {

            INotificationFrequency frequency = NotificationFrequencyFactory.New(Input.NotificationFrequencyType, Input.CustomNotificationFrequency);

            return new TaskItem(
                Input.Title,
                Input.Description,
                new task_scheduler_entities.Colour(
                    Input.R, Input.G, Input.B
                ),
                Input.StartTime,
                notificationManager,
                frequency,
                clock
            );
        }

        /// <summary>
        /// Converts the fields ofo the CreateTaskUseCase's Input property to a TaskItemDTO then
        /// returns it
        /// </summary>
        /// <returns>
        /// TaskItemDTO created from the data contained in the CreateTaskUseCase's Input property
        /// </returns>
        private TaskItemDTO GetInputAsTaskItemDTO() {

            return new TaskItemDTO {
                Title = Input.Title,
                Description = Input.Description,
                StartTime = Input.StartTime,
                CustomNotificationFrequency = Input.CustomNotificationFrequency,
                NotificationFrequencyType = Input.NotificationFrequencyType,
                R = Input.R,
                G = Input.G,
                B = Input.B
            };
        }

        /// <summary>
        /// Creates a TaskItemDAL from a TaskItem 
        /// </summary>
        /// <param name="taskItem">
        /// Will have its data used to create a TaskItemDAL
        /// </param>
        /// <returns>
        /// TaskItemDAL created from the data of the taskItem parameter
        /// </returns>
        /*
         * TODO: this should probably end up in a conversion class, of some sort, in the application
         * layer
         */
        private TaskItemDAL ConvertTaskItemToDAL(TaskItem taskItem) {

            Maybe<CustomNotificationFrequencyDAL> notificationFrequency = Maybe<CustomNotificationFrequencyDAL>.CreateEmpty();

            //if the new task should have a custom notification frequency
            if (Input.NotificationFrequencyType == NotificationFrequencyType.Custom) {
                //create a custom notification frequency DAL
                notificationFrequency = Maybe<CustomNotificationFrequencyDAL>.Create(
                    new CustomNotificationFrequencyDAL(
                        taskItem.ID,
                        Input.CustomNotificationFrequency
                    )
                );
            }

            return new TaskItemDAL(
                taskItem.ID,
                taskItem.Title,
                taskItem.Description,
                taskItem.StartTime,
                taskItem.LastNotificationTime,
                taskItem.Colour.R,
                taskItem.Colour.G,
                taskItem.Colour.B,
                notificationFrequency,
                (int)Input.NotificationFrequencyType
            );

        }
    }
}
