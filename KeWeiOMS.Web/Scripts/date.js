if (typeof (JSON) == 'undefined') {
    $('head').append($("<script type='text/javascript' src='/Scripts/json2.js'>"));
} else {

}


Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
        RegExp.$1.length == 1 ? o[k] :
        ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}
function getDate(value) {
    var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
    return date.format("yyyy-MM-dd hh:mm:ss");
}

function getOnlyDate(value) {
    var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
    return date.format("yyyy-MM-dd");
}
function getStartDateTime(value, t) {
    var today = new Date();
    var enddate;
    if (t == "d")
        enddate = getDate(today.setDate(today.getDate() - value).toString());
    else {
        enddate = getDate(today.setHours(today.getHours() - value).toString());
    }
    return enddate;
}

function getStartDate(value) {
    var today = new Date();
    var enddate = getOnlyDate(today.setDate(today.getDate() - value).toString());
    return enddate;
}


function getTrue(value) {
    if (value == 1)
        return "是";
    else
        return "否";
}

var checks = [["1", "是"], ["0", "否"]];
var checksall = [["ALL", "ALL"], ["1", "是"], ["0", "否"]];
var checkOrdersAll = [["ALL", "ALL"], ["0", "处理后24小时未配货"], ["1", "配货后12小时未包装"], ["2", "包装后12小时未发货"]];
var checkPurAll = [["ALL", "ALL"], ["0", "3天未发货"], ["1", "5天未到货"]];
var orderDateType = [["CreateOn", "同步时间"], ["ScanningOn", "扫描时间"]];
var checkSex = '[["男"],["女"]]';
var checkPrint = [["ALL", "ALL"], ["1", "未打印"], ["2", "已打印"]];

var Packer = 5;
var Examiner = 7;
var Pei = 8;
var Purchaser = 9;

function formPost(form, url, dlg, dg, t, r) {

    $('#' + form).form('submit', {
        url: url,
        onSubmit: function () {
            return $(this).form('validate');
        },
        success: function (msg) {
            var result = eval('(' + msg + ')');
           // var result = $.parseJSON(msg);
            if (result.IsSuccess) {
                alert("操作成功");
                if (dlg) {
                    $('#' + dlg).dialog('close');
                }
                if (dg) {
                    if (t) {
                        $('#' + dg).treegrid('reload');
                    }
                    else {
                        $('#' + dg).datagrid('reload');
                    }
                }
            } else {
                if (result.ErrorMsg)
                    alert("保存失败!" + result.ErrorMsg);
                else {
                    alert("保存失败!");
                }
            }
            if (result.Info) {
                window.open(r);
            }
        }
    });
}

function delData(url, dg, t) {
    if (url) {
        $.messager.confirm('确认', '确定删除?', function (r) {
            if (r) {
                $.post(url, function () {
                }).success(function (data) {
                    var msgstr = "删除成功";
                    $.messager.show({
                        title: '提示',
                        msg: msgstr,
                        timeout: 3000,
                        showType: 'slide'
                    });
                    if (t) {
                        $('#' + dg).treegrid('reload');
                    }
                    else {
                        $('#' + dg).datagrid('reload');
                    }
                }).error(function () {
                    $.messager.alert('错误', '删除发生错误');
                });

            }
        });
    }
}

function showdlg(url, dlg, handle) {
    $('#' + dlg).html();
    $('#' + dlg).load(url, function () {
        $(this).dialog({
            title: '新建',
            modal: true,
            loadingMessage: '正在加载...',
            buttons: [{
                text: '提交',
                iconCls: 'icon-ok',
                handler: handle
            }, {
                text: '取消',
                handler: function () {
                    $('#' + dlg).dialog('close');
                }
            }]
        });
    }).dialog('open');
}

//“查询”按钮，弹出查询框
function showSrarch(url, dlg, dg) {
    $('#' + dlg).load(url, function () {
        $(this).dialog({
            title: '查询',
            modal: true,
            loadingMessage: '正在加载...',
            buttons: [{
                text: '提交',
                iconCls: 'icon-ok',
                handler: function () {
                    var search = "";
                    $('#' + dlg).find(":text,:selected,select,textarea,:hidden,:checked,:password").each(function () {
                        search = search + this.id + "&" + this.value + "^";
                    });
                    //执行查询                        
                    $('#' + dg).datagrid('reload', { search: search });
                    $('#' + dlg).dialog('close');
                }
            }, {
                text: '取消',
                handler: function () {
                    $('#' + dlg).dialog('close');
                }
            }]
        });
    }).dialog('open');
    $('#' + dlg).dialog("open");
};

function GetPic(v) {
    return '<img  src=' + v + '  height="64px" width="64px" />';
}

$.fn.panel.defaults.onBeforeDestroy = function () {
    var frame = $('iframe', this);
    try {
        if (frame.length > 0) {
            for (var i = 0; i < frame.length; i++) {
                frame[i].contentWindow.document.write('');
                frame[i].contentWindow.close();
            }
            frame.remove();
            if ($.browser.msie) {
                CollectGarbage();
            }
        }
    } catch (e) {
    }
};

