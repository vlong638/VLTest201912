﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBee.Models
{
    public class UserRoleInfoModel
    {
        public long UserId { set; get; }
        public long RoleId { set; get; }
        public string RoleName { set; get; }
    }
}
