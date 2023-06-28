using BuildABot.Enums;
using BuildABot.Models;

namespace BuildABot.AI
{
    public abstract class State
    {
        protected StateMachine SM;

        protected State(StateMachine SM)
        {
            this.SM = SM;
        }

        protected void ChangeState(State NewState)
        {
            SM.ChangeState(NewState);
        }

        public abstract InputCommand? ProcessState(BotStateDTO BotState);
    }
}
