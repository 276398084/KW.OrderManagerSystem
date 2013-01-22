//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EmailTemplateTypeMap
    /// 邮件回复模板
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
    public class EmailTemplateTypeMap : ClassMap<EmailTemplateType> 
    {
        public EmailTemplateTypeMap()
        {
            Table("EmailTemplate");
            Id(x => x.Id);
            Map(x => x.Subject);
            Map(x => x.Title);
            Map(x => x.Content);
        }
    }
}
