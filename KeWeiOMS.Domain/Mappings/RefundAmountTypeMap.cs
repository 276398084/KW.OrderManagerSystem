//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// RefundAmountTypeMap
    /// 退款金额表
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
    public class RefundAmountTypeMap : ClassMap<RefundAmountType> 
    {
        public RefundAmountTypeMap()
        {
            Table("RefundAmount");
            Id(x => x.Id);
            Map(x => x.DId);
            Map(x => x.OrderNo).Length(255);
            Map(x => x.OrderExNo).Length(255);
            Map(x => x.Amount);
            Map(x => x.Platform).Length(255);
            Map(x => x.Account).Length(255);
            Map(x => x.AmountType).Length(255);
            Map(x => x.EmailAccount).Length(255);
            Map(x => x.TransactionNo).Length(255);
            Map(x => x.Status).Length(255);
            Map(x => x.AuditBy).Length(255);
            Map(x => x.AuditOn);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy).Length(255);
        }
    }
}
