//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// WarehouseLocationTypeMap
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
    public class WarehouseLocationTypeMap : ClassMap<WarehouseLocationType> 
    {
        public WarehouseLocationTypeMap()
        {
            Table("WarehouseLocation");
            Id(x => x.Id);
            Map(x => x.WId);
            Map(x => x.Warehouse).Length(50);
            Map(x => x.PositionsName).Length(50);
        }
    }
}
