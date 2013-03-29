//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// SKUCodeTypeMap
    /// SKUCode
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
    public class SKUCodeTypeMap : ClassMap<SKUCodeType>
    {
        public SKUCodeTypeMap()
        {
            Table("SKUCode");
            Id(x => x.Id);
            Map(x => x.Code);
            Map(x => x.SKU).Length(20);
            Map(x => x.IsOut);
            Map(x => x.IsSend);
            Map(x => x.SendOn);
            Map(x => x.IsNew);
            Map(x => x.IsScan);
            Map(x => x.OrderNo).Length(20);
            Map(x => x.PeiOn).Length(30); ;
            Map(x => x.CreateOn).Length(30); ;
            Map(x => x.PlanNo).Length(30); ;
        }
    }
}
