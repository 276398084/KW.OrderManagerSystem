//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// UserSetupTypeMap
    /// 人员分配表
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
    public class UserSetupTypeMap : ClassMap<UserSetupType> 
    {
        public UserSetupTypeMap()
        {
            Table("UserSetup");
            Id(x => x.Id);
            Map(x => x.SetupID);
            Map(x => x.SetupName).Length(400);
            Map(x => x.UId);
            Map(x => x.RealName).Length(400);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy).Length(400);
        }
    }
}
