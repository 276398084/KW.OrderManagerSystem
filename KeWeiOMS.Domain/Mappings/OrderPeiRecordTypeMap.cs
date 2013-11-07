//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderPeiRecordTypeMap
    /// 配货记录表
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
    public class OrderPeiRecordTypeMap : ClassMap<OrderPeiRecordType>
    {
        public OrderPeiRecordTypeMap()
        {
            Table("OrderPeiRecord");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderNo).Length(50);
            Map(x => x.CreateOn);
            Map(x => x.PeiBy).Length(50);
            Map(x => x.ValiBy).Length(50);
            Map(x => x.ScanBy).Length(50);
            Map(x => x.PackBy).Length(50);
        }
    }
}
