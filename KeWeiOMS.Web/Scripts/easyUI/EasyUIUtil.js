
/**
 * @author 孙宇
 * 
 * @requires jQuery,EasyUI,jQuery cookie plugin
 * 
 * 更换EasyUI主题的方法
 * 
 * @param themeName
 *            主题名称
 */
changeTheme = function (themeName) {
    //delCookie('Theme');
    //SetCookie('Theme', themeName);
    $.cookie('Theme', themeName);
    setTheme(themeName);
};
setTheme = function (themeName) {
    //var themeName = $.cookie('Theme');
    //if (themeName == null) { themeName = "metro-blue"; }
    //var $easyuiTheme = $('#easyuiTheme');
    //var url = $easyuiTheme.attr('href');
    //var href = url.substring(0, url.indexOf('themes')) + 'themes/' + themeName + '/easyui.css';
    //$easyuiTheme.attr('href', href);

    //var $iframe = $('iframe');
    //if ($iframe.length > 0) {
    //    for (var i = 0; i < $iframe.length; i++) {
    //        var ifr = $iframe[i];
    //        $(ifr).contents().find('#easyuiTheme').attr('href', href);
    //    }
    //}

    $("#layout_north_pfMenu .menu-item").removeClass("accordion-expand");
    $("#layout_north_pfMenu .menu-item[l=" + themeName + "]").addClass("accordion-expand");
}

//function SetCookie(name,value)//两个参数，一个是cookie的名子，一个是值

//    {
//        var Days =30; //此 cookie 将被保存 30 天

//        var exp = new Date();    //new Date("December 31, 9998");

//        exp.setTime(exp.getTime() + Days*24*60*60*1000);

//        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ";path=/";;
//}


//function delCookie(name)//删除cookie
//    {
//        var exp = new Date();

//        exp.setTime(exp.getTime() - 1);

//        var cval=getCookie(name);

//        if (cval != null)
//            document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString() + "; path=/";;

//    }




