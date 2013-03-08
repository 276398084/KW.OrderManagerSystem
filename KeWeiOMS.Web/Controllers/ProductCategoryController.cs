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
    public class ProductCategoryController : BaseController
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
        public JsonResult Create(ProductCategoryType obj)
        {
            try
            {
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
        public ProductCategoryType GetById(int Id)
        {
            ProductCategoryType obj = NSession.Get<ProductCategoryType>(Id);
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
            ProductCategoryType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ProductCategoryType obj)
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
                ProductCategoryType obj = GetById(id);
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
            IList<ProductCategoryType> objList = NSession.CreateQuery("from ProductCategoryType").List<ProductCategoryType>();
            return Json(new { total = objList.Count, rows = objList });
        }


        public JsonResult ParentList()
        {
            IList<ProductCategoryType> objList = NSession.CreateQuery("from ProductCategoryType").List<ProductCategoryType>();
            IList<ProductCategoryType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            SystemTree tree = new SystemTree { id = "0", text = "根目录" };
            List<SystemTree> trees = new List<SystemTree>();
            foreach (ProductCategoryType item in fristList)
            {
                List<ProductCategoryType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList);
                tree.children.Add(new SystemTree { id = item.Id.ToString(), text = item.Name, children = tree2 });
                GetChildren(objList, item, tree2);

            }
            trees.Add(tree);
            return Json(trees);
        }

        public JsonResult ALLList()
        {
            IList<ProductCategoryType> objList = NSession.CreateQuery("from ProductCategoryType").List<ProductCategoryType>();
            IList<ProductCategoryType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            List<SystemTree> tree = new List<SystemTree>();
            foreach (ProductCategoryType item in fristList)
            {
                List<ProductCategoryType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList);
                tree.Add(new SystemTree { id = item.Id.ToString(), text = item.Name, children = tree2 });
                GetChildren(objList, item, tree2);

            }
            return Json(new { total = objList.Count, rows = fristList });
        }

        public JsonResult PList()
        {
            IList<ProductCategoryType> objList = NSession.CreateQuery("from ProductCategoryType").List<ProductCategoryType>();
            IList<ProductCategoryType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();

            List<SystemTree> trees = new List<SystemTree>();
            foreach (ProductCategoryType item in fristList)
            {
                List<ProductCategoryType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree2(fooList);
                trees.Add(new SystemTree { id = item.Name, text = item.Name, children = tree2 });
                GetChildren2(objList, item, tree2);
            }
            return Json(trees);
        }
        public List<SystemTree> ConvertToTree2(List<ProductCategoryType> fooList)
        {
            List<SystemTree> tree = new List<SystemTree>();
            foreach (ProductCategoryType item in fooList)
            {
                tree.Add(new SystemTree { id = item.Name, text = item.Name });
            }
            return tree;

        }
        private void GetChildren2(IList<ProductCategoryType> objList, ProductCategoryType item, List<SystemTree> trees)
        {
            foreach (ProductCategoryType k in item.children)
            {
                SystemTree tree = trees.Find(p => p.text == k.Name);
                List<ProductCategoryType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                List<SystemTree> mlist = ConvertToTree2(kList);
                tree.children = mlist;
                GetChildren2(objList, k, mlist);
            }
        }

        public List<SystemTree> ConvertToTree(List<ProductCategoryType> fooList)
        {
            List<SystemTree> tree = new List<SystemTree>();
            foreach (ProductCategoryType item in fooList)
            {
                tree.Add(new SystemTree { id = item.Id.ToString(), text = item.Name });
            }
            return tree;

        }

        private void GetChildren(IList<ProductCategoryType> objList, ProductCategoryType item)
        {
            foreach (ProductCategoryType k in item.children)
            {
                List<ProductCategoryType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                GetChildren(objList, k);
            }
        }

        private void GetChildren(IList<ProductCategoryType> objList, ProductCategoryType item, List<SystemTree> trees)
        {
            foreach (ProductCategoryType k in item.children)
            {
                SystemTree tree = trees.Find(p => p.id == k.Id.ToString());
                List<ProductCategoryType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                List<SystemTree> mlist = ConvertToTree(kList);
                tree.children = mlist;
                GetChildren(objList, k, mlist);
            }
        }

    }
}

