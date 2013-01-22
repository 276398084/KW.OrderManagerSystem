//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// DictionaryClasTypeMap
    /// 数据字典分类
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
    public class DictionaryClassTypeMap : ClassMap<DictionaryClassType> 
    {
        public DictionaryClassTypeMap()
        {
            Table("DictionaryClass");
            Id(x => x.Id);
            Map(x => x.ClassName);
            Map(x => x.Code);
            Map(x => x.AllowDelete);
        }
    }
}
