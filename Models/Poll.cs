using System;
using System.Collections.Generic;

namespace PollAPI.Models
{
    public partial class Poll
    {
        public int PollId { get; set; }
        public int PollNumber { get; set; }
        public string PollContent { get; set; } = null!;
    }
}
