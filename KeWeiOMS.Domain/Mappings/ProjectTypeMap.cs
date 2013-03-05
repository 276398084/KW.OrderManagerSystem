//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ProjectTypeMap
    /// 项目管理
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
    public class ProjectTypeMap : ClassMap<ProjectType> 
    {
        public ProjectTypeMap()
        {
            Table("Projects");
            Id(x => x.Id);
            Map(x => x.ProjectName).Length(100);
            Map(x => x.Status).Length(50);
            Map(x => x.ProjectManager).Length(100);
            Map(x => x.Content).Length(2000);
            Map(x => x.BeginDate);
            Map(x => x.EndDate);
            Map(x => x.NeedDay);
            Map(x => x.RealEndDate);
            Map(x => x.CreateOn);
            Map(x => x.CreateBy).Length(200);
            Map(x => x.Evaluate).Length(200);
        }
    }
}
