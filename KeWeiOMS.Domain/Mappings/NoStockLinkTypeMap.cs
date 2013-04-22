//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// NoStockLinkTypeMap
    /// 无库存链接
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
    public class NoStockLinkTypeMap : ClassMap<NoStockLinkType> 
    {
        public NoStockLinkTypeMap()
        {
            Table("NoStockLink");
            Id(x => x.Id);
            Map(x => x.PId);
            Map(x => x.SKU).Length(255);
            Map(x => x.OldSKU).Length(255);
            Map(x => x.Supplier).Length(255);
            Map(x => x.Url).Length(255);
            Map(x => x.CreateBy).Length(255);
            Map(x => x.CreateOn);
            Map(x => x.QPrice);
            Map(x => x.Freight);
            Map(x => x.Adr).Length(255);
            Map(x => x.Received).Length(255);
        }
    }
}
