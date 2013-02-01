//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsFreightTypeMap
    /// 物流费用表
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
    public class LogisticsFreightTypeMap : ClassMap<LogisticsFreightType> 
    {
        public LogisticsFreightTypeMap()
        {
            Table("LogisticsFreight");
            Id(x => x.Id);
            Map(x => x.AreaCode);
            Map(x => x.BeginWeight);
            Map(x => x.EndWeight);
            Map(x => x.FristWeight);
            Map(x => x.IncrementWeight);
            Map(x => x.FristFreight);
            Map(x => x.IncrementFreight);
            Map(x => x.EveryFee);
            Map(x => x.ProcessingFee);
            Map(x => x.IsDiscountALL);
        }
    }
}
