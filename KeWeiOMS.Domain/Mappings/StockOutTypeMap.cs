//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// StockOutTypeMap
    /// 出库记录表
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
    public class StockOutTypeMap : ClassMap<StockOutType> 
    {
        public StockOutTypeMap()
        {
            Table("StockOut");
            Id(x => x.Id);
            Map(x => x.OrderNo);
            Map(x => x.SKU);
            Map(x => x.Qty);
            Map(x => x.SourceQty);
            Map(x => x.WId);
            Map(x => x.CreateBy);
            Map(x => x.CreateOn);
        }
    }
}
