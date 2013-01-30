//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// WarehouseLocationType
    /// 库位表
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
    public class WarehouseLocationType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public virtual int WId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public virtual String Warehouse { get; set; }

        /// <summary>
        /// 仓位名称
        /// </summary>
        public virtual String PositionsName { get; set; }

    }
}
