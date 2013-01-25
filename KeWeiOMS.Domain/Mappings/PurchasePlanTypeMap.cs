//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PurchasePlanTypeMap
    /// 采购计划表
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
    public class PurchasePlanTypeMap : ClassMap<PurchasePlanType> 
    {
        public PurchasePlanTypeMap()
        {
            Table("PurchasePlan");
            Id(x => x.Id);
            Map(x => x.PlanNo);
            Map(x => x.SKU);
            Map(x => x.Price);
            Map(x => x.Qty);
            Map(x => x.Freight);
            Map(x => x.ProductName);
            Map(x => x.Suppliers);
            Map(x => x.SId);
            Map(x => x.LogisticsMode);
            Map(x => x.TrackCode);
            Map(x => x.PostStatus);
            Map(x => x.Status);
            Map(x => x.BuyBy);
            Map(x => x.CreateBy);
            Map(x => x.CreateOn);
            Map(x => x.BuyOn);
            Map(x => x.SendOn);
            Map(x => x.ReceiveOn);
        }
    }
}
