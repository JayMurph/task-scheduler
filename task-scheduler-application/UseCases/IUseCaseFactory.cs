namespace task_scheduler_application.UseCases {
    /// <summary>
    /// Required components for a factory that produces Use-Cases. 
    /// The factory is intended to be initialized with, and store, the dependencies needed for the
    /// Use-Case it will produce.
    /// </summary>
    /// <typeparam name="IUseCase">The type of Use-Case to be produced by the factory</typeparam>
    public interface IUseCaseFactory<IUseCase> {
        IUseCase New(); 
    }
}
