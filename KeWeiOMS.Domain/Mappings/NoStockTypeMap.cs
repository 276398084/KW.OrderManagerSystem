//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// NoStockTypeMap
    /// 无库存商品
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
    public class NoStockTypeMap : ClassMap<NoStockType> 
    {
        public NoStockTypeMap()
        {
            Table("NoStock");
            Id(x => x.Id);
            Map(x => x.SKU).Length(255);
            Map(x => x.OldSKU).Length(255);
            Map(x => x.Name).Length(255);
            Map(x => x.PicUrl).Length(255);
            Map(x => x.Standard).Length(255);
            Map(x => x.Price);
            Map(x => x.CreateBy).Length(255);
            Map(x => x.CreateOn);
            Map(x => x.Enabled);
        }
    }
}
