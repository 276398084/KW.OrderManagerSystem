﻿//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// DictionaryTypeMap
    /// 数据字典明细
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
    public class DictionaryTypeMap : ClassMap<DictionaryType> 
    {
        public DictionaryTypeMap()
        {
            Table("Dictionarys");
            Id(x => x.Id);
            Map(x => x.DicCode).Length(50);
            Map(x => x.FullName).Length(50);
            Map(x => x.DicValue).Length(50);
            Map(x => x.AllowDelete);
        }
    }
}
