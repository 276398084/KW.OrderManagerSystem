//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// MachinesOldTypeMap
    /// 设备表过去使用
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
    public class MachinesOldTypeMap : ClassMap<MachinesOldType> 
    {
        public MachinesOldTypeMap()
        {
            Table("MachinesOld");
            Id(x => x.Id);
            Map(x => x.Code).Length(50);
            Map(x => x.Name).Length(50);
            Map(x => x.Status).Length(50);
            Map(x => x.UserName).Length(50);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
        }
    }
}
