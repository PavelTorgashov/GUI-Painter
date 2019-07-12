using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridTableBuilder.GridModel.GUI
{
    /// <summary> Grid with GUI graphics elements </summary>
    public class GuiBuilder
    {
        Grid grid;
        public ISelectable Selected;

        public GuiBuilder(Grid grid)
        {
            this.grid = grid;
        }

        public IEnumerable<object> AllElements
        {
            get
            {
                foreach (var elem in grid.AllElements)
                    yield return elem;

                foreach (var track in grid.Nodes.Select(n => n.OriginalLocation.X).Distinct().Select(x => new TrackLine(grid, x, false)).OrderBy(t => t.Location))
                    yield return track;

                foreach (var track in grid.Nodes.Select(n => n.OriginalLocation.Y).Distinct().Select(y => new TrackLine(grid, y, true)).OrderBy(t => t.Location))
                    yield return track;

                yield return new SizeMarker(grid, new PointF(grid.Nodes.Max(n => n.OriginalLocation.X) + 10, grid.Nodes.Max(n => n.OriginalLocation.Y) + 10));
                yield return new TrackLinesIntervalLabels(grid);
            }
        }
    }
}
