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
            Map(x => x.Subject).Length(200);
            Map(x => x.Content);
            Map(x => x.BuyerCode).Length(200);
            Map(x => x.BuyerEmail).Length(200);
            Map(x => x.MessageId).Length(50);
            Map(x => x.MessageType).Length(50);
            Map(x => x.OrderNo).Length(100);
            Map(x => x.SKU).Length(200);
            Map(x => x.Title).Length(200);
            Map(x => x.ProductUrl).Length(200);
            Map(x => x.ProductPrice);
            Map(x => x.Account).Length(200);
            Map(x => x.Platform).Length(200);
            Map(x => x.Status);
            Map(x => x.ReplyBy).Length(200);
            Map(x => x.IsReply);
            Map(x => x.GenerateOn);
            Map(x => x.CreateOn);
            Map(x => x.ReplyOn);
        }
    }
}
