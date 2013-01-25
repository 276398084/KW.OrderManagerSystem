//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// Table1TypeMap
    /// 库位表
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
    public class Table1TypeMap : ClassMap<Table1Type> 
    {
        public Table1TypeMap()
        {
            Table("Table1");
            Id(x => x.Id);
            Map(x => x.WId);
            Map(x => x.Warehouse);
            Map(x => x.PositionsName);
        }
    }
}
