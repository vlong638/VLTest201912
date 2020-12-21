﻿using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class User
    {
        public const string TableName = "User";

        public long Id { set; get; }
        public string Name { set; get; }
    }
}