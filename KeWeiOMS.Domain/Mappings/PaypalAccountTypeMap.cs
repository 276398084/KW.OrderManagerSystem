//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PaypalAccountTypeMap
    /// paypal账户
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
    public class PaypalAccountTypeMap : ClassMap<PaypalAccountType> 
    {
        public PaypalAccountTypeMap()
        {
            Table("PaypalAccount");
            Id(x => x.Id);
            Map(x => x.AccountName);
            Map(x => x.AppKey);
            Map(x => x.AppPwd);
            Map(x => x.AppToken);
            Map(x => x.Status);
        }
    }
}
