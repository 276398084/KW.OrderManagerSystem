//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// WarehouseType
    /// 仓库表
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
    public class WarehouseType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 仓库代码
        /// </summary>
        public virtual String WCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public virtual String WName { get; set; }

        /// <summary>
        /// 仓库地址
        /// </summary>
        public virtual String Address { get; set; }

    }
}
