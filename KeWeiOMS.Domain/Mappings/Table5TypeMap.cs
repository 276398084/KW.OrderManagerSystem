//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// Table5TypeMap
    /// 平台账户费用表
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
    public class Table5TypeMap : ClassMap<Table5Type> 
    {
        public Table5TypeMap()
        {
            Table("Table5");
            Id(x => x.Id);
            Map(x => x.AccountId);
            Map(x => x.AccountName);
            Map(x => x.AmountBegin);
            Map(x => x.AmountEnd);
            Map(x => x.FeeFormula);
            Map(x => x.FeeName);
        }
    }
}
