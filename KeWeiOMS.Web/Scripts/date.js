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


function getStartDate(value) {
    var today = new Date();
    var enddate = getOnlyDate(today.setDate(today.getDate() - value).toString());
    return enddate;
}
function getTrue(value) {
    if (value == 1)
        return "�;
    else
        return "�;
}


var checks = [["1", "�], ["0", "�]];

var checkSex = '[["�],["�]]';

var checkPrint = [["0", "全部"], ["1", "未打�], ["2", "已打�]];
var Packer = 5;
var Examiner = 7;
var Pei = 8;
var Purchaser = 9;



function formPost(form, url, dlg, dg, t) {

    $('#' + form).form('submit', {
        url: url,
        onSubmit: function () {
            return $(this).form('validate');
        },
        success: function (msg) {

            var result = $.parseJSON(msg);
            if (result.IsSuccess) {
                alert("操作成功");
                if (dlg) {
                    $('#' + dlg).dialog('close');
                }
                if (dg) {
                    if (t) {
                        if (t == 't')
                            $('#' + dg).treegrid('reload');
                        else {
                            window.location.reload();
                        }
                    }
                    else {
                        $('#' + dg).datagrid('reload');
                    }
                }
            } else {
                alert("保存失败!");
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

//“查询”按钮，弹出查询�function showSrarch(url, dlg, dg) {
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