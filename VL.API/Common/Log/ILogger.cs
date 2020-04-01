using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.API.Common.Log
{
    public interface ILogger
    {
        void Info(string message);
        void Info(Exception ex);
        void Info(LogData logData);
        void Error(string message);
        void Error(Exception ex);
        void Error(LogData logData);

        //void Info(Exception ex);
        //void Error(Exception ex);
    }
}
