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
        /// 姓名
        /// </summary>
        public virtual String Realname { get; set; }

        /// <summary>
        /// 默认角色主键
        /// </summary>
        public virtual int RoleId { get; set; }

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
        /// 最后修改密码日期
        /// </summary>
        public virtual DateTime ChangePasswordDate { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public virtual String OICQ { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public virtual String Email { get; set; }

        /// <summary>
        /// 家庭住址
        /// </summary>
        public virtual String HomeAddress { get; set; }

        /// <summary>
        /// 允许登录时间开始
        /// </summary>
        public virtual DateTime AllowStartTime { get; set; }

        /// <summary>
        /// 允许登录时间结束
        /// </summary>
        public virtual DateTime AllowEndTime { get; set; }

        /// <summary>
        /// 暂停用户开始日期
        /// </summary>
        public virtual DateTime LockStartDate { get; set; }

        /// <summary>
        /// 暂停用户结束日期
        /// </summary>
        public virtual DateTime LockEndDate { get; set; }

        /// <summary>
        /// 第一次登录时间
        /// </summary>
        public virtual DateTime FirstVisit { get; set; }

        /// <summary>
        /// 上一次登录时间
        /// </summary>
        public virtual DateTime PreviousVisit { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public virtual DateTime LastVisit { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public virtual int LogOnCount { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual String AuditStatus { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public virtual int IsVisible { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public virtual int UserOnLine { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public virtual String IPAddress { get; set; }

        /// <summary>
        /// MAC地址
        /// </summary>
        public virtual String MACAddress { get; set; }

        /// <summary>
        /// 单点登录标识
        /// </summary>
        public virtual String OpenId { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public virtual int DeletionStateCode { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        public virtual int Enabled { get; set; }

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
