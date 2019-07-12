using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridTableBuilder.GridModel
{
    public interface IDrawable
    {
        int Priority { get; }
        void Draw(Graphics gr, DrawParams ps);
    }

    public class DrawParams
    {
        public bool IsSelected;
        public bool IsEditMode;
    }
}
