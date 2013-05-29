using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web.Common.Config
{
    /// <summary>
    /// API网址
    /// 
    /// </summary>
    public class SMTConfig
    {
        #region AliAPI名称
        /// <summary>
        /// 根据ID获得产品
        /// </summary>
        public const string ApifindAeProductById = "findAeProductById";
        /// <summary>
        /// 分页获得产品
        /// </summary>
        public const string ApifindProductInfoListQuery = "findProductInfoListQuery";
        /// <summary>
        /// 上传产品
        /// </summary>
        public const string ApipostAeProduct = "postAeProduct";
        /// <summary>
        /// 编辑产品
        /// </summary>
        public const string ApieditAeProduct = "editAeProduct";
        /// <summary>
        /// 计算运费
        /// </summary>
        public const string ApicalculateFreight = "calculateFreight";
        /// <summary>
        /// 根据订单Id获得订单那
        /// </summary>
        public const string ApifindOrderById = "findOrderById";
        /// <summary>
        /// 分页获得订单
        /// </summary>
        public const string ApifindOrderListQuery = "findOrderListQuery";
        /// <summary>
        /// 上传产品图片
        /// </summary>
        public const string ApiuploadTempImage = "uploadTempImage";

        /// <summary>
        /// 上传获得实际的属性设置
        /// </summary>
        public const string ApigetAttributesResultByCateId = "getAttributesResultByCateId";
        #endregion



    }
}