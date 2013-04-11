//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EbayTypeMap
    /// Ebay
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
    public class EbayTypeMap : ClassMap<EbayType>
    {
        public EbayTypeMap()
        {
            Table("Ebay");
            Id(x => x.Id);
            Map(x => x.ItemId).Length(250);
            Map(x => x.ItemTitle).Length(250);
            Map(x => x.Currency).Length(250);
            Map(x => x.Price).Length(250);
            Map(x => x.PicUrl).Length(250);
            Map(x => x.StartNum);
            Map(x => x.NowNum);
            Map(x => x.SKU);
            Map(x => x.ProductUrl).Length(250);
            Map(x => x.StartTime);
            Map(x => x.CreateOn);
            Map(x => x.Account).Length(250);
            Map(x => x.Status).Length(250);
        }
    }
}
