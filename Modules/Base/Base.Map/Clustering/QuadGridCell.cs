using System.Collections.Generic;

namespace Base.Map.Clustering
{
    internal class QuadGridCell
    {
        public readonly int X;
        public readonly int Y;

        public QuadGridCell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public long Id => GetId(X, Y);

        public static long GetId(int x, int y)
        {
            long id = x;
            id = id << 32;
            id = id | (long)(uint)y;
            return id;
        }

        public static QuadGridCell FromId(long id)
        {
            var x = (int)(id >> 32);
            var y = (int)(id & 0xFFFFFFFF);

            return new QuadGridCell(x, y);
        }
    }

    internal class QuadGridCellComparer : IEqualityComparer<QuadGridCell>
    {
        public bool Equals(QuadGridCell first, QuadGridCell second)
        {
            return first.X == second.X && first.Y == second.Y;
        }

        public int GetHashCode(QuadGridCell obj)
        {
            unchecked
            {
                return (obj.X * 397) ^ obj.Y;
            }
        }
    }
}