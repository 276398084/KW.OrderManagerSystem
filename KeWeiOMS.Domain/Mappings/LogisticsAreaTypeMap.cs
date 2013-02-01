//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsAreaTypeMap
    /// 分区表
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
    public class LogisticsAreaTypeMap : ClassMap<LogisticsAreaType> 
    {
        public LogisticsAreaTypeMap()
        {
            Table("LogisticsArea");
            Id(x => x.Id);
            Map(x => x.AreaName).Length(250);
            Map(x => x.LId);
        }
    }
}
