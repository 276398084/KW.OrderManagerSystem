//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AccountType
    /// 语言表
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0  创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>毛才君</name>
    /// <date>2013-11-25</date>
    /// </author>
    /// </summary>
    public class LanguageType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [Required]
        [Display(Name = "语言")]
        public virtual String Language { get; set; }
        /// <summary>
        /// 母语
        /// </summary>
        [Required]
        [Display(Name = "母语")]
        public virtual String NativeLanguage { get; set; }

        /// <summary>
        /// 显示的文本
        /// </summary>
        [Required]
        [Display(Name = "文本")]
        public virtual String Text { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        [Display(Name = "是否启用")]
        public virtual bool Enable { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        [Display(Name = "注释")]
        public virtual String Note { get; set; }
    }
}
