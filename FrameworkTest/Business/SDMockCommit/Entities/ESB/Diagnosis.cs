using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 住院诊断
    /// </summary>
    [Table("[V_FWPT_GY_ZHUYUANZD]")]
    public class Diagnosis
    {
        //patient_id
        //visit_id
        //inp_no
        //file_no
        //complaints
        //hpi_desc
        //emr_last_update
        //diag_type_code
        //diag_type_name
        //diag_no
        //diag_code
        //diag_desc
        //diag_last_update
        //downloadtime

        /// <summary>
        /// 诊断描述
        /// </summary>
        public string diag_desc { get; set; }
        /// <summary>
        /// 诊断编码
        /// </summary>
        public string diag_code { get; set; }
    }
}
