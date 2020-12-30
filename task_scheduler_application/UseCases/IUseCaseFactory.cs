namespace task_scheduler_application.UseCases {
    public interface IUseCaseFactory<T> where T : class{
        T New(); 
    }
}
