//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// MachinType
    /// 设备管理
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
    public class MachineType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public virtual String Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String MachineClass { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 使用人
        /// </summary>
        public virtual String UserName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// 购买时间
        /// </summary>
        public virtual DateTime BuyDate { get; set; }

        /// <summary>
        /// 购买金额
        /// </summary>
        public virtual String BuyMoney{ get; set; }

        /// <summary>
        /// 购买人
        /// </summary>
        public virtual String BuyBy{ get; set; }

    }
}
