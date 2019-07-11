using GridTableBuilder.Controls;
using System.Windows.Forms;

namespace GridTableBuilder.GridModel
{
    interface IDragger
    {
        void Start(MouseController mc);
        Cursor Cursor { get; }
    }
}
