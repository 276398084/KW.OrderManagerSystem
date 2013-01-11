//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PermissionScopeTypeMap
    /// 数据权限存储表
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
    public class PermissionScopeTypeMap : ClassMap<PermissionScopeType> 
    {
        public PermissionScopeTypeMap()
        {
            Table("PermissionScope");
            Id(x => x.Id);
            Map(x => x.ResourceCategory);
            Map(x => x.ResourceId);
            Map(x => x.TargetCategory);
            Map(x => x.TargetId);
            Map(x => x.PermissionId);
            Map(x => x.PermissionConstraint);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.Enabled);
            Map(x => x.DeletionStateCode);
            Map(x => x.Description);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy);
            Map(x => x.CreateUserId);
            Map(x => x.ModifiedOn);
            Map(x => x.ModifiedBy);
            Map(x => x.ModifiedUserId);
        }
    }
}
