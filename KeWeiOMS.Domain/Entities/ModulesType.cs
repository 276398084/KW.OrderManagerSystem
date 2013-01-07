//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ModulesType
    /// 模块（菜单）表
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
    public class ModulesType
    {
        public ModulesType()
        {

        }
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 父节点主键
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
        public virtual String Category { get; set; }

        /// <summary>
        /// 图标编号
        /// </summary>
        public virtual String ImageIndex { get; set; }

        /// <summary>
        /// 选中状态图标编号
        /// </summary>
        public virtual String SelectedImageIndex { get; set; }

        /// <summary>
        /// 导航地址
        /// </summary>
        public virtual String NavigateUrl { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public virtual String Target { get; set; }

        /// <summary>
        /// 是公开
        /// </summary>
        public virtual int IsPublic { get; set; }

        /// <summary>
        /// 是菜单
        /// </summary>
        public virtual int IsMenu { get; set; }

        /// <summary>
        /// 展开状态
        /// </summary>
        public virtual int Expand { get; set; }

        /// <summary>
        /// 操作权限编号(数据权限范围)
        /// </summary>
        public virtual String PermissionItemCode { get; set; }

        /// <summary>
        /// 需要数据权限过滤的表(,符号分割)
        /// </summary>
        public virtual String PermissionScopeTables { get; set; }

        /// <summary>
        /// 允许编辑
        /// </summary>
        public virtual int AllowEdit { get; set; }

        /// <summary>
        /// 允许删除
        /// </summary>
        public virtual int AllowDelete { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        public virtual int SortCode { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public virtual int DeletionStateCode { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        public virtual int Enabled { get; set; }

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