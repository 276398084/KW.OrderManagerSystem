//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PermissionScopeType
    /// 操作权限域
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
    public class PermissionScopeType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 什么类型的
        /// </summary>
        public virtual String ResourceCategory { get; set; }

        /// <summary>
        /// 什么资源
        /// </summary>
        public virtual int ResourceId { get; set; }

        /// <summary>
        /// 对什么类型的
        /// </summary>
        public virtual String TargetCategory { get; set; }

        /// <summary>
        /// 对什么资源
        /// </summary>
        public virtual int TargetId { get; set; }

    }
}
