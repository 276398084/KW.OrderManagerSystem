//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrganizeTypeMap
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
    public class OrganizeTypeMap : ClassMap<OrganizeType> 
    {
        public OrganizeTypeMap()
        {
            Table("Organizes");
            Id(x => x.Id);
            Map(x => x.ParentId);
            Map(x => x.Code);
            Map(x => x.ShortName);
            Map(x => x.FullName);
            Map(x => x.Category);
            Map(x => x.Layer);
            Map(x => x.OuterPhone);
            Map(x => x.InnerPhone);
            Map(x => x.Fax);
            Map(x => x.Postalcode);
            Map(x => x.Address);
            Map(x => x.Web);
            Map(x => x.LeadershipRoleId);
            Map(x => x.AssistantLeadershipRoleId);
            Map(x => x.ManagerRoleId);
            Map(x => x.AssistantManagerRoleId);
            Map(x => x.FinancialRoleId);
            Map(x => x.AccountingRoleId);
            Map(x => x.CashierRoleId);
            Map(x => x.SystemManagerRoleId);
            Map(x => x.IsInnerOrganize);
            Map(x => x.Bank);
            Map(x => x.BankAccount);
            Map(x => x.DeletionStateCode);
            Map(x => x.Enabled);
            Map(x => x.SortCode);
            Map(x => x.Description);
            Map(x => x.CreateOn);
            Map(x => x.CreateUserId);
            Map(x => x.CreateBy);
            Map(x => x.ModifiedOn);
            Map(x => x.ModifiedUserId);
            Map(x => x.ModifiedBy);
        }
    }
}
