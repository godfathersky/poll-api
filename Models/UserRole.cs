using System;
using System.Collections.Generic;

namespace PollAPI.Models
{
    public partial class UserRole
    {
        public short UserRoleId { get; set; }
        public int UserId { get; set; }
        public short RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
