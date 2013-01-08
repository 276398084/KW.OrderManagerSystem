using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using KeWeiOMS.Domain;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Web.UI;

namespace KeWeiOMS.Web.Controllers
{
    public class HomeController : Controller
    {
        protected ISession Session = NHibernateHelper.CreateSession();

        //lim
        // GET: /User/

        public ViewResult Index()
        {
            return View();
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public JsonResult Create(ModulesType user)
        {
            try
            {
                Session.SaveOrUpdate(user);
                Session.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { erroeMsg = "出错了" });
            }
            return Json(new { IsSuccess = "OK" });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ModulesType GetById(string Id)
        {
            ModulesType customer = Session.Get<ModulesType>(Id);
            if (customer == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return customer;
            }
        }

        //
        // GET: /User/Edit/5
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(decimal id)
        {
            ModulesType user = GetById(id.ToString());
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ModulesType user)
        {
            JsonResult json = new JsonResult();
            try
            {
                Session.Update(user);
                Session.Flush();
            }
            catch (Exception ee)
            {
                json.Data = ee.Message;
                return json;
            }
            json.Data = true;
            return json;
        }


        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(decimal id)
        {
            JsonResult json = new JsonResult();
            json.Data = true;
            try
            {
                ModulesType customer = GetById(id.ToString());
                Session.Delete(customer);
                Session.Flush();
            }
            catch (Exception ee)
            {
                json.Data = ee.Message;
            }

            return json;
        }

        public JsonResult List(int page, int rows)
        {


            ////HQL查询
            IList<ModulesType> customerList = Session.CreateQuery("from Customer")
                .SetFirstResult(rows * page)
                .SetMaxResults(page)
                .List<ModulesType>();

            return Json(new { total = customerList.Count, rows = customerList });
            ////条件查询
            //return Session.CreateCriteria(typeof(Customer))
            //    .SetProjection(Projections.ProjectionList()
            //    .Add(Projections.Property("Id"), "Id")
            //    .Add(Projections.Property("Name"), "Name")
            //    .Add(Projections.Property("Tel"), "Tel")
            //    .Add(Projections.Property("Address"), "Address")
            //    .Add(Projections.Property("Sex"), "Sex")
            //    .Add(Projections.Property("CreateDate"), "CreateDate")
            //    .Add(Projections.Property("Version"),"Version"))
            //    .AddOrder(Order.Asc("Id"))
            //    .SetFirstResult(pageStart * pageLimit)
            //    .SetResultTransformer(Transformers.AliasToBean(typeof(Customer)))
            //    .List<Customer>();

            ///SQL查询
            //IList<Customer> customerList = Session.CreateSQLQuery("SELECT * FROM Customer")
            //    .SetFirstResult(pageStart*pageLimit)
            //    .SetMaxResults(pageLimit)
            //    .SetResultTransformer(Transformers.AliasToBean<Customer>()).List<Customer>(); 
            //return customerList;




        }



    }

    //public class CustomerService
    //{


    //    /// <summary>
    //    /// 创建
    //    /// </summary>
    //    public void CreateCustomer(Customer customer)
    //    {
    //        Session.Save(customer);
    //        Session.Flush();
    //    }

    //    /// <summary>
    //    /// 删除
    //    /// </summary>
    //    public void DeleteCustomer(string Id)
    //    {
    //        Customer customer = GetById(Id);
    //        Session.Delete(customer);
    //        Session.Flush();
    //    }

    //    /// <summary>
    //    /// 修改
    //    /// </summary>
    //    /// <param name="customer"></param>
    //    public void UpdateCustomer(Customer customer)
    //    {
    //        Session.Update(customer);
    //        Session.Flush();
    //    }

    //    /// <summary>
    //    /// 根据Id获取
    //    /// </summary>
    //    /// <param name="Id"></param>
    //    /// <returns></returns>
    //    public Customer GetById(string Id)
    //    {
    //        Customer customer = Session.Get<Customer>(Id);
    //        if (customer == null)
    //        {
    //            throw new Exception("返回实体为空");
    //        }
    //        else
    //        {
    //            return customer;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取分页
    //    /// </summary>
    //    /// <param name="pageStart"></param>
    //    /// <param name="pageLimit"></param>
    //    /// <returns></returns>
    //    public IList<Customer> GetCustomerPageModel(int pageStart, int pageLimit)
    //    {
    //        ////HQL查询
    //        IList<Customer> customerList = Session.CreateQuery("from Customer")
    //            .SetFirstResult(pageStart * pageLimit)
    //            .SetMaxResults(pageLimit)
    //            .List<Customer>();
    //        return customerList;

    //        ////条件查询
    //        //return Session.CreateCriteria(typeof(Customer))
    //        //    .SetProjection(Projections.ProjectionList()
    //        //    .Add(Projections.Property("Id"), "Id")
    //        //    .Add(Projections.Property("Name"), "Name")
    //        //    .Add(Projections.Property("Tel"), "Tel")
    //        //    .Add(Projections.Property("Address"), "Address")
    //        //    .Add(Projections.Property("Sex"), "Sex")
    //        //    .Add(Projections.Property("CreateDate"), "CreateDate")
    //        //    .Add(Projections.Property("Version"),"Version"))
    //        //    .AddOrder(Order.Asc("Id"))
    //        //    .SetFirstResult(pageStart * pageLimit)
    //        //    .SetMaxResults(pageLimit)
    //        //    .SetResultTransformer(Transformers.AliasToBean(typeof(Customer)))
    //        //    .List<Customer>();

    //        ///SQL查询
    //        //IList<Customer> customerList = Session.CreateSQLQuery("SELECT * FROM Customer")
    //        //    .SetFirstResult(pageStart*pageLimit)
    //        //    .SetMaxResults(pageLimit)
    //        //    .SetResultTransformer(Transformers.AliasToBean<Customer>()).List<Customer>(); 
    //        //return customerList;
    //    }

    //    /// <summary>
    //    /// 获取所以
    //    /// </summary>
    //    /// <returns></returns>
    //    public IList<Customer> GetAll()
    //    {
    //        return Session.CreateCriteria(typeof(Customer))
    //            .AddOrder(Order.Asc("CreateDate")).List<Customer>();
    //    }
    //}
}
