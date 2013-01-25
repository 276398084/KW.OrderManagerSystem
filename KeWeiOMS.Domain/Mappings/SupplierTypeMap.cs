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
            Map(x => x.SuppliersName);
            Map(x => x.QQ);
            Map(x => x.WW);
            Map(x => x.Phone);
            Map(x => x.Tel);
            Map(x => x.Web);
            Map(x => x.Memo);
        }
    }
}
