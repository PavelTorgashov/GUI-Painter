using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class FloatHelper
    {
        public static bool Around(this float a, float b)
        {
            return Math.Abs(a - b) < 0.0001f;
        }
    }
}
