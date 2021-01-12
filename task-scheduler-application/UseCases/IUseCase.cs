namespace task_scheduler_application.UseCases {
    public interface IUseCase<in T, out U> { 
        T Input { set; }
        U Output { get; }
        void Execute();
    }
}
