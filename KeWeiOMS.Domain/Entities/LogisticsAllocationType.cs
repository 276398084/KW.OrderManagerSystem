//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsAllocationType
    /// 承运商划分
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
    public class LogisticsAllocationType
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 1.金额
        //2.国家
        //3.产品
        //4.重量
        /// </summary>
        public virtual int AllocationType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string AId { get; set; }

        /// <summary>
        /// 查询SQL
        /// </summary>
        public virtual String QuerySql { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        public virtual String LogisticsMode { get; set; }

        /// <summary>
        /// 开始
        /// </summary>
        public virtual double NBegin { get; set; }

        /// <summary>
        /// 结束
        /// </summary>
        public virtual double NEnd { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 优先级 数字越大越靠前
        /// </summary>
        public virtual int SortCode { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
