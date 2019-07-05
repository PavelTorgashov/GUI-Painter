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
        public int Priority { get; protected set; } = -1;

        public abstract void Render(Graphics gr, GraphicsPath path);
    }
}
