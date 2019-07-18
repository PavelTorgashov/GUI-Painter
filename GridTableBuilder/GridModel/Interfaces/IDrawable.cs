using System.Drawing;

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
        public bool IsPreview;
        public Color ServiceLineColor;
        public Color DrawLineColor;
        public Color SelectedLineColor;
    }
}
