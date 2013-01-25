//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderTypeMap
    /// 订单表
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
    public class OrderTypeMap : ClassMap<OrderType> 
    {
        public OrderTypeMap()
        {
            Table("Orders");
            Id(x => x.Id);
            Map(x => x.OrderNo);
            Map(x => x.OrderExNo);
            Map(x => x.Status);
            Map(x => x.IsMerger);
            Map(x => x.IsSplit);
            Map(x => x.IsOutOfStock);
            Map(x => x.IsRepeat);
            Map(x => x.CurrencyCode);
            Map(x => x.Amount);
            Map(x => x.TId);
            Map(x => x.BuyerName);
            Map(x => x.BuyerEmail);
            Map(x => x.BuyerId);
            Map(x => x.BuyerMemo);
            Map(x => x.SellerMemo);
            Map(x => x.CutOffMemo);
            Map(x => x.LogisticMode);
            Map(x => x.Country);
            Map(x => x.AddressId);
            Map(x => x.Weight);
            Map(x => x.Freight);
            Map(x => x.GenerateOn);
            Map(x => x.CreateOn);
            Map(x => x.ScanningOn);
            Map(x => x.ScanningBy);
        }
    }
}
