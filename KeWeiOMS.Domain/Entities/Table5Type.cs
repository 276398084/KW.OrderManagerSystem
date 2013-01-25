//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// Table5Type
    /// 平台账户费用表
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
    public class Table5Type
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 账户ID
        /// </summary>
        public virtual int AccountId { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public virtual String AccountName { get; set; }

        /// <summary>
        /// 总金额开始
        /// </summary>
        public virtual String AmountBegin { get; set; }

        /// <summary>
        /// 总金额结束
        /// </summary>
        public virtual String AmountEnd { get; set; }

        /// <summary>
        /// 公式
        /// </summary>
        public virtual String FeeFormula { get; set; }

        /// <summary>
        /// 费用名称
        /// </summary>
        public virtual String FeeName { get; set; }

    }
}
