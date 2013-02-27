//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AttendType
    /// 考勤表
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
    public class AttendType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public virtual String UserCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual String RealName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public virtual DateTime CurrentDate { get; set; }

        /// <summary>
        /// 上午上班
        /// </summary>
        public virtual String AMStart { get; set; }

        /// <summary>
        /// 上午下班
        /// </summary>
        public virtual String AMEnd { get; set; }

        /// <summary>
        /// 下午上班
        /// </summary>
        public virtual String PMStart { get; set; }

        /// <summary>
        /// 下午下班
        /// </summary>
        public virtual String PMEnd { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public virtual String IP { get; set; }

    }
}
