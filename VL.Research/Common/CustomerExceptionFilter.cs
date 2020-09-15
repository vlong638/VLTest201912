using Exceptionless;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using System;

namespace VL.Research.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="modelMetadataProvider"></param>
        public CustomerExceptionFilter(IWebHostEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        /// <summary>
        /// 发生异常进入
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)//如果异常没有处理
            {
                if (_hostingEnvironment.IsDevelopment())//如果是开发环境
                {
                    var result = new ViewResult { ViewName = "../Home/Exception" };
                    result.ViewData = new ViewDataDictionary(_modelMetadataProvider,
                                                                context.ModelState);
                    result.ViewData.Add("Exception", context.Exception);//传递数据
                    context.Result = result;
                }
                else
                {
                    context.Result = new JsonResult(new
                    {
                        Result = false,
                        Code = 500,
                        Message = context.Exception.Message
                    });
                }
                //集成ExceptionLess
                //context.Exception.ToExceptionless().Submit();
                
                //集成Log4Net

                context.ExceptionHandled = true;//异常已处理
            }
        }
    }
}

