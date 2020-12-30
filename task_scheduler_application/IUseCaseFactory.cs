namespace task_scheduler_application {
    public interface IUseCaseFactory<T> where T : class{
        T New(); 
    }
}
