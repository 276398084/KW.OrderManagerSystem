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
            Map(x => x.OrderNo);
            Map(x => x.ExSKU);
            Map(x => x.Title);
            Map(x => x.Qty);
            Map(x => x.SKU);
            Map(x => x.Remark);
            Map(x => x.Standard);
            Map(x => x.Price);
            Map(x => x.Url);
        }
    }
}
