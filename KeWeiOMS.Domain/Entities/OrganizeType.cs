//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrganizeType
    /// 组织机构、部门表
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
    public class OrganizeType
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
        /// 简称
        /// </summary>
        public virtual String ShortName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String FullName { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public virtual String Category { get; set; }

        /// <summary>
        /// 层
        /// </summary>
        public virtual int Layer { get; set; }

        /// <summary>
        /// 外线电话
        /// </summary>
        public virtual String OuterPhone { get; set; }

        /// <summary>
        /// 内线电话
        /// </summary>
        public virtual String InnerPhone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public virtual String Fax { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public virtual String Postalcode { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public virtual String Address { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public virtual String Web { get; set; }

        /// <summary>
        /// 领导
        /// </summary>
        public virtual String LeadershipRoleId { get; set; }

        /// <summary>
        /// 分管领导
        /// </summary>
        public virtual String AssistantLeadershipRoleId { get; set; }

        /// <summary>
        /// 主负责人
        /// </summary>
        public virtual String ManagerRoleId { get; set; }

        /// <summary>
        /// 副负责人
        /// </summary>
        public virtual String AssistantManagerRoleId { get; set; }

        /// <summary>
        /// 财务主管
        /// </summary>
        public virtual String FinancialRoleId { get; set; }

        /// <summary>
        /// 会计
        /// </summary>
        public virtual String AccountingRoleId { get; set; }

        /// <summary>
        /// 出纳
        /// </summary>
        public virtual String CashierRoleId { get; set; }

        /// <summary>
        /// 系统管理员
        /// </summary>
        public virtual String SystemManagerRoleId { get; set; }

        /// <summary>
        /// 内部组织机构
        /// </summary>
        public virtual int IsInnerOrganize { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public virtual String Bank { get; set; }

        /// <summary>
        /// 银行帐号
        /// </summary>
        public virtual String BankAccount { get; set; }

        /// <summary>
        /// 删除标记
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
