//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// NewGuideType
    /// 新产品导购
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
    public class NewGuideType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 新SKU
        /// </summary>
        public virtual String NewSku { get; set; }

        /// <summary>
        /// 旧SKU
        /// </summary>
        public virtual String OldSku { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public virtual String Pic { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public virtual String Url { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public virtual String ColorSize { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 采购标记
        /// </summary>
        public virtual int IsCheck { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
