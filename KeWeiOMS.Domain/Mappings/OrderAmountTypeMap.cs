//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderAmountTypeMap
    /// 订单金额表
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
    public class OrderAmountTypeMap : ClassMap<OrderAmountType>
    {
        public OrderAmountTypeMap()
        {
            Table("OrderAmount");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderNo).Length(20);
            Map(x => x.OrderExNo).Length(50);
            Map(x => x.AgainCount);
            Map(x => x.SplitCount);
            Map(x => x.TotalFreight);
            Map(x => x.TotalCosts);
            Map(x => x.Fee);
            Map(x => x.TransactionFees);
            Map(x => x.OtherFees);
            Map(x => x.OrderAmount);
            Map(x => x.CurrencyCode).Length(20);
            Map(x => x.ExchangeRate);
            Map(x => x.RMB);
            Map(x => x.Status).Length(20);
            Map(x => x.Account).Length(50);
            Map(x => x.Platform).Length(20);
        }
    }
}
