function doToggleAll() {
    var hasHidden = false;
    $("#tbResult tr.tr-details").each(function() {
        if (!$(this).is(":visible")) {
            hasHidden = true;
            return false
        }
    });
    var resetDetailExpandState = function($tr) {
        var $btn = $tr.closest("tbody").find("a.toggle-details");
        $btn.text($tr.is(":visible") ? LC.Summary: LC.Details)
    };
    if (hasHidden) {
        $("#tbResult tr.tr-details").each(function() {
            $(this).show();
            resetDetailExpandState($(this));
            $(".tb-actions .act-toggle-all").text(LC.Collapse).addClass("act-collapse-all")
        })
    } else {
        $("#tbResult tr.tr-details").each(function() {
            $(this).hide();
            resetDetailExpandState($(this));
            $(".tb-actions .act-toggle-all").text(LC.Expand).removeClass("act-collapse-all")
        })
    }
}
function doToggleDetails(sender) {
    var resetExpandState = function() {
        var hasHidden = false;
        $("#tbResult>tbody").each(function() {
            var $tr1 = $("tr.tr-details", $(this));
            if ($tr1.length > 0 && !$tr1.is(":visible")) {
                hasHidden = true;
                return false
            }
        });
        if (hasHidden) {
            $(".tb-actions .act-toggle-all").text(LC.Expand).removeClass("act-collapse-all")
        } else {
            $(".tb-actions .act-toggle-all").text(LC.Collapse).addClass("act-collapse-all")
        }
    };
    var $sender = $(sender),
    $tbody = $sender.closest("tbody"),
    $tr1 = $tbody.find("tr:eq(1)");
    if ($tr1.length > 0 && !$tr1.is(":visible")) {
        $tr1.show();
        $tbody.addClass("expanded");
        $sender.text(LC.Summary);
        resetExpandState()
    } else {
        $tr1.hide();
        $tbody.removeClass("expanded");
        $sender.text(LC.Details);
        resetExpandState()
    }
}
function renderStat() {
    var $tb = $("#tbResult");
    var statResults = [0, 0, 0, 0];
    $tb.find("tbody[packstate]").each(function() {
        var idx = parseInt($(this).attr("packstate"), 10);
        statResults[idx] += 1
    });
    $("#statResult em").each(function(i) {
        $(this).text("(" + statResults[i] + ")")
    })
}
function renderTrackResult(sender, r) {
    var langMapName = {
        cn: "c",
        hk: "d",
        en: "e"
    };
    var langObjName = eval("langMapName." + CUR_LANG);
    var dat = r.dat;
    var $sender = $(sender),
    $tbd = $sender.closest("tbody"),
    $tr0 = $(">tr:eq(0)", $tbd),
    $td = null;
    if (r.ret <= 0) {
        $tbd.attr("err", r.ret);
        $tr0.find(".s-infoState").html("");
        $tr0.find(".s-lastEvent").html(eval("LC.TrackResultErr_" + ( - r.ret)));
        $tr0.find(".s-packState").html("");
        $tr0.find(".s-actions").html('<a class="red retrace" onclick="doTrack(this,event)" title="' + LC.RetraceTitle + '">' + LC.Retrace + "</a>");
        return
    }
    $tbd.attr("childtype", dat.e);
    $tbd.attr("infostatemain", dat.g);
    $tbd.attr("infostatechild", dat.h);
    $tbd.attr("packstate", dat.f);
    $tr0.find(".s-infoState, .s-lastEvent, .s-packState, .s-actions").html("");
    $tr0.find(".s-num").append('<a target="_blank" href="../sim-post.shtml?num=' + dat.c + "&pt=" + dat.e + '" title="' + LC.ViewOfficialTitle + '">' + LC.ViewOfficial + "</a>").find("p").append('<a target="_blank" href="../report-post.shtml?m=' + dat.d + "&c=" + dat.e + '" class="ico-rp" title="' + LC.Report + '"></a>');
    if (dat.f === 0) {
        var $guess_html = $('<dl class="guest"><dt>' + LC.NumberMaybe + "</dt></dl>");
        for (var i = 0; i < RULE_EXPRESS.length; ++i) {
            var item = RULE_EXPRESS[i],
            ok = new RegExp("^(" + item.b + ")$", "i").test(dat.c);
            if (!ok) {
                continue
            }
            var express = EXPRESS_ENUM.first(function(itm) {
                return itm.a === item.a
            });
            $guess_html.append('<dd><a target="blank" href="express-' + express.a + ".shtml?nums=" + dat.c + '">' + eval("express." + langObjName) + "</a></dd>")
        }
        if ($guess_html.find("DD").length > 0) {
            $tr0.find(".s-lastEvent").append($guess_html)
        }
    }
    var setInfoState = function($td, c, s, isChild) {
        var stateCss = "";
        switch (s) {
        case 0:
        case 2:
            break;
        case 1:
            stateCss = "green";
            break;
        default:
            stateCss = "red";
            break
        }
        var enu = POST_ENUM.first(function(item) {
            return item.a == c
        });
        var country_html = enu.b == "" ? eval("enu." + langObjName) : ('<a target="_blank" title="' + LC.Post_Title + '" href="' + enu.b + '">' + eval("enu." + langObjName) + "</a>");
        var btn_html = (isChild ? '<a class="ac-change" href="javascript:;" title="' + LC.Post_ChangeCountryTitle + '" onclick="changeCountry(this,event)">' + LC.Post_ChangeCountry + "</a>": "");
        $td.html("<p>" + country_html + btn_html + '</p><em class="' + stateCss + '" title="' + eval("LC.InfoStateTitle_" + s) + '">' + eval("LC.InfoState_" + s) + "</em>")
    };
    setInfoState($(".s-from", $tr0), dat.d, dat.g, false);
    setInfoState($(".s-to", $tr0), dat.e, dat.h, true);
    if (dat.z !== null) {
        var lastEvent = ['<span lang="' + (dat.y.length > 0 ? dat.m: dat.l) + '" onmouseover="showTranslationTip(this)" onmouseout="hideTranslationTip(this)">', dat.z.a + ", " + dat.z.b, "</span>"].join("");
        $tr0.find(".s-lastEvent").html(lastEvent)
    }
    $td = $tr0.find(".s-packState").html("");
    $td.append($('<div class="' + (dat.f == 3 ? "green": "") + '" title="' + eval("LC.PackageStateTitle_" + dat.f) + '">' + eval("LC.PackageState_" + dat.f) + "</div>"));
    if (dat.i > -1) {
        $td.append($("<div>(" + dat.i + " " + LC.Day + ")</div>"))
    }
    $td = $tr0.find(".s-actions").html("<div></div>");
    var $divActions = $(">div", $td);
    if ((dat.x && dat.x.length > 0) || (dat.y && dat.y.length > 0)) {
        $divActions.append($(['<a class="toggle-details" href="javascript:;" onclick="doToggleDetails(this)" title="' + LC.DetailsOrSummaryTitle + '">' + LC.Details + "</a>", '<a class="copy" href="javascript:;" title="' + LC.CopyTitle + '">' + LC.Copy + "</a>", ].join("")));
        var $btnCopy = $divActions.find(".copy").bindZeroClipboard("default")
    }
    if ((dat.g !== 0 && dat.g !== 1 && dat.g !== 2) || (dat.h !== 0 && dat.h !== 1 && dat.h !== 2)) {
        $divActions.append('<a class="red retrace" onclick="doTrack(this,event)" title="' + LC.RetraceTitle + '">' + LC.Retrace + "</a>")
    }
    if ((dat.x && dat.x.length > 0) || (dat.y && dat.y.length > 0)) {
        var setEvents = function($dl, gt, ct, c, ev, tt, ln) {
            var html_times = '<span title="' + LC.GatherTimeSpanTitle + '">' + LC.GatherTimeSpan + ":" + gt + "&nbsp;" + LC.Millisecond + "</span>";
            var d = $.parseDate(ct);
            if (d.getTime() !== $.parseDate("9999-12-31T00:00:00").getTime()) {
                d.setHours(d.getHours() - 8);
                html_times += ', <span title="' + LC.CacheTimeTitle + '">' + LC.CacheTime + ":" + d.format("yyyy/mm/dd HH:MM:ss") + "</span>"
            }
            var enu = POST_ENUM.first(function(item) {
                return item.a == c
            });
            $dl.append("<dt>" + tt + " - " + eval("enu." + langObjName) + ":" + html_times + "</dt>");
            for (var i = 0; i < ev.length; ++i) {
                var item = ev[i];
                $dl.append('<dd><span lang="' + ln + '" onmouseover="showTranslationTip(this)" onmouseout="hideTranslationTip(this)">' + item.a + ", " + item.b + "</span></dd>")
            }
        };
        var $tr1 = $('<tr class="tr-details"><td colspan="6"><dl></dl><dl></dl></td></tr>');
        setEvents($("dl:eq(0)", $tr1), dat.k, dat.w, dat.e, dat.y, LC.Post_To, dat.m);
        setEvents($("dl:eq(1)", $tr1), dat.j, dat.v, dat.d, dat.x, LC.Post_From, dat.l);
        $tr1.insertAfter($tr0)
    }
}
function doTrack(sender, event, num) {
    if (typeof(event) === "undefined") {
        var e = event ? event: (window.event ? window.event: null);
        if (e.preventDefault) {
            e.preventDefault()
        } else {
            e.returnValue = false
        }
    }
    var $tbd = $(sender).closest("tbody");
    var $trSummary = $tbd.find(">tr:eq(0)");
    $trSummary.nextAll("tr").remove();
    if (!num) {
        num = $tbd.find(".s-num b").text()
    }
    var html_loading = '<img src="/themes/icons/loading.gif" alt="' + LC.LoadingPicAlt + '"/>';
    $tbd.find(".s-num").html("<p><b>" + num + "</b></p>");
    $tbd.find(".s-lastEvent").text(LC.LoadingDesc);
    $tbd.find(".s-infoState, .s-packState, .s-actions").html(html_loading);
    $tbd.removeAttr("err childtype infostatemain infostatechild packstate");
    var pt = parseInt($tbd.attr("pt"), 10);
    if (isNaN(pt)) {
        pt = 0
    }
    var url = getOneRestServer("/Rest/HandlerTrackPost.ashx");
    $.ajax({
        type: "get",
        dataType: "jsonp",
        url: url,
        context: $tbd,
        crossDomain: true,
        timeout: 140000,
        data: {
            lo: document.location.host,
            pt: pt,
            num: num,
            hs: hs([num, pt])
        },
        success: function(r) {
            renderTrackResult($tbd, r);
            renderStat()
        },
        error: function(XMLHttpRequest, textStatus, err) {
            var $context = $(this);
            $context.attr("err", 0);
            $context.find(".s-infoState").html("");
            $context.find(".s-lastEvent").html(textStatus + ":" + err);
            $context.find(".s-packState").html("");
            $context.find(".s-actions").html('<a class="red retrace" onclick="doTrack(this,event)" title="' + LC.RetraceTitle + '">' + LC.Retrace + "</a>")
        }
    })
}
function doRetraceErrors(sender, event) {
    $("#tbResult A.retrace").trigger("click")
}
function doToggleGroup(sender) {
    var $sender = $(sender);
    if ($("#tbResult tbody.group").length > 0) {
        $("#tbResult tbody.group").remove();
        var count = $("#tbResult tbody").length;
        for (var i = 0; i < count; ++i) {
            var $item = $("#trackItem_" + i);
            $("#tbResult").append($item);
            $item.removeClass("even").removeClass("odd").addClass(i % 2 == 0 ? "even": "odd")
        }
        $sender.text(LC.Group).attr("title", LC.GroupTitle);
        return
    }
    var hasLoading = false;
    $("#tbResult tbody").each(function() {
        var err = $(this).attr("err"),
        packstate = $(this).attr("packstate");
        if (typeof(err) === "undefined" && typeof(packstate) === "undefined") {
            hasLoading = true;
            return false
        }
    });
    if (hasLoading) {
        $.alert(LC.DialogMessage, LC.Err_ForbidGroup);
        return
    }
    $("#tbResult").append('<tbody id="groupError" class="group"><tr><td colspan="6">' + LC.Group_ErrorState + "<em>(0)</em></td></tr></tbody>");
    for (var i = 0; i <= 3; ++i) {
        $("#tbResult").append('<tbody id="groupState' + i + '" class="group"><tr><td colspan="6">' + eval("LC.PackageState_" + i) + "<em>(0)</em></td></tr></tbody>")
    }
    var arr = $('#tbResult tbody[id^="trackItem_"]').toArray();
    var stat = [0, 0, 0, 0, 0];
    for (var i = arr.length - 1; i >= 0; --i) {
        var $item = $(arr[i]);
        var err = $item.attr("err"),
        packstate = parseInt($item.attr("packstate"), 10);
        if (err) {
            stat[stat.length - 1] += 1;
            $("#groupError").find("EM").text("(" + stat[stat.length - 1] + ")");
            $("#groupError").after($item)
        } else {
            if (!isNaN(packstate)) {
                stat[packstate] += 1;
                $("#groupState" + packstate).find("EM").text("(" + stat[packstate] + ")");
                $("#groupState" + packstate).after($item)
            }
        }
    }
    $("#tbResult tbody.group").each(function() {
        $(this).nextUntil("tbody.group").each(function(i) {
            $(this).removeClass("even").removeClass("odd").addClass(i % 2 == 0 ? "even": "odd")
        })
    });
    $sender.text(LC.Ungroup).attr("title", LC.UngroupTitle)
}
function translateToLang(newLang) {
    if (!isNaN(newLang)) {
        $.cookie("translateToLang", newLang, {
            path: "/",
            expires: 3650
        })
    }
    try {
        var v = parseInt($.cookie("translateToLang"), 10);
        return isNaN(v) ? 0: v
    } catch(e) {
        return 0
    }
}
function renderTranslationLang() {
    var lang = translateToLang();
    var o = LANG.first(function(item) {
        return item.a == lang
    });
    if (CUR_LANG === "cn") {
        $("#btnTranlate em").text(o.c)
    } else {
        if (CUR_LANG === "hk") {
            $("#btnTranlate em").text(o.d)
        } else {
            $("#btnTranlate em").text(o.e)
        }
    }
}
function doConfigTranslation(sender) {
    $.showDialog({
        url: "/" + CUR_LANG + "/tools/translation.htm",
        title: "translator",
        data: {
            onSelected: "doConfigTranslationCallback",
            locate: translateToLang
        },
        width: 560,
        height: 380
    })
}
function doConfigTranslationCallback(e) {
    translateToLang(e.lang.a);
    renderTranslationLang();
    $.getDialog(e._dialogId).hide()
}
function showTranslationTip(sender) {
    var $sender = $(sender);
    var toLang = translateToLang();
    if (toLang === 0) {
        return
    }
    var fromLang = parseInt($sender.attr("lang"), 10);
    if (fromLang === toLang && fromLang === 0) {
        return
    }
    var fromLangName = LANG.first(function(item) {
        return item.a == fromLang
    }).b;
    var toLangName = LANG.first(function(item) {
        return item.a == toLang
    }).b;
    if (!fromLangName || !toLangName) {
        return
    }
    var text = $(sender).text();
    if (text === "") {
        return
    }
    $.tips.get("translationTip").show($sender, '<img src="/themes/icons/loading.gif" alt="' + LC.LoadingPicAlt + '"/>');
    bingTranslate(fromLangName, toLangName, "translationCallback", text)
}
function hideTranslationTip() {
    $.tips.get("translationTip").hide()
}
function translationCallback(response) {
    $.tips.get("translationTip").setContent(response)
}
function changeCountry(sender, event) {
    var $sender = $(sender),
    $tbd = $sender.closest("tbody"),
    pt = parseInt($tbd.attr("pt"), 10);
    if (isNaN(pt)) {
        pt = 0
    }
    $.showDialog({
        url: "/" + CUR_LANG + "/tools/postType.shtml",
        title: "select destination country",
        data: {
            locate: pt,
            onSelected: "changeCountryCallback",
            token: $tbd.attr("id")
        },
        width: 700,
        height: 320
    })
}
function changeCountryCallback(e) {
    $.getDialog(e._dialogId).hide();
    var $tbd = $("#" + e.token);
    var oldValue = parseInt($tbd.attr("pt"), 10);
    if (oldValue === parseInt(e.value, 10)) {
        return
    }
    $tbd.attr("pt", e.value);
    doTrack($tbd, null)
}
function initTrack() {
    var numbers = $.trim($.getQueryString(location.href, "nums"));
    if (numbers === "") {
        return
    }
    var nums = [],
    re = /^[A-Za-z0-9]+$/;
    numbers = numbers.split(/\s|\,/ig);
    for (var i = 0, sum = 0; i < numbers.length; ++i) {
        var line = $.trim(numbers[i]).toUpperCase();
        var mt = re.exec(line);
        if (mt === null || mt.length === 0 || nums.indexOf(mt[0]) >= 0) {
            continue
        }
        nums.push(mt[0]);
        if (++sum >= 40) {
            break
        }
    }
    $("#frmPost textarea").val(nums.join("\r\n"));
    if (nums.length > 0) {
        var $body = (window.opera) ? (document.compatMode == "CSS1Compat" ? $("html") : $("body")) : $("html,body");
        $body.animate({
            scrollTop: $("#mainNavWrap").offset().top
        },
        1000)
    }
    var $tb = $("#tbResult");
    $tb.find(">tbody").remove();
    for (var i = 0; i < nums.length; ++i) {
        var css = i % 2 == 0 ? "even": "odd";
        var num = nums[i];
        var $html = $(['<tbody id="trackItem_' + i + '" class="' + css + '">', "<tr>", '<td class="s-num"></td>', '<td class="s-infoState s-from"></td>', '<td class="s-infoState s-to"></td>', '<td class="s-lastEvent"></td>', '<td class="s-packState"></td>', '<td class="s-actions"></td>', "</tr>", "</tbody>"].join(""));
        $html.mouseover(function() {
            $(this).addClass("hover")
        }).mouseout(function() {
            $(this).removeClass("hover")
        });
        $tb.append($html);
        doTrack($html.filter("tbody:eq(0)"), null, num)
    }
    initInputs()
}
function initPage() {
    renderTranslationLang();
    $.zeroClipboard({
        id: "default",
        events: {
            load: function(client) {},
            mousedown: function(client) {
                var $sender = $(client.currentTarget);
                var senderId = $sender.attr("id");
                var copyText = "";
                if (senderId === "btnCopyAll" || senderId === "btnCopyDelivered" || senderId === "btnCopyOther") {
                    var $tbd = null;
                    if (senderId === "btnCopyAll") {
                        $tbd = $("#tbResult>tbody")
                    } else {
                        if (senderId === "btnCopyDelivered") {
                            $tbd = $("#tbResult>tbody[packstate=3]")
                        } else {
                            if (senderId === "btnCopyOther") {
                                $tbd = $("#tbResult>tbody[packstate][packstate!=3]")
                            }
                        }
                    }
                    $tbd.each(function() {
                        copyText += $(this).find(".s-num b").text() + "\r\n";
                        $(this).find("tr.tr-details dd, tr.tr-details dt").each(function() {
                            copyText += ($(this).text() + "\r\n")
                        });
                        copyText += "======================================\r\n"
                    })
                } else {
                    if (senderId === "btnCopyAll_Summary" || senderId === "btnCopyDelivered_Summary" || senderId === "btnCopyOther_Summary") {
                        if (senderId === "btnCopyAll_Summary") {
                            $tbd = $("#tbResult>tbody")
                        } else {
                            if (senderId === "btnCopyDelivered_Summary") {
                                $tbd = $("#tbResult>tbody[packstate=3]")
                            } else {
                                if (senderId === "btnCopyOther_Summary") {
                                    $tbd = $("#tbResult>tbody[packstate][packstate!=3]")
                                }
                            }
                        }
                        $tbd.each(function() {
                            copyText += $(".s-num B", this).text() + "\t";
                            var $from = $(".s-from P", this).clone();
                            $from.find(".ac-change").remove();
                            copyText += $from.text() + "\t";
                            var $to = $(".s-to P", this).clone();
                            $to.find(".ac-change").remove();
                            copyText += $to.text() + "\t";
                            copyText += $(".s-lastEvent", this).text() + "\t" + $(".s-packState DIV:eq(0)", this).text() + "\t" + $(".s-packState DIV:eq(1)", this).text() + "\r\n"
                        })
                    } else {
                        var $tbd = $sender.closest("tbody");
                        copyText += $tbd.find(".s-num b").text() + "\r\n";
                        $tbd.find("tr.tr-details dd, tr.tr-details dt").each(function() {
                            copyText += ($(this).text() + "\r\n")
                        });
                        copyText += "======================================\r\n"
                    }
                }
                if (copyText === "") {
                    $.zeroClipboard("default").setText("");
                    $.alert(LC.DialogMessage, LC.Msg_NotEventsCopyed, "warning")
                } else {
                    $.zeroClipboard("default").setText(copyText + "Power by www.17track.net")
                }
            },
            complete: function(client, text) {
                if (text === "") {
                    return
                }
                $.alert(LC.DialogMessage, LC.Msg_EventsCopyed, "correct")
            }
        }
    });
    $(".clip-wrap a").bindZeroClipboard("default");
    $.tips.create({
        tipId: "translationTip"
    });
    $.tips.create({
        tipId: "resultTip"
    });
    $("#tbResult A.help").mouseenter(function() {
        var id = $(this).attr("rel");
        var $target = $("#" + id);
        if ($target.length === 0) {
            return
        }
        var html = '<div class="help-tips">' + $target.html() + "</div>";
        $.tips.get("resultTip").show(this, html)
    }).mouseleave(function(e) {
        e.preventDefault();
        $.tips.get("resultTip").hide()
    });
    var resultBoxTop = $(".result-box").offset().top;
    $(window).scroll(function() {
        var offsetTop = $(window).scrollTop();
        var resultBoxHeight = $(".result-box").outerHeight();
        if (offsetTop > resultBoxTop) {
            $("#goResultTop").show().css({
                top: Math.min(offsetTop - resultBoxTop, resultBoxHeight) + 100 + "px"
            })
        } else {
            $("#goResultTop").hide().css({
                top: "0px"
            })
        }
    });
    $("#goResultTop").click(function() {
        var $body = (window.opera) ? (document.compatMode == "CSS1Compat" ? $("html") : $("body")) : $("html,body");
        var $box = $(".result-box");
        var offsetTop = $(window).scrollTop() - resultBoxTop;
        $body.animate({
            scrollTop: 0
        },
        0)
    }).hide();
    initInputs();
    if (getCookieNums("hideTip", false) === true) {
        $("#trackTipsWrap").hide()
    }
    $(".action-bar .popup").find(">A").click(function(e) {
        e.preventDefault();
        $(this).addClass("open").next(".popup-container").show()
    }).end().find(".close-button").click(function(e) {
        e.preventDefault();
        var $pop = $(this).closest(".popup");
        $(">A", $pop).removeClass("open");
        $(">.popup-container", $pop).hide()
    })
}
function closeTrackTips() {
    setCookieNums("hideTip", true);
    $("#trackTipsWrap").hide()
};