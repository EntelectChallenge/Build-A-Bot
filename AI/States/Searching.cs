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
    }
}
