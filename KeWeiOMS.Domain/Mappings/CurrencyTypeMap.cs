//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// CurrencyTypeMap
    /// 汇率表
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
    public class CurrencyTypeMap : ClassMap<CurrencyType>
    {
        public CurrencyTypeMap()
        {
            Table("Currency");
            Id(x => x.Id);
            Map(x => x.CurrencyCode).Length(30);
            Map(x => x.CurrencyName).Length(30);
            Map(x => x.CurrencySign).Length(30);
            Map(x => x.CurrencyValue);
            Map(x => x.CreateOn);
        }
    }
}
