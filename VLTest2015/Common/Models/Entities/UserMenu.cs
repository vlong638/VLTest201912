﻿using Dapper.Contrib.Extensions;
using System;

namespace VLTest2015.Services
{
    [Table(TableName)]
    public class UserMenu
    {
        public const string TableName = "A_UserMenu";

        public long Id { set; get; }
        public long UserId { set; get; }
        public string MenuName { set; get; }
        public string URL { set; get; }
        public string EntityAppConfig { set; get; }
    }
}
