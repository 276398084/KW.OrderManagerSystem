//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// QuestionOrderTypeMap
    /// 问题订单表
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
    public class QuestionOrderTypeMap : ClassMap<QuestionOrderType> 
    {
        public QuestionOrderTypeMap()
        {
            Table("QuestionOrders");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderNo).Length(40);
            Map(x => x.Subjest).Length(40);
            Map(x => x.Status);
            Map(x => x.Content).Length(800);
            Map(x => x.CreateBy).Length(40);
            Map(x => x.CreateOn);
            Map(x => x.SolveBy).Length(40);
            Map(x => x.SolveOn);
        }
    }
}
