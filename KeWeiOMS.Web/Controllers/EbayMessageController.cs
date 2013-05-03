using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using System.Data.SqlClient;
using System.Data;
using NHibernate;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;
using System.Web;
using KeWeiOMS.NhibernateHelper;


namespace KeWeiOMS.Web.Controllers
{
    public class EbayMessageController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(EbayMessageType obj)
        {
            try
            {
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
            }
            return Json(new { IsSuccess = "true" });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EbayMessageType GetById(int Id)
        {
            EbayMessageType obj = NSession.Get<EbayMessageType>(Id);
            if (obj == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return obj;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            EbayMessageType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(EbayMessageType obj)
        {

            try
            {
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
            }
            return Json(new { IsSuccess = "true" });

        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                EbayMessageType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string type = "";
            string where = "";
            string orderby = " order by Id desc ";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }

            if (!string.IsNullOrEmpty(search))
            {
                string key = search.Substring(0, search.IndexOf("$"));
                type = search.Substring(search.LastIndexOf("$") + 1);
                where = Utilities.Resolve(key);
                if (where.Length > 0)
                {
                    where = "where (( " + where;
                }

            }
            string account = FindAccount();
            if (account != "")
            {
                if (!string.IsNullOrEmpty(where))
                {
                    where += " and " + account;
                }
                else
                {
                    where = " where ((" + account;
                }
            }
            if (!string.IsNullOrEmpty(where))
            {
                where += " and ReplayOnlyBy is null) ";
            }
            else
            {
                where = " where (ReplayOnlyBy is null ";
            }
            where += " or ReplayOnlyBy ='" + CurrentUser.Realname + "')";
            if (!string.IsNullOrEmpty(type))
            {
                string pid = type.Substring(0, type.IndexOf("~"));
                string ctext = type.Substring(type.IndexOf("~") + 1, type.LastIndexOf("~") - type.IndexOf("~")-1);
                string cid = type.Substring(type.LastIndexOf("~") + 1);
                if (pid == "待处理消息")
                {
                    where += " and MessageStatus ='未回复'";
                }
                if (pid == "已处理消息")
                {
                    where += " and MessageStatus ='已回复'";
                }
                if (pid == "所有消息")
                {   
                }
                if (pid == "未分配消息")
                {
                    where = GetUnAssign();
                }
                where += " and Subject like '%"+cid+"%'";
            }
            IList<EbayMessageType> objList = NSession.CreateQuery("from EbayMessageType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<EbayMessageType>();

            object count = NSession.CreateQuery("select count(Id) from EbayMessageType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        private string GetUnAssign()
        {
            int i = 0;
            string where = " where ReplayOnlyBy is null ";
            string name = CurrentUser.Realname;
            IList<EbayReplayType> ac = NSession.CreateQuery("from EbayReplayType").List<EbayReplayType>();
            foreach (var item in ac)
            {
                if (i ==0)
                {
                    where += "and Shop<>'" + item.ReplayAccount + "' ";
                    i++;
                }
                else
                {
                    where += "and Shop<>'" + item.ReplayAccount + "' ";
                }
                
            }
            return where;
        }
        public JsonResult ToExcel(string search)
        {
            try
            {
                List<EbayMessageType> objList = NSession.CreateQuery("from EbayMessageType " + Utilities.SqlWhere(search))
                    .List<EbayMessageType>().ToList();
                Session["ExportDown"] = ExcelHelper.GetExcelXml(Utilities.FillDataTable((objList)));
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true, ErrorMsg = "导出成功" });
        }

        public string init(string key, string all)
        {
            string qty = "";
            Regex regex = new Regex(key, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
            if (regex.IsMatch(all))
            {
                qty = "0";
            }
            return qty;
        }

        private string FindAccount()
        {
            string where = "";
            string name = CurrentUser.Realname;
            IList<EbayReplayType> ac = NSession.CreateQuery("from EbayReplayType where ReplayBy='" + name + "'").List<EbayReplayType>();
            foreach (var item in ac)
            {
                if (where == "")
                {
                    where += " Shop='" + item.ReplayAccount + "' ";
                }
                else
                {
                    where += " or Shop='" + item.ReplayAccount + "' ";
                }
            }
            return where;
        }

        public JsonResult IsRead(int id)
        {
            EbayMessageType obj = GetById(id);
            if (obj.MessageStatus != "未回复")
            {
                return Json(new { Msg = 1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg = 0 }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Syn()
        {
            try
            {
                EbayMessageUtil.syn(NSession);
            }
            catch (Exception ee)
            {
                return Json(new { ErrorMsg = "出错了", IsSuccess = false });
            }
            return Json(new { Msg = "同步成功" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Forward(string id)
        {
            try
            {
                int mid = int.Parse(id.Substring(0, id.IndexOf("$")).ToString());
                string name = id.Substring(id.IndexOf("$") + 1, id.IndexOf("~") - id.IndexOf("$") - 1);
                string remark = id.Substring(id.IndexOf("~") + 1);
                EbayMessageType obj = GetById(mid);
                obj.ReplayOnlyBy = name;
                obj.ForwardWhy = remark;
                NSession.Update(obj);
                NSession.Flush();
                return Json(new { Msg = 0 }, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception e)
            {

            }
            return Json(new { Msg = 1 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTree()
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType").List<DictionaryType>();
            IList<DictionaryType> fristList = objList.Where(p => p.DicCode == "ebay.Type").ToList();
            List<SystemTree> trees = new List<SystemTree>();
            SystemTree tree = new SystemTree { id = "0", text = "分类信息" };
            foreach (DictionaryType item in fristList)
            {
                List<DictionaryType> fooList = objList.Where(p => p.DicCode == "Message.Type").ToList();
                List<SystemTree> tree2 = ConvertToTree(fooList);
                tree.children.Add(new SystemTree { id = item.DicValue, text = item.FullName, children = tree2 });
            }
            trees.Add(tree);
            return Json(trees);
        }

        public List<SystemTree> ConvertToTree(List<DictionaryType> fooList)
        {
            List<SystemTree> tree = new List<SystemTree>();
            foreach (DictionaryType item in fooList)
            {
                tree.Add(new SystemTree { id = item.DicValue, text = item.FullName});
            }
            return tree;

        }
    }
}

