using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FrameworkTest.Business.SDMockCommit
{
    //public class SourcePregnant : SourceData<PregnantInfo>
    //{
    //    public override string SourceId => Data.Id.ToString();

    //    public override SourceType SourceType => SourceType.PregnantInfo; 
    //}

    //public SyncTask_Pregnant



    public interface SourceData
    {
        string IdCard { get; }
        string SourceId { get; }
        SourceType SourceType { get; }
    }

    //public abstract class SourceData<T>: SourceData
    //{
    //    public T Data { set; get; }
    //    public abstract string SourceId {  get; }
    //    public abstract SourceType SourceType { get; }
    //}

    public abstract class SyncTask<T1> where T1 : SourceData
    {
        protected SyncTask(ServiceContext context)
        {
            Context = context;
        }

        public ServiceContext Context { set; get; }

        public Action<T1> DoLogSource { set; get; }
        public Action<T1, StringBuilder> DoLogCreate { set; get; }
        /// <summary>
        /// 获取待新建的来源数据
        /// </summary>
        /// <returns></returns>
        public abstract List<T1> GetSourceDatas(UserInfo userInfo);

        public abstract SyncOrder DoCommit(WCQBJ_CZDH_DOCTOR_READResponse base8, UserInfo userInfo, SourceData_ProfessionalExaminationModel sourceData, StringBuilder logger);

        public abstract void DoWork(ServiceContext context, UserInfo userInfo, T1 sourceData);
        public virtual void Start_Auto_DoWork(ServiceContext context, UserInfo userInfo, int interval = 1000 * 10)
        {
            while (true)
            {
                var sourceDatas = GetSourceDatas(userInfo);
                foreach (var sourceData in sourceDatas)
                {
                    DoLogSource?.Invoke(sourceData);
                    DoWork(context, userInfo, sourceData);
                }
                System.Threading.Thread.Sleep(interval);
            }
        }

        public WCQBJ_CZDH_DOCTOR_READResponse GetBase8(UserInfo userInfo, string idCard, StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userInfo.UserId}&sParams=P${idCard}$P$P";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
            logger.AppendLine($"查询-Base8");
            logger.AppendLine(url);
            logger.AppendLine(result);
            return re;
        }
    }
}
