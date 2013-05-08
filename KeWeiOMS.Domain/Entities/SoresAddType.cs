//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// SoresAddType
    /// 仓库补分表
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
    public class SoresAddType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public virtual String Worker { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public virtual double Sore { get; set; }

        /// <summary>
        /// 小时
        /// </summary>
        public virtual double Hours { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual String WorkType { get; set; }

        /// <summary>
        /// 日期时间
        /// </summary>
        public virtual DateTime WorkDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
