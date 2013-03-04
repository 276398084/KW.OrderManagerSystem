using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Web;

namespace KeWeiOMS.NhibernateHelper
{
    public class HttpSessionStorage : ISessionStorage
    {
        #region ISessionStorage 成员

        public ISession Get()
        {
            return (ISession)HttpContext.Current.Items["NhbSession"];
        }

        public void Set(ISession value)
        {
            if (value != null)
            {
                HttpContext.Current.Items.Add("NhbSession", value);
            }
        }

        #endregion

    }
}
