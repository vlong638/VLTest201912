namespace ResearchAPI.CORS.Common
{
    public class BusinessEntity
    {
        public BusinessEntity(long id, long businessTypeId, string name, string sourceName)
        {
            Id = id;
            BusinessTypeId = businessTypeId;
            Name = name;
            SourceName = sourceName;
        }

        public long Id { set; get; }
        public long BusinessTypeId { set; get; }
        public string Name { set; get; }
        public string SourceName { set; get; }
    }
}