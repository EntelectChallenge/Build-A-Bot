using BuildABot.AI;
using BuildABot.Enums;
using BuildABot.Models;

namespace BuildABot.Services
{
    public class BotService
    {
        private Guid BotId;
        private StateMachine SM = new();

        public BotCommand ProcessState(BotStateDTO botState)
        {
            InputCommand action = SM.ProcessState(botState);
            return new BotCommand
            {
                BotId = BotId,
                Action = action,
            };
        }

        public void SetBotId(Guid botId)
        {
            BotId = botId;
        }
    }
}
