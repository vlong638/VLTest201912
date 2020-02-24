using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Amusing.Objects.Entities
{
    /// <summary>
    /// 生物
    /// </summary>
    public class Creature
    {
        /// <summary>
        /// 标识键
        /// </summary>
        public Guid Id { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }
    }

    /// <summary>
    /// 地区
    /// </summary>
    public class Area
    {
        /// <summary>
        /// 标识键
        /// </summary>
        public Guid Id { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }
    }
    
    /// <summary>
    /// 玩家
    /// </summary>
    public class Player : Creature
    {
        /// <summary>
        /// 等级(+基础属性)
        /// </summary>
        public LevelCell LevelCell { set; get; }
        /// <summary>
        /// 装备(+基础属性,+战斗属性)
        /// </summary>
        public Equipment Equipment { set; get; }

        /// <summary>
        /// 基础属性汇总
        /// </summary>
        public BaseProperty BasePropertySummary { set; get; }
        /// <summary>
        /// 战斗属性汇总
        /// </summary>
        public CombatProperty CombatPropertySummary { set; get; } 
    }

    /// <summary>
    /// 装备
    /// </summary>
    public class Equipment {
        /// <summary>
        /// 标识键
        /// </summary>
        public Guid Id { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 基础属性加成
        /// </summary>
        public BaseProperty BaseProperty { set;get;}
        /// <summary>
        /// 战斗属性加成
        /// </summary>
        public CombatProperty CombatProperty { set;get; }
    }
    /// <summary>
    /// 装备要素
    /// </summary>
    public class EquipmentComponent
    { 
    }

    /// <summary>
    /// 等级的单元
    /// </summary>
    public class LevelCell
    {
        /// <summary>
        /// 经验
        /// </summary>
        public int Exp { set; get; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { set; get; }

        public static Dictionary<int, int> LevelPoints = new Dictionary<int, int>()
        {
            { 1,3},
            { 2,3},
            { 3,3},
            { 4,3},
            { 5,3},
            { 6,3},
            { 7,3},
            { 8,3},
            { 9,3},
            { 10,3},
        };

        /// <summary>
        /// 可用的属性点
        /// </summary>
        public int AvailablePoint { set; get; }
        /// <summary>
        /// 累计升级投入的基础属性
        /// </summary>
        public BaseProperty BasePoint = new BaseProperty();
    }

    /// <summary>
    /// 基础属性 2物理+2法术+2防御+1随机
    /// 基础属性=>战斗属性
    /// </summary>
    public class BaseProperty
    {
        ///TODO 数值平衡器
        ///一个100次攻击的收益曲线有助于合理的数值设定

        /// <summary>
        /// 力量
        /// </summary>
        public int Str { set; get; }
        /// <summary>
        /// 敏捷
        /// </summary>
        public int Agi { set; get; }

        /// <summary>
        /// 智力
        /// </summary>
        public int Int { set; get; }
        /// <summary>
        /// 信仰
        /// </summary>
        public int Blf { set; get; }

        /// <summary>
        /// 体质
        /// </summary>
        public int Vit { set; get; }
        /// <summary>
        /// 意志
        /// </summary>
        public int Wil { set; get; }

        /// <summary>
        /// 幸运
        /// </summary>
        public int Luk { set; get; }

        /// <summary>
        /// 基础属性
        /// </summary>
        /// <returns></returns>
        public CombatProperty ToCombatProperty()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 战斗属性
    /// </summary>
    public class CombatProperty
    {
        #region 物理

        /// <summary>
        /// 生命值
        /// </summary>
        public int HP { set; get; }
        /// <summary>
        /// 攻击力高值
        /// </summary>
        public int MaxATK { set; get; }
        /// <summary>
        /// 攻击力低值
        /// </summary>
        public int MinATK { set; get; }
        /// <summary>
        /// 防御力
        /// </summary>
        public int DEF { set; get; }

        #endregion

        #region 法术

        /// <summary>
        /// 法力值
        /// </summary>
        public int MP { set; get; }
        /// <summary>
        /// 魔力值
        /// </summary>
        public int MATK { set; get; }
        /// <summary>
        /// 魔法抗性
        /// </summary>
        public int MDEF { set; get; }

        //法术收益=法力值*法力系数(耗魔魔法高)*魔力值*魔力系数(高级魔法高)/(1+魔法抗性)

        #endregion

        #region 特殊攻击

        ///设计.特殊攻击
        ///特殊攻击通过玩家的特殊攻击概率触发
        ///各种不同的攻击模式有着不同的玩法
        ///考虑到能力需要提炼自玩家统一的能力
        ///特殊能力统一采用 反应条模式
        ///后续如果加入更多样的玩法,需要严格把控平衡性

        #region 设计.连击模式

        /// *连击模式:连续攻击模式
        /// 攻击时出现类似第五人格破解弹窗
        /// 分为Fail(白),Good(绿),Cool(蓝),Nice(橙),Perfect(红)
        /// 其中基础攻击次数为1,技能可能使基础攻击次数变换为n
        /// 不同的命中效果将得到攻击次数加成
        /// Fail=直接终止后续攻击
        /// Good=无加成
        /// Cool=0.1加成
        /// Nice=0.3加成
        /// Perfect=0.5加成
        ///
        /// 数值
        /// 常用设定为3,5,7,9次攻击
        /// 预期:基础值+上限
        /// 小数位为Combo叠加,不四舍五入
        /// 基础攻击次数   Combo叠加值               连击系数
        /// 3    理论上限为 3+1.5+0.5+0.5=5.5         max(5)
        /// 5    理论上限为 5+2.5+1+0.5+0.5=9.5       max(9)
        /// 7    理论上限为 7+3.5+1.5+1+0.5=13.5      max(13)
        /// 9    理论上限为 9+4.5+2+1+0.5+0.5=17.5    max(17)
        /// 最终伤害 =  技能伤害系数*连击系数     常规值      上限值
        /// 如三连击    技能伤害系数0.7           0.7*3=2.1       0.7*5=3.5 
        /// 如五连击    技能伤害系数0.8           0.9*5=4.5       0.9*9=8.1(+4.6)
        /// 如七连击    技能伤害系数0.9           1.2*7=8.4       1.2*13=14.3(+6.2)
        /// 如九连击    技能伤害系数1.1           1.6*9=13.5      1.5*17=25.5(+9.2)
        /// 
        /// 连击体验
        /// 体验上参考音乐游戏的Combo来强化玩家体验
        /// 每日提供固定的的音乐体验次数,玩家可以播放广告或者购买解锁音乐灵魂加持
        /// 没有音乐的连击是没有灵魂的
        /// 节奏感和连击加成是容易获得正向反馈的设计
        ///
        /// 超神效果
        /// 我希望在连击的末尾加入连击效应
        /// 借以进一步强化玩家的正向反馈
        /// 类似DOTA中斧王的斩首效果
        /// 震撼的效果和具备诱惑力的能力加成
        /// 
        /// 动画特效
        /// 连击的动画为不断的连续攻击
        /// 高层数的攻击效果希望借鉴DOTA的超神之路
        /// 


        #endregion

        #region 设计.重击模式

        ///首版采用类似连击的设定
        ///连击系统
        ///
        ///重击为连击系数转换的眩晕时间
        ///


        #endregion

        #region 设计.暴击模式

        ///首版采用类似连击的设定
        ///连击系统
        ///
        ///暴击为连击系数转换的暴击率
        ///暴击为破甲设定
        ///暴击伤害=攻击力/(1+物理抗性-暴击率)
        ///
        /// 物理抗性上限90%,理论物理减免90%
        /// 伤害=攻击力*(1-物理抗性)
        /// 
        /// 暴击率上限95%,理论物理加成二十倍
        /// 伤害=攻击力/(1-暴击率)
        ///

        #endregion

        #endregion

        #region 状态

        /// <summary>
        /// 连击抗性(敏捷)
        /// </summary>
        public int ComboResistant { set; get; }
        /// <summary>
        /// 暴击抗性(体质)
        /// </summary>
        public int CriticalAttackResistant{ set; get; }
        /// <summary>
        /// 重击抗性(意念)
        /// </summary>
        public int StunResistant { set; get; }

        #endregion
    }

    /// <summary>
    /// 战斗的单位
    /// </summary>
    public class CombatCell
    {

        //基础属性升级时,得到新一级的基础属性,并换算形成新的战斗属性
        //战斗属性为战斗即时采用的属性
        //战斗属性得自(人物属性,装备属性)

        #region 战斗属性


        #endregion
    }
}
