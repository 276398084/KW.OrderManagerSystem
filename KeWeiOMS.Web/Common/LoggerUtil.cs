using KeWeiOMS.Domain;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public class LoggerUtil
    {
        //订单日志
        public static void GetOrderRecord(OrderType order, string recordType, string Content, UserType CurrentUser, ISession NSession)
        {
            GetOrderRecord(order.Id, order.OrderNo, recordType, Content, CurrentUser, NSession);
        }

        public static void GetOrderRecord(OrderType order, string recordType, string Content, ISession NSession)
        {
            UserType CurrentUser = null;
            if (HttpContext.Current.Session["account"] != null)
            {
                CurrentUser = (UserType)HttpContext.Current.Session["account"];
            }
            GetOrderRecord(order.Id, order.OrderNo, recordType, Content, CurrentUser, NSession);
        }

        public static void GetOrderRecord(int oid, string OrderNo, string recordType, string Content, UserType CurrentUser, ISession NSession)
        {
            OrderRecordType orderRecord = new OrderRecordType();
            orderRecord.OId = oid;
            orderRecord.OrderNo = OrderNo;
            orderRecord.RecordType = recordType;
            orderRecord.CreateBy = CurrentUser.Realname;
            orderRecord.Content = Content;
            orderRecord.CreateOn = DateTime.Now;
            NSession.Save(orderRecord);
            NSession.Flush();
        }


        //商品日志
        public static void GetProductRecord(ProductType obj, string recordType, string Content,UserType CurrentUser,ISession NSession)
        {
            GetProductRecord(obj.Id, obj.OldSKU, obj.SKU, recordType, Content, CurrentUser, NSession);

        }

        private static void GetProductRecord(int OId, string OldSKU, string SKU, string recordType, string Content, UserType CurrentUser, ISession NSession)
        {
            ProductRecordType productrecoud = new ProductRecordType();
            productrecoud.OldSKU = OldSKU;
            productrecoud.SKU = SKU;
            productrecoud.OId = OId;
            productrecoud.RecordType = recordType;
            productrecoud.Content = Content;
            productrecoud.CreateBy = CurrentUser.Realname;
            productrecoud.CreateOn = DateTime.Now;
            NSession.Save(productrecoud);
            NSession.Flush();
        }


         //采购计划日志
        public static void GetPurchasePlanRecord(PurchasePlanType obj, string recordType, string Content, UserType CurrentUser, ISession NSession)
        {
            GetPurchasePlanRecord(obj.Id, obj.PlanNo, obj.SKU, recordType, Content, CurrentUser, NSession);
        }

        private static void GetPurchasePlanRecord(int OId, string PlanNo, string SKU, string recordType, string Content, UserType CurrentUser, ISession NSession)
        {
            PurchasePlanRecordType obj = new PurchasePlanRecordType();
            obj.OId = OId;
            obj.SKU = SKU;
            obj.PlanNo = PlanNo;
            obj.RecordType = recordType;
            obj.Content = Content;
            obj.CreateBy = CurrentUser.Realname;
            obj.CreateOn = DateTime.Now;
            NSession.Save(obj);
            NSession.Flush();
        }
    }
}