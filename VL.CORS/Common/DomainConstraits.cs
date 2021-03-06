﻿using Autobots.Infrastracture.Common.ValuesSolution;
using ResearchAPI.CORS.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    public enum PKVType
    {
        /// <summary>
        /// 业务对象
        /// </summary>
        BusinessEntity,
        /// <summary>
        /// 业务对象,数据源
        /// </summary>
        BusinessEntitySource,
        /// <summary>
        /// 业务对象属性
        /// </summary>
        BusinessEntityProperty,
        /// <summary>
        /// 业务对象属性,数据源
        /// </summary>
        BusinessEntityPropertySource,
    }

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
        public static Dictionary<long, string> BusinessEntityDisplayDic { private set; get; }
        public static Dictionary<long, string> BusinessEntitySourceDic { private set; get; }
        public static List<BusinessEntityProperty> BusinessEntityProperties { private set; get; }
        public static Dictionary<long, string> BusinessEntityPropertyDisplayDic { private set; get; }
        public static Dictionary<long, string> BusinessEntityPropertySourceDic { private set; get; }
        public static Routers Routes { get; private set; }
        public static Dictionary<long, string> ScheduleStatuss { get; private set; }
        public static Dictionary<long, string> ViewAuthorizeTypes { private set; get; }
        public static Dictionary<string, string> LabOrders { get; private set; }
        public static List<IGrouping<string, VLKeyValueWithParent<string, string, string, string>>> LabResults { get; private set; }
        public static Dictionary<string, string> LabResultItems { get; private set; }
        public static List<BusinessEntityTemplate> Templates { get; private set; }

        internal static string RenderIdToText<T>(T id, Dictionary<T, string> source) where T : IComparable
        {
            return id.RenderIdToText(source);
        }
        internal static List<string> RenderIdToText<T>(List<T> ids, Dictionary<T, string> source)
        {
            return ids.RenderIdToText(source);
        }

        public static void InitData(ReportTaskService reportTaskService)
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
                    BusinessEntities.Add(new BusinessEntity(businessEntity.Id, businessEntities.Id, businessEntity.DisplayName, businessEntity.SourceName));
                    foreach (var property in businessEntity.Properties)
                    {
                        BusinessEntityProperties.Add(new BusinessEntityProperty()
                        {
                            Id = property.Id,
                            TableName = property.From,
                            ColumnName = property.SourceName,
                            DisplayName = property.DisplayName,
                            ColumnType = property.ColumnType,
                            EnumType = property.EnumType,
                            BusinessEntityId = businessEntity.Id,
                        });
                    }
                }
            }
            BusinessEntityDisplayDic = new Dictionary<long, string>();
            foreach (var item in BusinessEntities.Select(c => new VLKeyValue<long, string>(c.Id, c.Name)))
            {
                BusinessEntityDisplayDic.Add(item.Key, item.Value);
            }
            BusinessEntitySourceDic = new Dictionary<long, string>();
            foreach (var item in BusinessEntities.Select(c => new VLKeyValue<long, string>(c.Id, c.SourceName)))
            {
                BusinessEntitySourceDic.Add(item.Key, item.Value);
            }
            BusinessEntityPropertyDisplayDic = new Dictionary<long, string>();
            foreach (var item in BusinessEntityProperties.Select(c => new VLKeyValue<long, string>(c.Id, c.DisplayName)))
            {
                BusinessEntityPropertyDisplayDic.Add(item.Key, item.Value);
            }
            BusinessEntityPropertySourceDic = new Dictionary<long, string>();
            foreach (var item in BusinessEntityProperties.Select(c => new VLKeyValue<long, string>(c.Id, c.ColumnName)))
            {
                BusinessEntityPropertySourceDic.Add(item.Key, item.Value);
            }
            Routes = ConfigHelper.GetRouters(@"Configs/XMLConfigs/BusinessEntities", "Routers.xml");
            #endregion

            ScheduleStatuss  = ConfigHelper.GetDictionary<long>("ScheduleStatus"); 
            ViewAuthorizeTypes = ConfigHelper.GetDictionary<long>("ViewAuthorizeType");
            Departments = ConfigHelper.GetDictionary<long>("Department");
            Users = reportTaskService.GetUsersDictionary().Data;
            Roles = reportTaskService.GetProjectRolesDictionary().Data;
            if (Roles==null)
            {
                throw new Exception("Roles 未初始化");
            }
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
            LabResultItems = new Dictionary<string, string>();
            foreach (var item in labs)
            {
                if (!LabResultItems.ContainsKey(item.Value))
                {
                    LabResultItems.Add(item.Value, item.Key);
                }
            }
            //Templates
            Templates = new List<BusinessEntityTemplate>();
            Templates.Add(ConfigHelper.GetBusinessEntityTemplate("Configs\\XMLConfigs\\BusinessEntities", "Template_孕周产检.xml"));
            Templates.Add(ConfigHelper.GetBusinessEntityTemplate("Configs\\XMLConfigs\\BusinessEntities", "Template_孕周检验.xml"));
        }
    }
}
