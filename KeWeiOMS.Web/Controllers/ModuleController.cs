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
    public class ModuleController : BaseController
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
        public JsonResult Create(ModuleType obj)
        {
            try
            {
                obj.CreateBy = CurrentUser.Realname;
                obj.CreateOn = DateTime.Now;
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
        public ModuleType GetById(int Id)
        {
            ModuleType obj = NSession.Get<ModuleType>(Id);
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
            ModuleType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ModuleType obj)
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
                ModuleType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        public JsonResult ParentList()
        {
            IList<ModuleType> objList = NSession.CreateQuery("from ModuleType").List<ModuleType>();
            IList<ModuleType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            SystemTree tree = new SystemTree { id = "0", text = "根目录" };
            List<SystemTree> trees = new List<SystemTree>();
            foreach (ModuleType item in fristList)
            {
                List<ModuleType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList);
                tree.children.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, children = tree2 });
                GetChildren(objList, item, tree2);

            }
            trees.Add(tree);
            return Json(trees);
        }

        public JsonResult ALLList()
        {
            IList<ModuleType> objList = NSession.CreateQuery("from ModuleType").List<ModuleType>();
            IList<ModuleType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();


            foreach (ModuleType item in fristList)
            {
                List<ModuleType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                GetChildren(objList, item);
            }

            return Json(new { total = fristList.Count, rows = fristList });


        }
        public JsonResult SelectListByRole(int id)
        {
            return SelectList(id, ResourceCategoryEnum.Role.ToString());
        }

        public JsonResult SelectListByUser(int id)
        {
            return SelectList(id, ResourceCategoryEnum.User.ToString());
        }

        private JsonResult SelectList(int id, string type)
        {
            IList<ModuleType> objList = NSession.CreateQuery("from ModuleType").List<ModuleType>();
            //获得这个类型的菜单权限
            List<PermissionScopeType> scopeList = NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2 and TargetCategory =:p3").SetString("p1", type).SetInt32("p2", id).SetString("p3", TargetCategoryEnum.Module.ToString()).List<PermissionScopeType>().ToList<PermissionScopeType>();

            IList<ModuleType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            List<SystemTree> tree = new List<SystemTree>();
            SystemTree root = new SystemTree { id = "0", text = "所有菜单" };
            foreach (ModuleType item in fristList)
            {
                List<ModuleType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList, scopeList);


                //if (scopeList.FindIndex(p => p.TargetId == item.Id) >= 0)
                //{
                //    root.children.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, children = tree2, @checked = true });
                //}
                //else
                //{
                    root.children.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, children = tree2 });
               // }
                GetChildren(objList, item, tree2);
            }

            tree.Add(root);
            return Json(tree);
        }

        public List<SystemTree> ConvertToTree(List<ModuleType> fooList, List<PermissionScopeType> scopeList = null)
        {
            List<SystemTree> tree = new List<SystemTree>();
            foreach (ModuleType item in fooList)
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

        private void GetChildren(IList<ModuleType> objList, ModuleType item)
        {
            foreach (ModuleType k in item.children)
            {
                List<ModuleType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                GetChildren(objList, k);
            }
        }

        private void GetChildren(IList<ModuleType> objList, ModuleType item, List<SystemTree> trees)
        {
            foreach (ModuleType k in item.children)
            {
                SystemTree tree = trees.Find(p => p.id == k.Id.ToString());
                List<ModuleType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                List<SystemTree> mlist = ConvertToTree(kList);
                tree.children = mlist;
                GetChildren(objList, k, mlist);
            }
        }

    }
}

