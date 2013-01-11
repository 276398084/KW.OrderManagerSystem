//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PermissionScopeType
    /// 数据权限存储表
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
        public virtual String ResourceId { get; set; }

        /// <summary>
        /// 对什么类型的
        /// </summary>
        public virtual String TargetCategory { get; set; }

        /// <summary>
        /// 对什么资源
        /// </summary>
        public virtual String TargetId { get; set; }

        /// <summary>
        /// 有什么权限
        /// </summary>
        public virtual int PermissionId { get; set; }

        /// <summary>
        /// 权限约束表达式
        /// </summary>
        public virtual String PermissionConstraint { get; set; }

        /// <summary>
        /// 开始生效日期
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// 结束生效日期
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        public virtual int Enabled { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public virtual int DeletionStateCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual String Description { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 创建用户主键
        /// </summary>
        public virtual String CreateUserId { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public virtual DateTime ModifiedOn { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        public virtual String ModifiedBy { get; set; }

        /// <summary>
        /// 修改用户主键
        /// </summary>
        public virtual String ModifiedUserId { get; set; }

    }
}
