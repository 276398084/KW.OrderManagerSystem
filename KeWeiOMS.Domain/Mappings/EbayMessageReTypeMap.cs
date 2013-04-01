//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EbayMessageReTypeMap
    /// Ebay邮件回复
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
    public class EbayMessageReTypeMap : ClassMap<EbayMessageReType> 
    {
        public EbayMessageReTypeMap()
        {
            Table("EbayMessageRe");
            Id(x => x.Id);
            Map(x => x.MessageId);
            Map(x => x.SubjectRe).Length(255);
            Map(x => x.BodyRe).Length(1000);
            Map(x => x.ReplayOn);
            Map(x => x.ReplayBy).Length(255);
            Map(x=>x.IsUpload);
            Map(x => x.EbayId).Length(255);
            Map(x => x.ItemId).Length(255);
            Map(x => x.SenderEmail);
            Map(x => x.SenderID);
            Map(x => x.UploadTime);
        }
    }
}
