using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeWeiOMS.NhibernateHelper
{
    public interface IRepository<T> where T : Entity
    {
        T Load(string id);
        T Get(string id);
        IList<T> GetAll();
        void SaveOrUpdate(T entity);
        void Update(T entity);
        void Delete(string id);
        void PhysicsDelete(string id);
    }
}
