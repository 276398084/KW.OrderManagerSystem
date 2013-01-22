//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AccountTypeMap
    /// 平台账户表
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
    public class AccountTypeMap : ClassMap<AccountType> 
    {
        public AccountTypeMap()
        {
            Table("Account");
            Id(x => x.Id);
            Map(x => x.AccountName);
            Map(x => x.AccountUrl);
            Map(x => x.ApiKey);
            Map(x => x.ApiSecret);
            Map(x => x.ApiToken);
            Map(x => x.Platform);
            Map(x => x.Status);
            Map(x => x.Description);
            Map(x => x.Manager);
            Map(x => x.Phone);
            Map(x => x.Email);
        }
    }
}
