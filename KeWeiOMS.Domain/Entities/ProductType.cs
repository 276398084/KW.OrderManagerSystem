﻿//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ProductType
    /// 商品表
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
    public class ProductType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public virtual String SKU { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public virtual String OldSKU { get; set; }



        /// <summary>
        /// 类别
        /// </summary>
        public virtual String Category { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual String ProductName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public virtual String Standard { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual int Weight { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual int TotalWeight { get; set; }

        /// <summary>
        /// 最小重量
        /// </summary>
        public virtual double MinWeight { get; set; }

        /// <summary>
        /// 最大重量
        /// </summary>
        public virtual double MaxWeight { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public virtual int Long { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public virtual int Wide { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public virtual int High { get; set; }

        /// <summary>
        /// 库存天数
        /// </summary>
        public virtual int DayByStock { get; set; }

        /// <summary>
        /// 简单描述
        /// </summary>
        public virtual String Summary { get; set; }

        /// <summary>
        /// 包装注意事项
        /// </summary>
        public virtual String PackMemo { get; set; }

        /// <summary>
        /// 是否侵权
        /// </summary>
        public virtual int IsInfraction { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public virtual String Model { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public virtual String Brand { get; set; }

        /// <summary>
        /// 大图片网址
        /// </summary>
        public virtual String PicUrl { get; set; }

        /// <summary>
        /// 大图数量
        /// </summary>
        public virtual int PicQty { get; set; }

        /// <summary>
        /// 小图片网址
        /// </summary>
        public virtual String SPicUrl { get; set; }

        /// <summary>
        /// 采购人
        /// </summary>
        public virtual String Purchaser { get; set; }

        /// <summary>
        /// 验货人
        /// </summary>
        public virtual String Examiner { get; set; }

        /// <summary>
        /// 包装人
        /// </summary>
        public virtual String Packer { get; set; }

        /// <summary>
        /// 包装系数
        /// </summary>
        public virtual double PackCoefficient { get; set; }

        /// <summary>
        /// 电子
        /// </summary>
        public virtual int IsElectronic { get; set; }

        /// <summary>
        /// 电池
        /// </summary>
        public virtual int HasBattery { get; set; }

        /// <summary>
        /// 液体
        /// </summary>
        public virtual int IsLiquid { get; set; }

        /// <summary>
        /// 是否扫描
        /// </summary>
        public virtual int IsScan { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public virtual String Location { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 产品管理人
        /// </summary>
        public virtual String Manager { get; set; }

        /// <summary>
        /// 7
        /// </summary>
        public virtual int SevenDay { get; set; }

        /// <summary>
        /// 30
        /// </summary>
        public virtual int ThirtyDay { get; set; }

        /// <summary>
        ///15
        /// </summary>
        public virtual int Fifteen { get; set; }


        /// <summary>
        /// Enabled
        /// </summary>
        public virtual int Enabled { get; set; }

        /// <summary>
        /// 是否组合产品
        /// </summary>
        public virtual int IsZu { get; set; }

        /// <summary>
        /// rows
        /// </summary>
        public virtual String rows { get; set; }

        /// <summary>
        /// rows2
        /// </summary>
        public virtual String rows2 { get; set; }


        public virtual String Infraction { get; set; }


        public virtual String ProductAttribute { get; set; }
        
    }
}
