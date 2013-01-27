//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderBuyerTypeMap
    /// 订单买家表
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
    public class OrderBuyerTypeMap : ClassMap<OrderBuyerType> 
    {
        public OrderBuyerTypeMap()
        {
            Table("OrderBuyer");
            Id(x => x.Id);
            Map(x => x.BuyerName).Length(40);
            Map(x => x.BuyerEmail).Length(100);
            Map(x => x.BuyCount);
            Map(x => x.BuyAmount);
            Map(x => x.FristBuyOn);
            Map(x => x.ListBuyOn);
            Map(x => x.Remark).Length(400);
            Map(x => x.BuyerType).Length(40);
            Map(x => x.Platform).Length(40);
        }
    }
}
