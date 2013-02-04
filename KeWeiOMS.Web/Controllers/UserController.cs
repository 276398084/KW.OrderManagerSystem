using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;

namespace KeWeiOMS.Web.Controllers
{
    public class UserController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            UserType u = new UserType();
            u.Code = Utilities.GetUserNo();
            return View(u);
        }

        public ActionResult GetCompetence()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(UserType obj)
        {
            try
            {
                obj.CreateOn = DateTime.Now;
                obj.LastVisit = DateTime.Now;

                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { errorMsg = "出错了" });
            }
            return Json(new { IsSuccess = "true" });
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserType user)
        {
            if (Session["__VCode"] == null || (Session["__VCode"] != null && user.ValidateCode != Session["__VCode"].ToString()))
            {
                ModelState.AddModelError("Username", "验证码错误！"); //return "";
                return View();
            }
            if (ModelState.IsValid)
            {
                IList<UserType> list = NSession.CreateQuery(" from  UserType where Username=:p1 and Password=:p2").SetString("p1", user.Username).SetString("p2", user.Password).List<UserType>();
                if (list.Count > 0)
                {   //登录成功
                    Session["account"] = list[0];
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("Username", "用户名或者密码出错。");
            return View();

        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UserType GetById(int Id)
        {
            UserType obj = NSession.Get<UserType>(Id);
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
            UserType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(UserType obj)
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
                UserType obj = GetById(id);
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
            IList<UserType> objList = NSession.CreateQuery("from UserType")
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<UserType>();

            return Json(new { total = objList.Count, rows = objList });
        }

        public JsonResult QList(string Id)
        {

            IList<UserType> objList = NSession.CreateQuery("from UserType where RoleId=" + Id)
                .List<UserType>();
            return Json(objList);
        }
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound()
        {
            return View();
        }

        public void ValidateCode()
        {
            // 在此处放置用户代码以初始化页面
            string vnum;
            vnum = GetByRndNum(4);
            Response.ClearContent(); //需要输出图象信息 要修改HTTP头 
            Response.ContentType = "image/jpeg";

            CreateValidateCode(vnum);

        }

        private void CreateValidateCode(string vnum)
        {
            Bitmap Img = null;
            Graphics g = null;
            Random random = new Random();
            int gheight = vnum.Length * 15;
            Img = new Bitmap(gheight, 26);
            g = Graphics.FromImage(Img);
            Font f = new Font("微软雅黑", 16, FontStyle.Bold);
            //Font f = new Font("宋体", 9, FontStyle.Bold);

            g.Clear(Color.White);//设定背景色
            Pen blackPen = new Pen(ColorTranslator.FromHtml("#e1e8f3"), 18);

            for (int i = 0; i < 128; i++)// 随机产生干扰线，使图象中的认证码不易被其它程序探测到
            {
                int x = random.Next(gheight);
                int y = random.Next(20);
                int xl = random.Next(6);
                int yl = random.Next(2);
                g.DrawLine(blackPen, x, y, x + xl, y + yl);
            }

            SolidBrush s = new SolidBrush(ColorTranslator.FromHtml("#411464"));
            g.DrawString(vnum, f, s, 1, 1);

            //画边框
            blackPen.Width = 1;
            g.DrawRectangle(blackPen, 0, 0, Img.Width - 1, Img.Height - 1);
            Img.Save(Response.OutputStream, ImageFormat.Jpeg);
            s.Dispose();
            f.Dispose();
            blackPen.Dispose();
            g.Dispose();
            Img.Dispose();

            //Response.End();
        }

        //-----------------给定范围获得随机颜色
        Color GetByRandColor(int fc, int bc)
        {
            Random random = new Random();

            if (fc > 255)
                fc = 255;
            if (bc > 255)
                bc = 255;
            int r = fc + random.Next(bc - fc);
            int g = fc + random.Next(bc - fc);
            int b = fc + random.Next(bc - bc);
            Color rs = Color.FromArgb(r, g, b);
            return rs;
        }

        //取随机产生的认证码(数字)
        public string GetByRndNum(int VcodeNum)
        {

            string VNum = "";

            Random rand = new Random();

            for (int i = 0; i < VcodeNum; i++)
            {
                VNum += VcArray[rand.Next(0, 9)];
            }
            Session["__VCode"] = VNum;
            return VNum;
        }
        private static readonly string[] VcArray = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    }
}

