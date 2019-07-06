using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostEffectTest
{
    /// <summary>
    /// Base class for effects (shadow, glow, bevel, etc)
    /// </summary>
    public abstract class BaseEffect
    {
        public abstract int Priority { get; }
        public abstract void Render(Graphics gr, GraphicsPath path);
    }
}
