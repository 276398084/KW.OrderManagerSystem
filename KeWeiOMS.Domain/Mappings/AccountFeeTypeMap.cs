//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AccountFeeTypeMap
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
    public class AccountFeeTypeMap : ClassMap<AccountFeeType> 
    {
        public AccountFeeTypeMap()
        {
            Table("AccountFees");
            Id(x => x.Id);
            Map(x => x.AccountId);
            Map(x => x.AccountName).Length(50);
            Map(x => x.AmountBegin);
            Map(x => x.AmountEnd);
            Map(x => x.FeeFormula).Length(200);
            Map(x => x.FeeName).Length(50);
        }
    }
}
