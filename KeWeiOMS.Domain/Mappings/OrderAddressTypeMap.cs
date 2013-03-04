//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderAddressTypeMap
    /// 订单买家地址表
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
    public class OrderAddressTypeMap : ClassMap<OrderAddressType>
    {
        public OrderAddressTypeMap()
        {
            Table("OrderAddress");
            Id(x => x.Id);
            Map(x => x.BId);
            Map(x => x.Addressee).Length(100);
            Map(x => x.Tel).Length(100);
            Map(x => x.Phone).Length(100);
            Map(x => x.Street).Length(400);
            Map(x => x.County).Length(100);
            Map(x => x.City).Length(100);
            Map(x => x.Province).Length(100);
            Map(x => x.Country).Length(100);
            Map(x => x.CountryCode).Length(100);
            Map(x => x.Email).Length(100);
            Map(x => x.PostCode).Length(100);
        }
    }
}
