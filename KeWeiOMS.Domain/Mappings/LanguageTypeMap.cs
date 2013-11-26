//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AccountTypeMap
    /// 语言表
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0  创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>毛才君</name>
    /// <date>2013-11-25</date>
    /// </author>
    /// </summary>
    public class LanguageTypeMap : ClassMap<LanguageType>
    {
        public LanguageTypeMap()
        {
            Table("Language");
            Id(x => x.Id);
            Map(x => x.Language).Length(255);
            Map(x => x.NativeLanguage).Length(255);
            Map(x => x.Text).Length(2000);
            Map(x => x.Note).Length(2000);
            Map(x => x.Enable);
        }
    }
}
