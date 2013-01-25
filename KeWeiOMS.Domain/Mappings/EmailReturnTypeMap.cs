//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EmailReturnTypeMap
    /// 邮件回复表
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
    public class EmailReturnTypeMap : ClassMap<EmailReturnType> 
    {
        public EmailReturnTypeMap()
        {
            Table("EmailReturn");
            Id(x => x.Id);
            Map(x => x.REmail);
            Map(x => x.Subject);
            Map(x => x.Content);
            Map(x => x.MyEmail);
            Map(x => x.EId);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy);
        }
    }
}
