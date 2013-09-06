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
        // public ISession NSession = NHibernateHelper.CreateSession();
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            UserType u = new UserType();
            u.Code = Utilities.GetUserNo(NSession);
            return View(u);
        }

        public ActionResult ChangePassword()
        {
            ViewData["Username"] = CurrentUser.Username;
            return View();
        }

        public JsonResult Change(string oldpsd, string newpsd)
        {
            try
            {
                if (oldpsd == CurrentUser.Password)
                {
                    UserType obj = GetById(CurrentUser.Id);
                    obj.Password = newpsd;
                    NSession.Update(obj);
                    NSession.Flush();
                    Utilities.LoginByUser(obj.Username, obj.Password, NSession);
                    Utilities.CreateCookies(obj.Username, obj.Password);

                }
                else
                    return Json(new { IsSuccess = false, ErrorMsg = "旧密码不正确" });
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        public ActionResult GetCompetence(string id)
        {
            ViewData["uid"] = id;
            return View();
        }

        [HttpPost]
        public JsonResult Create(UserType obj)
        {
            try
            {


                IList<UserType> list = NSession.CreateQuery("from UserType where Username='" + obj.Username + "'").List<UserType>();
                if (list.Count > 0)
                {
                    return Json(new { IsSuccess = false, ErrorMsg = "用户名重复！" });
                }
                obj.CreateOn = DateTime.Now;
                obj.LastVisit = DateTime.Now;

                NSession.SaveOrUpdate(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            Utilities.ClearCookies();
            return View("Login");

        }

        [HttpPost]
        public ActionResult Login(UserType user)
        {
            if (Session["__VCode"] == null || (Session["__VCode"] != null && user.ValidateCode != Session["__VCode"].ToString()))
            {
                ModelState.AddModelError("Username", "验证码错误！");
                return View();
            }

            bool iscon = Utilities.LoginByUser(user.Username, user.Password, NSession);
            if (iscon)
            {
                Utilities.CreateCookies(user.Username, user.Password);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Username", "用户名或者密码出错。");
            return View();
        }

        public ActionResult SetMP(string a, string m, string p, int uid)
        {
            string[] ms = m.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] ps = p.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] fos = a.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            PermissionScopeType sc = null;
            NSession.CreateQuery("delete from PermissionScopeType where ResourceCategory='" +
                                  ResourceCategoryEnum.User.ToString() + "' and ResourceId=" + uid).ExecuteUpdate();
            foreach (var item in ms)
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                sc = new PermissionScopeType();
                sc.ResourceCategory = ResourceCategoryEnum.User.ToString();
                sc.ResourceId = uid;
                sc.TargetCategory = TargetCategoryEnum.Module.ToString();
                sc.TargetId = Convert.ToInt32(item);
                NSession.Save(sc);
                NSession.Flush();
            }

            foreach (var item in ps)
            {
                if (string.IsNullOrEmpty(item))
                    continue; ;
                sc = new PermissionScopeType();
                sc.ResourceCategory = ResourceCategoryEnum.User.ToString();
                sc.ResourceId = uid;
                sc.TargetCategory = TargetCategoryEnum.PermissionItem.ToString();
                sc.TargetId = Convert.ToInt32(item);
                NSession.Save(sc);
                NSession.Flush();
            }
            foreach (var item in fos)
            {
                if (string.IsNullOrEmpty(item))
                    continue; ;
                sc = new PermissionScopeType();
                sc.ResourceCategory = ResourceCategoryEnum.User.ToString();
                sc.ResourceId = uid;
                sc.TargetCategory = TargetCategoryEnum.Account.ToString();
                sc.TargetId = Convert.ToInt32(item);
                NSession.Save(sc);
                NSession.Flush();
            }
            return Json(new { IsSuccess = true });
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
                OrganizeType obj1 = NSession.Get<OrganizeType>(obj.DId);
                if (obj1 != null)
                {
                    obj.DepartmentName = obj1.ShortName;
                }
                OrganizeType obj2 = NSession.Get<OrganizeType>(obj.CId);
                if (obj2 != null)
                {
                    obj.CompanyName = obj2.ShortName;
                }
                NSession.Update(obj);
                NSession.Flush();
            }
            catch (Exception ee)
            {
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });

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
                return Json(new { IsSuccess = false, ErrorMsg = "出错了" });
            }
            return Json(new { IsSuccess = true });
        }

        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string orderby = Utilities.OrdeerBy(sort, order);
            string where = Utilities.SqlWhere(search);
            IList<UserType> objList = NSession.CreateQuery("from UserType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows * page)
                .List<UserType>();
            object count = NSession.CreateQuery("select count(Id) from UserType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ListQ(string q)
        {
            IList<UserType> objList = NSession.CreateQuery("from UserType where Realname like '%" + q + "%'")
                .List<UserType>();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QList(string Id)
        {

            IList<UserType> objList = NSession.CreateQuery("from UserType where RoleId=" + Id)
                .List<UserType>();
            return Json(objList);
        }

        public JsonResult QQList()
        {

            IList<UserType> objList = NSession.CreateQuery("from UserType")
                .List<UserType>();
            return Json(objList);
        }

        public JsonResult QList2()
        {
            IList<UserType> objList = NSession.CreateQuery("from UserType where DId in (19,24,25,15)")
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



        #region 验证码
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
        #endregion

    }
}

