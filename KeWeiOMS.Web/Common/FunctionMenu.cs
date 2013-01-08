using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KeWeiOMS.Domain;
using KeWeiOMS.NhibernateHelper;
using NHibernate;


namespace KeWeiOMS.Web
{
    /// <summary>
    /// 导航菜单构造器
    /// </summary>
    public class FunctionMenu
    {
        /// <summary>
        /// 支持两层
        /// </summary>
        /// <returns></returns>
        public static List<MenuItem> GetFunctionMenus()
        {
            List<MenuItem> items = new List<MenuItem>();
            ISession Session = NHibernateHelper.CreateSession();
            IList<ModulesType> customerList = Session.CreateQuery("from ModulesType").List<ModulesType>();

            //加载第一层
            List<ModulesType> list = customerList.Where(p => p.ParentId == 0 && p.IsMenu == 1 && p.DeletionStateCode == 0).OrderBy(f => f.SortCode).ToList<ModulesType>();
            //加载所有
            List<ModulesType> all = customerList.Where(p =>  p.IsMenu == 1 && p.DeletionStateCode == 0).OrderBy(f => f.SortCode).ToList();

            //循环加载第一层
            foreach (var functionType in list)
            {
                MenuItem mi = new MenuItem() { icon = functionType.ImageIndex, menuid = functionType.Id, menuname = functionType.FullName, url = functionType.NavigateUrl };

                //从all中加载自己的子菜单
                List<ModulesType> subs = all.Where(p => p.ParentId == mi.menuid).OrderBy(f => f.SortCode).ToList();

                //如果有
                if (subs.Any())
                {
                    List<MenuItem> subitems = new List<MenuItem>();
                    //循环添加子菜单
                    foreach (var subtype in subs)
                    {
                        MenuItem submi = new MenuItem() { icon = subtype.ImageIndex, menuid = subtype.Id, menuname = subtype.FullName, url = subtype.NavigateUrl };

                        subitems.Add(submi);
                    }
                    mi.menus = subitems;
                }
                items.Add(mi);

            }



            return items;
        }
    }
}
