//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// RefundAmountTypeMap
    /// 退款金额表
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
    public class RefundAmountTypeMap : ClassMap<RefundAmountType> 
    {
        public RefundAmountTypeMap()
        {
            Table("RefundAmount");
            Id(x => x.Id);
            Map(x => x.DId);
            Map(x => x.OrderNo).Length(255);
            Map(x => x.Amount);
        }
    }
}
