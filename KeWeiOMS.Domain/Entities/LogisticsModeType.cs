//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsModeType
    /// 承运商明细
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
    public class LogisticsModeType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 明细名称
        /// </summary>
        public virtual String LogisticsName { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public virtual String LogisticsCode { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public virtual int ParentID { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public virtual double Discount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Remark { get; set; }

    }
}
