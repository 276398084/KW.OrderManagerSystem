//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// SKUCodeType
    /// SKUCode
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
    public class SKUCodeType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// IsOut
        /// </summary>
        public virtual int IsOut { get; set; }
        /// <summary>
        /// IsOut
        /// </summary>
        public virtual int IsNew { get; set; }

        /// <summary>
        /// SendOn
        /// </summary>
        public virtual string SendOn { get; set; }



    }
}
