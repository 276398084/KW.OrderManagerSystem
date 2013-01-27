using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web
{
    public class Utilities
    {
        public const string OrderNo = "OrderNo";
        public static string GetOrderNo()
        {
            string result = string.Empty;
            ISession NSession = NHibernateHelper.CreateSession();
            IList<SerialNumberType> list = NSession.CreateQuery(" from SerialNumberType where Code=:p").SetString("p", OrderNo).List<SerialNumberType>();

            if (list.Count > 0)
            {
                list[0].BeginNo = list[0].BeginNo + 1;
                NSession.Update(list[0]);
                NSession.Flush();
                return list[0].BeginNo.ToString();

            }
            return "";

        }
    }

    public class ResultInfo
    {
        public virtual string Key { get; set; }
        public virtual string Field1 { get; set; }
        public virtual string Field2 { get; set; }
        public virtual string Field3 { get; set; }
        public virtual string Field4 { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public virtual string Info { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public virtual string Result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Memo { get; set; }
    }
}