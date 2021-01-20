namespace task_scheduler_utility {

    /// <summary>
    /// Holds a reference or value type that may or may not be initialized.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Maybe<T> {

        /// <summary>
        /// Indicates if the Maybe has been initialized with a value
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Returns the object that the Maybe holds
        /// </summary>
        public T Value { get; private set; }

        private Maybe(T value) {
            Value = value;
            HasValue = true;
        }

        private Maybe() {
            HasValue = false;
        }

        /// <summary>
        /// Creates and initializes the Maybe with a value
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns></returns>
        public static Maybe<T> Create(T value) {
            //TODO: test for equality to null, in the case of a reference type
            //if(Object.ReferenceEquals(data, null)){ } ??
            return new Maybe<T>(value);
        }

        /// <summary>
        /// Returns an empty Maybe
        /// </summary>
        /// <returns></returns>
        public static Maybe<T> CreateEmpty() {
            return new Maybe<T>();
        }
    }
}