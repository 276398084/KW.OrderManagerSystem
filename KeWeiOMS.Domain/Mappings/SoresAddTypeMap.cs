//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// SoresAddTypeMap
    /// 仓库补分表
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
    public class SoresAddTypeMap : ClassMap<SoresAddType> 
    {
        public SoresAddTypeMap()
        {
            Table("SoresAdd");
            Id(x => x.Id);
            Map(x => x.Worker).Length(255);
            Map(x => x.Sore);
            Map(x => x.Hours);
            Map(x => x.WorkType).Length(255);
            Map(x => x.WorkDate);
            Map(x => x.CreateBy).Length(255);
            Map(x => x.CreateOn);
        }
    }
}
