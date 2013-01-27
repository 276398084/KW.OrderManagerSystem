//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// SuppliersProductTypeMap
    /// 供应商产品表
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
    public class SuppliersProductTypeMap : ClassMap<SuppliersProductType> 
    {
        public SuppliersProductTypeMap()
        {
            Table("SuppliersProduct");
            Id(x => x.Id);
            Map(x => x.SId);
            Map(x => x.SKU).Length(50);
            Map(x => x.Price);
            Map(x => x.Web).Length(50);
        }
    }
}
