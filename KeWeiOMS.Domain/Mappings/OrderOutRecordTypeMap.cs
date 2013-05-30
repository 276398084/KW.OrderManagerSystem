//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderOutRecordTypeMap
    /// 订单缺货表
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
    public class OrderOutRecordTypeMap : ClassMap<OrderOutRecordType>
    {
        public OrderOutRecordTypeMap()
        {
            Table("OrderOutRecord");
            Id(x => x.Id);
            Map(x => x.CreateBy);
            Map(x => x.OrderNo);
            Map(x => x.OId);
            Map(x => x.OrderExNo);
            Map(x => x.IsRestoration);
            Map(x => x.RestorationBy);
            Map(x => x.RestorationOn);
            Map(x => x.CreateOn);
            Map(x => x.Remark).Length(2000);

        }
    }
}
