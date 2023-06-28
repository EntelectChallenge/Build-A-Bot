using BuildABot.AI.DataStructures.Spatial;
using BuildABot.Enums;
using BuildABot.Models;
using BuildABot.AI.Algorithms;
using Path = BuildABot.AI.DataStructures.Pathfinding.Path;

namespace BuildABot.AI.States
{
    public class Searching : State
    {
        private int HeroWidth = 2;
        private int HeroHeight = 2;

        public Searching(StateMachine SM) : base(SM) {}

        public override InputCommand? ProcessState(BotStateDTO BotState)
        {
            if (SearchForCollectibles(BotState) != null)
            {
                return null;
            }

            if (ShouldJump(BotState))
            {
                return InputCommand.UPRIGHT;
            } else if (CanClimb(BotState))
            {
                return InputCommand.UP;
            }

            return InputCommand.RIGHT;
        }

        private BoundingBox GetPlayerBoundingBox(BotStateDTO state)
        {
            var x = (state.HeroWindow.Length / 2) - 1;
            var y = state.HeroWindow[0].Length / 2;
            return new BoundingBox(x, y, HeroWidth, HeroHeight);
        }

        private bool IsOnPlatform(BotStateDTO state)
        {
            var playerBounds = GetPlayerBoundingBox(state);
            for (int x = playerBounds.Left; x <= playerBounds.Right; x++)
            {
                if (state.HeroWindow[x][playerBounds.Bottom - 2] == (int)ObjectType.Platform)
                    return true;
            }
            return false;
        }

        private bool IsOnGround(BotStateDTO state)
        {
            var playerBounds = GetPlayerBoundingBox(state);
            for (int x = playerBounds.Left; x <= playerBounds.Right; x++)
            {
                if (state.HeroWindow[x][playerBounds.Bottom - 2] == (int)ObjectType.Solid)
                    return true;
            }
            return false;
        }

        private bool CanClimb(BotStateDTO state)
        {
            var playerBounds = GetPlayerBoundingBox(state);
            for (int x = playerBounds.Left; x < playerBounds.Right; x++)
            {
                for (int y = playerBounds.Bottom - 1; y <= playerBounds.Top; y++)
                {
                    if (state.HeroWindow[x][y] == (int)ObjectType.Ladder) return true;
                }
            }
            return false;
        }

        private bool IsHazardInFrontOfPlayer(BotStateDTO state)
        {
            var position = GetPlayerBoundingBox(state);
            int startX = position.Left;
            int endX = position.Right + 1;

            for (int x = startX; x < endX; x++)
            {
                for (int y = position.Bottom - 2; y <= position.Top + 1; y++)
                {
                    if (state.HeroWindow[x][y] == (int)ObjectType.Hazard)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsWallInFrontOfPlayer(BotStateDTO state)
        {
            var playerBounds = GetPlayerBoundingBox(state);
            int startX = playerBounds.Left;
            int endX = playerBounds.Right + 2;

            for (int x = startX; x <= endX + 2; x++)
            {
                if (state.HeroWindow[x][playerBounds.Bottom - 1] == (int)ObjectType.Solid)
                    return true;
            }
            return false;
        }

        private bool ShouldJump(BotStateDTO state)
        {

            return (IsOnGround(state) || IsOnPlatform(state)) &&
                (IsHazardInFrontOfPlayer(state) || IsWallInFrontOfPlayer(state));
        }

        private static List<Point> FindCollectibles(BotStateDTO state)
        {
            var collectibles = new List<Point>();
            for (int x = 0; x < state.HeroWindow.Length; x++)
            {
                var col = state.HeroWindow[x];
                for (int y = 0; y < col.Length; y++)
                {
                    if (state.HeroWindow[x][y] == (int)ObjectType.Collectible)
                    {
                        collectibles.Add(new Point(x, y));
                    }
                }
            }
            return collectibles;
        }

        private Path? SearchForCollectibles(BotStateDTO BotState)
        {
            var playerBounds = GetPlayerBoundingBox(BotState);

            // Find all collectibles
            List<Point> collectibles = FindCollectibles(BotState);

            if (collectibles.Count <= 0)
            {
                return null;
            }

            collectibles.Sort((a, b) =>
            {
                return Pathfinding.ManhattanDistance(playerBounds.Position, a) - Pathfinding.ManhattanDistance(playerBounds.Position, b);
            }); 

            // Get 3 closest collectibles
            List<Point> closestCollectibles = collectibles
                .Take(3).ToList();

            // Calculate which collectible has the shortest path
            Point closestCollectibleByPath = closestCollectibles.First();
            Path? closestPath = Pathfinding.AStarSearch(BotState, playerBounds.Position,  closestCollectibleByPath);
            foreach (var collectible in closestCollectibles.Skip(1))
            {
                int closestPathDistance = closestPath is Path path ? path.Length : Int32.MaxValue;
                var newPath = Pathfinding.AStarSearch(BotState, playerBounds.Position, collectible);

                if (newPath is Path newP && newP.Length < closestPathDistance)
                {
                    closestCollectibleByPath = collectible;
                    closestPath = newPath;
                }
            }

            // If closestPath is null, we haven't managed to pathfind to any collectibles, so keep searching.
            if (closestPath is Path)
            {
                ChangeState(new Collecting(SM, closestPath));
                return closestPath;
            }
            return null;
        }
    }
}
