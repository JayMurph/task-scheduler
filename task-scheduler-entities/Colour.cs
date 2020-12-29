using System;
using System.Collections.Generic;
using System.Text;

namespace task_scheduler_entities {
    public struct Colour {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Colour(byte r, byte g, byte b, byte a) {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
    }
}
