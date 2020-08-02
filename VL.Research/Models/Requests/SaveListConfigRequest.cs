using VL.Research.Common;

namespace VL.Research.Models
{
    public class SaveListConfigRequest
    {
        public long CustomConfigId { set; get; }
        public string ListName { set; get; }
        public ViewConfig ViewConfig { set; get; }
        public string URL { get; set; }
    }
}