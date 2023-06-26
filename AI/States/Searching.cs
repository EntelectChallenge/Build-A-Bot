using BuildABot.AI.DataStructures.Spatial;
using BuildABot.Enums;
using BuildABot.Models;

namespace BuildABot.AI.States
{
    public class Searching : State
    {
        public Searching(StateMachine SM) : base(SM) {}

        public override InputCommand ProcessState(BotStateDTO BotState)
        {
            return InputCommand.RIGHT;
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
    }
}
