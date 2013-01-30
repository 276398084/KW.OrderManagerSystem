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
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
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
                throw new Exception("返回实体为空");
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
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });

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
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }

        public JsonResult List(int page, int rows)
        {
            IList<EmailType> objList = NSession.CreateQuery("from EmailType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<EmailType>();

            return Json(new { total = objList.Count, rows = objList });
        }
        //获取邮件模板
        public JsonResult getEmailTemp()
        {
            IList<EmailTemplateType> EmaiTemp = NSession.CreateQuery("from EmailTemplateType").List<EmailTemplateType>();
            for (int i = 0; i < EmaiTemp.Count; i++)
            {
                EmaiTemp[i].Content = EmaiTemp[i].Content.ToString().Substring(0, 100) + "...";
            }
            return Json(EmaiTemp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getEmailTempDetail()
        {
            int id = int.Parse(Request["id"].ToString());
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
            return View(ob);
        }

        [HttpPost]
        public JsonResult EmailRerrr(EmailReturnType obj)
        {
            try
            {
                SendMail(obj.REmail,obj.Subject,obj.Content);
                obj.CreateOn = DateTime.Now;
                obj.CreateBy = DateTime.Now;
                NSession.Save(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
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


    }
}

