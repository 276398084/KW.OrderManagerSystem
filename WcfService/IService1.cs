using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public interface IService1
    {
        [OperationContract]
        [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
        string GMarket(string ItemId, string ItemTitle, string Price, string PicUrl, int Nownum, string ProductUrl, string Account);
    }

}
