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
            Map(x => x.OrderNo).Length(40);
            Map(x => x.OrderExNo).Length(40);
            Map(x => x.Status).Length(10);
            Map(x => x.IsPrint);
            Map(x => x.IsMerger);
            Map(x => x.IsSplit);
            Map(x => x.IsOutOfStock);
            Map(x => x.IsRepeat);
            Map(x => x.CurrencyCode).Length(10);
            Map(x => x.Amount);
            Map(x => x.TId).Length(40);
            Map(x => x.BuyerName).Length(40);
            Map(x => x.BuyerEmail).Length(80);
            Map(x => x.BuyerId);
            Map(x => x.BuyerMemo).Length(1000);
            Map(x => x.SellerMemo).Length(1000);
            Map(x => x.CutOffMemo).Length(300);
            Map(x => x.LogisticMode).Length(40);
            Map(x => x.ErrorInfo).Length(500);
            Map(x => x.Country).Length(40);
            //Map(x => x.AddressId);
            Map(x => x.Weight);
            Map(x => x.Freight);
            Map(x => x.GenerateOn);
            Map(x => x.CreateOn);
            Map(x => x.ScanningOn);
            Map(x => x.ScanningBy).Length(40);
            Map(x => x.Account).Length(40);
            Map(x => x.Platform).Length(40);
            //HasMany<OrderProductType>(u => u.Products).AsSet().KeyColumn("OId").Cascade.All();

            References<OrderAddressType>(x => x.AddressInfo).Column("AddressId");
        }
    }
}
