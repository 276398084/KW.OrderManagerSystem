//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// HaiItemType
    /// HaiItem
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
    public class HaiItemType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// ItemId
        /// </summary>
        public virtual String ItemId { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public virtual String Location { get; set; }

    }
}
