using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_application {
    class StringValueAttribute : Attribute{
        public string Value { get; protected set; }
        public StringValueAttribute(string value) {
            Value = value;
        }
    }
}
