//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// WarehouseStockTypeMap
    /// 仓库库存表
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
    public class WarehouseStockTypeMap : ClassMap<WarehouseStockType> 
    {
        public WarehouseStockTypeMap()
        {
            Table("WarehouseStock");
            Id(x => x.Id);
            Map(x => x.WId);
            Map(x => x.Warehouse).Length(50);
            Map(x => x.PId);
            Map(x => x.SKU).Length(50);
            Map(x => x.Title).Length(200);
            Map(x => x.Qty);
            Map(x => x.UpdateOn);
        }
    }
}
