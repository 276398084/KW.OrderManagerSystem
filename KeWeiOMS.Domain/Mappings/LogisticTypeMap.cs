//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsTypeMap
    /// 承运商
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
    public class LogisticsTypeMap : ClassMap<LogisticsType> 
    {
        public LogisticsTypeMap()
        {
            Table("Logistics");
            Id(x => x.Id);
            Map(x => x.LogisticsName).Length(250);
            Map(x => x.LogisticsCode).Length(250);
            Map(x => x.HasTrackCode);
            Map(x => x.CodeLength);
            Map(x => x.Remark).Length(250);
            Map(x => x.CreateOn);
        }
    }
}
