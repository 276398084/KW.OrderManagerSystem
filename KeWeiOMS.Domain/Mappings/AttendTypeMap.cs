//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AttendTypeMap
    /// 考勤表
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
    public class AttendTypeMap : ClassMap<AttendType> 
    {
        public AttendTypeMap()
        {
            Table("Attend");
            Id(x => x.Id);
            Map(x => x.UserId);
            Map(x => x.UserCode).Length(255);
            Map(x => x.姓名).Length(255);
            Map(x => x.CurrentDate);
            Map(x => x.AMStart).Length(30);
            Map(x => x.AMEnd).Length(30);
            Map(x => x.PMStart).Length(30);
            Map(x => x.PMEnd).Length(30);
            Map(x => x.IP).Length(255);
        }
    }
}
