//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// CurrencyType
    /// 汇率表
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
    public class CurrencyType
    {
        /// <summary>
        /// 主键表
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public virtual String CurrencyName { get; set; }



        /// <summary>
        /// 货币
        /// </summary>
        public virtual String CurrencyCode { get; set; }
        /// <summary>
        /// 符号
        /// </summary>
        public virtual String CurrencySign { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public virtual double CurrencyValue { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
