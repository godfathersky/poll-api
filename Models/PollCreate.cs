namespace PollAPI.Models
{
    public class PollCreate
    {
        public string PollId { get; set; } = string.Empty;
        public string PollTitle { get; set; } = string.Empty;
        public string PollType { get; set; } = string.Empty;
        public string PollAnswer { get; set; } = string.Empty;
    }
}
