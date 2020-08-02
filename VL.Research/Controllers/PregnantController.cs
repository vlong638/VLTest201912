using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VL.Consolo_Core.Common.ControllerSolution;
using VL.Consolo_Core.Common.PagerSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Research.Common;
using VL.Research.Models;

namespace VL.Research.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PregnantController : APIBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        public PregnantController()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案列表)]
        public APIResult<VLPagerResult<PagedListOfPregnantInfoModel>> GetPagedListOfPregnantInfo([FromServices] PregnantService pregnantService, int page, int rows, string name)
        {
            var pars = new GetPagedListOfPregnantInfoRequest()
            {
                PersonName = name,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = pregnantService.GetPagedListOfPregnantInfo(pars);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Success(serviceResult.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="name"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案列表)]
        public APIResult<VLPagerTableResult<DataTable>> GetConfigurablePagedListOfPregnantInfo([FromServices] PregnantService pregnantService, int page, int rows, string name, string sort, string order)
        {
            var pars = new GetPagedListOfPregnantInfoRequest()
            {
                PersonName = name,
                PageIndex = page,
                PageSize = rows,
                Orders = sort == null ? new Dictionary<string, bool>() : (new Dictionary<string, bool>() { { sort, (order == "asc") } }),
            };
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListPages.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(ViewConfig.NodeElementName);
            var tableConfigs = tableElements.Select(c => new ViewConfig(c));
            var tableConfig = tableConfigs.FirstOrDefault(c => c.ViewName == "O_PregnantInfo");
            var displayProperties = tableConfig.Properties.Where(c => c.IsNeedOnPage);
            pars.FieldNames = displayProperties.Select(c => c.ColumnName).ToList();

            var serviceResult = pregnantService.GetConfigurablePagedListOfPregnantInfo(pars);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Message);
            return Success(serviceResult.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        [HttpPost]
        [VLAuthentication(Authority.查看孕妇档案详情)]
        public APIResult<PregnantInfo> GetPregnantInfo([FromServices] PregnantService pregnantService, long pregnantInfoId)
        {
            if (pregnantInfoId == 0)
            {
                return Error(default(PregnantInfo), "缺少有效的 pregnantInfoId");
            }
            var serviceResult = pregnantService.GetPregnantInfoByPregnantInfoId(pregnantInfoId);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Success(serviceResult.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        [HttpPost]
        [VLAuthentication(Authority.查看产检列表)]
        public APIResult<VLPagerResult<PagedListOfVisitRecordModel>> GetPagedListOfVisitRecord([FromServices] PregnantService pregnantService, int page, int rows, long pregnantInfoId)
        {
            var pars = new GetPagedListOfVisitRecordRequest()
            {
                PregnantInfoId = pregnantInfoId,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = pregnantService.GetPagedListOfVisitRecord(pars);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Message);
            return Success(serviceResult.Data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantService"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        [HttpPost]
        [VLAuthentication(Authority.查看检查列表)]
        public APIResult<VLPagerResult<PagedListOfLabOrderModel>> GetPagedListOfLabOrder([FromServices] PregnantService pregnantService, int page, int rows, long pregnantInfoId)
        {
            var pars = new GetPagedListOfLabOrderRequest()
            {
                PregnantInfoId = pregnantInfoId,
                PageIndex = page,
                PageSize = rows,
            };
            var serviceResult = pregnantService.GetPagedListOfLabOrder(pars);
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, serviceResult.Messages);
            return Success(serviceResult.Data);
        }

        /// <summary>
        /// 全统计汇总
        ///顺产人数
        ///剖宫产人数
        ///引产人数
        ///顺转剖人数
        ///侧切人数
        ///裂伤人数
        ///新生儿人数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public APIResult<AllStaticsModel> GetAllStatics()
        {
            var serviceResult = new ServiceResult<AllStaticsModel>(new AllStaticsModel()
            {
                EutociaCount = 100,
                CesareanCount = 22,
                OdinopoeiaCount = 3,
                EutociaChangeToCesarean = 1,
                CutCount = 44,
                BreakCount = 55,
                ChildCount = 111,
            });
            if (!serviceResult.IsSuccess)
                return Error(serviceResult.Data, "");
            return Success(serviceResult.Data);
        }
    }
}
