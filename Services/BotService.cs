using BuildABot.AI;
using BuildABot.Enums;
using BuildABot.Models;

namespace BuildABot.Services
{
    public class BotService
    {
        private Guid BotId;
        private StateMachine SM = new();

        public BotCommand? ProcessState(BotStateDTO botState)
        {
            InputCommand? maybeAction = SM.ProcessState(botState);
            return maybeAction is InputCommand action ? new BotCommand
            {
                BotId = BotId,
                Action = action,
            } : null;
        }

        public void SetBotId(Guid botId)
        {
            BotId = botId;
        }
    }
}
