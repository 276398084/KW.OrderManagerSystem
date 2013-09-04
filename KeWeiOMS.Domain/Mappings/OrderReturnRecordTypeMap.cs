//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderReturnRecordTypeMap
    /// 退件记录
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0 XiDong 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>XiDong</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class OrderReturnRecordTypeMap : ClassMap<OrderReturnRecordType> 
    {
        public OrderReturnRecordTypeMap()
        {
            Table("OrderReturnRecord");
            Id(x => x.Id);
            Map(x => x.OrderNo).Length(200);
            Map(x => x.OrderExNO).Length(800);
            Map(x => x.OId);
            Map(x => x.ReturnType).Length(200);
            Map(x => x.ReturnLogisticsMode).Length(200);
            Map(x => x.NewLogisticsMode).Length(200);
            Map(x => x.OldTrackCode).Length(200);
            Map(x => x.NewTrackCode).Length(200);
            Map(x => x.BuyerName).Length(800);
            Map(x => x.Account).Length(200);
            Map(x => x.Platform).Length(200);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy).Length(200);
            Map(x => x.Amount);
            Map(x => x.Country).Length(200);
            Map(x => x.CurrencyCode).Length(200);
            Map(x => x.OrderCreateOn);
            Map(x => x.OrderSendOn);
        }
    }
}
