namespace ResearchAPI.CORS.Common
{
    public class BusinessType
    {
        public BusinessType(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public long Id { set; get; }
        public string Name { set; get; }
    }
}

