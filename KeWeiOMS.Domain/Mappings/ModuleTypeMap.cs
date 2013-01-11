//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ModuleTypeMap
    /// 模块（菜单）表
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
    public class ModuleTypeMap : ClassMap<ModuleType> 
    {
        public ModuleTypeMap()
        {
            Table("Modules");
            Id(x => x.Id);
            Map(x => x.ParentId);
            Map(x => x.Code);
            Map(x => x.FullName);
            Map(x => x.Category);
            Map(x => x.ImageIndex);
            Map(x => x.SelectedImageIndex);
            Map(x => x.NavigateUrl);
            Map(x => x.Target);
            Map(x => x.IsPublic);
            Map(x => x.IsMenu);
            Map(x => x.Expand);
            Map(x => x.PermissionItemCode);
            Map(x => x.PermissionScopeTables);
            Map(x => x.AllowEdit);
            Map(x => x.AllowDelete);
            Map(x => x.SortCode);
            Map(x => x.DeletionStateCode);
            Map(x => x.Enabled);
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
