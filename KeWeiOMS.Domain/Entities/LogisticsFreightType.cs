//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsFreightType
    /// 物流费用表
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0  创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name></name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class LogisticsFreightType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 分区代码
        /// </summary>
        public virtual int AreaCode { get; set; }

        /// <summary>
        /// 开始
        /// </summary>
        public virtual double BeginWeight { get; set; }

        /// <summary>
        /// 结束
        /// </summary>
        public virtual double EndWeight { get; set; }

        /// <summary>
        /// 首重
        /// </summary>
        public virtual double FristWeight { get; set; }

        /// <summary>
        /// 续重
        /// </summary>
        public virtual double IncrementWeight { get; set; }

        /// <summary>
        /// 起步费
        /// </summary>
        public virtual double FristFreight { get; set; }

        /// <summary>
        /// 递增
        /// </summary>
        public virtual double IncrementFreight { get; set; }

        /// <summary>
        /// 每克费用
        /// </summary>
        public virtual double EveryFee { get; set; }

        /// <summary>
        /// 出力费
        /// </summary>
        public virtual double ProcessingFee { get; set; }

        /// <summary>
        /// 是否全部打折
        /// </summary>
        public virtual int IsDiscountALL { get; set; }

    }
}
