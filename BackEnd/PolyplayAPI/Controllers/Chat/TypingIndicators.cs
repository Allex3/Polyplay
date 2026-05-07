namespace PolyplayAPI.Controllers.Chat
{
    public class TypingIndicators
    {
        public string Type { get; set; } = null!;
        public string Room { get; set; } = null!;
        public bool Active { get; set; }
    }

    public class UsersCount
    {
        public int Count { get; set; }
    }
}
