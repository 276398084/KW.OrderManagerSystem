//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsModeTypeMap
    /// 承运商明细
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
    public class LogisticsModeTypeMap : ClassMap<LogisticsModeType> 
    {
        public LogisticsModeTypeMap()
        {
            Table("LogisticsMode");
            Id(x => x.Id);
            Map(x => x.LogisticsName).Length(250);
            Map(x => x.LogisticsCode).Length(250);
            Map(x => x.ParentID);
            Map(x => x.Discount);
            Map(x => x.Remark).Length(250);
        }
    }
}
