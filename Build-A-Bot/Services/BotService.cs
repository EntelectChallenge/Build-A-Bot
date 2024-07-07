using BuildABot.Enums;
using BuildABot.Models;

namespace BuildABot.Services;

public class BotService
{
    private Guid BotId;

    public BotCommand ProcessState(BotStateDTO botState)
    {
        return new BotCommand
        {
            BotId = BotId,
            Action = (int)BotAction.Right,
        };
    }

    public void SetBotId(Guid botId)
    {
        BotId = botId;
    }
}