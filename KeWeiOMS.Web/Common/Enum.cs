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

        已发货 = 5,
        作废订单 = 7
    }

    public enum ProductStatusEnum
    {
        销售中 = 0,
        热卖 = 1,
        滞销 = 2,
        清仓 = 3,
        停产 = 4,
        暂停销售 = 5,
        提价销售 = 6
    }

    public enum PlatformEnum
    {
        SMT = 9999998,
        Ebay = 9999997,
        Amazon = 9999996,
        Gmarket = 9999994,
        DH = 9999993
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

    public enum RoleEnum
    {
        配货人员 = 0,
        配货检验人员 = 1,
        包装人员 = 2,
        清点人员 = 3,
        到货检验人员 = 4,
        邮件回复人员 = 5,
        邮件主管 = 6
    }

    public enum ProductAttributeEnum
    {
        粉末,
        液体,
        大电池,
        纽扣电池,
        仿牌,
        电子,
        磁铁,
        普货
    }




}