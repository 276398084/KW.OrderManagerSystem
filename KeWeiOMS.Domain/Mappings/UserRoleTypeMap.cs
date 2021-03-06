﻿//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// UserRoleTypeMap
    /// 用户角色关系表
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
    public class UserRoleTypeMap : ClassMap<UserRoleType> 
    {
        public UserRoleTypeMap()
        {
            Table("UserRole");
            Id(x => x.Id);
            Map(x => x.UserId);
            Map(x => x.RoleId);
            Map(x => x.Description).Length(800);
            Map(x => x.DeletionStateCode);
            Map(x => x.SortCode);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy).Length(50);
        }
    }
}
