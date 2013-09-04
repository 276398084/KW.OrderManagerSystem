//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// UserSetupType
    /// 人员分配表
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
    public class UserSetupType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// SetupID
        /// </summary>
        public virtual int SetupID { get; set; }

        /// <summary>
        /// SetupName
        /// </summary>
        public virtual String SetupName { get; set; }

        /// <summary>
        /// UId
        /// </summary>
        public virtual int UId { get; set; }

        /// <summary>
        /// RealName
        /// </summary>
        public virtual String RealName { get; set; }

        /// <summary>
        /// CreateON
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        public virtual String CreateBy { get; set; }

    }
}
