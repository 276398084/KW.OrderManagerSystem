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
            Map(x => x.MId);
            Map(x => x.OrderNo).Length(40);
            Map(x => x.OrderExNo).Length(800);
            Map(x => x.Status).Length(10);
            Map(x => x.IsPrint);
            Map(x => x.IsAudit);
            Map(x => x.IsLiu);
            Map(x => x.IsStop);
            Map(x => x.IsError);
            Map(x => x.IsMerger);
            Map(x => x.IsUpload);
            Map(x => x.Enabled);
            Map(x => x.IsSplit);
            Map(x => x.IsCanSplit);
            Map(x => x.IsOutOfStock);
            Map(x => x.IsRepeat);
            Map(x => x.CurrencyCode).Length(10);
            Map(x => x.Amount);
            Map(x => x.OrderCurrencyCode).Length(10);
            Map(x => x.OrderCurrencyCode2).Length(10);
            Map(x => x.OrderFees);
            Map(x => x.OrderFees2);
            Map(x => x.RMB);
            Map(x => x.TId).Length(800);
            Map(x => x.BuyerName).Length(200);
            Map(x => x.BuyerEmail).Length(200);
            Map(x => x.BuyerId);
            Map(x => x.BuyerMemo).CustomType("StringClob").CustomSqlType("ntext");
            Map(x => x.SellerMemo).Length(1000);
            Map(x => x.CutOffMemo).Length(1000);
            Map(x => x.LogisticMode).Length(40);
            Map(x => x.ErrorInfo).Length(500);
            Map(x => x.Country).Length(50);
            Map(x => x.TrackCode).Length(50);
            Map(x => x.TrackCode2).Length(50);
            Map(x => x.AddressId);
            Map(x => x.Weight);
            Map(x => x.Freight);
            Map(x => x.GenerateOn);
            Map(x => x.CreateOn);
            Map(x => x.ScanningOn);
            Map(x => x.ScanningBy).Length(40);
            Map(x => x.Account).Length(40);
            Map(x => x.Platform).Length(40);
            Map(x => x.UID);
        }
    }
}
