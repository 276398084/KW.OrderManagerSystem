//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PlacardTypeMap
    /// 公告管理
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
    public class PlacardTypeMap : ClassMap<PlacardType> 
    {
        public PlacardTypeMap()
        {
            Table("Placard");
            Id(x => x.Id);
            Map(x => x.CardType).Length(20);
            Map(x => x.Title).Length(100);
            Map(x => x.Content).Length(1000);
            Map(x => x.CreateBy).Length(20);
            Map(x => x.CreateOn);
            Map(x => x.IsTop);
        }
    }
}
