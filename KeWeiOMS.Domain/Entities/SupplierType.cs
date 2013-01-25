//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// SupplierType
    /// 供应商表
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
    public class SupplierType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual String SuppliersName { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public virtual String QQ { get; set; }

        /// <summary>
        /// 旺旺
        /// </summary>
        public virtual String WW { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual String Phone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual String Tel { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public virtual String Web { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Memo { get; set; }

    }
}
