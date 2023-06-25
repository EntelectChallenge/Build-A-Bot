using BuildABot.Enums;
using BuildABot.Models;

namespace BuildABot.AI
{
    public class StateMachine
    {
        private State CurrentState;

        public void ChangeState(State NewState)
        {
            CurrentState = NewState;
        }

        public InputCommand ProcessState(BotStateDTO BotState)
        {
            return CurrentState.ProcessState(BotState);
        }
    }
}
