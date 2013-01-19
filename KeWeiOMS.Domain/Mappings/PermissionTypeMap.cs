//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PermissionTypeMap
    /// 操作权限存储表
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
    public class PermissionTypeMap : ClassMap<PermissionType> 
    {
        public PermissionTypeMap()
        {
            Table("Permissions");
            Id(x => x.Id);
            Map(x => x.ResourceCategory);
            Map(x => x.ResourceId);
            Map(x => x.PermissionId);
        }
    }
}
