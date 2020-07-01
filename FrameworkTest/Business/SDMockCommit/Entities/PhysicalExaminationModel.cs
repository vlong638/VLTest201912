using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FrameworkTest.Business.SDMockCommit
{
    public class PhysicalExaminationModel
    {
        /// <summary>
        /// 舒张压
        /// </summary>
        internal string dbp { set; get; }
        /// <summary>
        /// 收缩压
        /// </summary>
        internal string sbp { set; get; }

        public string pi_personname { set; get; }
        public string pi_weight { set; get; }
        public string pi_height { set; get; }
        public string pi_bmi { set; get; }

        public string Id { set; get; }
        public string idcard { set; get; }
        public string firstvisitdate { set; get; }
        public string lastestvisitdate { set; get; }
        public string weight { set; get; }
        public string temperature { set; get; }
        public string heartrate { set; get; }
    }
}
