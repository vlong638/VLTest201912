using System;
using System.Collections.Generic;

namespace Autobots.Common.ServiceCommon
{
    interface IDataLogFactory
    {
        /// <summary>
        /// 获取数据日志
        /// </summary>
        /// <returns></returns>
        DataLog GetDataLog();
    }

    interface IDataLogCompare
    {
        /// <summary>
        /// 对比差异
        /// </summary>
        /// <param name="old"></param>
        /// <param name="new"></param>
        /// <returns></returns>
        List<DataLogChange> GetDataLogChanges(string @old, string @new);
    }

    public class DataLog
    {
        public string Old { set; get; }
        public string New { set; get; }
        public DateTime IssueTime { set; get; }
        public string OperateBy { set; get; }
    }

    public class DataLogChange
    {
        public DataLogChange()
        {
        }

        public DataLogChange(string key, string old, string @new)
        {
            Key = key;
            Old = old;
            New = @new;
        }

        public string Key { set; get; }
        public string Old { set; get; }
        public string New { set; get; }
    }
}
