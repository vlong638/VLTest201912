namespace ResearchAPI.CORS.Common
{
    public class BusinessEntityProperty
    {
        public BusinessEntityProperty(long id, string name, long businessEntityId)
        {
            this.Id = id;
            this.Name = name;
            this.BusinessEntityId = businessEntityId;
        }

        public long Id { set; get; }
        public string Name { set; get; }
        public long BusinessEntityId { set; get; }
    }
}