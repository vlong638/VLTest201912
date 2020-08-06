using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.FileSolution;
using VL.Research.Common;
using VL.Research.Models;
using VL.Research.Services;

namespace VL.Research.Controllers
{

    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login([FromServices] UserService userService, LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = userService.PasswordSignIn(model.UserName, model.Password, false);
            if (result.IsSuccess)
            {
                var user = result.PagedData;
                var authorityIds = userService.GetAllUserAuthorityIds(result.PagedData.Id).PagedData;

                #region 登录缓存处理

                //SetCurrentUser(new CurrentUser()
                //{
                //    UserId = user.Id,
                //    UserName = user.Name,
                //    AuthorityIds = authorityIds.ToList(),
                //});

                #endregion

                return RedirectToLocal(returnUrl);
            }
            else
            {
                switch (result.Code)
                {
                    case (int)SignInStatus.LockedOut:
                        return View("Lockout");
                    case (int)SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case (int)SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", string.Join(",", result.Messages));
                        return View(model);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public ActionResult PregnantInfoList()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        [HttpGet]
        [VLAuthentication(Authority.查看孕妇档案详情)]
        public ActionResult PregnantInfo(long pregnantInfoId)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        [HttpGet]
        [VLAuthentication(Authority.查看检查列表)]
        public ActionResult LabOrderList(long pregnantInfoId)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        [HttpGet]
        [VLAuthentication(Authority.查看产检列表)]
        public ActionResult VisitRecordList(long pregnantInfoId)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        [HttpGet]
        [VLAuthentication(Authority.查看检查详情)]
        public ActionResult LabOrderDetail(long pregnantInfoId)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public FileResult GetConfigurablePagedListOfPregnantInfoExcel([FromServices] PregnantService pregnantService, string name)
        {
            throw new NotImplementedException();
            //var sort = "";
            //var order = "";
            //var pars = new GetPagedListOfPregnantInfoRequest()
            //{
            //    PersonName = name,
            //    PageIndex = 1,
            //    PageSize = 100000,
            //    Orders = string.IsNullOrEmpty(sort) ? new Dictionary<string, bool>() : (new Dictionary<string, bool>() { { sort, (order == "asc") } }),
            //};
            //string fileName = $"GetConfigurablePagedListOfPregnantInfoExcel{ DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xls";
            //string filePath = Path.Combine(FileHelper.GetDirectory("~/Download"), fileName);
            //var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListPages.xml");
            //XDocument doc = XDocument.Load(path);
            //var tableElements = doc.Descendants("Table");
            //var tableConfigs = tableElements.Select(c => new ViewConfig(c));
            //var tableConfig = tableConfigs.FirstOrDefault(c => c.ViewName == "O_PregnantInfo");
            //var displayProperties = tableConfig.Properties.Where(c => c.IsNeedOnPage);
            //pars.FieldNames = displayProperties.Select(c => c.ColumnName).ToList();
            //var serviceResult = pregnantService.GetConfigurablePagedListOfPregnantInfo(pars);
            //if (!serviceResult.IsSuccess)
            //    throw new NotImplementedException(serviceResult.Message);

            //var displayNames = displayProperties.Select(c => c.DisplayName).ToList();
            //var dt = serviceResult.PagedData.SourceData;
            //IWorkbook workbook = null;
            //FileStream fs = null;
            //IRow row = null;
            //ISheet sheet = null;
            //ICell cell = null;
            //foreach (var line in dt)
            //{
            //    workbook = new HSSFWorkbook();
            //    sheet = workbook.CreateSheet("Sheet0");//创建一个名称为Sheet0的表  
            //    int rowCount = dt.Rows.Count;//行数  
            //    int columnCount = dt.Columns.Count;//列数  

            //    //设置列头  
            //    row = sheet.CreateRow(0);//excel第一行设为列头  
            //    for (int c = 0; c < displayNames.Count(); c++)
            //    {
            //        cell = row.CreateCell(c);
            //        cell.SetCellValue(displayNames[c]);
            //    }

            //    //设置每行每列的单元格,  
            //    for (int i = 0; i < rowCount; i++)
            //    {
            //        row = sheet.CreateRow(i + 1);
            //        for (int j = 0; j < columnCount; j++)
            //        {
            //            cell = row.CreateCell(j);//excel第二行开始写入数据  
            //            cell.SetCellValue(dt.Rows[i][j].ToString());
            //        }
            //    }
            //    using (fs = System.IO.File.OpenWrite(filePath))
            //    {
            //        workbook.Write(fs);//向打开的这个xls文件中写入数据  
            //    }
            //}
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //}
            //return Download(filePath, fileName);
        }


        [HttpGet]
        [VLAuthentication]
        public ActionResult AllStatistics()
        {
            return View();
        }


    }
}
