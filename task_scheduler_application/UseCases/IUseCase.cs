namespace task_scheduler_application.UseCases {
    public interface IUseCase<in T, out U> where T : class where U : class{
        T Input { set; }
        U Output { get; }
        void Execute();
    }
}
