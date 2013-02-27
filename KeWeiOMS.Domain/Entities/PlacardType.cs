//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PlacardType
    /// 公告管理
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
    public class PlacardType
    {
        /// <summary>
        /// 编号
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public virtual String CardType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 置顶
        /// </summary>
        public virtual int IsTop { get; set; }

    }
}
