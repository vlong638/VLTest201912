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
    public class ChildDischarge_SourceData : SourceDataForESB
    {
        public ChildDischargeModel SourceData;

        public ChildDischarge_SourceData(ChildDischargeModel pe)
        {
            this.SourceData = pe;
        }

        public string inp_no => SourceData.inp_no;
        public string idcard => SourceData.shenfenzh;
        public string PersonName => SourceData.PATNAME;
        public string SourceId => SourceData.inp_no;
        public TargetType TargetType => TargetType.ChildDischarge;
    }
}
