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
            Map(x => x.PlanNo).Length(50);
            Map(x => x.SKU).Length(50);
            Map(x => x.Price);
            Map(x => x.Qty);
            Map(x => x.DaoQty);
            Map(x => x.Freight);
            Map(x => x.ProductName).Length(200);
            Map(x => x.ProductUrl).Length(300);
            Map(x => x.PicUrl).Length(300);
            Map(x => x.Suppliers).Length(50);
            Map(x => x.SId);
            Map(x => x.LogisticsMode).Length(50);
            Map(x => x.TrackCode).Length(50);
            Map(x => x.PostStatus).Length(50);
            Map(x => x.Status).Length(20);
            Map(x => x.BuyBy).Length(50);
            Map(x => x.CreateBy).Length(50);
            Map(x => x.CreateOn);
            Map(x => x.BuyOn);
            Map(x => x.SendOn);
            Map(x => x.ReceiveOn);
            Map(x => x.Memo).Length(800);
        }
    }
}
