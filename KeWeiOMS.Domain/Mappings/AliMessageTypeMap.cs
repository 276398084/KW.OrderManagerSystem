//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AliMessageTypeTypeMap
    /// AliMessageType
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
    public class AliMessageTypeMap : ClassMap<AliMessageType>
    {
        public AliMessageTypeMap()
        {
            Table("AliMessage");
            Id(x => x.Id);
            Map(x => x.MId);
            Map(x => x.HaveFile);
            Map(x => x.OrderUrl).Length(400);
            Map(x => x.OrderId).Length(100);
            Map(x => x.ReceiverLoginId).Length(100);
            Map(x => x.ReceiverName).Length(200);
            Map(x => x.FileUrl).Length(400);
            Map(x => x.ProductId);
            Map(x => x.Content).Length(4000);
            Map(x => x.SenderName).Length(200);
            Map(x => x.SenderLoginId).Length(100);
            Map(x => x.ProductUrl).Length(4000);
            Map(x => x.ProductName).Length(300);
            Map(x => x.IsRead);
            Map(x => x.TypeId).Length(200);
            Map(x => x.MessageType).Length(50);
            Map(x => x.RelationId);
            Map(x => x.CreateOn);
            Map(x => x.SynOn);
            Map(x => x.Shop).Length(100);
            Map(x => x.ReplayBy).Length(200);
            Map(x => x.IsReplay);
            Map(x => x.ReplayContent).Length(2000);
            Map(x => x.ReplayOn);
            Map(x => x.IsUpload);
            Map(x => x.UploadOn);
        }
    }
}
