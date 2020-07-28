using Dapper.Contrib.Extensions;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 医嘱
    /// </summary>
    [Table("[V_FWPT_MZ_YIJI]")]
    public class Advice
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
        /// 医嘱名称
        /// </summary>
        public string yizhumc { get; set; }
    }
}
