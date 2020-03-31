using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.API.Common.Services
{
    public abstract class ServiceBase
    {
        protected ServiceContext ServiceContext;

        public ServiceBase()
        {
            this.ServiceContext = new ServiceContext();
        }
    }
}
