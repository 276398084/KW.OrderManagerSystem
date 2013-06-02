//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// UserLoginTypeMap
    /// 用户登陆日志
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
    public class UserLoginTypeMap : ClassMap<UserLoginType> 
    {
        public UserLoginTypeMap()
        {
            Table("UserLogin");
            Id(x => x.Id);
            Map(x => x.UserCode).Length(255);
            Map(x => x.UserName).Length(255);
            Map(x => x.CreateOn);
            Map(x => x.IP).Length(255);
            Map(x => x.MAC).Length(255);
        }
    }
}
