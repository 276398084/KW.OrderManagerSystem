//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// NewGuideTypeMap
    /// 新产品导购
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
    public class NewGuideTypeMap : ClassMap<NewGuideType> 
    {
        public NewGuideTypeMap()
        {
            Table("NewGuide");
            Id(x => x.Id);
            Map(x => x.NewSku).Length(50);
            Map(x => x.OldSku).Length(50);
            Map(x => x.Title).Length(400);
            Map(x => x.Pic).Length(400);
            Map(x => x.Url).Length(400);
            Map(x => x.Price);
            Map(x => x.ColorSize).Length(50);
            Map(x => x.Remark).Length(50);
            Map(x => x.Status).Length(50);
            Map(x => x.IsCheck);
            Map(x => x.CreateOn);
        }
    }
}
