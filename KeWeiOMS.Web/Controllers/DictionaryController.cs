﻿using System;
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
    public class DictionaryController : BaseController
    {
        public ViewResult Index(string code)
        {
            ViewData["code"] = code;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(DictionaryType obj)
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
            return Json(new { IsSuccess = true  });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DictionaryType GetById(int Id)
        {
            DictionaryType obj = NSession.Get<DictionaryType>(Id);
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
            DictionaryType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Save(DictionaryType obj)
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

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                DictionaryType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        public JsonResult List(string code)
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where  DicCode=:t").SetString("t", code)
                .List<DictionaryType>();
            return Json(new { total = objList.Count, rows = objList });
        }

        public JsonResult PruchasePlanList()
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
                .SetString("DicCode","Stock.Plan")
                .List<DictionaryType>();

            return Json(objList,JsonRequestBehavior.AllowGet);
        }
        public void DelDictionary(int id)
        {
            DictionaryType log = GetById(id);
            NSession.Delete(log);
            NSession.Flush();
        }
        public JsonResult GetDisputeClass()
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
                .SetString("DicCode", "Dispute.Class")
                .List<DictionaryType>();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDisputeSolution()
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
                .SetString("DicCode", "Dispute.Solution")
                .List<DictionaryType>();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPlacard()
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
                .SetString("DicCode", "Placard.Tag")
                .List<DictionaryType>();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMachineStatus()
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
                .SetString("DicCode", "Machine.Status")
                .List<DictionaryType>();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMachineClass()
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
                .SetString("DicCode", "Machine.Class")
                .List<DictionaryType>();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIntType()
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
                .SetString("DicCode", "Stock.IntType")
                .List<DictionaryType>();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }
       public JsonResult GetOutType()
        {
            IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
                .SetString("DicCode", "Stock.OutType")
                .List<DictionaryType>();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }
       public JsonResult PruchaseTrouble()
       {
           IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
               .SetString("DicCode", "Purchase.Trouble")
               .List<DictionaryType>();

           return Json(objList, JsonRequestBehavior.AllowGet);
       }
       public JsonResult PruchaseTroubleDeal()
       {
           IList<DictionaryType> objList = NSession.CreateQuery("from DictionaryType where DicCode=:DicCode")
               .SetString("DicCode", "Purchase.TroubleDeal")
               .List<DictionaryType>();

           return Json(objList, JsonRequestBehavior.AllowGet);
       }
    }
}

