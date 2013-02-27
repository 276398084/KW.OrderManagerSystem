//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PlanDaoTypeMap
    /// 采购到货表
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
    public class PlanDaoTypeMap : ClassMap<PlanDaoType> 
    {
        public PlanDaoTypeMap()
        {
            Table("PlanDao");
            Id(x => x.Id);
            Map(x => x.PlanNo).Length(50);
            Map(x => x.SKU).Length(50);
            Map(x => x.Title).Length(200);
            Map(x => x.BuyOn);
            Map(x => x.SendOn);
            Map(x => x.LogisticMode).Length(200);
            Map(x => x.TrackCode).Length(200);
            Map(x => x.PlanQty);
            Map(x => x.RealQty);
            Map(x => x.Suppliers).Length(50);
            Map(x => x.Price);
            Map(x => x.Status).Length(50);
            Map(x => x.CheckBy).Length(50);
            Map(x => x.PassRate);
            Map(x => x.DaoOn);
            Map(x => x.SKUCode);
            Map(x => x.ValiBy).Length(50);
            Map(x => x.PlanMemo).Length(400);
            Map(x => x.Memo).Length(400);
        }
    }
}
