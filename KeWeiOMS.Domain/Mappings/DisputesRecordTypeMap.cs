//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// DisputesRecordTypeMap
    /// 纠纷处理记录
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
    public class DisputesRecordTypeMap : ClassMap<DisputesRecordType> 
    {
        public DisputesRecordTypeMap()
        {
            Table("DisputesRecord");
            Id(x => x.Id);
            Map(x => x.DId);
            Map(x => x.DealType).Length(255);
            Map(x => x.Content);
            Map(x => x.CreateBy).Length(255);
            Map(x => x.CreateOn);
        }
    }
}
