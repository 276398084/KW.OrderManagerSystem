//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// HaiItemTypeMap
    /// HaiItem
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
    public class HaiItemTypeMap : ClassMap<HaiItemType>
    {
        public HaiItemTypeMap()
        {
            Table("HaiItem");
            Id(x => x.Id);
            Map(x => x.ItemId).Length(40);
            Map(x => x.Location).Length(100);
        }
    }
}
