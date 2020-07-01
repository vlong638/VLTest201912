using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;
using System.Linq;
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
        string SourceId { get; }
        SourceType SourceType { get; }
    }

    //public abstract class SourceData<T>: SourceData
    //{
    //    public T Data { set; get; }
    //    public abstract string SourceId {  get; }
    //    public abstract SourceType SourceType { get; }
    //}

    public abstract class SyncTaskBO<T1> where T1 : SourceData
    {
        public Action<T1> DoLogSource { set; get; }
        public Action<T1, StringBuilder> DoLogCreate { set; get; }

        /// <summary>
        /// 获取待新建的来源数据
        /// </summary>
        /// <returns></returns>
        public abstract List<T1> GetSourceDatas(ServiceContext context, UserInfo userInfo);

        public abstract bool IsExist(UserInfo userInfo, T1 SourceData, StringBuilder logger, ref string errorMessage);

        public abstract bool DoCommit(UserInfo userInfo, T1 sourceData1, StringBuilder logger, ref string errorMessage);

        public void CreateSyncLog(ServiceContext context, SyncOrder syncLog)
        {
            return;
        }

        public virtual void Start_AutoCommit(ServiceContext context, UserInfo userInfo, int interval = 1000 * 10)
        {
            while (true)
            {
                var sourceDatas = GetSourceDatas(context,userInfo);
                foreach (var sourceData in sourceDatas)
                {
                    DoLogSource?.Invoke(sourceData);
                    DoWork(context, userInfo, sourceData);
                }
                System.Threading.Thread.Sleep(interval);
            }
        }

        public void DoWork(ServiceContext context, UserInfo userInfo, T1 sourceData)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var errorMessage = "";
                if (IsExist(userInfo, sourceData, logger, ref errorMessage))
                {
                    logger.AppendLine("待新建数据已存在");
                    context.SDService.SaveSyncOrder(context.Hele_DBContext.DbGroup, new SyncOrder()
                    {
                        SourceId = sourceData.SourceId,
                        SourceType = sourceData.SourceType,
                        SyncTime = DateTime.Now,
                        SyncStatus = SyncStatus.Existed,
                    });
                    return;
                }
                if (DoCommit(userInfo, sourceData, logger, ref errorMessage))
                {
                    logger.AppendLine("新建数据成功");
                    context.SDService.SaveSyncOrder(context.Hele_DBContext.DbGroup, new SyncOrder()
                    {
                        SourceId = sourceData.SourceId,
                        SourceType = sourceData.SourceType,
                        SyncTime = DateTime.Now,
                        SyncStatus = SyncStatus.Success,
                    });
                }
                else
                {
                    logger.AppendLine("新建数据失败");
                    context.SDService.SaveSyncOrder(context.Hele_DBContext.DbGroup, new SyncOrder()
                    {
                        SourceId = sourceData.SourceId,
                        SourceType = sourceData.SourceType,
                        SyncTime = DateTime.Now,
                        SyncStatus = SyncStatus.Error,
                        ErrorMessage = errorMessage,
                    });
                }
            }
            catch (Exception ex)
            {
                logger.AppendLine("出现异常:" + ex.ToString());
                context.SDService.SaveSyncOrder(context.Hele_DBContext.DbGroup, new SyncOrder()
                {
                    SourceId = sourceData.SourceId,
                    SourceType = sourceData.SourceType,
                    SyncTime = DateTime.Now,
                    SyncStatus = SyncStatus.Error,
                    ErrorMessage = ex.ToString(),
                });
            }
            DoLogCreate?.Invoke(sourceData, logger);
        }
    }
}
