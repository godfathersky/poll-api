using System;
using System.Collections.Generic;

namespace PollAPI.Models
{
    public partial class Poll
    {
        public int Id { get; set; }
        public string PollId { get; set; } = null!;
        public string PollTitle { get; set; } = null!;
        public string PollType { get; set; } = null!;
        public string PollAnswer { get; set; } = null!;
    }
}
