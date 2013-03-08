using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class OrganizeController : BaseController
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
        public JsonResult Create(OrganizeType obj)
        {
            try
            {
                obj.CreateOn = DateTime.Now;
                obj.CreateBy = CurrentUser.Realname;
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OrganizeType GetById(int Id)
        {
            OrganizeType obj = NSession.Get<OrganizeType>(Id);
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
            OrganizeType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(OrganizeType obj)
        {

            try
            {
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });

        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                OrganizeType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        public JsonResult List()
        {
            IList<OrganizeType> objList = NSession.CreateQuery("from OrganizeType").List<OrganizeType>();
            IList<OrganizeType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            foreach (OrganizeType item in fristList)
            {
                List<OrganizeType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                GetChildren(objList, item);

            }
            return Json(new { total = fristList.Count, rows = fristList });
        }

        public JsonResult RootList()
        {
            IList<OrganizeType> objList = NSession.CreateQuery("from OrganizeType where ParentId=0").List<OrganizeType>();

            return Json(objList);
        }

        public JsonResult BuMenList(int id)
        {
            IList<OrganizeType> objList = NSession.CreateQuery("from OrganizeType where ParentId=" + id).List<OrganizeType>();
            IList<OrganizeType> list = NSession.CreateQuery("from OrganizeType").List<OrganizeType>();
            List<SystemTree> tree = new List<SystemTree>();

            foreach (OrganizeType item in objList)
            {
                List<OrganizeType> fooList = list.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList);
                tree.Add(new SystemTree { id = item.Id.ToString(), text = item.ShortName, children = tree2 });
                GetChildren(list, item, tree2);


            }
            return Json(tree);
        }

        public JsonResult ParentList()
        {
            var root = new SystemTree { id = "0", text = "root" };
            IList<OrganizeType> objList = NSession.CreateQuery("from OrganizeType").List<OrganizeType>();
            IList<OrganizeType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            List<SystemTree> tree = new List<SystemTree>();
            tree.Add(root);
            foreach (OrganizeType item in fristList)
            {
                List<OrganizeType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList);

                GetChildren(objList, item, tree2);
                root.children.Add(new SystemTree { id = item.Id.ToString(), text = item.ShortName, children = tree2 });

            }
            return Json(tree);
        }

        public List<SystemTree> ConvertToTree(List<OrganizeType> fooList)
        {
            List<SystemTree> tree = new List<SystemTree>();
            foreach (OrganizeType item in fooList)
            {
                tree.Add(new SystemTree { id = item.Id.ToString(), text = item.ShortName });
            }
            return tree;

        }

        private void GetChildren(IList<OrganizeType> objList, OrganizeType item)
        {
            foreach (OrganizeType k in item.children)
            {
                List<OrganizeType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                GetChildren(objList, k);
            }
        }

        private void GetChildren(IList<OrganizeType> objList, OrganizeType item, List<SystemTree> trees)
        {
            foreach (OrganizeType k in item.children)
            {
                SystemTree tree = trees.Find(p => p.id == k.Id.ToString());
                List<OrganizeType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                List<SystemTree> mlist = ConvertToTree(kList);
                tree.children = mlist;
                GetChildren(objList, k, mlist);
            }
        }



    }
}

