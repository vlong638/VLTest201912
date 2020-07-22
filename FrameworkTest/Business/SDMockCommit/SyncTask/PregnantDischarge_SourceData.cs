﻿using Dapper;
using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FrameworkTest.Business.SDMockCommit
{
    public class PregnantDischarge_SourceData : SourceDataForESB
    {
        public PregnantDischarge SourceData;

        public PregnantDischarge_SourceData(PregnantDischarge pe)
        {
            this.SourceData = pe;
        }

        //public string IdCard => SourceData.idcard;
        //public string PersonName => SourceData.personname;
        //public string SourceId => SourceData.Id.ToString();

        public string inp_no => SourceData.inp_no;
        public string PersonName => "蓝艳云";
        public string SourceId => SourceData.inp_no;
        public TargetType TargetType => TargetType.PregnantDischarge;
    }
}