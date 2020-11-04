using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autobots.EMRServices.Utils
{
    public interface IConfigFactory
    {
        Dictionary<string, string> GetConfig(string section);
    }
}