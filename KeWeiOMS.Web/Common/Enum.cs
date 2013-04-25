using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public enum OrderStatusEnum
    {
        待处理 = 0,
        已处理 = 1,
        待拣货 = 2,
        待包装 = 3,
        待发货 = 4,
        已发货 = 5,
        已完成 = 6,
        作废订单 = 7,
        停售订单 = 8
    }

    public enum ProductStatusEnum
    {
        销售中 = 0,
        热卖 = 1,
        滞销 = 2,
        清仓 = 3,
        停产 = 4,
        暂停销售 = 5
    }

    public enum PlatformEnum
    {
        SMT = 9999998,
        Ebay = 9999997,
        Amazon = 9999996,
        B2C = 9999995,
        Gmarket = 9999994,
        LT = 9999993
    }

    public enum ResourceCategoryEnum
    {
        User,
        Role,
        Department
    }
    public enum TargetCategoryEnum
    {
        Module,
        PermissionItem,
        Account
    }

    public enum PrintCategoryEnum
    {
        订单,
        多物品订单,
        商品
    }


}