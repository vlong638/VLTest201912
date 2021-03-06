﻿using System;
using System.Web;

namespace VLTest2015.Utils
{
    public class CacheHelper
    {
        static string _loginCacheKey = "StrLogin";
        /// <summary>
        /// 获取登录缓存key
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static string GetLoginCacheKey(long userId)
        {
            return _loginCacheKey + "_" + userId;
        }

        static string _permissionCacheKey = "StrPermissions";
        /// <summary>
        /// 获取授权缓存key
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static string GetPermissionCacheKey(long userId)
        {
            return _permissionCacheKey + "_" + userId;
        }

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static object GetCache(string cacheKey)
        {
            var objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object objObject)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object objObject, TimeSpan timeout)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, System.Web.Caching.Cache.NoAbsoluteExpiration, timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存（绝对过期）
        /// </summary>
        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            var objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveCache(string cacheKey)
        {
            var cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            var cache = HttpRuntime.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}