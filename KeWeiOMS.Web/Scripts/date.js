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

function getTrue(value) {
    if (value == 1)
        return "是";
    else
        return "否";
}

var checks = [["1", "是"], ["0", "否"]];

var checkSex = '[["不详"],["男"],["女"]]';

var Packer = 5;
var Examiner = 7;
var Purchaser = 9;

function formPost(form, url, dlg, dg, t) {

    $('#' + form).form('submit', {
        url: url,
        onSubmit: function () {
            return $(this).form('validate');
        },
        success: function (result) {
            var result = eval('(' + result + ')');
            if (result.errorMsg) {
                $.messager.show({
                    title: '提示',
                    msg: '保存失败:' + result.errorMsg,
                    timeout: 0,
                    showType: 'slide'
                });
            } else {

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
                $.messager.show({
                    title: '提示',
                    msg: '保存成功',
                    timeout: 2000,
                    showType: 'slide'
                });
            }
        }
    });
}

function delData(url, dg, t) {
    if (url) {
        alert('2' + url);
        $.messager.confirm('确认', '确定删除?', function (r) {
            if (r) {
                $.post(url, function () {
                }).success(function (data) {
                    var msgstr = "删除成功";
                    if (data != true) {
                        msgstr = "删除失败" + data;
                        $.messager.show({
                            title: '提示',
                            msg: msgstr,
                            timeout: 0,
                            showType: 'slide'
                        });
                    } else {
                        $.messager.show({
                            title: '提示',
                            msg: msgstr,
                            timeout: 3000,
                            showType: 'slide'
                        });
                    }

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
