﻿using System;
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

        public ServiceResult<T> Success<T>(T data)
        {
            return new ServiceResult<T>(data);
        }
        public ServiceResult<T> Error<T>(T data, string message = "", int code = 0)
        {
            return new ServiceResult<T>(data) { Message = message, Code = code };
        }
    }
}
