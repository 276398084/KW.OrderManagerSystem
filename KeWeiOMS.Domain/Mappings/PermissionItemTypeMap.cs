//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PermissionItemTypeMap
    /// 操作权限项定义
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
    public class PermissionItemTypeMap : ClassMap<PermissionItemType> 
    {
        public PermissionItemTypeMap()
        {
            Table("PermissionItems");
            Id(x => x.Id);
            Map(x => x.ParentId);
            Map(x => x.Code);
            Map(x => x.FullName);
            Map(x => x.CategoryCode);
            Map(x => x.IsScope);
            Map(x => x.IsPublic);
            Map(x => x.AllowEdit);
            Map(x => x.AllowDelete);
            Map(x => x.LastCall);
            Map(x => x.Enabled);
            Map(x => x.DeletionStateCode);
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
