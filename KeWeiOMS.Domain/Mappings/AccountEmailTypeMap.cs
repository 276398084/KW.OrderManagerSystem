//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AccountEmailTypeMap
    /// 账户邮件
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
    public class AccountEmailTypeMap : ClassMap<AccountEmailType> 
    {
        public AccountEmailTypeMap()
        {
            Table("AccountEmail");
            Id(x => x.Id);
            Map(x => x.AccountId);
            Map(x => x.AccountName);
            Map(x => x.Email);
        }
    }
}
