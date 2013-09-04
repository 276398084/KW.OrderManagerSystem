//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// SettlementTypeMap
    /// 结算管理
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0 XiDong 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>XiDong</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class SettlementTypeMap : ClassMap<SettlementType> 
    {
        public SettlementTypeMap()
        {
            Table("Settlements");
            Id(x => x.Id);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy).Length(200);
            Map(x => x.Amount);
            Map(x => x.Account).Length(200);
            Map(x => x.Memo).Length(2000);
        }
    }
}
