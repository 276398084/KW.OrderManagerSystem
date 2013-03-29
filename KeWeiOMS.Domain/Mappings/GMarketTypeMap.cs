//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// GMarketTypeMap
    /// GMarket
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
    public class GMarketTypeMap : ClassMap<GMarketType> 
    {
        public GMarketTypeMap()
        {
            Table("GMarket");
            Id(x => x.Id);
            Map(x => x.ItemId).Length(250);
            Map(x => x.ItemTitle).Length(250);
            Map(x => x.Price).Length(250);
            Map(x => x.PicUrl).Length(250);
            Map(x => x.NowNum);
            Map(x => x.Qty);
            Map(x => x.ProductUrl).Length(250);
            Map(x => x.CreateOn);
            Map(x => x.Account).Length(250);
        }
    }
}
