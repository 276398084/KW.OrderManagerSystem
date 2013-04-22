//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// QuestionOrderType
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
    public class QuestionOrderType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// OId
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// OrderNo
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// Subjest
        /// </summary>
        public virtual String Subjest { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// SolveBy
        /// </summary>
        public virtual String SolveBy { get; set; }

        /// <summary>
        /// SolveOn
        /// </summary>
        public virtual DateTime SolveOn { get; set; }

    }
}
