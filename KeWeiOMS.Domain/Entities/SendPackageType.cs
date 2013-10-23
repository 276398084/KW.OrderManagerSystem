//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AccountFeeType
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
    public class SendPackageType
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 包名称
        /// </summary>
        public virtual String PackageName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreateBy { get; set; }

        /// <summary>
        /// 包裹数量
        /// </summary>
        public virtual int PCount { get; set; }

        /// <summary>
        /// 包裹重量
        /// </summary>
        public virtual double PWeight { get; set; }



    }
}
