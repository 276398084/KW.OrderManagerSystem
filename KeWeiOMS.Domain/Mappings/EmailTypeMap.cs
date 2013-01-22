//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EmailTypeMap
    /// 邮件
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
    public class EmailTypeMap : ClassMap<EmailType> 
    {
        public EmailTypeMap()
        {
            Table("Emails");
            Id(x => x.Id);
            Map(x => x.Subject);
            Map(x => x.Content);
            Map(x => x.BuyerCode);
            Map(x => x.BuyerEmail);
            Map(x => x.MessageId);
            Map(x => x.MessageType);
            Map(x => x.OrderNo);
            Map(x => x.SKU);
            Map(x => x.Title);
            Map(x => x.ProductUrl);
            Map(x => x.ProductPrice);
            Map(x => x.Account);
            Map(x => x.Platform);
            Map(x => x.Status);
            Map(x => x.ReplyBy);
            Map(x => x.IsReply);
            Map(x => x.GenerateOn);
            Map(x => x.CreateOn);
            Map(x => x.ReplyOn);
        }
    }
}
