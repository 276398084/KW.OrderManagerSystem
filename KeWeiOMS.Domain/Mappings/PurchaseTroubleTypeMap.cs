//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PurchaseTroubleTypeMap
    /// 采购问题表
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
    public class PurchaseTroubleTypeMap : ClassMap<PurchaseTroubleType> 
    {
        public PurchaseTroubleTypeMap()
        {
            Table("PurchaseTrouble");
            Id(x => x.Id);
            Map(x => x.Status).Length(250);
            Map(x => x.PurchaseId);
            Map(x => x.PurchaseCode).Length(250);
            Map(x => x.SKU).Length(250);
            Map(x => x.Qty);
            Map(x => x.Price);
            Map(x => x.Freight);
            Map(x => x.Supplier).Length(250);
            Map(x => x.LogisticsMode).Length(250);
            Map(x => x.LogisticsCode).Length(250);
            Map(x => x.BuyOn);
            Map(x => x.ReceiveOn);
            Map(x => x.TroubleType).Length(250);
            Map(x => x.TroubleDetail).Length(250);
            Map(x => x.CreateBy).Length(250);
            Map(x => x.CreateOn);
            Map(x => x.DealBy).Length(250);
            Map(x => x.DealOn);
            Map(x => x.SolutionType).Length(250);
            Map(x => x.SolutionDetail).Length(250);
        }
    }
}
