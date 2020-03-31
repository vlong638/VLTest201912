﻿using System;

namespace VLTest2015.Services
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ServiceResponse<T> // : IResponse
    {
        public ServiceResponse()
        {
        }

        public ServiceResponse(T data)
        {
            Data = data;
            Status = true;
        }

        public T Data { set; get; }
        public bool Status { set; get; }
        public int ErrorCode { set; get; }
        public string ErrorMessage { set; get; }
    }
}