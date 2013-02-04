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
    public class PermissionItemController : BaseController
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
        public JsonResult Create(PermissionItemType obj)
        {
            try
            {
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PermissionItemType GetById(int Id)
        {
            PermissionItemType obj = NSession.Get<PermissionItemType>(Id);
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
            PermissionItemType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(PermissionItemType obj)
        {

            try
            {
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });

        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                PermissionItemType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List()
        {
            IList<PermissionItemType> objList = NSession.CreateQuery("from PermissionItemType").List<PermissionItemType>();

            IList<PermissionItemType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            foreach (PermissionItemType item in fristList)
            {
                List<PermissionItemType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                GetChildren(objList, item);

            }

            return Json(new { total = fristList.Count, rows = fristList });
        }

        public JsonResult ParentList()
        {
            IList<PermissionItemType> objList = NSession.CreateQuery("from PermissionItemType").List<PermissionItemType>();
            IList<PermissionItemType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            List<SystemTree> tree = new List<SystemTree>();
            foreach (PermissionItemType item in fristList)
            {
                List<PermissionItemType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList);
                tree.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, children = tree2 });
                GetChildren(objList, item, tree2);

            }
            tree.Insert(0, new SystemTree { id = "0", text = "root" });
            return Json(tree);
        }

        public JsonResult SelectList(string type)
        {
            int id = GetCurrentAccount().Id;
            if (type == ResourceCategoryEnum.Role.ToString())
                id = GetCurrentAccount().RoleId;
            IList<PermissionItemType> objList = NSession.CreateQuery("from PermissionItemType").List<PermissionItemType>();
            //获得这个类型的菜单权限
            List<PermissionScopeType> scopeList = NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2 and TargetCategory =:p3").SetString("p1", type).SetInt32("p2", Convert.ToInt32(id)).SetString("p3", TargetCategoryEnum.Module.ToString()).List<PermissionScopeType>().ToList<PermissionScopeType>();

            IList<PermissionItemType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            List<SystemTree> tree = new List<SystemTree>(); ;
            SystemTree root = new SystemTree { id = "0", text = "所有权限" };
            foreach (PermissionItemType item in fristList)
            {
                List<PermissionItemType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList, scopeList);


                if (scopeList.FindIndex(p => p.TargetId == item.Id) >= 0)
                {
                    root.children.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, children = tree2, @checked = true });
                }
                else
                {
                    root.children.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, children = tree2 });
                }
                GetChildren(objList, item, tree2);
            }
            tree.Add(root);
            return Json(tree);

        }

        public List<SystemTree> ConvertToTree(List<PermissionItemType> fooList, List<PermissionScopeType> scopeList = null)
        {
            List<SystemTree> tree = new List<SystemTree>();
            foreach (PermissionItemType item in fooList)
            {
                if (scopeList == null)
                    tree.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName });
                else
                {
                    if (scopeList.FindIndex(p => p.TargetId == item.Id) >= 0)
                    {
                        tree.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, @checked = true });
                    }
                    else
                    {
                        tree.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName });
                    }
                }
            }
            return tree;

        }

        private void GetChildren(IList<PermissionItemType> objList, PermissionItemType item)
        {
            foreach (PermissionItemType k in item.children)
            {
                List<PermissionItemType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                GetChildren(objList, k);
            }
        }

        private void GetChildren(IList<PermissionItemType> objList, PermissionItemType item, List<SystemTree> trees)
        {
            foreach (PermissionItemType k in item.children)
            {
                SystemTree tree = trees.Find(p => p.id == k.Id.ToString());
                List<PermissionItemType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                List<SystemTree> mlist = ConvertToTree(kList);
                tree.children = mlist;
                GetChildren(objList, k, mlist);
            }
        }

    }
}

