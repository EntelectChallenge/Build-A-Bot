using BuildABot.AI.DataStructures.Spatial;

namespace BuildABot.AI.DataStructures.Pathfinding
{
    // A node for use in pathfinding.
    public class Node : IEquatable<Node>
    {
        public int X;
        public int Y;
        public int GCost;
        public int HCost;
        public Node? parent;

        public int FCost => GCost + HCost;

        public Node(int _X, int _Y, Node? _parent)
        {
            X = _X;
            Y = _Y;
            GCost = parent != null ? parent.GCost + 1 : 0;
            parent = _parent;
        }

        public static implicit operator Point(Node n) => new(n.X, n.Y);

        public bool Equals(Node other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
