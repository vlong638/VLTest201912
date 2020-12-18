namespace ResearchAPI.CORS.Common
{
    public class BOBusinessType
    {
        public BOBusinessType(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public long Id { set; get; }
        public string Name { set; get; }
    }
}

