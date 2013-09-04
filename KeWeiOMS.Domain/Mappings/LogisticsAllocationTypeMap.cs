//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsAllocationTypeMap
    /// 承运商划分
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
    public class LogisticsAllocationTypeMap : ClassMap<LogisticsAllocationType> 
    {
        public LogisticsAllocationTypeMap()
        {
            Table("LogisticsAllocation");
            Id(x => x.Id);
            Map(x => x.AllocationType);
            Map(x => x.AId);
            Map(x => x.QuerySql).CustomType("StringClob").CustomSqlType("ntext"); 
            Map(x => x.LogisticsMode).Length(200);
            Map(x => x.NBegin).Length(200);
            Map(x => x.NEnd).Length(200);
            Map(x => x.Content).CustomType("StringClob").CustomSqlType("ntext"); ;
            Map(x => x.SortCode);
            Map(x => x.CreateBy).Length(200);
            Map(x => x.CreateOn);
        }
    }
}
