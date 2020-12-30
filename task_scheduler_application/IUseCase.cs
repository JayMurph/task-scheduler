namespace task_scheduler {
    public interface IUseCase<in T, out U> where T : class where U : class{
        T Input { set; }
        U Output { get; }
        void Execute();
    }
}
