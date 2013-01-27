//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderProductTypeMap
    /// 订单商品表
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
    public class OrderProductTypeMap : ClassMap<OrderProductType> 
    {
        public OrderProductTypeMap()
        {
            Table("OrderProducts");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderNo).Length(40);
            Map(x => x.ExSKU).Length(40);
            Map(x => x.Title).Length(400);
            Map(x => x.Qty);
            Map(x => x.SKU).Length(40);
            Map(x => x.Remark).Length(400);
            Map(x => x.Standard).Length(400);
            Map(x => x.Price);
            Map(x => x.Url).Length(200);
        }
    }
}
