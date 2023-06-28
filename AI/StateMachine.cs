using BuildABot.AI.States;
using BuildABot.Enums;
using BuildABot.Models;

namespace BuildABot.AI
{
    public class StateMachine
    {
        private State CurrentState;

        public StateMachine()
        {
            CurrentState = new Searching(this);
        }

        public void ChangeState(State NewState)
        {
            CurrentState = NewState;
        }

        public InputCommand? ProcessState(BotStateDTO BotState)
        {
            return CurrentState.ProcessState(BotState);
        }
    }
}
