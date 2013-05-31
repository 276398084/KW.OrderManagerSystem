using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public class AccountFreigheCount
    {
        /// <summary>
        /// 平台
        /// </summary>
        public virtual string Platform { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public virtual decimal Count { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public virtual decimal Amount { get; set; }

    }
}