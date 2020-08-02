using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.Research.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AllStaticsModel
    {
        /// <summary>
        /// 顺产人数
        /// </summary>
        public int EutociaCount { set; get; }
        /// <summary>
        /// 剖宫产人数
        /// </summary>
        public int CesareanCount { set; get; }
        /// <summary>
        /// 引产人数
        /// </summary>
        public int OdinopoeiaCount { set; get; }
        /// <summary>
        /// 顺转剖人数
        /// </summary>
        public int EutociaChangeToCesarean { set; get; }
        /// <summary>
        /// 侧切人数
        /// </summary>
        public int CutCount { set; get; }
        /// <summary>
        /// 裂伤人数
        /// </summary>
        public int BreakCount { set; get; }
        /// <summary>
        /// 新生儿人数
        /// </summary>
        public int ChildCount { set; get; }
    }
}
