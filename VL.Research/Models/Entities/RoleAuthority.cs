﻿using Dapper.Contrib.Extensions;

namespace BBee.Models
{
    [Table("A_RoleAuthority")]
    public class RoleAuthority
    {
        public long Id { set; get; }
        public long RoleId { set; get; }
        public long AuthorityId { set; get; }
    }
}
