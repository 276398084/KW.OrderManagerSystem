using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Data.SqlClient;
using NHibernate.Criterion;

namespace KeWeiOMS.Web.Controllers
{
    [ValidateInput(false)]
    public class OrderController : BaseController
    {
        #region 页面指向
        public ViewResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            OrderType obj = GetById(id);
            obj.AddressInfo = NSession.Get<OrderAddressType>(obj.AddressId);
            ViewData["id"] = id;
            return View(obj);
        }
        public ActionResult Create()
        {
            ViewData["OrderNO"] = Utilities.GetOrderNo(NSession);
            return View();
        }

        public ActionResult StopIndex()
        {
            return View();
        }
        public ActionResult UnHandle()
        {
            return View();
        }
        public ActionResult TodayIndex()
        {
            return View();

        }
        public ActionResult QueScan()
        {
            return View();

        }

        public ActionResult AliSendScan()
        {
            return View();

        }

        public ActionResult PeiScan()
        {
            return View();

        }
        public ActionResult SendScan()
        {
            return View();

        }

        public ActionResult IsNotQue()
        {
            return View();

        }
        public ActionResult OrderExport()
        {
            return View();
        }
        public ActionResult JiScan()
        {
            return View();
        }

        public ActionResult QuestionScan()
        {
            return View();
        }
        public ActionResult QuestionOrderIndex()
        {
            return View();
        }

        public ActionResult Import()
        {
            return View();
        }

        public ActionResult SplitNoSend()
        {
            return View();
        }
        #endregion

        #region 订单处理
        [HttpPost]
        public ActionResult OrderReplace(string ids, string oldField, string newField, string fieldName)
        {
            switch (fieldName)
            {
                case "Country":
                    OrderHelper.ReplaceByCountry(ids, oldField, newField, NSession);
                    break;
                case "SKU":
                    OrderHelper.ReplaceBySKU(ids, oldField, newField, NSession);
                    break;
                case "CurrencyCode":
                    OrderHelper.ReplaceByCurrencyOrLogistic(ids, oldField, newField, 1, NSession);
                    break;
                case "LogisticMode":
                    OrderHelper.ReplaceByCurrencyOrLogistic(ids, oldField, newField, 0, NSession);
                    break;
                default:
                    break;
            }
            IQuery Query = NSession.CreateQuery(string.Format("update {3} set {0}='{1}' where {0}='{2}'", fieldName, newField, oldField, fieldName == "SKU" ? "OrderProductType" : "OrderType"));
            int num = Query.ExecuteUpdate();
            return Json(new { IsSuccess = true });
        }
        public ActionResult OrderVali()
        {
            List<CountryType> countrys = NSession.CreateQuery("from CountryType").List<CountryType>().ToList();

            List<ProductType> products = NSession.CreateQuery("from ProductType").List<ProductType>().ToList();

            List<CurrencyType> currencys = NSession.CreateQuery("from CurrencyType").List<CurrencyType>().ToList();

            List<LogisticsModeType> logistics = NSession.CreateQuery("from LogisticsModeType").List<LogisticsModeType>().ToList();
            IList<OrderType> orders = NSession.CreateQuery(" from OrderType where Status='待处理'").List<OrderType>();
            foreach (var order in orders)
            {
                OrderHelper.ValiOrder(order, countrys, products, currencys, logistics, NSession);
            }
            return Json(new { IsSuccess = true });
        }
        public ActionResult OrderMerger()
        {
            IList<OrderType> orderTypes = NSession.CreateQuery("from OrderType where Status='待处理' and Platform='Ebay' and Enabled=1 and BuyerName in (select BuyerName from OrderType where Status='待处理'  and Platform='Ebay' and Enabled=1   group by BuyerName,Country,Account having count (BuyerName)>1)").List<OrderType>();
            string ids = "";
            foreach (var order in orderTypes)
            {
                ids += order.AddressId + ",";
            }
            List<OrderAddressType> orderAddressTypes = NSession.CreateQuery("from OrderAddressType where Id in(" + ids.Trim(',') + ")").List<OrderAddressType>().ToList();

            foreach (OrderType o in orderTypes)
            {
                o.AddressInfo = orderAddressTypes.Find(x => x.Id == o.AddressId);
            }
            List<int> strs = new List<int>();
            List<OrderType> copyOrders = new List<OrderType>(orderTypes.ToArray());
            foreach (OrderType o in orderTypes)
            {
                if (strs.Contains(o.Id))
                    continue;
                List<OrderType> orders =
                    copyOrders.Where(
                        x =>
                        x.BuyerName == o.BuyerName && x.Country == o.Country && x.Account == o.Account &&
                        x.AddressInfo.Street == o.AddressInfo.Street).ToList();
                OrderType order = new OrderType();

                if (orders.Count > 1)
                {
                    NSession.Clear();
                    order = CloneObjectEx(o) as OrderType;
                    order.Id = 0;
                    order.OrderNo = Utilities.GetOrderNo(NSession);
                    order.Amount = 0;
                    order.IsMerger = 1;
                    order.Enabled = 1;
                    NSession.SaveOrUpdate(order);
                    NSession.Flush();
                    foreach (var orderType in orders)
                    {
                        strs.Add(orderType.Id);
                        order.Amount += orderType.Amount;
                        order.OrderExNo += "|" + orderType.OrderExNo;
                        order.TId += "|" + orderType.TId;
                        orderType.MId = order.Id;
                        orderType.IsMerger = 1;
                        orderType.Enabled = 0;
                        NSession.Clear();
                        NSession.SaveOrUpdate(orderType);
                        NSession.Flush();
                        IList<OrderProductType> orderProductTypes =
                        NSession.CreateQuery(" from OrderProductType where OId=" + orderType.Id).List<OrderProductType>();
                        foreach (var orderProductType in orderProductTypes)
                        {
                            orderProductType.Id = 0;
                            orderProductType.OId = order.Id;
                            orderProductType.OrderNo = order.OrderNo;
                            NSession.Clear();
                            NSession.SaveOrUpdate(orderProductType);
                            NSession.Flush();
                        }
                    }
                    NSession.Clear();
                    NSession.SaveOrUpdate(order);
                    NSession.Flush();
                }
            }
            return Json(new { IsSuccess = true });
        }

        public object CloneObjectEx(object ObjectInstance)
        {

            BinaryFormatter bFormatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            bFormatter.Serialize(stream, ObjectInstance);
            stream.Seek(0, SeekOrigin.Begin);
            return bFormatter.Deserialize(stream);
        }
        #endregion


