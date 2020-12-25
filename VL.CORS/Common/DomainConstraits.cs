using Autobots.Infrastracture.Common.ValuesSolution;
using ResearchAPI.Controllers;
using ResearchAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainConstraits
    {
        public static long? AdminRoleId { private set; get; }
        public static long? MemberRoleId { private set; get; }
        public static long? OwnerRoleId { private set; get; }
        public static Dictionary<long, string> Roles { private set; get; }
        public static Dictionary<long, string> Users { private set; get; }
        public static Dictionary<long, string> Departments { private set; get; }
        public static List<BusinessType> BusinessTypes { private set; get; }
        public static List<BusinessEntity> BusinessEntities { private set; get; }
        public static Dictionary<long, string> BusinessEntityDic { private set; get; }
        public static List<BusinessEntityProperty> BusinessEntityProperties { private set; get; }
        public static Dictionary<long, string> BusinessEntityPropertyDic { private set; get; }
        public static Routers Routes { get; private set; }
        public static Dictionary<long, string> ViewAuthorizeTypes { private set; get; }
        public static Dictionary<string, string> LabOrders { get; private set; }
        public static List<IGrouping<string, VLKeyValue<string, string, string, string>>> LabResults { get; private set; }

        internal static string RenderIdToText<T>(T id, Dictionary<T, string> source)
        {
            List<T> ids = new List<T>() { id };
            var values = RenderIdsToText(ids, source);
            return values.First();
        }

        internal static List<string> RenderIdsToText<T>(List<T> ids, Dictionary<T, string> source)
        {
            var dic = source;
            var values = ids.Select(c => dic.ContainsKey(c) ? dic[c] : c.ToString()).ToList();
            return values;
        }

        public enum KVType
        {
            /// <summary>
            /// 浏览权限类型
            /// </summary>
            ViewAuthorizeType,
            /// <summary>
            /// 科室
            /// </summary>
            Department,
            /// <summary>
            /// 用户
            /// </summary>
            User,
            /// <summary>
            /// 角色
            /// </summary>
            Role,
            /// <summary>
            /// 业务对象类型
            /// </summary>
            BusinessType,
        }

        public enum PKVType
        {
            /// <summary>
            /// 业务对象
            /// </summary>
            BusinessEntity,
        }

        internal static string RenderIdsToText(long id, PKVType kvType)
        {
            List<long> ids = new List<long>() { id };
            var values = RenderIdsToText(ids, kvType);
            return values.First();
        }

        internal static List<string> RenderIdsToText(List<long> ids, PKVType kvType)
        {
            Dictionary<long, string> dic = null;
            switch (kvType)
            {
                case PKVType.BusinessEntity:
                    dic = BusinessEntityDic;
                    break;
                default:
                    break;
            }
            var values = ids.Select(c => dic.ContainsKey(c) ? dic[c] : c.ToString()).ToList();
            return values;
        }

        internal static void InitData(ReportTaskService reportTaskService)
        {
            #region BusinessEntity
            var businessEntitiesCollection = ConfigHelper.GetCOBusinessEntities();
            BusinessTypes = new List<BusinessType>();
            BusinessEntities = new List<BusinessEntity>();
            BusinessEntityProperties = new List<BusinessEntityProperty>();
            foreach (var businessEntities in businessEntitiesCollection)
            {
                BusinessTypes.Add(new BusinessType(businessEntities.Id, businessEntities.BusinessType));
                foreach (var businessEntity in businessEntities)
                {
                    BusinessEntities.Add(new BusinessEntity(businessEntity.Id, businessEntity.DisplayName, businessEntities.Id));
                    foreach (var property in businessEntity.Properties)
                    {
                        BusinessEntityProperties.Add(new BusinessEntityProperty()
                        {
                            Id = property.Id,
                            TableName = property.From,
                            ColumnName = property.ColumnName,
                            DisplayName = property.DisplayName,
                            BusinessEntityId = businessEntity.Id,
                        });
                    }
                }
            }
            BusinessEntityDic = new Dictionary<long, string>();
            foreach (var item in BusinessEntities.Select(c => new VLKeyValue<long, string>(c.Id, c.Name)))
            {
                BusinessEntityDic.Add(item.Key, item.Value);
            }
            BusinessEntityPropertyDic = new Dictionary<long, string>();
            foreach (var item in BusinessEntityProperties.Select(c => new VLKeyValue<long, string>(c.Id, c.DisplayName)))
            {
                BusinessEntityPropertyDic.Add(item.Key, item.Value);
            }
            Routes = ConfigHelper.GetRouters(@"Configs/XMLConfigs/BusinessEntities", "Routers.xml");
            #endregion

            ViewAuthorizeTypes = ConfigHelper.GetDictionary<long>("ViewAuthorizeType");
            Departments = ConfigHelper.GetDictionary<long>("Department");
            Users = reportTaskService.GetUsersDictionary().Data;
            Roles = reportTaskService.GetRolesDictionary().Data;
            AdminRoleId = Roles.First(c => c.Value == "项目管理员").Key;
            MemberRoleId = Roles.First(c => c.Value == "项目成员").Key;
            OwnerRoleId = Roles.First(c => c.Value == "项目创建人").Key;

            //Labs
            var labs = ConfigHelper.GetJsonConfig<string, string, string, string>("Labs");
            var result = new Dictionary<string, string>();
            foreach (var kv in labs)
            {
                if (!result.ContainsKey(kv.ParentKey))
                    result.Add(kv.ParentKey, kv.ParentValue);
            }
            LabOrders = result;
            LabResults = labs.GroupBy(c => c.ParentValue).ToList();
        }
    }
}
