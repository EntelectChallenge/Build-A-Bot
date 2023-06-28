namespace BuildABot.AI.DataStructures.Pathfinding
{
    public class Path
    {
        public List<Node> Nodes;

        public Path()
        {
            Nodes = new();
        }

        public Path(List<Node> nodes)
        {
            Nodes = nodes.ToList();
        }

        public void Add(Node node)
        {
            Nodes.Add(node);
        }

        public int Length => Nodes.Count;
    }

}
