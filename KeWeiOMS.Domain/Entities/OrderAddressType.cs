//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderAddressType
    /// 订单买家地址表
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
    public class OrderAddressType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int BId { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public virtual String Addressee { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual String Tel { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual String Phone { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public virtual String Street { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public virtual String County { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public virtual String City { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public virtual String Province { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public virtual String Country { get; set; }

        /// <summary>
        /// 国家代码
        /// </summary>
        public virtual String CountryCode { get; set; }

    }
}
