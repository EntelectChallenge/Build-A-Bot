using BuildABot.Enums;
using BuildABot.Models;

namespace BuildABot.Services
{
    public class BotService
    {
        private Guid BotId;

        public BotCommand ProcessState(BotStateDTO botState)
        {
            // Simply return right for now.
            return new BotCommand
            {
                BotId = BotId,
                Action = InputCommand.RIGHT,
            };
        }

        public void SetBotId(Guid botId)
        {
            BotId = botId;
        }
    }
}
