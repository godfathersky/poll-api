using System;
using System.Collections.Generic;

namespace PollAPI.Models
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int UserId { get; set; }
        public string UserLogin { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public byte[] UserPasswordHash { get; set; } = null!;
        public byte[] UserPasswordSalt { get; set; } = null!;
        public TimeSpan UserAddTime { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
