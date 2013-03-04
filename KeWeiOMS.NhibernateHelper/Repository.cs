using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace KeWeiOMS.NhibernateHelper
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected virtual ISession Session
        {
            get { return SessionBuilder.CreateSession(); }
        }

        #region IRepository<T> 成员
        public virtual T Load(string id)
        {
            try
            {
                T reslut = Session.Load<T>(id);
                if (reslut == null)
                    throw new Exception("返回实体为空");
                else
                    return reslut;
            }
            catch (Exception ex)
            {
                throw new Exception("获取实体失败", ex);
            }
        }

        public virtual T Get(string id)
        {
            try
            {
                T reslut = Session.Get<T>(id);
                if (reslut == null)
                    throw new Exception("返回实体为空");
                else
                    return reslut;
            }
            catch (Exception ex)
            {
                throw new Exception("获取实体失败", ex);
            }
        }

        public virtual IList<T> GetAll()
        {
            return Session.CreateCriteria<T>()
                .AddOrder(Order.Desc("Id"))
                .List<T>();
        }

        public virtual void SaveOrUpdate(T entity)
        {
            try
            {
                Session.SaveOrUpdate(entity);
                Session.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception("插入实体失败", ex);
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                Session.Update(entity);
                Session.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception("更新实体失败", ex);
            }
        }

        public virtual void PhysicsDelete(string id)
        {
            try
            {
                var entity = Get(id);
                Session.Delete(entity);
                Session.Flush();
            }
            catch (System.Exception ex)
            {
                throw new Exception("物理删除实体失败", ex);
            }
        }

        public virtual void Delete(string id)
        {
            try
            {
                var entity = Get(id);
                //entity.IsDelete = true;
                Update(entity);
            }
            catch (System.Exception ex)
            {
                throw new Exception("删除实体失败", ex);
            }
        }

        #endregion


    }
}
