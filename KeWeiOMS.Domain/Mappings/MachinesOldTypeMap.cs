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
            Map(x => x.MachineCode).Length(50);
            Map(x => x.StatusOld).Length(50);
            Map(x => x.UserNameOld).Length(50);
            Map(x => x.StartDateOld);
            Map(x => x.EndDateOld);
        }
    }
}
