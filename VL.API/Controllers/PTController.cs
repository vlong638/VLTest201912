using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VL.API.Common.Controllers;
using VL.API.PT.Entities;
using VL.API.PT.Services;

namespace VL.API.Controllers
{
    /// <summary>
    /// 产科
    /// </summary>
    public class PTController : V3ControllerBase
    {
        [HttpGet]
        public PregnantInfo GetPregnantInfoById([FromServices] PTService ptService, int id)
        {
            var entity = ptService.GetPregnantInfoById(id);
            return entity;
        }
    }
}
