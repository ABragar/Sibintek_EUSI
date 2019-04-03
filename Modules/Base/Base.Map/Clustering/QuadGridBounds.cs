using Base.Map.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Clustering
{
    internal class QuadGridBounds
    {
        public readonly QuadGridCell MinCell;
        public readonly QuadGridCell MaxCell;

        public readonly Bounds Bounds;

        public readonly int XSize;
        public readonly int YSize;

        private List<QuadGridCell> _cells;

        public QuadGridBounds(QuadGridCell minCell, QuadGridCell maxCell, Point min, Point max)
        {
            if (minCell == null)
                throw new ArgumentNullException(nameof(minCell));

            if (maxCell == null)
                throw new ArgumentNullException(nameof(maxCell));

            MinCell = minCell;
            MaxCell = maxCell;
            Bounds = new Bounds(min, max);
            XSize = maxCell.X - minCell.X + 1;
            YSize = maxCell.Y - minCell.Y + 1;
            _cells = null;
        }

        public double Width => Bounds.Max.X - Bounds.Min.X;

        public double Height => Bounds.Max.Y - Bounds.Min.Y;

        public double CellWidth => Width / XSize;

        public double CellHeight => Height / YSize;

        public bool IsValid => XSize > 0 && YSize > 0 && Width > 0 && Height > 0;

        public IEnumerable<QuadGridCell> Cells
        {
            get
            {
                if (!IsValid)
                {
                    return Enumerable.Empty<QuadGridCell>();
                }

                if (_cells != null)
                {
                    return _cells;
                }

                _cells = new List<QuadGridCell>();

                for (var x = MinCell.X; x <= MaxCell.X; x++)
                {
                    for (var y = MinCell.Y; y <= MaxCell.Y; y++)
                    {
                        _cells.Add(new QuadGridCell(x, y));
                    }
                }

                return _cells;
            }
        }
    }
}