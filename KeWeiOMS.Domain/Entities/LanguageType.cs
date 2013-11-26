//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

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
        public virtual String Language { get; set; }
        /// <summary>
        /// 母语
        /// </summary>
        public virtual String NativeLanguage { get; set; }

        /// <summary>
        /// 显示的文本
        /// </summary>
        public virtual String Text { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool Enable { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public virtual String Note { get; set; }
    }
}
