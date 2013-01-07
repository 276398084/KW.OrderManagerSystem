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

namespace KeWeiOMS.Web.Controllers
{
    public class HomeController : Controller
    {
       

        public ActionResult Index()
        {


            var session = NHibernateHelper.CreateSession();
             
                 var ss = new ModulesType { Target = "1", CreateOn = DateTime.Now, ModifiedOn = DateTime.Now };
                 session.Save(ss);
             
            return View();
        }

    }

    public class CustomerService 
    {
        protected ISession Session = NHibernateHelper.CreateSession();

        /// <summary>
        /// 创建
        /// </summary>
        public void CreateCustomer(Customer customer)
        {
            Session.Save(customer);
            Session.Flush();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteCustomer(string Id)
        {
            Customer customer = GetById(Id);
            Session.Delete(customer);
            Session.Flush();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            Session.Update(customer);
            Session.Flush();
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Customer GetById(string Id)
        {
            Customer customer = Session.Get<Customer>(Id);
            if (customer == null)
            {
                throw new Exception("返回实体为空");
            }
            else
            {
                return customer;
            }
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageStart"></param>
        /// <param name="pageLimit"></param>
        /// <returns></returns>
        public IList<Customer> GetCustomerPageModel(int pageStart, int pageLimit)
        {
            ////HQL查询
            IList<Customer> customerList = Session.CreateQuery("from Customer")
                .SetFirstResult(pageStart * pageLimit)
                .SetMaxResults(pageLimit)
                .List<Customer>();
            return customerList;

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
            //    .SetMaxResults(pageLimit)
            //    .SetResultTransformer(Transformers.AliasToBean(typeof(Customer)))
            //    .List<Customer>();

            ///SQL查询
            //IList<Customer> customerList = Session.CreateSQLQuery("SELECT * FROM Customer")
            //    .SetFirstResult(pageStart*pageLimit)
            //    .SetMaxResults(pageLimit)
            //    .SetResultTransformer(Transformers.AliasToBean<Customer>()).List<Customer>(); 
            //return customerList;
        }

        /// <summary>
        /// 获取所以
        /// </summary>
        /// <returns></returns>
        public IList<Customer> GetAll()
        {
            return Session.CreateCriteria(typeof(Customer))
                .AddOrder(Order.Asc("CreateDate")).List<Customer>();
        }
    }
}
