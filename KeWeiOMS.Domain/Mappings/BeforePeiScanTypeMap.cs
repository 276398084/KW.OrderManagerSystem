//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// BeforePeiScanTypeMap
    /// 配货前扫描
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
    public class BeforePeiScanTypeMap : ClassMap<BeforePeiScanType> 
    {
        public BeforePeiScanTypeMap()
        {
            Table("BeforePeiScan");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderNo).Length(255);
            Map(x => x.PeiBy).Length(255);
            Map(x => x.CreatBy).Length(255);
            Map(x => x.CreateOn);
        }
    }
}
