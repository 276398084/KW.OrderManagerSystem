﻿//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// RoleType
    /// 系统角色表
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
    public class RoleType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 组织机构主键
        /// </summary>
        public virtual int OrganizeId { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public virtual String Code { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public virtual String Realname { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        public virtual int SortCode { get; set; }

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

    }
}
