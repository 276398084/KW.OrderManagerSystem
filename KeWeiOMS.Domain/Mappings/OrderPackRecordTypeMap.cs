//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderPackRecordTypeMap
    /// 包装记录表
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
    public class OrderPackRecordTypeMap : ClassMap<OrderPackRecordType> 
    {
        public OrderPackRecordTypeMap()
        {
            Table("OrderPackRecord");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderNo).Length(50);
            Map(x => x.PackBy).Length(50);
            Map(x => x.ScanBy).Length(50);
            Map(x => x.PackOn);
            Map(x => x.PackCoefficient);
            Map(x => x.SKU);
        }
    }
}
