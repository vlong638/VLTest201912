using BBee.Common;

namespace BBee.Models
{
    public class SaveListConfigRequest
    {
        public long CustomConfigId { set; get; }
        public string ListName { set; get; }
        public ListConfig ListConfig { set; get; }
        public string URL { get; set; }
    }
}