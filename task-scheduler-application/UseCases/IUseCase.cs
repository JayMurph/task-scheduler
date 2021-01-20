namespace task_scheduler_application.UseCases {
    /// <summary>
    /// Required components for a generic Use-Case class; takes an input, performs and operation,
    /// and produces an output.
    /// </summary>
    /// <typeparam name="T">A type that the Use-Case uses as input data</typeparam>
    /// <typeparam name="U">A type that the Use-Case uses as output data</typeparam>
    public interface IUseCase<in T, out U> { 
        U Execute(T input);
    }
}
