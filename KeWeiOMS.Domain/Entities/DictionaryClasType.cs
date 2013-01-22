//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// DictionaryClasType
    /// 数据字典分类
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
    public class DictionaryClassType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public virtual String ClassName { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public virtual String Code { get; set; }

        /// <summary>
        /// 系统内置
        /// </summary>
        public virtual int AllowDelete { get; set; }

    }
}
