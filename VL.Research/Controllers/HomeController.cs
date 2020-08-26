using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ExcelExportSolution;
using VL.Consolo_Core.Common.ValuesSolution;
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

        public IActionResult Index2()
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
        /// <param name="apiContext"></param>
        /// <param name="userService"></param>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromServices] APIContext apiContext, [FromServices] UserService userService, LoginViewModel model, string returnUrl)
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
                var user = result.Data;
                var authorityIds = userService.GetAllUserAuthorityIds(result.Data.Id).Data;

                #region 登录缓存处理

                var claimIdentity = new ClaimsIdentity();
                claimIdentity.AddClaim(new Claim(ClaimTypes.Name, "jim"));
                await apiContext.HttpContext.SignInAsync(UserService.ShemeName, new ClaimsPrincipal(claimIdentity));
                return RedirectToLocal(returnUrl);

                #endregion
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
        public ActionResult CommonList(string viewName)
        {
            ViewBag.ViewName = viewName;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public ActionResult UserList()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public ActionResult RoleList()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public ActionResult CreateRole()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public ActionResult EditRole()
        {
            return View();
        }

        /// <summary>
        /// 编辑角色权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public ActionResult EditRoleAuthorities()
        {
            return View();
        }

        /// <summary>
        /// 编辑用户角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditUserRoles()
        {
            return View();
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
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public ActionResult VisitRecordList()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案列表)]
        public ActionResult LabOrderList()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregnantInfoId"></param>
        /// <returns></returns>
        [HttpGet]
        //[VLAuthentication(Authority.查看孕妇档案详情)]
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
        //[VLAuthentication(Authority.查看检查详情)]
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
            //var tableConfigs = tableElements.Select(c => new ListConfig(c));
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


        #region ExcelExport

        /// <summary>
        /// 通用导出方案
        /// </summary>
        /// <param name="service"></param>
        /// <param name="exportName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult CommonExport([FromServices] SharedService service, string exportName)
        {
            exportName = "高危妊娠表";
            var search = new List<VLKeyValue>() { new VLKeyValue("personname", "贾婷婷") };

            var path = System.IO.Path.Combine(AppContext.BaseDirectory, @"Common\ExcelExportSolution", "ExportConfig.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(ExportConfig.NodeElementName);
            var configs = tableElements.Select(c => new ExportConfig(c));
            var config = configs.FirstOrDefault(c => c.ExportName == exportName);
            if (config == null)
            {
                throw new NotImplementedException("无效的导出配置");
            }
            var modelPath = System.IO.Path.Combine(AppContext.BaseDirectory, @"Common\ExcelExportSolution", config.FileName);
            var filename = DateTime.Now.ToString("yyyyMMdd_HHmmss") + config.FileName;
            var outputPath = System.IO.Path.Combine(AppContext.BaseDirectory, @"Common\ExcelExportSolution", filename);
            if (!System.IO.File.Exists(modelPath))
            {
                throw new NotImplementedException("模板文件不存在");
            }
            using (System.IO.FileStream s = System.IO.File.OpenRead(modelPath))
            {
                var workbook = new XSSFWorkbook(s);
                foreach (var sheetConfig in config.Sheets)
                {
                    sheetConfig.UpdateWheres(search);
                    var sheet = workbook.GetSheet(sheetConfig.SheetName);
                    if (sheet != null)
                    {
                        sheetConfig.DataSources = new Dictionary<string, DataTable>();
                        foreach (var sourceConfig in sheetConfig.Sources)
                        {
                            var result = service.GetCommonSelectByExportConfig(sourceConfig);
                            if (result.Data == null)
                            {
                                throw new NotImplementedException("无效的数据源:" + sourceConfig.SourceName);
                            }
                            sheetConfig.DataSources[sourceConfig.SourceName] = result.Data;
                        }
                        sheetConfig.Render(sheet);
                    }
                }
                using (System.IO.Stream stream = System.IO.File.OpenWrite(outputPath))
                {
                    workbook.Write(stream);
                }
            }
            var ss = System.IO.File.OpenRead(outputPath);
            return File(ss, "application/vnd.android.package-archive", outputPath);
        }

        /// <summary>
        /// 通用导出方案
        /// </summary>
        /// <param name="sharedService"></param>
        /// <param name="moduleName"></param>
        /// <param name="exportName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult CommonExportInlineForFYPT([FromServices] SharedService sharedService, string moduleName, string exportName)
        {
            var search = new List<VLKeyValue>() { new VLKeyValue("idcard", "110101199003072025") };
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, @"XMLConfig", moduleName,exportName + ".xml");

            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(ExportConfig.NodeElementName);
            var configs = tableElements.Select(c => new ExportConfig(c));
            var config = configs.FirstOrDefault();
            if (config == null)
            {
                throw new NotImplementedException("无效的导出配置");
            }
            var modelPath = System.IO.Path.Combine(AppContext.BaseDirectory, @"XMLConfig", moduleName, config.FileName);
            var filename = DateTime.Now.ToString("yyyyMMdd_HHmmss") + config.FileName;
            var outputPath = System.IO.Path.Combine(AppContext.BaseDirectory, @"XMLConfig", moduleName, filename);
            if (!System.IO.File.Exists(modelPath))
            {
                throw new NotImplementedException("模板文件不存在");
            }
            using (System.IO.FileStream s = System.IO.File.OpenRead(modelPath))
            {
                var workbook = new XSSFWorkbook(s);
                foreach (var sheetConfig in config.Sheets)
                {
                    sheetConfig.UpdateWheres(search);
                    var sheet = workbook.GetSheet(sheetConfig.SheetName);
                    if (sheet != null)
                    {
                        sheetConfig.DataSources = new Dictionary<string, DataTable>();
                        foreach (var sourceConfig in sheetConfig.Sources)
                        {
                            var result = sharedService.GetCommonSelectByExportConfig(sourceConfig,DBSourceType.FYPT);
                            if (!result.IsSuccess)
                            {
                                throw new NotImplementedException("数据源存在异常:" + result.Message);
                            }
                            sheetConfig.DataSources[sourceConfig.SourceName] = result.Data;
                        }
                        sheetConfig.Render(sheet);
                    }
                }
                using (System.IO.Stream stream = System.IO.File.OpenWrite(outputPath))
                {
                    workbook.Write(stream);
                }
            }
            var ss = System.IO.File.OpenRead(outputPath);
            return File(ss, "application/vnd.android.package-archive", outputPath);
        }

        /// <summary>
        /// 获取 通用分页列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult CommonExportAll([FromServices] APIContext apiContext,string outputPath)
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, @"XMLConfig", outputPath);
            var stream = System.IO.File.OpenRead(outputPath);
            return File(stream, "application/vnd.android.package-archive", outputPath);
        }
        #endregion
    }
}
