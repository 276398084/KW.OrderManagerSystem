using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace KeWeiOMS.NhibernateHelper
{
    public interface ISessionStorage
    {
        ISession Get();
        void Set(ISession value);
    }
}
