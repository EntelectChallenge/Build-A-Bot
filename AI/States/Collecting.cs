using BuildABot.AI.DataStructures.Spatial;
using BuildABot.Enums;
using BuildABot.Models;
using Path = BuildABot.AI.DataStructures.Pathfinding.Path;

namespace BuildABot.AI.States
{
    public class Collecting : State
    {
        private Path PathToCollectible;
        private Point CurrentPoint;
        private int CurrentPointIndex;

        public Collecting(StateMachine SM, Path path) : base(SM)
        {
            PathToCollectible = path;
            CurrentPointIndex = 0;
            CurrentPoint = NextPoint();
        }

        public override InputCommand? ProcessState(BotStateDTO BotState)
        {
            if (CurrentPointIndex == PathToCollectible.Length - 1)
            {
                ChangeState(new Searching(SM));
                return null;
            }

            Point nextPoint = NextPoint();
            Point nextMoveDelta = DeltaToNextNode(nextPoint);

            CurrentPoint = nextPoint;

            switch ((nextMoveDelta.X, nextMoveDelta.Y))
            {
                case (1, 1):
                    return InputCommand.DOWNRIGHT;
                case (1, 0):
                    return InputCommand.RIGHT;
                case (1, -1):
                    return InputCommand.UPRIGHT;
                case (0, 1):
                    return InputCommand.DOWN;
                case (0, -1):
                    return InputCommand.UP;
                case (-1, 1):
                    return InputCommand.DOWNLEFT;
                case (-1, 0):
                    return InputCommand.LEFT;
                case (-1, -1):
                    return InputCommand.UPLEFT;
                default:
                    return InputCommand.RIGHT;
            }
        }

        private Point NextPoint()
        {
            return PathToCollectible.Nodes[CurrentPointIndex++];
        }

        private Point DeltaToNextNode(Point nextPoint)
        {
            var X = nextPoint.X - CurrentPoint.X;
            var Y = nextPoint.Y - CurrentPoint.Y;

            return new(X, Y);
        }
    }
}