        [HttpPost]
        public ActionResult ImportAmount(FormCollection form)
        {
            string Platform = form["Platform"];
            string Account = form["Account"];
            AccountType account = NSession.Get<AccountType>(Convert.ToInt32(Account));
            string file = form["hfile"];
            OrderHelper.ImportByAmount(account, file, NSession);
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public ActionResult Import(FormCollection form)
        {
            try
            {
                string Platform = form["Platform"];
                string Account = form["Account"];
                AccountType account = NSession.Get<AccountType>(Convert.ToInt32(Account));
                string file = form["hfile"];
                List<ResultInfo> results = new List<ResultInfo>();
                switch ((PlatformEnum)Enum.Parse(typeof(PlatformEnum), Platform))
                {
                    case PlatformEnum.SMT:
                        results = OrderHelper.ImportBySMT(account, file, NSession);
                        break;
                    case PlatformEnum.Ebay:
                        break;
                    case PlatformEnum.Amazon:
                        results = OrderHelper.ImportByAmazon(account, file, NSession);
                        break;
                    case PlatformEnum.B2C:
                        results = OrderHelper.ImportByB2C(account, file, NSession);
                        break;
                    case PlatformEnum.Gmarket:
                        results = OrderHelper.ImportByGmarket(account, file, NSession);
                        break;
                    case PlatformEnum.LT:
                        break;
                    default:
                        break;
                }
                Session["Results"] = results;
                return Json(new { IsSuccess = true, Info = true });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, ErrorMsg = ex.Message, Info = true });
            }
        }

        [HttpPost]
        public ActionResult GExport(string f)
        {
            try
            {
                int intColCount = 0;
                DataTable mydt = new DataTable("myTableName");

                DataColumn mydc;
                DataRow mydr;
                int col = 0;
                CsvReader csvReader = new CsvReader(f, Encoding.Default);
                List<string[]> liststrs = csvReader.ReadAllRow();
                string ids = "";
                for (int i = 0; i < liststrs.Count; i++)
                {
                    string[] aryline = liststrs[i];
                    if (i == 0)
                    {
                        for (int j = 0; j < aryline.Length; j++)
                        {
                            if (aryline[j] == "Cart no.")
                            {
                                col = j;
                            }
                            mydc = new DataColumn(aryline[j]);
                            mydt.Columns.Add(mydc);
                        }
                    }
                    else
                    {
                        mydr = mydt.NewRow();
                        for (int j = 0; j < mydt.Columns.Count; j++)
                        {

                            if (j == col)
                            {
                                ids += aryline[j] + ",";
                            }
                            if (aryline.Length > j)
                                mydr[j] = aryline[j];
                        }
                        mydt.Rows.Add(mydr);
                    }
                }

                ids = ids.Trim(',');
                ids = ids.Replace(",", "','");
                List<OrderType> list = NSession.CreateQuery("from OrderType where OrderExNo in('" + ids + "')").List<OrderType>().ToList();
                foreach (DataRow dataRow in mydt.Rows)
                {
                    OrderType order = list.Find(p => p.OrderExNo.Trim().ToUpper() == dataRow["Cart no."].ToString().Trim().ToUpper());
                    if (order != null)
                    {
                        dataRow["Tracking no."] = order.TrackCode;
                        dataRow["Delivery company"] = "Chinapost registered airmail";
                    }
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(mydt);
                Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
                return Json(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, ErrorMsg = ex.Message });

            }
        }

        [HttpPost]
        public ActionResult Synchronous(string Platform, int Account, DateTime st, DateTime et)
        {

            AccountType account = NSession.Get<AccountType>(Convert.ToInt32(Account));
            List<ResultInfo> results = new List<ResultInfo>();
            switch ((PlatformEnum)Enum.Parse(typeof(PlatformEnum), Platform))
            {
                case PlatformEnum.Ebay:
                    results = OrderHelper.APIByEbay(account, st, et, NSession);
                    break;
                case PlatformEnum.Amazon:
                    break;
                case PlatformEnum.B2C:
                    results = OrderHelper.APIByB2C(account, st, et, NSession);
                    break;
                case PlatformEnum.Gmarket:
                case PlatformEnum.LT:
                case PlatformEnum.SMT:
                default:
                    return Json(new { IsSuccess = false, ErrorMsg = "该平台没有同步功能！" });
            }
            Session["Results"] = results;
            return Json(new { IsSuccess = true, Info = true });
        }

        [HttpPost]
        public JsonResult Create(OrderType obj)
        {
            try
            {

                if (obj.OrderExNo.Trim() == "" || OrderHelper.IsExist(obj.OrderExNo.Trim(), NSession))
                {
                    return Json(new { IsSuccess = false, ErrorMsg = "平台订单号为空或者重复" });
                }
                object c = NSession.CreateQuery("select count(Id) from OrderType where OrderNo='" + obj.OrderNo + "'").UniqueResult();
                if (Convert.ToInt32(c) > 0)
                {
                    obj.OrderNo = Utilities.GetOrderNo(NSession);
                }

                obj.AddressInfo.CountryCode = obj.AddressInfo.Country;
                obj.AddressInfo.Email = obj.BuyerEmail;
                NSession.Save(obj.AddressInfo);
                AccountType acc = NSession.Get<AccountType>(Utilities.ToInt(obj.Account));
                if (acc != null)
                    obj.Account = acc.AccountName;
                obj.AddressId = obj.AddressInfo.Id;
                obj.Country = obj.AddressInfo.Country;
                obj.Status = OrderStatusEnum.待处理.ToString();
                obj.GenerateOn = obj.ScanningOn = obj.CreateOn = DateTime.Now;
                List<OrderProductType> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderProductType>>(obj.rows);
                obj.Enabled = 1;
                NSession.Save(obj);
                foreach (var item in list)
                {
                    item.OId = obj.Id;
                    item.OrderNo = obj.OrderNo;
                    NSession.Save(item);
                }
                NSession.Flush();
                LoggerUtil.GetOrderRecord(obj, "新建订单", "创建订单", CurrentUser, NSession);

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OrderType GetById(int Id)
        {
            OrderType obj = NSession.Get<OrderType>(Id);
            if (obj == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return obj;
            }
        }

        public ActionResult ScanExport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ScanExport3(DateTime st, DateTime et, string u, string key)
        {
            string sql = string.Format("select OrderNo,OrderExNo,TrackCode,TrackCode2,Account,ScanningOn from Orders where Status in ('已发货','已完成') and TrackCode<> TrackCode2 and TrackCode2 is not null and ScanningOn  between '{0}' and '{1}'", st.ToString("yyyy/MM/dd HH:mm:ss"), et.ToString("yyyy/MM/dd HH:mm:ss"));
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            // 设置编码和附件格式 
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.Web.HttpContext.Current.Response.Charset = "gb2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + st.ToShortDateString() + "-" + et.ToShortDateString() + ".xls");
            return File(System.Text.Encoding.UTF8.GetBytes(ExcelHelper.GetExcelXml(ds)), "attachment;filename=" + st.ToShortDateString() + "-" + et.ToShortDateString() + ".xls");
        }
        [HttpPost]
        public ActionResult ScanExport2(DateTime st, DateTime et, string u, string key)
        {
            string sql = string.Format("select OrderNo,OrderExNo,TrackCode,TrackCode2,Account,CreateOn from Orders where TrackCode2 is not null and TrackCode2 <>''  and CreateOn  between '{0}' and '{1}'", st.ToString("yyyy/MM/dd HH:mm:ss"), et.ToString("yyyy/MM/dd HH:mm:ss"));
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            // 设置编码和附件格式 
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.Web.HttpContext.Current.Response.Charset = "gb2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + st.ToShortDateString() + "-" + et.ToShortDateString() + ".xls");
            return File(System.Text.Encoding.UTF8.GetBytes(ExcelHelper.GetExcelXml(ds)), "attachment;filename=" + st.ToShortDateString() + "-" + et.ToShortDateString() + ".xls");
        }

        [HttpPost]
        public ActionResult ScanExport(DateTime st, DateTime et, string u, string key)
        {
            if (u != "")
            {
                u = "ScanningBy= '" + u + "' and";
            }
            if (key != "")
            {
                key = "and ScanningBy= '" + key + "' ";
            }
            List<LogisticsModeType> modes = NSession.CreateQuery("from LogisticsModeType").List<LogisticsModeType>().ToList();
            string sql = "select OrderNo as 'PackageNo',Weight as 'PackageWeight',ScanningBy,TrackCode as 'TrackCode',ScanningOn as 'ShippedTime',LogisticMode as 'LogisticsMode',Country from Orders where Status in ('已发货','已完成') and {0}  ScanningOn  between '{1}' and '{2}' {3}  order by ScanningOn asc ";
            sql = string.Format(sql, u, st.ToString("yyyy/MM/dd HH:mm:ss"), et.ToString("yyyy/MM/dd HH:mm:ss"), key);
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                LogisticsModeType mode = modes.Find(p => p.LogisticsCode == dataRow["LogisticsMode"].ToString().Trim());
                if (mode != null)
                    dataRow["LogisticsMode"] = mode.LogisticsName.Trim();
            }
            // 设置编码和附件格式 
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.Web.HttpContext.Current.Response.Charset = "gb2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + st.ToShortDateString() + "-" + et.ToShortDateString() + ".xls");
            return File(System.Text.Encoding.UTF8.GetBytes(ExcelHelper.GetExcelXml(ds)), "attachment;filename=" + st.ToShortDateString() + "-" + et.ToShortDateString() + ".xls");
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            OrderType obj = GetById(id);
            obj.AddressInfo = NSession.Get<OrderAddressType>(obj.AddressId);
            ViewData["id"] = id;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        [ValidateInput(false)]
        public ActionResult Edit(OrderType obj)
        {
            try
            {
                obj.Enabled = 1;
                OrderType obj2 = GetById(obj.Id);
                NSession.Update(obj.AddressInfo);
                NSession.Flush();
                NSession.Clear();
                obj.Country = obj.AddressInfo.Country;
                string str = Utilities.GetObjEditString(obj2, obj);
                NSession.Update(obj);
                NSession.Flush();
                NSession.Clear();
                List<OrderProductType> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderProductType>>(obj.rows);
                List<OrderProductType> pis = NSession.CreateQuery("from OrderProductType where OId=" + obj.Id).List<OrderProductType>().ToList<OrderProductType>();
                if (list.Count != pis.Count)
                {
                    str += "组合产品由<br>";
                    foreach (var item in pis)
                    {
                        str += Zu1(item);
                    }
                    str += "修改为<br> ";
                    foreach (var item in list)
                    {
                        // str += " ExSKU:" + item.ExSKU + " 名称:" + item.Title + " SKU:" + item.SKU + " 数量:" + item.Qty + " 规格:" + item.Standard + " 价格：" + item.Price + " 网址：" + item.Url + " 描述：" + item.Remark + "<br>";
                        str += Zu1(item);
                    }
                    str += "<br>";
                }
                else
                {
                    foreach (var it in pis)
                    {
                        int check = 0;
                        foreach (var it1 in list)
                        {
                            if (it.ExSKU == it1.ExSKU && it.Title == it1.Title && it.SKU == it1.SKU && it.Qty == it1.Qty && it.Standard == it1.Standard && it.Price == it1.Price && it.Url == it1.Url && it.Remark == it1.Remark)
                            {
                                check = 1;
                            }
                        }
                        if (check != 1)
                        {
                            str += "组合产品由<br>";
                            foreach (var item in pis)
                            {
                                str += Zu1(item);
                            }
                            str += "修改为<br> ";
                            foreach (var item in list)
                            {
                                str += Zu1(item);
                            }
                            str += "<br>";

                        }
                    }

                }

                NSession.CreateQuery("delete from OrderProductType where OId=" + obj.Id).ExecuteUpdate();
                foreach (OrderProductType orderProductType in list)
                {
                    orderProductType.OId = obj.Id;
                    orderProductType.OrderNo = obj.OrderNo;
                    NSession.Save(orderProductType);
                    NSession.Flush();
                }
                LoggerUtil.GetOrderRecord(obj, "修改订单", str, CurrentUser, NSession);

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });

        }
        public string Zu1(OrderProductType item)
        {
            string str = " ExSKU:" + item.ExSKU + " 名称:" + item.Title + " SKU:" + item.SKU + " 数量:" + item.Qty + " 规格:" + item.Standard + " 价格：" + item.Price + " 网址：" + item.Url + " 描述：" + item.Remark + "<br>";
            return str;
        }

        [HttpPost]
        public JsonResult SplitSendOrder(string o, int c)
        {
            NSession.Clear();
            OrderType obj = GetById(Utilities.ToInt(o));
            obj.IsSplit = 1;
            NSession.Update(obj);
            NSession.Flush();
            for (int i = 0; i < c; i++)
            {
                NSession.Clear();
                obj.RMB = 0;
                obj.Amount = 0;
                obj.OrderNo = Utilities.GetOrderNo(NSession);
                obj.MId = Utilities.ToInt(o);
                NSession.Save(obj);
                NSession.Flush();
                LoggerUtil.GetOrderRecord(obj, "发货拆分订单！", "拆分新建！", CurrentUser, NSession);
            }
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public JsonResult SplitOrder(string o, string rows)
        {
            NSession.Clear();
            OrderType obj = GetById(Utilities.ToInt(o));
            obj.IsSplit = 1;
            // obj.IsRepeat=1
            NSession.Update(obj);
            NSession.Flush();
            List<OrderProductType> ps = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderProductType>>(rows);
            LoggerUtil.GetOrderRecord(obj, "拆分订单！", "将订单拆分！", CurrentUser, NSession);
            NSession.Clear();
            obj.Amount = 0;
            obj.IsPrint = 0;
            obj.RMB = 0;
            obj.OrderNo = Utilities.GetOrderNo(NSession);
            obj.MId = Utilities.ToInt(o);
            NSession.Save(obj);
            NSession.Flush();
            OrderHelper.SaveAmount(obj, NSession);
            foreach (var orderProductType in ps)
            {
                NSession.Clear();
                OrderProductType opt = NSession.Get<OrderProductType>(orderProductType.Id);
                opt.Qty = opt.Qty - orderProductType.Qty;
                NSession.Update(opt);
                NSession.Flush();
                NSession.Clear();
                orderProductType.Id = 0;
                orderProductType.OId = obj.Id;
                orderProductType.OrderNo = obj.OrderNo;
                NSession.Save(orderProductType);
                NSession.Flush();
            }
            LoggerUtil.GetOrderRecord(obj, "拆分订单！", "拆分新建！", CurrentUser, NSession);
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public JsonResult ReOrder(string o, string rows)
        {
            NSession.Clear();
            OrderType obj = GetById(Utilities.ToInt(o));
            obj.IsRepeat = 1;
            NSession.Update(obj);
            NSession.Flush();
            List<OrderProductType> ps = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderProductType>>(rows);
            NSession.Clear();
            obj.Amount = 0;
            obj.IsPrint = 0;
            obj.IsRepeat = 1;
            obj.TrackCode = "";
            obj.Weight = 0;
            obj.CreateOn = DateTime.Now;
            obj.MId = obj.Id;
            obj.RMB = 0;
            obj.Status = OrderStatusEnum.已处理.ToString();
            obj.IsOutOfStock = 0;
            obj.IsAudit = 0;
            obj.OrderNo = Utilities.GetOrderNo(NSession);
            NSession.Save(obj);
            NSession.Flush();
            foreach (var orderProductType in ps)
            {
                orderProductType.OId = obj.Id;
                orderProductType.OrderNo = obj.OrderNo;
                NSession.Save(orderProductType);
                NSession.Flush();
            }
            SetQuestionOrder("订单重发", obj);

            LoggerUtil.GetOrderRecord(obj, "重发！", "将订单重发！", CurrentUser, NSession);
            return Json(new { IsSuccess = true });
        }
        private void SetQuestionOrder(string subject, OrderType orderType, string content = "")
        {
            QuestionOrderType question = new QuestionOrderType();
            question.OId = orderType.Id;
            question.OrderNo = orderType.OrderNo;
            question.Status = 0;
            question.Subjest = subject;
            if (string.IsNullOrEmpty(content)) { question.Content = orderType.CutOffMemo; }
            else
            {
                question.Content = content;
            }

            question.CreateBy = CurrentUser.Realname;
            question.CreateOn = DateTime.Now;
            question.SolveOn = DateTime.Now;
            NSession.Save(question);
            NSession.Flush();
        }


        [HttpPost]
        public JsonResult ReSend(string o)
        {
            IList<OrderType> list = NSession.CreateQuery(" from OrderType where Id in(" + o + ")").List<OrderType>();
            foreach (OrderType orderType in list)
            {
                if (orderType.Status == OrderStatusEnum.已发货.ToString())
                {
                    LoggerUtil.GetOrderRecord(orderType, "重新发货！", "将订单从已发货的订单中转为 待发货，重新发货！", CurrentUser, NSession);

                    IList<OrderProductType> ps = NSession.CreateQuery("from OrderProductType where OId=" + orderType.Id).List<OrderProductType>();
                    foreach (OrderProductType orderProductType in ps)
                    {
                        Utilities.StockIn(1, orderProductType.SKU.Trim(), orderProductType.Qty, 0, "重新发货",
                                          CurrentUser.Realname, "", NSession, true);
                    }
                    orderType.Status = OrderStatusEnum.待发货.ToString();
                    NSession.Save(orderType);
                    NSession.Flush();
                }

            }
            return Json(new { IsSuccess = true });
        }

        [HttpGet]
        public ActionResult ExportDown(string Id)
        {
            string str = "";
            object sb = Session["ExportDown"];
            if (sb != null)
            {
                str = sb.ToString();
            }
            if (Id == null)
            {
                System.Web.HttpContext.Current.Response.ContentType = "text/plain";
                System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                System.Web.HttpContext.Current.Response.Charset = "gb2312";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=zm.txt");
                return File(System.Text.Encoding.UTF8.GetBytes(str), "attachment;filename=zm.txt");
            }
            else
            {
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                System.Web.HttpContext.Current.Response.Charset = "gb2312";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
                return File(System.Text.Encoding.UTF8.GetBytes(str), "attachment;filename=" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
            }
        }

        [HttpPost]

        public ActionResult OrderHoldUp(string o, string t, string d, int s)
        {
            IList<OrderType> list = NSession.CreateQuery(" from OrderType where Id in(" + o + ")").List<OrderType>();
            foreach (OrderType orderType in list)
            {
                if (orderType.Status != OrderStatusEnum.已发货.ToString())
                {
                    NSession.Clear();
                    orderType.IsError = 1;
                    orderType.CutOffMemo = t + " " + d;
                    NSession.Update(orderType);
                    NSession.Flush();
                    string subjest = "拦截";
                    if (s == 1)
                    {
                        subjest = "拦截-重置产品入库";
                    }
                    SetQuestionOrder(subjest, orderType);

                    LoggerUtil.GetOrderRecord(orderType, "拦截订单！", "将订单拦截，原因：" + t + " " + d, CurrentUser, NSession);
                }

            }
            return Json(new { IsSuccess = true });
        }



        [HttpPost]
        public ActionResult ExportZM(string o)
        {
            IList<OrderType> list = NSession.CreateQuery(" from OrderType where Id in(" + o + ")").List<OrderType>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("order-id	buyer-name	buyer-phone-number	sku	product-name	quantity-purchased	recipient-name	ship-address-1	ship-address-2	ship-city	ship-state	ship-postal-code	ship-country");
            string tmpValues = "{0}\t{1}\t{2}\t{3}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}";
            foreach (OrderType foo in list)
            {
                IList<OrderProductType> ps =
                    NSession.CreateQuery("from OrderProductType where OId=" + foo.Id).List<OrderProductType>();
                if (ps.Count > 0)
                {
                    OrderAddressType orderAddress = NSession.Get<OrderAddressType>(foo.AddressId);
                    sb.AppendLine(string.Format(tmpValues, foo.OrderNo, foo.BuyerName, orderAddress.Tel, ps[0].SKU,
                                                ps[0].Qty, orderAddress.Addressee.Replace("\t", " ").Replace(",", " ").Replace("\r", " ").Replace("\n", " "), orderAddress.Street.Replace("\t", " ").Replace(",", " ").Replace("\r", " ").Replace("\n", " "), "",
                                                orderAddress.City.Replace("\t", " ").Replace(",", " ").Replace("\r", " ").Replace("\n", " "), orderAddress.Province.Replace("\t", " ").Replace(",", " ").Replace("\r", " ").Replace("\n", " "), orderAddress.PostCode,
                                                orderAddress.Country));

                }
            }
            Session["ExportDown"] = sb.ToString();
            return Json(new { IsSuccess = true });
        }

        public ActionResult ExportOrder2(string ids, string s)
        {
            string sql = @"select '' as '记录号',  O.OrderNo,OrderExNo,CurrencyCode,Amount,OrderFees,TId,BuyerName,BuyerEmail,LogisticMode,Country,O.Weight,TrackCode,OP.SKU,OP.Qty,p.Price,OP.Standard,0.00 as 'TotalPrice',O.CreateOn,O.ScanningOn,O.ScanningBy,O.Account,cast(O.IsSplit as nvarchar) as '拆分',cast(O.IsRepeat as nvarchar) as '重发'   from Orders O left join OrderProducts OP ON O.Id =OP.OId 
left join Products P On OP.SKU=P.SKU ";
            sql += " where  O.IsError =0 and O.Enabled=1 and O." + s + " in('" + ids.Replace(" ", "").Replace("\r", "").Trim().Replace("\n", "','").Replace("''", "") + "')";
            DataSet ds = GetOrderExport(sql);
            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public ActionResult ExportOrder(string o, string st, string et, string s, string a, string p, string dd)
        {
            StringBuilder sb = new StringBuilder();
            string sql = @"select '' as '记录号',O.OrderNo,OrderExNo,CurrencyCode,Amount,OrderFees,TId,BuyerName,BuyerEmail,LogisticMode,Country,O.Weight,TrackCode,OP.SKU,OP.Qty,p.Price,OP.Standard,0.00 as 'TotalPrice',O.CreateOn,O.ScanningOn,O.ScanningBy,O.Account,cast(O.IsSplit as nvarchar) as '拆分',cast(O.IsRepeat as nvarchar) as '重发'  from Orders O left join OrderProducts OP ON O.Id =OP.OId 
left join Products P On OP.SKU=P.SKU ";
            if (string.IsNullOrEmpty(o))
            {
                if (!string.IsNullOrEmpty(s))
                {
                    sql += " where O.IsError =0 and  O.Enabled=1 and O.Status='" + s + "' and O.Account='" + a + "' and  O." + dd + " between '" + st + "' and '" + et + "'";
                }
                else
                {
                    sql += " where O.IsError =0 and  O.Enabled=1 and O.Account='" + a + "' and  O." + dd + " between '" + st + "' and '" + et + "'";
                }
            }
            else
            {
                sql += " where O.IsError =0 and   O.Id in(" + o + ")";
            }
            DataSet ds = GetOrderExport(sql);

            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            return Json(new { IsSuccess = true });
        }

        private DataSet GetOrderExport(string sql)
        {
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql + " order by O.OrderExNo,O.OrderNo asc";
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);

            DataTable dt = ds.Tables[0];

            int i = 1;
            List<string> list = new List<string>();
            string str = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (list.Contains(dr["OrderExNo"].ToString().Trim()))
                {
                    if (dr["OrderNo"].ToString() == str)
                    {
                        dr[0] = "";
                        dr[1] = "";
                        dr[11] = 0;
                        dr[12] = "";
                    }
                    dr[2] = "";
                    dr[3] = "";
                    dr[4] = 0;
                    dr[5] = 0;
                    dr[6] = "";
                    dr[7] = "";
                    dr[8] = "";
                    dr[9] = "";
                    dr[10] = "";
                }
                else
                {
                    dr["记录号"] = i;
                    i++;
                    DataRow[] drs = dt.Select("OrderExNo='" + dr["OrderExNo"] + "'");
                    double amount = 0;
                    str = dr["OrderNo"].ToString();
                    foreach (DataRow dataRow in drs)
                    {
                        amount += Utilities.ToDouble(dataRow["Qty"].ToString()) * Utilities.ToDouble(dataRow["Price"].ToString());
                    }
                    dr["TotalPrice"] = amount;
                    list.Add(dr["OrderExNo"].ToString().Trim());
                }
                dr["拆分"] = dr["拆分"].ToString() == "1" ? "是" : "否";
                dr["重发"] = dr["重发"].ToString() == "1" ? "是" : "否";
            }
            return ds;
        }

        [HttpPost]
        public ActionResult ExportBiLiShi(string o)
        {
            StringBuilder sb = new StringBuilder();
            string sql = "select  * from Orders where O.Id in(" + o + ")";
            DataSet ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public ActionResult ErrorOrder(string o, string t, string d)
        {
            string str = t + " " + d;
            int i = NSession.CreateQuery(" Update OrderType set Status='" + OrderStatusEnum.作废订单.ToString() + "', IsAudit=0,SellerMemo='" + str + "'  where Id in(" + o + ")").ExecuteUpdate();
            IList<OrderType> orders = NSession.CreateQuery("from OrderType where Id In (" + o + ")").List<OrderType>();
            foreach (var orderType in orders)
            {
                SetQuestionOrder("作废订单-重置包裹入库", orderType);
                LoggerUtil.GetOrderRecord(orderType, "订单作废！", "将订单的状态设为作废订单！", CurrentUser, NSession);
            }
            if (i > 0)
                return Json(new { IsSuccess = true });
            else
            {
                return Json(new { IsSuccess = false });
            }
        }

        public ActionResult SplitZu(string o)
        {
            IList<OrderProductType> orders = NSession.CreateQuery("from OrderProductType where OId In (" + o + ")").List<OrderProductType>();
            OrderHelper.SplitProduct(orders, NSession);
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        public ActionResult ReError(string o)
        {

            int t = NSession.CreateQuery(" Update OrderType set Status='" + OrderStatusEnum.已处理.ToString() + "',IsError=0 where Id in(" + o + ")").ExecuteUpdate();
            IList<OrderType> orders = NSession.CreateQuery("from OrderType where Id In (" + o + ")").List<OrderType>();
            foreach (var orderType in orders)
            {
                LoggerUtil.GetOrderRecord(orderType, "设置订单作废！", "将订单状态设置为作废！", CurrentUser, NSession);
            }
            if (t > 0)
                return Json(new { IsSuccess = true });
            else
            {
                return Json(new { IsSuccess = false });
            }
        }

        [HttpPost]
        public ActionResult ReQue(string o)
        {
            int t = NSession.CreateQuery(" Update OrderType set IsOutOfStock=0 where Id in(" + o + ")").ExecuteUpdate();
            int t2 = NSession.CreateQuery(" Update OrderProductType set IsQue=0 where OId in(" + o + ")").ExecuteUpdate();
            IList<OrderType> orders = NSession.CreateQuery("from OrderType where Id In (" + o + ")").List<OrderType>();
            foreach (var orderType in orders)
            {
                LoggerUtil.GetOrderRecord(orderType, "撤销订单的缺货状态！", "将订单的缺货标记删除！", CurrentUser, NSession);
            }
            if (t > 0)
                return Json(new { IsSuccess = true });
            else
            {
                return Json(new { IsSuccess = false });
            }
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(string o)
        {
            try
            {

                NSession.Delete(" from OrderType where Id in(" + o + ")");
                NSession.Flush();

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        public JsonResult GetOrderBySend(string o)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待发货.ToString() || (!Config.IsJi && order.Status == OrderStatusEnum.待包装.ToString()))
                {
                    if (order.IsAudit == 0)
                    {
                        string tttt = "订单:" + order.OrderNo + ", 需要审核";
                        return Json(new { IsSuccess = false, Result = tttt });
                    }
                    if (order.IsError == 0 && string.IsNullOrEmpty(order.CutOffMemo))
                    {
                        string html = "订单:" + order.OrderNo + ", 当前状态：待发货，可以发货。<br>发货方式：" +
                                "<s id='logisticsMode'>" + order.LogisticMode + "</s>";
                        return Json(new { IsSuccess = true, Result = html });
                    }
                    else
                    {
                        string html = "订单:" + order.OrderNo + ", 无法扫描，请拦截此包裹，原因：" + order.CutOffMemo;
                        return Json(new { IsSuccess = false, Result = html });
                    }

                }
                return Json(new { IsSuccess = false, Result = " 无法出库！ 当前状态为：" + order.Status + "，需要订单状态为“待发货”方可扫描！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        public JsonResult OutStockBySend(string o, string t, int s, string l, double w)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待发货.ToString() || (!Config.IsJi && order.Status == OrderStatusEnum.待包装.ToString()))
                {
                    order.TrackCode = t;
                    order.Weight = Convert.ToInt32(w);
                    if (l != "")
                    {
                        order.LogisticMode = l;
                    }
                    order.ScanningOn = DateTime.Now;
                    order.Status = OrderStatusEnum.已发货.ToString();
                    order.ScanningBy = CurrentUser.Realname;
                    NSession.Update(order);
                    NSession.Flush();
                    IList<OrderProductType> ps =
                        NSession.CreateQuery("from OrderProductType where OId=" + order.Id).List<OrderProductType>();
                    foreach (var orderProductType in ps)
                    {
                        Utilities.StockOut(s, orderProductType.SKU, orderProductType.Qty, "扫描出库", CurrentUser.Realname, "", order.OrderNo, NSession);
                    }
                    NSession.CreateQuery("update SKUCodeType set IsSend=1,SendOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where OrderNo ='" + order.OrderNo + "'").ExecuteUpdate();
                    LoggerUtil.GetOrderRecord(order, "订单扫描发货！", "将订单扫描发货了！", CurrentUser, NSession);
                    new System.Threading.Thread(TrackCodeUpLoad) { IsBackground = true }.Start(order);
                    string html = "订单： " + order.OrderNo + "已经发货";
                    return Json(new { IsSuccess = true, Result = html, OId = order.Id });
                }
                return Json(new { IsSuccess = false, Result = " 无法出库！ 当前状态为：" + order.Status + "，需要订单状态为“待发货”方可扫描！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        public JsonResult GetOrderByAliSend(string o, int f)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                string html = "";
                OrderType order = orders[0];

                if (string.IsNullOrEmpty(order.TrackCode2))
                {
                    html = "订单:" + order.OrderNo + ", 没有设置过速卖通追踪号!可以设置条码";
                    return Json(new { IsSuccess = true, Result = html });
                }
                else
                {
                    if (f == 1)
                    {
                        html = "订单:" + order.OrderNo + ", 已经设置覆盖扫描!可以设置条码";
                        return Json(new { IsSuccess = true, Result = html });
                    }

                }
                html = "订单:" + order.OrderNo + ", 已经扫描过!";
                return Json(new { IsSuccess = false, Result = html });





            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        public JsonResult OutStockByAliSend(string o, string t, int f)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                string html = "";

                if (string.IsNullOrEmpty(order.TrackCode2))
                {
                    order.TrackCode2 = t;
                    NSession.Update(order);
                    NSession.Flush();
                    LoggerUtil.GetOrderRecord(order, "订单速卖通扫描！", "订单速卖通扫描！", CurrentUser, NSession);
                    html = "订单:" + order.OrderNo + ", 设置成功，追踪号为：" + t;
                    return Json(new { IsSuccess = true, Result = html });
                }
                else
                {
                    if (f == 1)
                    {
                        order.TrackCode2 = t;
                        NSession.Update(order);
                        NSession.Flush();
                        LoggerUtil.GetOrderRecord(order, "订单速卖通扫描！", "订单速卖通扫描！", CurrentUser, NSession);
                        html = "订单:" + order.OrderNo + ", 设置成功，追踪号为：" + t;
                        return Json(new { IsSuccess = true, Result = html });
                    }

                }
                html = "订单:" + order.OrderNo + ", 已经扫描过!";
                return Json(new { IsSuccess = false, Result = html });



            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }


        /// <summary>
        /// 4A级添加挂号到平台
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AAAA()
        {
            AccountType account = NSession.CreateQuery("from AccountType where Id=16").List<AccountType>()[0];

            OrderHelper.APIByEbayFee(account, DateTime.Now.AddDays(-27), DateTime.Now.AddDays(1), NSession);

            return Json(new { IsS = 1 });

            //List<string> list = new List<string>();

            //foreach (string s in list)
            //{
            //    Utilities.SetComposeStock(s, NSession);
            //}



            //IList<OrderProductType> orders = NSession.CreateQuery("from OrderProductType where OId in (select Id from OrderType where Status='已处理')").List<OrderProductType>();
            //OrderHelper.SplitProduct(orders, NSession);
            return Json(new { IsS = 1 }, JsonRequestBehavior.AllowGet);
            //IList<OrderType> orders = NSession.CreateQuery("from OrderType where CreateOn>'2013-03-20'").List<OrderType>();
            //List<CurrencyType> currencys = NSession.CreateQuery("from CurrencyType").List<CurrencyType>().ToList();
            //NSession.Clear();
            //foreach (var orderType in orders)
            //{
            //    orderType.Freight = Convert.ToDouble(OrderHelper.GetFreight(orderType.Weight, orderType.LogisticMode, orderType.Country, NSession));
            //    NSession.SaveOrUpdate(orderType);

            //    OrderHelper.SaveAmount(orderType, currencys, NSession);
            //}
            //return Json(new { IsS = 1 }, JsonRequestBehavior.AllowGet);

            //IList<OrderType> orders =
            //    NSession.CreateQuery("from OrderType where Account='jinbostore' and Status='已发货' and ScanningOn>'2013-03-25'").List
            //        <OrderType>();
            //foreach (var orderType in orders)
            //{
            //    Thread.Sleep(1000);
            //    new System.Threading.Thread(TrackCodeUpLoad) { IsBackground = true }.Start(orderType);
            //}
            //return Json(new { IsS = 1 }, JsonRequestBehavior.AllowGet);
        }

        void TrackCodeUpLoad(object order)
        {
            ISession session = NhbHelper.OpenSession();
            OrderType o = order as OrderType;
            if (o != null)
            {

                o.Freight = Convert.ToDouble(OrderHelper.GetFreight(o.Weight, o.LogisticMode, o.Country, session));
                session.SaveOrUpdate(o);
                session.Flush();

                //上传挂号条码
                UploadTrackCode(o);
            }
            session.Close();
        }

        private void UploadTrackCode(OrderType o)
        {
            if (o.Platform == PlatformEnum.B2C.ToString())
            {
                try
                {
                    using (
                        SqlConnection connection =
                            new SqlConnection("server=97.74.123.157;database=FeiduGS;uid=sa;pwd=`1q2w3e4r"))
                    {
                        string sql = "update GS_Order set ems='" + o.TrackCode + "' , ShippingStauts=1 where OrderNo='" +
                                     o.OrderExNo + "'";
                        SqlCommand sqlCommand = new SqlCommand(sql, connection);
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        connection.Close();
                        connection.Dispose();
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }

            if (o.Platform == PlatformEnum.Ebay.ToString())
            {
                if (o.TrackCode.StartsWith("R"))
                    EBayUtil.EbayUploadTrackCode(o.Account, o);
            }
        }

        public JsonResult GetOrderByQue(string orderNo)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + orderNo + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待拣货.ToString() || (order.Status == OrderStatusEnum.已处理.ToString()))
                {
                    if (order.IsOutOfStock != 1)
                    {
                        if (order.IsError == 1 || !string.IsNullOrEmpty(order.CutOffMemo))
                        {
                            string tttt = "订单:" + order.OrderNo + ", 无法扫描，请拦截此包裹，原因：" + order.CutOffMemo;
                            return Json(new { IsSuccess = false, Result = tttt });
                        }

                        if (order.IsAudit == 0)
                        {
                            string tttt = "订单:" + order.OrderNo + ", 需要审核";
                            return Json(new { IsSuccess = false, Result = tttt });
                        }
                        string html = "<table width='100%' border='1'><tr><td width='100px' align='right'><b>选择</b></td><td width='120px'><b>SKU</b></td><td  width='120px'><b>Qty</b></td><td  width='120px'><b>库存</b></td><td><b>Desc</b></td></tr>";

                        foreach (OrderProductType item in NSession.CreateQuery(" from OrderProductType where OId=" + order.Id).List<OrderProductType>())
                        {

                            object obj = NSession.CreateQuery("select count(Id) from SKUCodeType where SKU='" + item.SKU + "' and IsOut=0").UniqueResult();
                            if (obj == null)
                                obj = 0;
                            html += string.Format("<tr><td align='right'><input type='checkbox'  name='ck_{0}' code='{0}' checked=checked /></td><td>{1}</td><td>{2}</td><td>{4}</td><td>{3}</td></tr>", item.Id, item.SKU, item.Qty, item.Standard, obj);
                        }
                        html += "</table>";
                        return Json(new { IsSuccess = true, Result = html });
                    }
                    return Json(new { IsSuccess = false, Result = "已经是缺货订单！" });
                }
                return Json(new { IsSuccess = false, Result = "订单状态不符！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        public JsonResult OutStockByQue(string o, string ids)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待拣货.ToString() || (order.Status == OrderStatusEnum.已处理.ToString()))
                {
                    LoggerUtil.GetOrderRecord(order, "缺货扫描", CurrentUser.Realname + "将订单添加到 添加到缺货订单中！", CurrentUser, NSession);
                    order.IsOutOfStock = 1;
                    NSession.Update(order);
                    NSession.Flush();
                    foreach (OrderProductType item in NSession.CreateQuery(" from OrderProductType where OId=" + order.Id).List<OrderProductType>())
                    {
                        if (ids.IndexOf(item.Id.ToString()) != -1)
                        {
                            item.IsQue = 1;
                            NSession.Update(item);
                            NSession.Flush();
                        }
                    }
                    string html = "订单：" + order.OrderNo + " 添加到缺货！";
                    return Json(new { IsSuccess = true, Result = html });
                }
                return Json(new { IsSuccess = false, Result = "订单状态不符！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }



        public JsonResult GetOrderByPei(string orderNo)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + orderNo + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待拣货.ToString() || (order.Status == OrderStatusEnum.已处理.ToString()))
                {
                    if (order.IsError == 1 || !string.IsNullOrEmpty(order.CutOffMemo))
                    {
                        string tttt = "订单:" + order.OrderNo + ", 无法扫描，请拦截此包裹，原因：" + order.CutOffMemo;
                        return Json(new { IsSuccess = false, Result = tttt });
                    }
                    if (order.IsAudit == 0)
                    {
                        string tttt = "订单:" + order.OrderNo + ", 需要审核";
                        return Json(new { IsSuccess = false, Result = tttt });
                    }

                    string html = @"  <table width='100%' class='dataTable'>
                                                        <tr class='dataTableHead'>
                                                            <th width='300px' >图片</th><td width='200px'>SKU*数量</td><td>规格</td><td>扫描次数</td>
                                                        </tr>";
                    string html2 = @"<tr style='font-weight:bold; font-size:30px;' name='tr_{0}' code='{3}' qty='{1}' cqty='{4}'><td><img width='220px' src='/imgs/pic/{0}/1.jpg' /></td><td>{0}*{1}</td><td>{2}</td><td><span><span id='r_{3}' style='color:red'>{4}</span>/<span style='color:green'>{1}</span></td></tr>";
                    order.Products =
                        NSession.CreateQuery("from OrderProductType where OId=" + order.Id).List<OrderProductType>().ToList();
                    foreach (var p in order.Products)
                    {
                        IList<ProductType> products = NSession.CreateQuery("from ProductType where SKU=:p").SetString("p", p.SKU).SetMaxResults(1).List<ProductType>();
                        if (products.Count > 0)
                        {
                            if (products[0].IsScan == 1)
                            {
                                html += string.Format(html2, p.SKU.Trim().ToUpper(), p.Qty, p.Standard, p.Id, 0);
                            }
                            else
                            {
                                html += string.Format(html2, p.SKU.Trim().ToUpper(), p.Qty, p.Standard, p.Id, p.Qty);
                            }
                        }
                    }
                    html += "</table>";
                    return Json(new { IsSuccess = true, Result = html });
                }
                return Json(new { IsSuccess = false, Result = "订单状态不符！此订单的状态是：" + order.Status });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }

        public JsonResult OutStockByPei(string p1, string p2, string o, string skuCode)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待拣货.ToString() || (order.Status == OrderStatusEnum.已处理.ToString()))
                {
                    bool iscon = false;
                    OrderPeiRecordType orderPeiRecord = new OrderPeiRecordType { OrderNo = order.OrderNo, PeiBy = p1, ValiBy = p2, CreateOn = DateTime.Now, OId = order.Id, ScanBy = CurrentUser.Realname };
                    NSession.Save(orderPeiRecord);
                    NSession.Flush();
                    order.Status = OrderStatusEnum.待包装.ToString();
                    if (order.IsOutOfStock == 1)
                    {
                        iscon = true;
                    }
                    order.IsOutOfStock = 0;
                    NSession.Update(order);
                    NSession.Flush();
                    NSession.CreateQuery("update OrderProductType set IsQue=0 where OId =" + order.Id).ExecuteUpdate();
                    if (skuCode != "")
                        NSession.CreateQuery("update SKUCodeType set IsOut=1,PeiOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',OrderNo='" + order.OrderNo + "' where Code in ('" + skuCode.Replace(",", "','") + "')").ExecuteUpdate();
                    string html = "订单：" + order.OrderNo + " 配货完成！";
                    if (iscon)
                    {
                        LoggerUtil.GetOrderRecord(order, "订单配货扫描！", "将订单配货扫描，订单的缺货状态删除！", CurrentUser, NSession);
                    }
                    else
                    {
                        LoggerUtil.GetOrderRecord(order, "订单配货扫描！", "将订单配货扫描！", CurrentUser, NSession);
                    }

                    return Json(new { IsSuccess = true, Result = html });
                }
                return Json(new { IsSuccess = false, Result = "订单状态不符！现在的订单状态为：" + order.Status + " 将订单状态设置为“已处理”才能配货扫描！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });
        }



        public JsonResult OutStockByJi(string p, string o)
        {
            List<OrderType> orders = NSession.CreateQuery("from OrderType where OrderNo='" + o + "'").List<OrderType>().ToList();
            if (orders.Count > 0)
            {
                OrderType order = orders[0];
                if (order.Status == OrderStatusEnum.待包装.ToString())
                {
                    LoggerUtil.GetOrderRecord(order, "订单计件扫描！", "将订单 包装疾计件！", CurrentUser, NSession);
                    order.Status = OrderStatusEnum.待发货.ToString();
                    NSession.Update(order);
                    NSession.Flush();
                    IList<OrderProductType> orderproduct = NSession.CreateQuery("from OrderProductType where OId='" + order.Id+ "'").List<OrderProductType>();
                    int PackCoefficient=0;
                    string sku = "";
                    foreach(var item in orderproduct)
                    {
                        IList<ProductType> product = NSession.CreateQuery("from ProductType where SKU='"+item.SKU+"'").List<ProductType>();
                        if (product[0].PackCoefficient > PackCoefficient)
                        {
                            PackCoefficient = product[0].PackCoefficient;
                            sku = product[0].SKU;
                        }
                    }
                    OrderPackRecordType orderPackRecord = new OrderPackRecordType
                    {
                        OId = order.Id,
                        OrderNo = order.OrderNo,
                        PackBy = p,
                        PackOn = DateTime.Now,
                        ScanBy = CurrentUser.Realname,
                        PackCoefficient = PackCoefficient,
                        SKU=sku
                    };
                    NSession.Save(orderPackRecord);
                    NSession.Flush();
                    string html = "订单： " + order.OrderNo + "计件成功！包装人：" + p;
                    return Json(new { IsSuccess = true, Result = html });
                }
                return Json(new { IsSuccess = false, Result = " 无法出库！ 当前状态为：" + order.Status + "，需要订单状态为“待发货”方可扫描！" });
            }
            return Json(new { IsSuccess = false, Result = "找不到该订单" });

        }

        public JsonResult UnHandleList(int page, int rows, string sort, string order, string search)
        {
            return List(page, rows, sort, order, search, 1);
        }

        public JsonResult SplitNoSendList(string sort, string order, string search)
        {
            string where = " where Status<>'已发货' and MId<>0 ";
            string orderby = " order by Id desc";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }
            if (!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where Status<>'已发货' and MId<>0 and " + where;
                }
            }
            IList<OrderType> objList = NSession.CreateQuery("from OrderType " + where + orderby).List<OrderType>();

            for (int i = 0; i < objList.Count;i++ )
            {
                OrderType obj = GetById(objList[i].MId);
                if (obj.IsSplit != 1)
                    objList.Remove(objList[i]);
            }
            return Json(new { total = objList.Count, rows = objList });
        }

        public JsonResult List(int page, int rows, string sort, string order, string search, int isUn = 0)
        {
            string where = "";
            string orderby = " order by Id desc";
            string flag = "<>";
            if (isUn == 1)
                flag = "=";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }

            if (!string.IsNullOrEmpty(search))
            {
                where = Utilities.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where Enabled=1 and  Status" + flag + "'待处理' and " + where;
                }
            }
            if (where.Length == 0)
            {
                where = " where Enabled=1 and  Status" + flag + "'待处理'";
            }
            IList<OrderType> objList = NSession.CreateQuery("from OrderType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<OrderType>();

            object count = NSession.CreateQuery("select count(Id) from OrderType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ListQ(string q)
        {
            IList<OrderType> objList = NSession.CreateQuery("from OrderType where OrderNo like '%" + q + "%'")
                .SetFirstResult(0)
                .SetMaxResults(10)
                .List<OrderType>();
            return Json(new { total = objList.Count, rows = objList });
        }

        public JsonResult Record(int id)
        {
            IList<OrderRecordType> obj = NSession.CreateQuery("from OrderRecordType where Oid='" + id + "'").List<OrderRecordType>();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNotQueList()
        {
            IList<object[]> objs = NSession.CreateSQLQuery(string.Format(@"select * from (
select SKU,SUM(Qty) as Qty,(select isnull(SUM(Qty),0) from WarehouseStock where SKU=OP.SKU ) as NowQty,(select count(Id) from SKUCode where SKU=OP.SKU and IsOut=0) as unPeiQty,COUNT(O.Id) as'OrderQty' from Orders O left join OrderProducts OP On O.Id=OP.OId where O.IsOutOfStock=1 and OP.IsQue=1 and O.Status<>'作废订单' group by SKU
) as tbl  where NowQty>0")).List<object[]>();
            List<QueCount> list = new List<QueCount>();
            foreach (object[] objectse in objs)
            {
                QueCount oc = new QueCount { SKU = objectse[0].ToString(), Qty = Utilities.ToInt(objectse[1]), NowQty = Utilities.ToInt(objectse[2]), UnPeiQty = Utilities.ToInt(objectse[3]), OrderQty = Utilities.ToInt(objectse[4]) };

                list.Add(oc);
            }
            return Json(new { total = list.Count, rows = list });
        }

        public JsonResult Connect(int id)
        {
            List<OrderType> objlist = new List<OrderType>();
            OrderType obj = GetById(id);
            if (obj.MId != 0)
            {
                objlist = MidGetOrder(obj.MId);
            }
            return Json(objlist);
        }

        List<OrderType> arry = new List<OrderType>();
        public List<OrderType> MidGetOrder(int mid)
        {
            OrderType obj = GetById(mid);
            arry.Add(obj);
            if (obj.MId != 0)
            {
                MidGetOrder(obj.MId);
            }
            return arry;
        }

    }
}

