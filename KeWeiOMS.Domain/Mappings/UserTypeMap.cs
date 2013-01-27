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
            Map(x => x.Code).Length(50);
            Map(x => x.Username).Length(50);
            Map(x => x.Password).Length(200);
            Map(x => x.Realname).Length(50);
            Map(x => x.RoleId);
            Map(x => x.SecurityLevel);
            Map(x => x.CId);
            Map(x => x.CompanyName).Length(50);
            Map(x => x.DId);
            Map(x => x.DepartmentName).Length(50);
            Map(x => x.WorkgroupId);
            Map(x => x.WorkgroupName).Length(50);
            Map(x => x.Gender).Length(50);
            Map(x => x.Telephone).Length(50);
            Map(x => x.Mobile).Length(50);
            Map(x => x.Birthday).Length(50);
            Map(x => x.Duty).Length(50);
            Map(x => x.Email).Length(200);
            Map(x => x.HomeAddress).Length(200);
            Map(x => x.LastVisit);
            Map(x => x.AuditStatus).Length(50);
            Map(x => x.OpenId).Length(50);
            Map(x => x.DeletionStateCode);
            Map(x => x.SortCode);
            Map(x => x.Description).Length(800);
            Map(x => x.CreateOn);
            Map(x => x.CreateUserId).Length(50);
            Map(x => x.CreateBy).Length(50);
        }
    }
}
