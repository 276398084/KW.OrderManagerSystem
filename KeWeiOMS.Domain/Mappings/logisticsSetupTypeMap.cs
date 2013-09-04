//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// logisticsSetupTypeMap
    /// 平台物流分配表
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
    public class logisticsSetupTypeMap : ClassMap<logisticsSetupType> 
    {
        public logisticsSetupTypeMap()
        {
            Table("logisticsSetup");
            Id(x => x.Id);
            Map(x => x.Platform).Length(100);
            Map(x => x.SetupId);
            Map(x => x.SetupName).Length(400);
            Map(x => x.LId);
            Map(x => x.LogisticsName).Length(400);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy).Length(100);
        }
    }
}
