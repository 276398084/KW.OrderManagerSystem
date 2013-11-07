using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeWeiOMS.Web
{
    public class AliMessage
    {
        public bool haveFile { get; set; }
        public string orderUrl { get; set; }
        public string gmtCreate { get; set; }
        public string receiverLoginId { get; set; }
        public string messageType { get; set; }
        public string fileUrl { get; set; }
        public int productId { get; set; }
        public int id { get; set; }
        public string content { get; set; }
        public string senderName { get; set; }
        public string senderLoginId { get; set; }
        public string productUrl { get; set; }
        public bool read { get; set; }
        public string receiverName { get; set; }
        public object typeId { get; set; }
        public string productName { get; set; }
        public object orderId { get; set; }
        public int relationId { get; set; }
    }

    public class AliMessageList
    {
        public int total { get; set; }
        public List<AliMessage> msgList { get; set; }
        public bool success { get; set; }
    }
}
