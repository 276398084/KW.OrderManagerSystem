//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PrintTemplateType
    /// 打印模板
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
    public class PrintTemplateType
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 模板代码
        /// </summary>
        public virtual String Code { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public virtual String TempName { get; set; }

        /// <summary>
        /// 模板分类
        /// </summary>
        public virtual String TempType { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual String Description { get; set; }

    }
}
