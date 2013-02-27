//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// UserType
    /// 用户表
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
    public class UserType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public virtual String Code { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public virtual String Username { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public virtual String Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public virtual String ValidateCode { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        public virtual String Realname { get; set; }

        /// <summary>
        /// 默认角色主键
        /// </summary>
        public virtual int RoleId { get; set; }

        /// <summary>
        /// 默认角色主键
        /// </summary>
        public virtual String RoleName { get; set; }

        /// <summary>
        /// 安全级别
        /// </summary>
        public virtual int SecurityLevel { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual int CId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual String CompanyName { get; set; }

        /// <summary>
        /// 部门代码
        /// </summary>
        public virtual int DId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public virtual String DepartmentName { get; set; }

        /// <summary>
        /// 工作组代码
        /// </summary>
        public virtual int WorkgroupId { get; set; }

        /// <summary>
        /// 工作组名称
        /// </summary>
        public virtual String WorkgroupName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual String Gender { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual String Telephone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual String Mobile { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public virtual String Birthday { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public virtual String Duty { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public virtual String Email { get; set; }

        /// <summary>
        /// 家庭住址
        /// </summary>
        public virtual String HomeAddress { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public virtual DateTime LastVisit { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual String AuditStatus { get; set; }

        /// <summary>
        /// 单点登录标识
        /// </summary>
        public virtual String OpenId { get; set; }

        /// <summary>
        /// 删除标志
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
        /// 菜单模块
        /// </summary>
        public virtual List<ModuleType> Modules { get; set; }

        /// <summary>
        /// 操作权限
        /// </summary>
        public virtual List<PermissionItemType> Permissions { get; set; }

        /// <summary>
        /// 账户权限
        /// </summary>
        public virtual List<AccountType> Accounts { get; set; }



    }
}
