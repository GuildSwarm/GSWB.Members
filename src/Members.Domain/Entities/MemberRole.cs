﻿
namespace Members.Domain.Entities
{
    public class MemberRole
    {
        public required Member Member { get; set; }
        public required ulong RoleId { get; set; }
    }
}
