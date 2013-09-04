//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// logisticsSetupType
    /// 平台物流分配表
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
    public class logisticsSetupType
    {
        /// <summary>
        /// Column_8
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Platform
        /// </summary>
        public virtual String Platform { get; set; }

        /// <summary>
        /// SetupId
        /// </summary>
        public virtual string SetupId { get; set; }

        /// <summary>
        /// SetupName
        /// </summary>
        public virtual String SetupName { get; set; }

        /// <summary>
        /// LId
        /// </summary>
        public virtual int LId { get; set; }

        /// <summary>
        /// logisticsName
        /// </summary>
        public virtual String LogisticsName { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// CreatebY
        /// </summary>
        public virtual String CreateBy { get; set; }

    }
}
