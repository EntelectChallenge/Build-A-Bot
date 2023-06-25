using BuildABot.Enums;

namespace BuildABot.Models
{
    public class BotCommand
    {
        public Guid BotId { get; set; }
        public InputCommand Action { get; set; }
    }
}
