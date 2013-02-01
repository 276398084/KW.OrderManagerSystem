//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// LogisticsType
    /// 承运商
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
    public class LogisticsType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 承运商名称
        /// </summary>
        public virtual String LogisticsName { get; set; }

        /// <summary>
        /// 承运商代码
        /// </summary>
        public virtual String LogisticsCode { get; set; }

        /// <summary>
        /// 是否有追踪码
        /// </summary>
        public virtual int HasTrackCode { get; set; }

        /// <summary>
        /// 追踪码长度
        /// </summary>
        public virtual int CodeLength { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
