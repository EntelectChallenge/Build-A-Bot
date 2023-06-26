namespace BuildABot.AI.DataStructures.Spatial
{
    public class Point : IEquatable<Point>
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        bool IEquatable<Point>.Equals(Point? other)
        {
            return other is not null && X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Point operator+(Point a, Point b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator-(Point a, Point b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }
    }
}
