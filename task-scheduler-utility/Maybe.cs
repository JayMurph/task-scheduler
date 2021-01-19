using System;

namespace task_scheduler_utility {
    public class Maybe<T> {

        private readonly T t;

        public bool HasValue { get; private set; }
        public T Value { get => t; }

        private Maybe(T t) {
            this.t = t;
            HasValue = true;
        }

        private Maybe() {

        }

        public static Maybe<T> Create(T t) {
            return new Maybe<T>(t);
        }

        public static Maybe<T> CreateEmpty() {
            return new Maybe<T>();            
        }
    }
}
