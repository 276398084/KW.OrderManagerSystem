//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AliMessageTypeType
    /// AliMessageType
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
    public class AliMessageType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// MId
        /// </summary>
        public virtual int MId { get; set; }

        /// <summary>
        /// HaveFile
        /// </summary>
        public virtual Boolean HaveFile { get; set; }

        /// <summary>
        /// OrderUrl
        /// </summary>
        public virtual String OrderUrl { get; set; }

        /// <summary>
        /// OrderId
        /// </summary>
        public virtual String OrderId { get; set; }

        /// <summary>
        /// ReceiverLoginId
        /// </summary>
        public virtual String ReceiverLoginId { get; set; }

        /// <summary>
        /// ReceiverName
        /// </summary>
        public virtual String ReceiverName { get; set; }

        /// <summary>
        /// FileUrl
        /// </summary>
        public virtual String FileUrl { get; set; }

        /// <summary>
        /// ProductId
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// SenderName
        /// </summary>
        public virtual String SenderName { get; set; }

        /// <summary>
        /// SenderLoginId
        /// </summary>
        public virtual String SenderLoginId { get; set; }

        /// <summary>
        /// ProductUrl
        /// </summary>
        public virtual String ProductUrl { get; set; }

        /// <summary>
        /// ProductName
        /// </summary>
        public virtual String ProductName { get; set; }

        /// <summary>
        /// IsRead
        /// </summary>
        public virtual Boolean IsRead { get; set; }

        /// <summary>
        /// TypeId
        /// </summary>
        public virtual String TypeId { get; set; }

        /// <summary>
        /// MessageType
        /// </summary>
        public virtual String MessageType { get; set; }

        /// <summary>
        /// RelationId
        /// </summary>
        public virtual int RelationId { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// SynOn
        /// </summary>
        public virtual DateTime SynOn { get; set; }

        /// <summary>
        /// Shop
        /// </summary>
        public virtual String Shop { get; set; }

        /// <summary>
        /// ReplayBy
        /// </summary>
        public virtual String ReplayBy { get; set; }

        /// <summary>
        /// IsReplay
        /// </summary>
        public virtual Boolean IsReplay { get; set; }

        /// <summary>
        /// ReplayContent
        /// </summary>
        public virtual String ReplayContent { get; set; }

        /// <summary>
        /// ReplayOn
        /// </summary>
        public virtual DateTime ReplayOn { get; set; }

        /// <summary>
        /// IsUpload
        /// </summary>
        public virtual Boolean IsUpload { get; set; }

        /// <summary>
        /// UploadOn
        /// </summary>
        public virtual DateTime UploadOn { get; set; }

    }
}
