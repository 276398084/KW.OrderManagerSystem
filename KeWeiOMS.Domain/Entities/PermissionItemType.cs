//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PermissionItemType
    /// 操作权限项定义
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
    public class PermissionItemType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 父级主键
        /// </summary>
        public virtual int ParentId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public virtual String Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String FullName { get; set; }

        /// <summary>
        /// System\Application
        /// </summary>
        public virtual String CategoryCode { get; set; }

        /// <summary>
        /// 权限域
        /// </summary>
        public virtual int IsScope { get; set; }

        /// <summary>
        /// 是公开
        /// </summary>
        public virtual int IsPublic { get; set; }

        /// <summary>
        /// 允许编辑
        /// </summary>
        public virtual int AllowEdit { get; set; }

        /// <summary>
        /// 允许删除
        /// </summary>
        public virtual int AllowDelete { get; set; }

        /// <summary>
        /// 最后被调用日期
        /// </summary>
        public virtual DateTime LastCall { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        public virtual int Enabled { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public virtual int DeletionStateCode { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        public virtual int SortCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual String Description { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 创建用户主键
        /// </summary>
        public virtual String CreateUserId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public virtual DateTime ModifiedOn { get; set; }

        /// <summary>
        /// 修改用户主键
        /// </summary>
        public virtual String ModifiedUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        public virtual String ModifiedBy { get; set; }

    }
}
