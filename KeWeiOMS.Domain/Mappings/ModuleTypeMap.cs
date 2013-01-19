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
            Map(x => x.ImageIndex);
            Map(x => x.NavigateUrl);
            Map(x => x.SortCode);
            Map(x => x.DeletionStateCode);
            Map(x => x.Description);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy);
            HasMany(x => x.children);
        }
    }
}
