//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ProductCategoryTypeMap
    /// 类目
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
    public class ProductCategoryTypeMap : ClassMap<ProductCategoryType> 
    {
        public ProductCategoryTypeMap()
        {
            Table("ProductCategory");
            Id(x => x.Id);
            Map(x => x.ParentId);
            Map(x => x.Name).Length(50);
            Map(x => x.SortCode);
        }
    }
}
