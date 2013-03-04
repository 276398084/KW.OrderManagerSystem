//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ProductComposeTypeMap
    /// 组合产品标记表
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
    public class ProductComposeTypeMap : ClassMap<ProductComposeType> 
    {
        public ProductComposeTypeMap()
        {
            Table("ProductCompose");
            Id(x => x.Id);
            Map(x => x.PId);
            Map(x => x.SKU).Length(50);
            Map(x => x.SrcPId);
            Map(x => x.SrcSKU).Length(50);
            Map(x => x.SrcQty);
        }
    }
}
