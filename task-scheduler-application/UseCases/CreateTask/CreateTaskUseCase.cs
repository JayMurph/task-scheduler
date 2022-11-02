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
        private readonly BasicTaskManager taskManager;

        /// <summary>
        /// Receives and maintains Notifications produced by TaskItem's. Required for the
        /// construction of a new TaskItem.
        /// </summary>
        private readonly BasicNotificationManager notificationManager;

        /// <summary>
        /// Retrieves the current time. Required for the creation of a new TaskItem.
        /// </summary>
        private readonly IClock clock;

        /// <summary>
        /// Produces <see cref="TaskItemRepository"/>s, allowing the Use-Case to add newly created
        /// TaskItems to a database
        /// </summary>
        private readonly ITaskItemRepositoryFactory taskItemRepositoryFactory;

        #endregion 

        #region AddTaskUseCase Constructor

        public CreateTaskUseCase(
            BasicTaskManager taskManager,
            BasicNotificationManager notificationManager,
            IClock clock,
            ITaskItemRepositoryFactory taskItemRepositoryFactory) {

            this.taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            this.notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.taskItemRepositoryFactory = taskItemRepositoryFactory ?? throw new ArgumentNullException(nameof(taskItemRepositoryFactory));
        }

        #endregion


        public CreateTaskOutput Execute(CreateTaskInput input) {

            if (input is null) {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsValid()) {
                //create new Domain TaskItem from the supplied input data
                TaskItem newTask = InputToTaskItem(input);

                if (taskManager.Add(newTask)) {

                    ITaskItemRepository taskItemRepo = taskItemRepositoryFactory.New();

                    //Create a TaskItemDAL to save to the database
                    TaskItemDAL taskItemDAL = TaskItemAndInputToDAL(newTask, input);

                    //add the new TaskItemDAL to the database
                    if(taskItemRepo.Add(taskItemDAL)) {

                        //save the changed make to the TaskItemRepository
                        if (taskItemRepo.Save()) {
                            //create DTO to return as Output data
                            TaskItemDTO taskItemDTO = InputToTaskItemDTO(input);

                            //fill output data and return
                            return new CreateTaskOutput { Success = true , TaskItemDTO = taskItemDTO };
                        }
                        else {
                            //failed to save state of repository
                            //remove taskItem from domain TaskManager
                            if (!taskManager.Remove(newTask)) {
                                //TaskItem could not be removed. we're now screwed . . .
                                //TODO: decide what to do here
                            }

                            return new CreateTaskOutput { Success = false, Error = "Unable to save the new Task."  };
                        }
                    }
                    else {
                        //failed to save task to repository
                        //remove taskItem from domain TaskManager
                        if (!taskManager.Remove(newTask)) {
                            //TaskItem could not be removed. we're now screwed . . .
                            //TODO: decide what to do here
                        }

                        return new CreateTaskOutput {  Success = false, Error = "Unable to save the new Task." };
                    }
                }
                else {
                    //unable to add new TaskItem to domain TaskManager
                    return new CreateTaskOutput { Success = false, Error = "Unable to process the new Task." };
                }
            }
            else {
                //Input is not valid
                return new CreateTaskOutput { Success = false, Error = input.GetErrorMessage() };
            }

        }

        /// <summary>
        /// Converts the fields of the CreateTaskUseCase's Input property to a TaskItem and returns
        /// it
        /// </summary>
        /// <returns>
        /// TaskItem created with the data contained in the CreateTaskUseCase's Input property
        /// </returns>
        private TaskItem InputToTaskItem(CreateTaskInput input) {

            INotificationFrequency frequency = NotificationFrequencyFactory.New(input.NotificationFrequencyType, input.CustomNotificationFrequency);

            return new TaskItem(
                input.Title,
                input.Description,
                new task_scheduler_entities.Colour(
                    input.R, input.G, input.B
                ),
                input.StartTime,
                notificationManager,
                frequency,
                clock
            );
        }

        /// <summary>
        /// Converts a CreateTaskInput object into a TaskItemDTO
        /// </summary>
        /// <returns>
        /// TaskItemDTO created from a CreateTaskInput object
        /// </returns>
        private static TaskItemDTO InputToTaskItemDTO(CreateTaskInput input) {

            return new TaskItemDTO {
                Title = input.Title,
                Description = input.Description,
                StartTime = input.StartTime,
                CustomNotificationFrequency = input.CustomNotificationFrequency,
                NotificationFrequencyType = input.NotificationFrequencyType,
                R = input.R,
                G = input.G,
                B = input.B
            };
        }

        /// <summary>
        /// Creates a TaskItemDAL from a TaskItem and CreateTaskInput
        /// </summary>
        /// <param name="taskItem">
        /// Will have its data used to create a TaskItemDAL
        /// </param>
        /// <param name="input">
        /// </param>
        /// <returns>
        /// TaskItemDAL created from the data of the taskItem parameter
        /// </returns>
        /*
         * TODO: this should probably end up in a conversion class, of some sort, in the application
         * layer
         */
        private TaskItemDAL TaskItemAndInputToDAL(TaskItem taskItem, CreateTaskInput input) {

            Maybe<CustomNotificationFrequencyDAL> notificationFrequency = Maybe<CustomNotificationFrequencyDAL>.CreateEmpty();

            //if the new task should have a custom notification frequency
            if (input.NotificationFrequencyType == NotificationFrequencyType.Custom) {
                //create a custom notification frequency DAL
                notificationFrequency = Maybe<CustomNotificationFrequencyDAL>.Create(
                    new CustomNotificationFrequencyDAL(
                        taskItem.ID,
                        input.CustomNotificationFrequency
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
                (int)input.NotificationFrequencyType
            );

        }
    }
}
