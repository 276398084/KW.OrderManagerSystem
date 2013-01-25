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
            Map(x => x.SKU);
            Map(x => x.Category);
            Map(x => x.ProductName);
            Map(x => x.Price);
            Map(x => x.Weight);
            Map(x => x.Long);
            Map(x => x.Wide);
            Map(x => x.High);
            Map(x => x.DayByStock);
            Map(x => x.Summary);
            Map(x => x.PackMemo);
            Map(x => x.IsInfraction);
            Map(x => x.Model);
            Map(x => x.Brand);
            Map(x => x.PicUrl);
            Map(x => x.PicQty);
            Map(x => x.SPicUrl);
            Map(x => x.Purchaser);
            Map(x => x.Examiner);
            Map(x => x.Packer);
            Map(x => x.PackCoefficient);
            Map(x => x.IsElectronic);
            Map(x => x.HasBattery);
            Map(x => x.Location);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy);
        }
    }
}
