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
            Map(x => x.AccountName).Length(40);
            Map(x => x.AppKey).Length(100);
            Map(x => x.AppPwd).Length(100);
            Map(x => x.AppToken).Length(400);
            Map(x => x.Status);
        }
    }
}
