﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeWeiOMS.Web
{
    /// <summary>
    /// 菜单项模型
    /// </summary>
    public class MenuItem
    {

        public int menuid { get; set; }
        public string menuname { get; set; }
        public string icon { get; set; }
        public string url { get; set; }

        public int index { get; set; }

        public List<MenuItem> menus { get; set; }
    }


}
