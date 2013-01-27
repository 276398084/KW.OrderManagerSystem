//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// SupplierTypeMap
    /// 供应商表
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
    public class SupplierTypeMap : ClassMap<SupplierType> 
    {
        public SupplierTypeMap()
        {
            Table("Suppliers");
            Id(x => x.Id);
            Map(x => x.SuppliersName).Length(50);
            Map(x => x.QQ).Length(50);
            Map(x => x.WW).Length(50);
            Map(x => x.Phone).Length(50);
            Map(x => x.Tel).Length(50);
            Map(x => x.Web).Length(200);
            Map(x => x.Memo).Length(400);
        }
    }
}
