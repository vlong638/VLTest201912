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
    public class ProfessionalExaminationModel_SourceData : SourceDataForPregnant
    {
        public ProfessionalExaminationModel SourceData;

        public ProfessionalExaminationModel_SourceData(ProfessionalExaminationModel pe)
        {
            this.SourceData = pe;
        }

        public string IdCard => SourceData.idcard;
        public string PersonName => SourceData.personname;
        public string SourceId => SourceData.id.ToString();
        public TargetType TargetType => TargetType.ProfessionalExamination;
    }
}
