//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderReturnRecordType
    /// 退件记录
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0 XiDong 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>XiDong</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class OrderReturnRecordType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// OrderNo
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// OrderExNO
        /// </summary>
        public virtual String OrderExNO { get; set; }

        /// <summary>
        /// OId
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// ReturnType
        /// </summary>
        public virtual String ReturnType { get; set; }

        /// <summary>
        /// ReturnLogisticsMode
        /// </summary>
        public virtual String ReturnLogisticsMode { get; set; }

        /// <summary>
        /// NewLogisticsMode
        /// </summary>
        public virtual String NewLogisticsMode { get; set; }

        /// <summary>
        /// OldTrackCode
        /// </summary>
        public virtual String OldTrackCode { get; set; }

        /// <summary>
        /// NewTrackCode
        /// </summary>
        public virtual String NewTrackCode { get; set; }

        /// <summary>
        /// BuyerName
        /// </summary>
        public virtual String BuyerName { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        public virtual String Account { get; set; }

        /// <summary>
        /// Platform
        /// </summary>
        public virtual String Platform { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public virtual double Amount { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public virtual String Country { get; set; }

        /// <summary>
        /// CurrencyCode
        /// </summary>
        public virtual String CurrencyCode { get; set; }

        /// <summary>
        /// OrderCreateOn
        /// </summary>
        public virtual DateTime OrderCreateOn { get; set; }

        /// <summary>
        /// OrderSendOn
        /// </summary>
        public virtual DateTime OrderSendOn { get; set; }

    }
}
