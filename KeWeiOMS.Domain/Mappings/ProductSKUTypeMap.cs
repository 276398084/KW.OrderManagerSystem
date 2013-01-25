//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ProductSKUTypeMap
    /// 商品SKU表
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
    public class ProductSKUTypeMap : ClassMap<ProductSKUType> 
    {
        public ProductSKUTypeMap()
        {
            Table("ProductSKU");
            Id(x => x.Id);
            Map(x => x.ParentSKU);
            Map(x => x.SKU);
            Map(x => x.Price);
            Map(x => x.Memo);
            Map(x => x.Qty);
            Map(x => x.DayOfStock);
        }
    }
}
