//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// MessageAmountTypeMap
    /// 邮件记事表
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
    public class MessageAmountTypeMap : ClassMap<MessageAmountType> 
    {
        public MessageAmountTypeMap()
        {
            Table("MessageAmount");
            Id(x => x.Id);
            Map(x => x.OrderNo).Length(40);
            Map(x => x.OrderExNo).Length(50);
            Map(x => x.Platform).Length(50);
            Map(x => x.Account).Length(50);
            Map(x => x.GenerateOn);
            Map(x => x.SendOn);
            Map(x => x.Amount);
            Map(x => x.CurrencyCode).Length(50);
            Map(x => x.SKU).Length(50);
            Map(x => x.Qty);
            Map(x => x.LogisticsMode).Length(50);
            Map(x => x.Status);
            Map(x => x.Solution).Length(50);
            Map(x => x.RefundAmount);
            Map(x => x.CreateBy).Length(50);
            Map(x => x.CreateOn);
            Map(x => x.SolveBy).Length(50);
            Map(x => x.SloveOn);
        }
    }
}
