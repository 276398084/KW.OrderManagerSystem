//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// BeforePeiScanType
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
    public class BeforePeiScanType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// 配货人
        /// </summary>
        public virtual String PeiBy { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreatBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
