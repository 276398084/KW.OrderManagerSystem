//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ProjectType
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
    public class ProjectType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual String ProjectName { get; set; }

        /// <summary>
        /// 状态（未开始，执行中，暂停中，已完成）
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 项目负责人
        /// </summary>
        public virtual String ProjectManager { get; set; }

        /// <summary>
        /// 项目进程
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 项目开始时间
        /// </summary>
        public virtual DateTime BeginDate { get; set; }

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// 工期
        /// </summary>
        public virtual double NeedDay { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public virtual DateTime RealEndDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 评价
        /// </summary>
        public virtual String Evaluate { get; set; }

    }
}
