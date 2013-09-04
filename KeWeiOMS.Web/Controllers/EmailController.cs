using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Collections;

namespace KeWeiOMS.Web.Controllers
{
    public class EmailController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(EmailType obj)
        {
            try
            {
                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EmailType GetById(int Id)
        {
            EmailType obj = NSession.Get<EmailType>(Id);
            if (obj == null)
            {
                obj = new EmailType { Id = 0 };
                return obj;
            }
            else
            {
                return obj;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            EmailType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(EmailType obj)
        {

            try
            {
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });

        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {

            try
            {
                EmailType obj = GetById(id);
                NSession.Delete(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        public JsonResult List(int page, int rows, string sort, string order,string search)
        {
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<EmailType> objList = NSession.CreateQuery("from EmailType "+where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<EmailType>();
            object count = NSession.CreateQuery("select count(Id) from EmailType " + where).UniqueResult();
            return Json(new { total =count, rows = objList });
        }
        //获取邮件模板
        [OutputCache(Location = OutputCacheLocation.None)]
        public JsonResult getEmailTemp()
        {
            IList<EmailTemplateType> EmaiTemp = NSession.CreateQuery("from EmailTemplateType where Enable=1").List<EmailTemplateType>();
            for (int i = 0; i < EmaiTemp.Count; i++)
            {
                if (EmaiTemp[i].Content.ToString().Length > 100)
                {
                    EmaiTemp[i].Content = EmaiTemp[i].Content.ToString().Substring(0, 100) + "...";
                }
            }
            return Json(EmaiTemp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getEmailTempDetail(int id)
        {
            IList<EmailTemplateType> EmailTemp = NSession.CreateQuery("from EmailTemplateType c where c.Id=:id").SetInt32("id", id).List<EmailTemplateType>();
            return Json(EmailTemp, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EmailRe(int id)
        {

            EmailType obj = GetById(id);
            EmailReturnType ob = new EmailReturnType { REmail = obj.BuyerEmail, Subject = "Re:" + obj.Subject };
            ViewData["sub"] = obj.Subject;
            ViewData["con"] = obj.Content;
            ViewData["eid"] = obj.Id;
            ViewData["Reply"] = obj.IsReply;
            return View(ob);
        }

        [HttpPost]
        public JsonResult EmailRe(EmailReturnType obj)
        {
            try
            {
                DateTime ReTime = DateTime.Now;
                SendMail(obj.REmail, obj.Subject, obj.Content);
                obj.CreateOn = ReTime;
                obj.CreateBy = ReTime;

                NSession.Save(obj);
                IList<EmailType> mail = NSession.CreateQuery("from EmailType c where c.Id=:id").SetInt32("id", obj.EId).List<EmailType>();
                mail[0].IsReply = 1;
                mail[0].ReplyOn = ReTime;
                NSession.Update(mail[0]);
                NSession.Flush();

            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true  });
        }

        //发送邮件
        static public void SendMail(string adr, string title, string text)
        {
            SmtpClient smtp = new SmtpClient(); //实例化一个SmtpClient
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //将smtp的出站方式设为 Network
            smtp.EnableSsl = false;//smtp服务器是否启用SSL加密

            smtp.Host = "smtp.163.com"; //指定 smtp 服务器地址
            smtp.Port = 25;             //指定 smtp 服务器的端口，默认是25，如果采用默认端口，可省去
            //如果需要认证，则用下面的方式
            smtp.Credentials = new NetworkCredential("feidutest", "feiduq");

            MailMessage mm = new MailMessage(); //实例化一个邮件类
            mm.Priority = MailPriority.Normal; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
            mm.From = new MailAddress("feidutest@163.com", "飞度贸易", Encoding.GetEncoding(936));
            mm.To.Add(adr);
            //邮件标题
            mm.Subject = title;
            mm.SubjectEncoding = Encoding.GetEncoding(936);
            // 这里非常重要，如果你的邮件标题包含中文，这里一定要指定，否则对方收到的极有可能是乱码。
            mm.BodyEncoding = Encoding.GetEncoding(936);
            //邮件正文的编码， 设置不正确， 接收者会收到乱码
            mm.Body = text;
            //邮件正文
            smtp.Send(mm); //发送邮件
        }

        public JsonResult GetNext(int id)
        {
            int check = 0;
            IList<EmailType> list = NSession.CreateQuery("from EmailType order by Id order by desc").List<EmailType>();
            foreach (var item in list)
            {
                if (check == 1 && item.IsReply!=1)
                {
                    return Json(new { Msg = item.Id }, JsonRequestBehavior.AllowGet);
                }
                if (item.Id == id)
                {
                    check = 1;
                }

            }
            return Json(new { Msg = 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsRead(int id)
        {
            EmailType obj = GetById(id);
            if (obj.IsReply !=0)
            {
                return Json(new { Msg = 1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Msg = 0 }, JsonRequestBehavior.AllowGet);
        }

    }
}

