﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Grid_Model_old
{
    public class PointNode
    {
        public Point Location { get; set; }
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public int Index { get; set; }

        public PointNode(Point offset)
        {
            Location = offset;
        }

        public bool IsEmpty
        {
            get { return Edges.Count == 0; }
        }

        /// <summary>
        /// Проходная узловая точка на линии рёбер
        /// </summary>
        public bool IsAnadromous
        {
            get
            {
                var verticals = Edges.Count(x => x.IsVertical);
                var horizontals = Edges.Count(x => x.IsHorizontal);
                return !(verticals > 0 && horizontals > 0);
            }
        }

        public override string ToString()
        {
            return $"p{Index}";
        }
    }
}
