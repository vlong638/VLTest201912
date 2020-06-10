using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLTest2015.Services;
using VLVLTest2015.Common.Pager;

namespace VLTest2015.Common.Models.RequestDTO
{
    public class GetListConfigRequest
    {
        public string ListName { set; get; }
        public long CustomConfigId { set; get; }
    }
}