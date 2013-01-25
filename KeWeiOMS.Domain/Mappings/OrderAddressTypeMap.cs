//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderAddresTypeMap
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
            Map(x => x.Addressee);
            Map(x => x.Tel);
            Map(x => x.Phone);
            Map(x => x.Street);
            Map(x => x.County);
            Map(x => x.City);
            Map(x => x.Province);
            Map(x => x.Country);
            Map(x => x.CountryCode);
        }
    }
}
