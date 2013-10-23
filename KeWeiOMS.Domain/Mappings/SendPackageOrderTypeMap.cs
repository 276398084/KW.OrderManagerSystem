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
    public class SendPackageOrderTypeMap : ClassMap<SendPackageOrderType>
    {
        public SendPackageOrderTypeMap()
        {
            Table("SendPackageOrder");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderNo);
            Map(x => x.PackageName);
            Map(x => x.PackId);


        }
    }
}
