//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderRecordTypeMap
    /// 订单操作日志
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
    public class OrderRecordTypeMap : ClassMap<OrderRecordType> 
    {
        public OrderRecordTypeMap()
        {
            Table("OrderRecord");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderNo).Length(40);
            Map(x => x.CreateOn);
            Map(x => x.RecordType).Length(40);
            Map(x => x.Content).Length(1000);
            Map(x => x.CreateBy).Length(40);
        }
    }
}
