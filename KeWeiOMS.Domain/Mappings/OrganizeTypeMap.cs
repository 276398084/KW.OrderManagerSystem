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
            Map(x => x.DeletionStateCode);
            Map(x => x.SortCode);
            Map(x => x.Description);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy);
            HasMany(x => x.children);
        }
    }
}
