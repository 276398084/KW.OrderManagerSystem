//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PrintTemplateTypeMap
    /// 打印模板
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
    public class PrintTemplateTypeMap : ClassMap<PrintTemplateType> 
    {
        public PrintTemplateTypeMap()
        {
            Table("PrintTemplate");
            Id(x => x.Id);
            Map(x => x.Code).Length(20);
            Map(x => x.TempName).Length(40);
            Map(x => x.TempType).Length(20);
            Map(x => x.Content);
            Map(x => x.Description).Length(200);
        }
    }
}
