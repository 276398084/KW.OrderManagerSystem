//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ReturnAddresTypeMap
    /// 回邮地址表
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
    public class ReturnAddresTypeMap : ClassMap<ReturnAddresType> 
    {
        public ReturnAddresTypeMap()
        {
            Table("ReturnAddress");
            Id(x => x.Id);
            Map(x => x.RetuanName);
            Map(x => x.Phone);
            Map(x => x.Tel);
            Map(x => x.PostCode);
            Map(x => x.Street);
            Map(x => x.County);
            Map(x => x.City);
            Map(x => x.Province);
            Map(x => x.Country);
            Map(x => x.CountryCode);
        }
    }
}
