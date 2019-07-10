using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridTableBuilder.GridModel
{
    public interface ISelectable
    {
        int Priority { get; }
        bool IsHit(Point mouseLocation);
    }
}
