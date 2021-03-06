﻿//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PurchasePlanRecordTypeMap
    /// 采购计划日志
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
    public class PurchasePlanRecordTypeMap : ClassMap<PurchasePlanRecordType> 
    {
        public PurchasePlanRecordTypeMap()
        {
            Table("PurchasePlanRecord");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.PlanNo).Length(255);
            Map(x => x.SKU).Length(255);
            Map(x => x.RecordType).Length(255);
            Map(x => x.Content).Length(2000);
            Map(x => x.CreateBy).Length(255);
            Map(x => x.CreateOn);
        }
    }
}
