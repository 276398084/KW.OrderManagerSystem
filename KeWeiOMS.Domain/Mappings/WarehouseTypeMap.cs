//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// WarehouseTypeMap
    /// 仓库表
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
    public class WarehouseTypeMap : ClassMap<WarehouseType> 
    {
        public WarehouseTypeMap()
        {
            Table("Warehouse");
            Id(x => x.Id);
            Map(x => x.WCode);
            Map(x => x.WName);
            Map(x => x.Address);
        }
    }
}
