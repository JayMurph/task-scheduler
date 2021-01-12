namespace task_scheduler_application.UseCases {
    public interface IUseCaseFactory<IUseCase> {
        IUseCase New(); 
    }
}
