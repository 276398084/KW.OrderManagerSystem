//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderWeightRecordTypeMap
    /// 订单重量扫描结果
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0 XiDong 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>XiDong</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class OrderWeightRecordTypeMap : ClassMap<OrderWeightRecordType> 
    {
        public OrderWeightRecordTypeMap()
        {
            Table("OrderWeightRecord");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderExNo).Length(200);
            Map(x => x.MaxWeight);
            Map(x => x.MinWeight);
            Map(x => x.CreateBy).Length(200);
            Map(x => x.Weight);
            Map(x => x.SKU).Length(200);
            Map(x => x.Qty);
            Map(x => x.CreateOn);
        }
    }
}
