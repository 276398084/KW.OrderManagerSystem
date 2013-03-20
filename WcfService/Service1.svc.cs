using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NHibernate;
using KeWeiOMS.NhibernateHelper;
using KeWeiOMS.Domain;
using NHibernate.Cfg;

namespace WcfService
{
   
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
        [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Service1 : IService1
    {
        [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
        public ISession NSession = SessionBuilder.CreateSession();

           [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
        public string GMarket(string ItemId,string ItemTitle,string Price,string PicUrl,int Nownum,string ProductUrl,string Account)
        {
            try
            {
                // 在此处添加操作实现
                GMarketType obj = new GMarketType();
                obj.ItemId = ItemId;
                obj.ItemTitle = ItemTitle;
                obj.Price = Price;
                obj.PicUrl = PicUrl;
                obj.NowNum = Nownum;
                obj.Account = Account;
                obj.CreateOn = DateTime.Now;
                NSession.Save(obj);
                NSession.Flush();
            }
            catch (Exception ex)
            {
                return "保存失败";
            }
            return "保存成功";
        }
    }
}
