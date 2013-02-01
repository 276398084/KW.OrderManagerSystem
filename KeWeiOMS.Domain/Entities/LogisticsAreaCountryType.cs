//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsAreaCountryType
    /// 分区国家表
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
    public class LogisticsAreaCountryType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 分区ID
        /// </summary>
        public virtual int AreaCode { get; set; }

        /// <summary>
        /// 国家代码
        /// </summary>
        public virtual String CountryCode { get; set; }

        /// <summary>
        /// 国家名称
        /// </summary>
        public virtual String Country { get; set; }

    }
}
