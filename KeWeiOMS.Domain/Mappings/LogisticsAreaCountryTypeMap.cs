//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsAreaCountryTypeMap
    /// 分区国家表
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
    public class LogisticsAreaCountryTypeMap : ClassMap<LogisticsAreaCountryType> 
    {
        public LogisticsAreaCountryTypeMap()
        {
            Table("LogisticsAreaCountry");
            Id(x => x.Id);
            Map(x => x.AreaCode);
            Map(x => x.CountryCode).Length(250);
            Map(x => x.Country).Length(250);
        }
    }
}
