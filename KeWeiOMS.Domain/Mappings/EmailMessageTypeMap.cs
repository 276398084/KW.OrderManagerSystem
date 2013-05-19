//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EmailMessageTypeMap
    /// 邮件留言
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
    public class EmailMessageTypeMap : ClassMap<EmailMessageType> 
    {
        public EmailMessageTypeMap()
        {
            Table("EmailMessage");
            Id(x => x.Id);
            Map(x => x.MessageId).Length(255);
            Map(x => x.OrderExNo).Length(255);
            Map(x => x.RserverDate);
            Map(x => x.CreateOn);
        }
    }
}
