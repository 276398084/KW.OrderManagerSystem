//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// SettlementType
    /// 结算管理
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0 XiDong 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>XiDong</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class SettlementType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public virtual double Amount { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        public virtual String Account { get; set; }

        /// <summary>
        /// Memo
        /// </summary>
        public virtual String Memo { get; set; }

    }
}
