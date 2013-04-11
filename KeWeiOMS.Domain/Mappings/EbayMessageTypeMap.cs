//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EbayMessageTypeMap
    /// Ebay消息
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
    public class EbayMessageTypeMap : ClassMap<EbayMessageType> 
    {
        public EbayMessageTypeMap()
        {
            Table("EbayMessage");
            Id(x => x.Id);
            Map(x => x.MessageId);
            Map(x => x.MessageType);
            Map(x => x.SenderEmail);
            Map(x => x.SenderID);
            Map(x => x.Subject);
            Map(x => x.Body);
            Map(x => x.MessageStatus);
            Map(x => x.ItemId);
            Map(x=>x.Shop);
            Map(x => x.CreationDate);
            Map(x => x.CreateOn);
            Map(x => x.ReplayBy);
            Map(x => x.ReplayOn);
            Map(x=>x.ReplayOnlyBy);
            Map(x => x.ForwardWhy);
        }
    }
}
