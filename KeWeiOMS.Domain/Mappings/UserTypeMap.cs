//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// UserTypeMap
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
    public class UserTypeMap : ClassMap<UserType> 
    {
        public UserTypeMap()
        {
            Table("Users");
            Id(x => x.Id);
            Map(x => x.Code);
            Map(x => x.Username);
            Map(x => x.Password);
            Map(x => x.Realname);
            Map(x => x.RoleId);
            Map(x => x.SecurityLevel);
            Map(x => x.CId);
            Map(x => x.CompanyName);
            Map(x => x.DId);
            Map(x => x.DepartmentName);
            Map(x => x.WorkgroupId);
            Map(x => x.WorkgroupName);
            Map(x => x.Gender);
            Map(x => x.Telephone);
            Map(x => x.Mobile);
            Map(x => x.Birthday);
            Map(x => x.Duty);
            Map(x => x.Email);
            Map(x => x.HomeAddress);
            Map(x => x.LastVisit);
            Map(x => x.AuditStatus);
            Map(x => x.OpenId);
            Map(x => x.DeletionStateCode);
            Map(x => x.SortCode);
            Map(x => x.Description);
            Map(x => x.CreateOn);
            Map(x => x.CreateUserId);
            Map(x => x.CreateBy);
        }
    }
}
