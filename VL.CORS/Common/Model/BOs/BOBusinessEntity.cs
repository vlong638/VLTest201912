﻿namespace ResearchAPI.CORS.Common
{
    public class BOBusinessEntity
    {
        public BOBusinessEntity(long id, string name, long businessTypeId)
        {
            Id = id;
            Name = name;
            BusinessTypeId = businessTypeId;
        }

        public long Id { set; get; }
        public string Name { set; get; }
        public long BusinessTypeId { set; get; }
    }
}