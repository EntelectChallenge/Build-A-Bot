using BuildABot.AI.DataStructures.Spatial;
using BuildABot.AI.DataStructures.Pathfinding;
using BuildABot.Enums;
using BuildABot.Models;
using Path = BuildABot.AI.DataStructures.Pathfinding.Path;

namespace BuildABot.AI.Algorithms
{
    public static class Pathfinding
    {
        public static int ManhattanDistance(Point currentPoint, Point goal)
        {
            return Math.Abs(currentPoint.X - goal.X) + Math.Abs(currentPoint.Y - goal.Y);
        }

        public static Path? AStarSearch(BotStateDTO state, Point start, Point end)
        {
            var startNode = new Node(start.X, start.Y,  null);
            var endNode = new Node(end.X, end.Y, null);

            startNode.HCost = ManhattanDistance(start, end);

            HashSet<Node> openSet = new();
            HashSet<Node> closedSet = new();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.First();
                if (currentNode.Equals(endNode))
                {
                    endNode.parent = currentNode.parent;
                    return ConstructPath(endNode);
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                foreach (var neighbour in Neighbours(state, currentNode))
                {
                    if (closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    neighbour.HCost = ManhattanDistance(neighbour, end);
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                        continue;
                    }
                    var openNeighbour = openSet.Where(neighbour.Equals).First();
                    if (neighbour.GCost < openNeighbour.GCost)
                    {
                        openNeighbour.GCost = neighbour.GCost;
                        openNeighbour.parent = neighbour.parent;
                    }
                }
                openSet = openSet.OrderBy((node) => node.FCost).ToHashSet();
            }

            return null;
        }

        private static HashSet<Node> Neighbours(BotStateDTO state, Node node)
        {
            var neighbours = new HashSet<Node>();
            for (int x = node.X - 1; x <= node.X + 1; x++)
            {
                for (int y = node.Y - 1; y <= node.Y + 1; y++)
                {
                    // Check that point isn't out of bounds.
                    if (x < 0 || y < 0 || x >= state.HeroWindow.Length || y >= state.HeroWindow[0].Length)
                    {
                        continue;
                    }
                    if ((y == node.Y && x == node.X) || !IsPointWalkable(state, new Point(x, y)))
                    {
                        continue;
                    }
                    neighbours.Add(new(x, y,  node));
                }
            }
            return neighbours;
        }

        public static bool IsPointWalkable(BotStateDTO state, Point point)
        {
            if (point.X >= state.HeroWindow.Length || point.X < 0 || point.Y >= state.HeroWindow[0].Length || point.Y - 2 < 0)
            {
                return false;
            }

            var tile = (ObjectType)state.HeroWindow[point.X][point.Y];
            var tilesBelow = new[] {
                (ObjectType)state.HeroWindow[point.X][point.Y - 1], 
                (ObjectType)state.HeroWindow[point.X][point.Y - 2],
            };

            if (tile == ObjectType.Hazard || tile == ObjectType.Solid)
            {
                return false;
            }
            else if (
                tilesBelow.Contains(ObjectType.Platform)
                || tilesBelow.Contains(ObjectType.Solid)
                )
            {
                return true;
            }

            return false;
        }

        private static Path ConstructPath(Node node)
        {
            Path path = new();
            path.Add(node);

            while (node.parent != null)
            {
                node = node.parent;
                path.Add(node);
            }
            return path;
        }
    }
}
