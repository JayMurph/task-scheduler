namespace task_scheduler {
    interface IUseCaseFactory<T> where T : class{
        T New(); 
    }
}
