namespace task_scheduler {
    public interface IUseCaseFactory<T> where T : class{
        T New(); 
    }
}
