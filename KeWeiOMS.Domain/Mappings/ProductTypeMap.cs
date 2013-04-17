//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ProductTypeMap
    /// 商品表
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
    public class ProductTypeMap : ClassMap<ProductType>
    {
        public ProductTypeMap()
        {
            Table("Products");
            Id(x => x.Id);
            Map(x => x.SKU).Length(50);
            Map(x => x.OldSKU).Length(50);
            Map(x => x.Category).Length(50);
            Map(x => x.ProductName).Length(200);
            Map(x => x.Price);
            Map(x => x.Standard).Length(100);
            Map(x => x.Status).Length(10);
            Map(x => x.Weight);
            Map(x => x.Long);
            Map(x => x.Wide);
            Map(x => x.High);
            Map(x => x.DayByStock);
            Map(x => x.Summary).Length(2000);
            Map(x => x.PackMemo).Length(200);
            Map(x => x.IsInfraction);
            Map(x => x.Model).Length(50);
            Map(x => x.Brand).Length(50);
            Map(x => x.PicUrl).Length(200);
            Map(x => x.PicQty);
            Map(x => x.SPicUrl).Length(200);
            Map(x => x.Purchaser).Length(50);
            Map(x => x.Examiner).Length(50);
            Map(x => x.Packer).Length(50);
            Map(x => x.PackCoefficient);
            Map(x => x.IsElectronic);
            Map(x => x.IsLiquid);
            Map(x => x.IsScan);
            Map(x => x.SevenDay);
            Map(x => x.ThirtyDay);
            Map(x => x.Fifteen);
            Map(x => x.IsZu);
            Map(x => x.HasBattery);
            Map(x => x.Location).Length(50);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy).Length(50);
            Map(x => x.Manager).Length(50);
            Map(x => x.Enabled);
        }
    }
}
