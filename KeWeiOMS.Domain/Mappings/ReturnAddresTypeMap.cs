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
            Map(x => x.RetuanName).Length(50);
            Map(x => x.Phone).Length(50);
            Map(x => x.Tel).Length(50);
            Map(x => x.PostCode).Length(50);
            Map(x => x.Street).Length(200);
            Map(x => x.County).Length(50);
            Map(x => x.City).Length(50);
            Map(x => x.Province).Length(50);
            Map(x => x.Country).Length(50);
            Map(x => x.CountryCode).Length(50);
        }
    }
}
