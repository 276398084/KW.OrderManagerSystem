//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// UserLoginType
    /// 用户登陆日志
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
    public class UserLoginType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public virtual String UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public virtual String UserName { get; set; }

        /// <summary>
        /// 登陆时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public virtual String IP { get; set; }

        /// <summary>
        /// MAC
        /// </summary>
        public virtual String MAC { get; set; }

    }
}
