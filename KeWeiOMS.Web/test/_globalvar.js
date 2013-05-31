var LANG = [{
    a: 0,
    b: "",
    c: "不翻译",
    d: "不翻譯",
    e: "Not Translated"
},
{
    a: 1,
    b: "ar",
    c: "阿拉伯语",
    d: "阿拉伯語",
    e: "Arabic"
},
{
    a: 2,
    b: "bg",
    c: "保加利亚语",
    d: "保加利亞語",
    e: "Bulgarian"
},
{
    a: 3,
    b: "ca",
    c: "加泰罗尼亚语",
    d: "加泰羅尼亞語",
    e: "Catalan"
},
{
    a: 4,
    b: "zh-CHS",
    c: "中文简体",
    d: "中文簡體",
    e: "Chinese (Simplified)"
},
{
    a: 5,
    b: "zh-CHT",
    c: "中文繁体",
    d: "中文繁體",
    e: "Chinese (Traditional)"
},
{
    a: 6,
    b: "cs",
    c: "捷克语",
    d: "捷克語",
    e: "Czech"
},
{
    a: 7,
    b: "da",
    c: "丹麦语",
    d: "丹麥語",
    e: "Danish"
},
{
    a: 8,
    b: "nl",
    c: "荷兰语",
    d: "荷蘭語",
    e: "Dutch"
},
{
    a: 9,
    b: "en",
    c: "英语",
    d: "英語",
    e: "English"
},
{
    a: 10,
    b: "et",
    c: "爱沙尼亚语",
    d: "愛沙尼亞語",
    e: "Estonian"
},
{
    a: 11,
    b: "fi",
    c: "芬兰语",
    d: "芬蘭語",
    e: "Finnish"
},
{
    a: 12,
    b: "fr",
    c: "法语",
    d: "法語",
    e: "French"
},
{
    a: 13,
    b: "de",
    c: "德语",
    d: "德語",
    e: "German"
},
{
    a: 14,
    b: "el",
    c: "希腊语",
    d: "希臘語",
    e: "Greek"
},
{
    a: 15,
    b: "ht",
    c: "海地克里奥尔语",
    d: "海地克里奧爾語",
    e: "Haitian Creole"
},
{
    a: 16,
    b: "he",
    c: "希伯来语",
    d: "希伯來語",
    e: "Hebrew"
},
{
    a: 17,
    b: "hi",
    c: "印地语",
    d: "印地語",
    e: "Hindi"
},
{
    a: 18,
    b: "hu",
    c: "匈牙利语",
    d: "匈牙利語",
    e: "Hungarian"
},
{
    a: 19,
    b: "id",
    c: "印度尼西亚语",
    d: "印度尼西亞語",
    e: "Indonesian"
},
{
    a: 20,
    b: "it",
    c: "意大利语",
    d: "意大利語",
    e: "Italian"
},
{
    a: 21,
    b: "ja",
    c: "日语",
    d: "日語",
    e: "Japanese"
},
{
    a: 22,
    b: "ko",
    c: "韩语",
    d: "韓語",
    e: "Korean"
},
{
    a: 23,
    b: "lv",
    c: "拉脱维亚语",
    d: "拉脫維亞語",
    e: "Latvian"
},
{
    a: 24,
    b: "lt",
    c: "立陶宛语",
    d: "立陶宛語",
    e: "Lithuanian"
},
{
    a: 25,
    b: "no",
    c: "挪威语",
    d: "挪威語",
    e: "Norwegian"
},
{
    a: 26,
    b: "pl",
    c: "波兰语",
    d: "波蘭語",
    e: "Polish"
},
{
    a: 27,
    b: "pt",
    c: "葡萄牙语",
    d: "葡萄牙語",
    e: "Portuguese"
},
{
    a: 28,
    b: "ro",
    c: "罗马尼亚语",
    d: "羅馬尼亞語",
    e: "Romanian"
},
{
    a: 29,
    b: "ru",
    c: "俄语",
    d: "俄語",
    e: "Russian"
},
{
    a: 30,
    b: "sk",
    c: "斯洛伐克语",
    d: "斯洛伐克語",
    e: "Slovak"
},
{
    a: 31,
    b: "sl",
    c: "斯洛文尼亚语",
    d: "斯洛文尼亞語",
    e: "Slovenian"
},
{
    a: 32,
    b: "es",
    c: "西班牙语",
    d: "西班牙語",
    e: "Spanish"
},
{
    a: 33,
    b: "sv",
    c: "瑞典语",
    d: "瑞典語",
    e: "Swedish"
},
{
    a: 34,
    b: "th",
    c: "泰语",
    d: "泰語",
    e: "Thai"
},
{
    a: 35,
    b: "tr",
    c: "土耳其语",
    d: "土耳其語",
    e: "Turkish"
},
{
    a: 36,
    b: "uk",
    c: "乌克兰语",
    d: "烏克蘭語",
    e: "Ukrainian"
},
{
    a: 37,
    b: "vi",
    c: "越南语",
    d: "越南語",
    e: "Vietnamese"
}];
var REST_SERVERS = ["http://s1.17track.net"];
var RULE_EXPRESS = [{
    a: 100001,
    b: "\\d{10}",
    c: ["*** *** *** *"]
},
{
    a: 100002,
    b: "(1Z[A-Z|0-9]{9}\\d{7})|(\\d{12})|(T\\d{10})|(\\d{9})",
    c: ["1Z !!! !!! !!! *** *** *", "T *** *** *** *", "*** *** *** ***", "*** *** ***"]
},
{
    a: 100003,
    b: "\\d{12}",
    c: ["*** *** *** ***"]
},
{
    a: 100004,
    b: "(GD\\d{9})|(\\d{9})",
    c: ["GD *** *** *** WW", "*** *** ***"]
},
{
    a: 190002,
    b: "[A|a]\\d{15}",
    c: ["A *** *** *** *** ***"]
},
{
    a: 190003,
    b: "(ATC\\d{10})|([A-Z]{2}\\d{6}[A-Z|0-9]{3})",
    c: ["ATC *** *** *** *", "## *** *** !!!"]
},
{
    a: 190004,
    b: "[A-Z]{3}\\d{6}[A-Z]{1}[A-Z|0-9]{2}\\d{6}",
    c: ["### *** *** #!! *** ***"]
},
{
    a: 190005,
    b: "[A-Z]{2}\\d{6}[A-Z|0-9]{3}",
    c: ["## *** *** !!!"]
}];
var EXPRESS_ENUM = [{
    a: 0,
    b: "",
    c: "无法识别",
    d: "無法識別",
    e: "Unknown"
},
{
    a: 100001,
    b: "http://www.dhl.com",
    c: "DHL",
    d: "DHL",
    e: "DHL"
},
{
    a: 100002,
    b: "http://www.ups.com",
    c: "UPS",
    d: "UPS",
    e: "UPS"
},
{
    a: 100003,
    b: "http://www.fedex.com",
    c: "Fedex",
    d: "Fedex",
    e: "Fedex"
},
{
    a: 100004,
    b: "http://www.tnt.com",
    c: "TNT",
    d: "TNT",
    e: "TNT"
},
{
    a: 190002,
    b: "http://www.flytexpress.com",
    c: "飞特物流",
    d: "飛特物流",
    e: "FLYT"
},
{
    a: 190003,
    b: "http://www.hh-exp.com",
    c: "华翰物流",
    d: "華翰物流",
    e: "HH-EXP"
},
{
    a: 190004,
    b: "http://www.chukou1.com",
    c: "出口易",
    d: "出口易",
    e: "CK1"
},
{
    a: 190005,
    b: "http://www.btdair.com",
    c: "宝通达",
    d: "寶通達",
    e: "BTD"
}];
var POST_ENUM = [{
    a: 0,
    b: "",
    c: "无法识别",
    d: "無法識別",
    e: "Unknown"
},
{
    a: 1011,
    b: "http://www.posten.ax/tracktracesearch.con?iPage=165&iLan=1",
    c: "奥兰群岛[芬兰]",
    d: "奧蘭群島[芬蘭]",
    e: "Aland Island[FI]"
},
{
    a: 1021,
    b: "http://afghanpost.gov.af/",
    c: "阿富汗",
    d: "阿富汗",
    e: "Afghanistan"
},
{
    a: 1031,
    b: "http://80.78.70.6/webtracking/",
    c: "阿尔巴尼亚",
    d: "阿爾巴尼亞",
    e: "Albania"
},
{
    a: 1041,
    b: "http://www.poste.dz/",
    c: "阿尔及利亚",
    d: "阿爾及利亞",
    e: "Algeria"
},
{
    a: 1051,
    b: "",
    c: "安道尔",
    d: "安道爾",
    e: "Andorra"
},
{
    a: 1061,
    b: "http://www.correiosdeangola.ao/",
    c: "安哥拉",
    d: "安哥拉爾",
    e: "Angola"
},
{
    a: 1071,
    b: "http://www.aps.ai/",
    c: "安圭拉[英国]",
    d: "安圭拉[英國]",
    e: "Anguilla[GB]"
},
{
    a: 1081,
    b: "",
    c: "南极洲",
    d: "南極洲",
    e: "Antarctica"
},
{
    a: 1091,
    b: "http://www.postoffice.gov.ac/",
    c: "阿森松岛[英国]",
    d: "阿森松島[英國]",
    e: "Ascension Island[GB]"
},
{
    a: 1101,
    b: "",
    c: "安提瓜和巴布达",
    d: "安提瓜和巴布達",
    e: "Antigua And Barbuda"
},
{
    a: 1111,
    b: "http://200.26.194.208:8085/ipswt/",
    c: "安的列斯群岛[荷兰]",
    d: "安的列斯群島[荷蘭]",
    e: "Netherlands Antilles[NL]"
},
{
    a: 1121,
    b: "http://www.correoargentino.com.ar/seguimiento_envios/oidn",
    c: "阿根廷",
    d: "阿根廷",
    e: "Argentina"
},
{
    a: 1131,
    b: "http://www.haypost.am/view-lang-eng-page-212.html",
    c: "亚美尼亚",
    d: "亞美尼亞",
    e: "Armenia"
},
{
    a: 1141,
    b: "http://ipspostaruba.no-ip.biz/ipswt/",
    c: "阿鲁巴[荷兰]",
    d: "阿魯巴[荷蘭]",
    e: "Aruba[NL]"
},
{
    a: 1151,
    b: "http://auspost.com.au/track/",
    c: "澳大利亚",
    d: "澳大利亞",
    e: "Australia"
},
{
    a: 1161,
    b: "https://nachschau.post.at/?lang=en",
    c: "奥地利",
    d: "奧地利",
    e: "Austria"
},
{
    a: 1171,
    b: "http://www.azems.az/en/index_en.htm",
    c: "阿塞拜疆",
    d: "阿塞拜疆",
    e: "Azerbaijan"
},
{
    a: 1181,
    b: "",
    c: "亚速尔和马德拉[葡萄牙]",
    d: "亞速爾和馬德拉[葡萄牙]",
    e: "Azores and Madeira[PT]"
},
{
    a: 2011,
    b: "",
    c: "巴哈马",
    d: "巴哈馬",
    e: "Bahamas"
},
{
    a: 2021,
    b: "http://www.bahrain.bh/wps/portal/!ut/p/c5/04_SB8K8xLLM9MSSzPy8xBz9CP0os3g3E0cj0wAXIwMDA0NDAyNzM_cAlyA3AwNvA6B8pFm8s7ujh4m5j4GBhYubhYGRk6mZZ6CBi4GBpykB3eEg-_DrB8kb4ACOBvp-Hvm5qfrBiUX6BbkRBlkmjooAzKgHmA!!/dl3/d3/L0lHSkovd0RNQU5rQUVnQSEhL1lCZncvZW4!/",
    c: "巴林",
    d: "巴林",
    e: "Bahrain"
},
{
    a: 2031,
    b: "http://www.bangladeshpost.gov.bd/track_RP.asp",
    c: "孟加拉国",
    d: "孟加拉國",
    e: "Bangladesh"
},
{
    a: 2033,
    b: "http://www.bangladeshpost.gov.bd/track.asp",
    c: "孟加拉国",
    d: "孟加拉國",
    e: "Bangladesh"
},
{
    a: 2041,
    b: "http://www.bps.gov.bb/",
    c: "巴巴多斯",
    d: "巴巴多斯",
    e: "Barbados"
},
{
    a: 2051,
    b: "http://search.belpost.by/",
    c: "白俄罗斯",
    d: "白俄羅斯",
    e: "Belarus"
},
{
    a: 2061,
    b: "http://track.bpost.be/etr/light/showSearchPage.do?oss_language=EN",
    c: "比利时",
    d: "比利時",
    e: "Belgium"
},
{
    a: 2071,
    b: "http://www.belizepostalservice.gov.bz",
    c: "伯利兹",
    d: "伯利茲",
    e: "Belize"
},
{
    a: 2081,
    b: "http://www.laposte.bj/",
    c: "贝宁",
    d: "貝寧",
    e: "Benin"
},
{
    a: 2091,
    b: "http://www.bpo.bm/track_and_trace.aspx",
    c: "百慕达[英国]",
    d: "百慕達[英國]",
    e: "Bermuda[GB]"
},
{
    a: 2101,
    b: "http://www.bhutanpost.com.bt/index.php?id=124",
    c: "不丹",
    d: "不丹",
    e: "Bhutan"
},
{
    a: 2111,
    b: "http://www.correosbolivia.com/correos/buscar.php",
    c: "玻利维亚",
    d: "玻利維亞",
    e: "Bolivia"
},
{
    a: 2121,
    b: "http://www.posta.ba/trace/Default1E.asp",
    c: "波黑",
    d: "波黑",
    e: "Bosnia And Herzegovina"
},
{
    a: 2131,
    b: "http://tracking.botswanapost.co.bw/tracking/IPSWeb_submit.htm",
    c: "博茨瓦纳",
    d: "博茨瓦納",
    e: "Botswana"
},
{
    a: 2141,
    b: "",
    c: "布维岛[挪威]",
    d: "布維島[挪威]",
    e: "Bouvet Island[NO]"
},
{
    a: 2151,
    b: "http://www.correios.com.br/servicos/rastreamento/rastreamento.cfm?ididioma=2",
    c: "巴西",
    d: "巴西",
    e: "Brazil"
},
{
    a: 2161,
    b: "http://www.post.gov.bn",
    c: "文莱",
    d: "文萊",
    e: "Brunei"
},
{
    a: 2171,
    b: "http://www.bgpost.bg/IPSWebTracking/",
    c: "保加利亚",
    d: "保加利亞",
    e: "Bulgaria"
},
{
    a: 2181,
    b: "http://www.mpt.bf",
    c: "布基纳法索",
    d: "布基納法索",
    e: "Burkina Faso"
},
{
    a: 2191,
    b: "http://www.poste.bi/",
    c: "布隆迪",
    d: "布隆迪",
    e: "Burundi"
},
{
    a: 3011,
    b: "http://intmail.183.com.cn/itemtrace_en.jsp",
    c: "中国",
    d: "中國",
    e: "China"
},
{
    a: 3013,
    b: "http://www.11183.com.cn/english.html",
    c: "中国",
    d: "中國",
    e: "China"
},
{
    a: 3021,
    b: "http://119.15.82.82/www/?page_id=2",
    c: "柬埔寨",
    d: "柬埔寨",
    e: "Cambodia"
},
{
    a: 3031,
    b: "http://campost.cm/camp/index.php?lang=en%20",
    c: "喀麦隆",
    d: "喀麥隆",
    e: "Cameroon"
},
{
    a: 3041,
    b: "http://www.canadapost.ca/cpo/mc/default.jsf?LOCALE=en",
    c: "加拿大",
    d: "加拿大",
    e: "Canada"
},
{
    a: 3051,
    b: "",
    c: "加那利群岛[西班牙]",
    d: "加那利群島[西班牙]",
    e: "Canary Islands[ES]"
},
{
    a: 3061,
    b: "http://www.correios.cv/",
    c: "佛得角",
    d: "佛得角",
    e: "Cape Verde"
},
{
    a: 3071,
    b: "",
    c: "开曼群岛[英国]",
    d: "開曼群島[英國]",
    e: "Cayman Islands[GB]"
},
{
    a: 3081,
    b: "",
    c: "中非共和国",
    d: "中非共和國",
    e: "Central African Republic"
},
{
    a: 3101,
    b: "http://courier.correos.cl/seguimientoweb/Resumen.aspx",
    c: "智利",
    d: "智利",
    e: "Chile"
},
{
    a: 3111,
    b: "",
    c: "圣诞岛[澳大利亚]",
    d: "聖誕島[澳大利亞]",
    e: "Christmas Island[AU]"
},
{
    a: 3121,
    b: "http://www.csuivi.courrier.laposte.fr/suivi/index",
    c: "科特迪瓦",
    d: "科特迪瓦",
    e: "Côte d'Ivoire"
},
{
    a: 3123,
    b: "http://www.chronopost.fr/transport-express/livraison-colis/homepage",
    c: "科特迪瓦",
    d: "科特迪瓦",
    e: "Côte d'Ivoire"
},
{
    a: 3131,
    b: "http://190.27.245.138/SIPOST/Consulta_Web/default.aspx",
    c: "哥伦比亚",
    d: "哥倫比亞",
    e: "Colombia"
},
{
    a: 3141,
    b: "http://www.snpt.km/",
    c: "科摩罗",
    d: "科摩羅",
    e: "Comoros"
},
{
    a: 3151,
    b: "",
    c: "刚果共和国",
    d: "剛果共和国",
    e: "Republic of Congo"
},
{
    a: 3161,
    b: "",
    c: "刚果民主共和国",
    d: "剛果民主共和國",
    e: "Democratic Republic of Congo"
},
{
    a: 3171,
    b: "",
    c: "库克群岛[新西兰]",
    d: "庫克群島[新西蘭]",
    e: "Cook Islands[NZ]"
},
{
    a: 3181,
    b: "https://www.correos.go.cr/rastreo/RastreoEnvios.html",
    c: "哥斯达黎加",
    d: "哥斯達黎加",
    e: "Costa Rica"
},
{
    a: 3191,
    b: "http://ips.posta.hr/",
    c: "克罗地亚",
    d: "克羅地亞",
    e: "Croatia"
},
{
    a: 3201,
    b: "",
    c: "古巴",
    d: "古巴",
    e: "Cuba"
},
{
    a: 3211,
    b: "http://ips.cypruspost.gov.cy/ipswebtrack/",
    c: "塞浦路斯",
    d: "塞浦路斯",
    e: "Cyprus"
},
{
    a: 3221,
    b: "http://www.ceskaposta.cz/en/nastroje/sledovani-zasilky.php",
    c: "捷克",
    d: "捷克",
    e: "Czech"
},
{
    a: 3231,
    b: "",
    c: "乍得",
    d: "乍得",
    e: "Chad"
},
{
    a: 3241,
    b: "",
    c: "科科斯基林群岛[澳大利亚]",
    d: "科科斯基林群島[澳大利亞]",
    e: "Cocos (Keeling) Islands[AU]"
},
{
    a: 4011,
    b: "http://www.postdanmark.dk/en/tracktrace/Pages/home.aspx",
    c: "丹麦",
    d: "丹麥",
    e: "Denmark"
},
{
    a: 4021,
    b: "http://www.laposte.dj/",
    c: "吉布提",
    d: "吉布提",
    e: "Djibouti"
},
{
    a: 4031,
    b: "",
    c: "多米尼克",
    d: "多米尼克",
    e: "Dominique"
},
{
    a: 4041,
    b: "http://www.inposdom.gob.do/",
    c: "多米尼加",
    d: "多米尼加",
    e: "Dominican"
},
{
    a: 5011,
    b: "http://ipsweb.correosdelecuador.com.ec/ipswebtracking/",
    c: "厄瓜多尔",
    d: "厄瓜多爾",
    e: "Ecuador"
},
{
    a: 5021,
    b: "http://217.52.211.9/ipswebtracking/",
    c: "埃及",
    d: "埃及",
    e: "Egypt"
},
{
    a: 5031,
    b: "http://www.epg.ae/eponline/faces/eservice/mailtrack.xhtml",
    c: "阿联酋",
    d: "阿聯酋",
    e: "United Arab Emirates"
},
{
    a: 5041,
    b: "https://veeb.post.ee/eplisweb/main",
    c: "爱沙尼亚",
    d: "愛沙尼亞",
    e: "Estonia"
},
{
    a: 5051,
    b: "http://213.55.81.131/ipswebtrack/",
    c: "埃塞俄比亚",
    d: "埃塞俄比亞",
    e: "Ethiopia"
},
{
    a: 5061,
    b: "http://196.200.104.139/ipsweb/",
    c: "厄立特里亚",
    d: "厄立特里亞",
    e: "Eritrea"
},
{
    a: 5071,
    b: "",
    c: "赤道几内亚",
    d: "赤道幾內亞",
    e: "Equatorial Guinea"
},
{
    a: 5081,
    b: "",
    c: "东帝汶",
    d: "東帝汶",
    e: "East Timor"
},
{
    a: 6011,
    b: "",
    c: "福克兰群岛[英国]",
    d: "福克蘭群島[英國]",
    e: "Falkland Islands[GB]"
},
{
    a: 6021,
    b: "http://leita.posta.fo/",
    c: "法罗群岛[丹麦]",
    d: "法羅群島[丹麥]",
    e: "Faroe Islands[DK]"
},
{
    a: 6031,
    b: "http://www.postfiji.com.fj/pages.cfm/services/track-trace/",
    c: "斐济",
    d: "斐濟",
    e: "Fiji"
},
{
    a: 6041,
    b: "http://www.posti.fi/itemtracking/posti/search_by_shipment_id",
    c: "芬兰",
    d: "芬蘭",
    e: "Finland"
},
{
    a: 6051,
    b: "http://www.csuivi.courrier.laposte.fr/suivi/index",
    c: "法国",
    d: "法國",
    e: "France"
},
{
    a: 6053,
    b: "http://www.chronopost.fr/transport-express/livraison-colis/homepage",
    c: "法国",
    d: "法國",
    e: "France"
},
{
    a: 6061,
    b: "",
    c: "大都会[法国]",
    d: "大都會[法國]",
    e: "Metropolitan[FR]"
},
{
    a: 6071,
    b: "",
    c: "圭亚那[法国]",
    d: "圭亞那[法國]",
    e: "Guiana[FR]"
},
{
    a: 7011,
    b: "",
    c: "加蓬",
    d: "加蓬",
    e: "Gabon"
},
{
    a: 7021,
    b: "http://www.gampost.gm/",
    c: "冈比亚",
    d: "岡比亞",
    e: "Gambia"
},
{
    a: 7031,
    b: "http://georgianpost.ge/?lng=eng",
    c: "格鲁吉亚",
    d: "格魯吉亞",
    e: "Georgia"
},
{
    a: 7041,
    b: "http://nolp.dhl.de/nextt-online-public/set_identcodes.do?lang=en",
    c: "德国",
    d: "德國",
    e: "Germany"
},
{
    a: 7051,
    b: "http://197.253.65.52/ipswebtracking/",
    c: "加纳",
    d: "加納",
    e: "Ghana"
},
{
    a: 7061,
    b: "http://www.gibraltar.gov.gi/",
    c: "直布罗陀[英国]",
    d: "直布羅陀[英國]",
    e: "Gibraltar[GB]"
},
{
    a: 7071,
    b: "http://212.205.82.71/trackandtrace/Default.asp",
    c: "希腊",
    d: "希臘",
    e: "Greece"
},
{
    a: 7073,
    b: "http://www.elta-courier.gr/en/search.asp",
    c: "希腊",
    d: "希臘",
    e: "Greece"
},
{
    a: 7081,
    b: "http://www.post.gl/da-DK/Privat/Info/Servicer/TrackTrace/Sider/Forside.aspx",
    c: "格陵兰[丹麦]",
    d: "格陵蘭[丹麥]",
    e: "Greenland[DK]"
},
{
    a: 7091,
    b: "http://www.grenadapostal.com/",
    c: "格林纳达[英国]",
    d: "格林納達[英國]",
    e: "Grenada[GB]"
},
{
    a: 7101,
    b: "",
    c: "瓜德罗普岛[法国]",
    d: "瓜德羅普島[法國]",
    e: "Guadeloupe[FR]"
},
{
    a: 7111,
    b: "",
    c: "关岛[美国]",
    d: "關島[美國]",
    e: "Guam[US]"
},
{
    a: 7121,
    b: "http://www.elcorreo.com.gt/ems/formulario.php",
    c: "危地马拉",
    d: "危地馬拉",
    e: "Guatemala"
},
{
    a: 7131,
    b: "",
    c: "几内亚共和国",
    d: "幾內亞共和國",
    e: "Republic Of Guinea"
},
{
    a: 7141,
    b: "",
    c: "圭亚那",
    d: "圭亞那",
    e: "Guyana"
},
{
    a: 7151,
    b: "http://www.guernseypost.com",
    c: "根西岛[英国]",
    d: "根西島[英國]",
    e: "Guernsey[GB]"
},
{
    a: 7161,
    b: "http://correiosguine.site11.com",
    c: "几内亚比绍",
    d: "幾內亞比紹",
    e: "Guinea Bissau"
},
{
    a: 8011,
    b: "http://www.hongkongpost.com/eng/tracking/index.htm",
    c: "香港",
    d: "香港",
    e: "Hong Kong"
},
{
    a: 8021,
    b: "http://postehaiti.gouv.ht/",
    c: "海地",
    d: "海地",
    e: "Haiti"
},
{
    a: 8031,
    b: "",
    c: "夏威夷[美国]",
    d: "夏威夷[美國]",
    e: "Hawaii[US]"
},
{
    a: 8041,
    b: "http://www.honducor.gob.hn/",
    c: "洪都拉斯",
    d: "洪都拉斯",
    e: "Honduras"
},
{
    a: 8051,
    b: "http://posta.hu/tracking",
    c: "匈牙利",
    d: "匈牙利",
    e: "Hungary"
},
{
    a: 9011,
    b: "http://www.postur.is/en/desktopdefault.aspx/tabid-516/",
    c: "冰岛",
    d: "冰島",
    e: "Iceland"
},
{
    a: 9021,
    b: "http://ipsweb.ptcmysore.gov.in/ipswebtracking/",
    c: "印度",
    d: "印度",
    e: "India"
},
{
    a: 9031,
    b: "http://ems.posindonesia.co.id/emsview1.php",
    c: "印度尼西亚",
    d: "印度尼西亞",
    e: "Indonesia"
},
{
    a: 9041,
    b: "http://tntsearch.post.ir/searchPage.aspx",
    c: "伊朗",
    d: "伊朗",
    e: "Iran"
},
{
    a: 9051,
    b: "http://track.anpost.ie/TrackOne.aspx",
    c: "爱尔兰",
    d: "愛爾蘭",
    e: "Ireland"
},
{
    a: 9061,
    b: "http://www.israelpost.co.il/itemtrace.nsf/mainsearch?OpenForm&L=EN",
    c: "以色列",
    d: "以色列",
    e: "Israel"
},
{
    a: 9071,
    b: "http://www.poste.it/online/dovequando/home.do",
    c: "意大利",
    d: "意大利",
    e: "Italy"
},
{
    a: 9081,
    b: "http://www.iraqipost.net/",
    c: "伊拉克",
    d: "伊拉克",
    e: "Iraq"
},
{
    a: 9091,
    b: "http://www.gov.im",
    c: "马恩岛[英国]",
    d: "馬恩島[英國]",
    e: "Isle Of Man[GB]"
},
{
    a: 10011,
    b: "http://www.jamaicapost.gov.jm/",
    c: "牙买加",
    d: "牙買加",
    e: "Jamaica"
},
{
    a: 10021,
    b: "http://tracking.post.japanpost.jp/service/numberSearch.do?locale=en&searchKind=S004",
    c: "日本",
    d: "日本",
    e: "Japan"
},
{
    a: 10031,
    b: "http://194.165.151.254/ipswebtrack/",
    c: "约旦",
    d: "約旦",
    e: "Jordan"
},
{
    a: 11011,
    b: "http://online.kazpost.kz/en/Tracking/Index",
    c: "哈萨克斯坦",
    d: "哈薩克斯坦",
    e: "Kazakhstan"
},
{
    a: 11021,
    b: "http://196.202.202.203/ipswebtrack/",
    c: "肯尼亚",
    d: "肯尼亞",
    e: "Kenya"
},
{
    a: 11031,
    b: "http://www.parcelforce.com",
    c: "英国",
    d: "英國",
    e: "United Kingdom"
},
{
    a: 11041,
    b: "",
    c: "基里巴斯",
    d: "基里巴斯",
    e: "Kiribati"
},
{
    a: 11051,
    b: "http://service.epost.go.kr/iservice/ems/ems_eng.jsp",
    c: "韩国",
    d: "韓國",
    e: "Korea"
},
{
    a: 11061,
    b: "",
    c: "朝鲜",
    d: "朝鮮",
    e: "Democratic People's Republic of Kore"
},
{
    a: 11071,
    b: "http://www.ptkonline.com/ptk/eng/",
    c: "科索沃",
    d: "科索沃",
    e: "Kosovo"
},
{
    a: 11081,
    b: "",
    c: "科威特",
    d: "科威特",
    e: "Kuwait"
},
{
    a: 11091,
    b: "http://kyrgyzpost.kg/",
    c: "吉尔吉斯斯坦",
    d: "吉爾吉斯斯坦",
    e: "Kirghizstan"
},
{
    a: 12011,
    b: "",
    c: "老挝",
    d: "老撾",
    e: "Laos"
},
{
    a: 12021,
    b: "http://www.manspasts.lv/webtracking_test/?lang=en",
    c: "拉脱维亚",
    d: "拉脫維亞",
    e: "Latvia"
},
{
    a: 12031,
    b: "http://www.libanpost.com.lb/TrackTrace/tabid/59/Default.aspx",
    c: "黎巴嫩",
    d: "黎巴嫩",
    e: "Lebanon"
},
{
    a: 12041,
    b: "http://lesothopost.org.ls/",
    c: "莱索托",
    d: "萊索托",
    e: "Lesotho"
},
{
    a: 12051,
    b: "http://www.mopt.gov.lr/",
    c: "利比里亚",
    d: "利比里亞",
    e: "Liberia"
},
{
    a: 12061,
    b: "http://libyapost.ly/en/",
    c: "利比亚",
    d: "利比亞",
    e: "Libya"
},
{
    a: 12071,
    b: "http://www.post.li/privatkunden/post-dienstleistungen/online-postschalter/post-verfolgen/sendungsverfolgung.html",
    c: "列支敦士登",
    d: "列支敦士登",
    e: "Liechtenstein"
},
{
    a: 12081,
    b: "http://www.post.lt/en/help/parcel-search",
    c: "立陶宛",
    d: "立陶宛",
    e: "Lithuania"
},
{
    a: 12091,
    b: "",
    c: "圣卢西亚",
    d: "聖盧西亞",
    e: "St. Lucia"
},
{
    a: 12101,
    b: "http://www.trackandtrace.lu/homepage.htm?locale=en_GB",
    c: "卢森堡",
    d: "盧森堡",
    e: "Luxembourg"
},
{
    a: 13011,
    b: "http://www.macaupost.gov.mo/contents/MailTrack.aspx",
    c: "澳门",
    d: "澳門",
    e: "Macau"
},
{
    a: 13012,
    b: "http://ems.macaupost.gov.mo/contents/EMStrack.aspx",
    c: "澳门",
    d: "澳門",
    e: "Macau"
},
{
    a: 13021,
    b: "http://195.26.134.46/IPSWebTracking/",
    c: "马其顿",
    d: "馬其頓",
    e: "Macedonia"
},
{
    a: 13031,
    b: "http://www.paositra.mg/",
    c: "马达加斯加",
    d: "馬達加斯加",
    e: "Madagascar"
},
{
    a: 13041,
    b: "http://www.malawiposts.com/",
    c: "马拉维",
    d: "馬拉維",
    e: "Malawi"
},
{
    a: 13051,
    b: "http://www.pos.com.my/pos/appl/service/sub_registered_mail.asp",
    c: "马来西亚",
    d: "馬來西亞",
    e: "Malaysia"
},
{
    a: 13052,
    b: "http://www.poslaju.com.my/pos_tracking.aspx",
    c: "马来西亚",
    d: "馬來西亞",
    e: "Malaysia"
},
{
    a: 13061,
    b: "https://www.maldivespost.com/store/",
    c: "马尔代夫",
    d: "馬爾代夫",
    e: "Maldives"
},
{
    a: 13071,
    b: "http://mali.viky.net/",
    c: "马里",
    d: "馬里",
    e: "Mali"
},
{
    a: 13081,
    b: "http://trackandtrace.maltapost.com/",
    c: "马耳他",
    d: "馬耳他",
    e: "Malta"
},
{
    a: 13091,
    b: "",
    c: "马里亚纳群岛[美国]",
    d: "馬里亞納群島[美國]",
    e: "Mariana Islands[US]"
},
{
    a: 13101,
    b: "",
    c: "马绍尔群岛",
    d: "馬紹爾群島",
    e: "Marshall"
},
{
    a: 13111,
    b: "",
    c: "马提尼克[法国]",
    d: "馬提尼克[法國]",
    e: "Martinique[FR]"
},
{
    a: 13121,
    b: "https://www.mauripost.mr/",
    c: "毛里塔尼亚",
    d: "毛里塔尼亞",
    e: "Mauritania"
},
{
    a: 13131,
    b: "http://www.mauritiuspost.biz/",
    c: "毛里求斯",
    d: "毛里求斯",
    e: "Mauritius"
},
{
    a: 13141,
    b: "http://www.correosdemexico.gob.mx/English/Paginas/track.aspx",
    c: "墨西哥",
    d: "墨西哥",
    e: "Mexico"
},
{
    a: 13151,
    b: "",
    c: "密克罗尼西亚",
    d: "密克羅尼西亞",
    e: "Micronesia"
},
{
    a: 13161,
    b: "http://www.posta.md:8082/",
    c: "摩尔多瓦",
    d: "摩爾多瓦",
    e: "Moldova"
},
{
    a: 13171,
    b: "http://www.lapostemonaco.mc/",
    c: "摩纳哥",
    d: "摩納哥",
    e: "Monaco"
},
{
    a: 13181,
    b: "",
    c: "蒙古",
    d: "蒙古",
    e: "Mongolia"
},
{
    a: 13191,
    b: "http://195.66.165.22/posiljkainfo/pretraga.aspx",
    c: "黑山",
    d: "黑山",
    e: "Montenegro"
},
{
    a: 13201,
    b: "",
    c: "蒙特塞拉特[英国]",
    d: "蒙特塞拉特[英國]",
    e: "Montserrat[GB]"
},
{
    a: 13211,
    b: "http://www.ems.ma/Conteneur.aspx?id=6&id1=1",
    c: "摩洛哥",
    d: "摩洛哥",
    e: "Morocco"
},
{
    a: 13221,
    b: "http://www.correios.co.mz/",
    c: "莫桑比克",
    d: "莫桑比克",
    e: "Mozambique"
},
{
    a: 13231,
    b: "http://www.mpt.net.mm/",
    c: "缅甸",
    d: "緬甸",
    e: "Myanmar"
},
{
    a: 13241,
    b: "",
    c: "马约特岛[法国]",
    d: "馬約特島[法國]",
    e: "Mayotte[FR]"
},
{
    a: 14011,
    b: "http://www.nampost.com.na/",
    c: "纳米比亚",
    d: "納米比亞",
    e: "Namibia"
},
{
    a: 14021,
    b: "",
    c: "瑙鲁",
    d: "瑙魯",
    e: "Nauru"
},
{
    a: 14031,
    b: "",
    c: "尼泊尔",
    d: "尼泊爾",
    e: "Nepal"
},
{
    a: 14041,
    b: "http://www.postnl.nl",
    c: "荷兰",
    d: "荷蘭",
    e: "Netherlands"
},
{
    a: 14051,
    b: "http://webtrack.opt.nc/ipswebtracking/",
    c: "新喀里多尼亚[法国]",
    d: "新喀裡多尼亞[法國]",
    e: "New Caledonia[FR]"
},
{
    a: 14061,
    b: "http://www.nzpost.co.nz/tools/tracking",
    c: "新西兰",
    d: "新西蘭",
    e: "New Zealand"
},
{
    a: 14071,
    b: "http://www.correos.gob.ni/",
    c: "尼加拉瓜",
    d: "尼加拉瓜",
    e: "Nicaragua"
},
{
    a: 14081,
    b: "http://www.posten.no/en/",
    c: "挪威",
    d: "挪威",
    e: "Norway"
},
{
    a: 14091,
    b: "http://www.niger-poste.net/",
    c: "尼日尔",
    d: "尼日爾",
    e: "Niger"
},
{
    a: 14101,
    b: "http://www.emsng.com/Track-Confirm.aspx",
    c: "尼日利亚",
    d: "尼日利亞",
    e: "Nigeria"
},
{
    a: 14111,
    b: "",
    c: "纽埃",
    d: "紐埃",
    e: "Niue"
},
{
    a: 14121,
    b: "http://www.stamps.gov.nf/",
    c: "诺福克岛[澳大利亚]",
    d: "諾福克島[澳大利亞]",
    e: "Norfolk Island[AU]"
},
{
    a: 15011,
    b: "http://85.154.223.131/TrackAndTraceWithCalculator/TrackAndTrace/Parcel.aspx?lan=eng",
    c: "阿曼",
    d: "阿曼",
    e: "Oman"
},
{
    a: 16011,
    b: "http://ep.gov.pk/",
    c: "巴基斯坦",
    d: "巴基斯坦",
    e: "Pakistan"
},
{
    a: 16021,
    b: "http://www.mtit.gov.ps/",
    c: "巴勒斯坦",
    d: "巴勒斯坦",
    e: "Palestine"
},
{
    a: 16031,
    b: "http://201.227.74.84/cotelwt/",
    c: "巴拿马",
    d: "巴拿馬",
    e: "Panama"
},
{
    a: 16041,
    b: "http://www.postpng.com.pg/",
    c: "巴布亚新几内亚",
    d: "巴布亞新幾內亞",
    e: "Papua New Guinea"
},
{
    a: 16051,
    b: "http://www.correoparaguayo.gov.py/",
    c: "巴拉圭",
    d: "巴拉圭",
    e: "Paraguay"
},
{
    a: 16061,
    b: "http://clientes.serpost.com.pe/Web-Original/",
    c: "秘鲁",
    d: "秘魯",
    e: "Peru"
},
{
    a: 16071,
    b: "http://webtrk1.philpost.org/index.asp",
    c: "菲律宾",
    d: "菲律賓",
    e: "Philippines"
},
{
    a: 16081,
    b: "http://sledzenie.poczta-polska.pl/",
    c: "波兰",
    d: "波蘭",
    e: "Poland"
},
{
    a: 16091,
    b: "",
    c: "波利尼西亚[法国]",
    d: "波利尼西亞[法國]",
    e: "Polynesia[FR]"
},
{
    a: 16101,
    b: "http://www.ctt.pt/feapl_2/app/open/tools.jspx?lang=01",
    c: "葡萄牙",
    d: "葡萄牙",
    e: "Portugal"
},
{
    a: 16111,
    b: "https://tools.usps.com/go/TrackConfirmAction_input",
    c: "波多黎各[美国]",
    d: "波多黎各[美國]",
    e: "Puerto Rico[US]"
},
{
    a: 16121,
    b: "",
    c: "皮特凯恩群岛[英国]",
    d: "皮特凱恩群島[英國]",
    e: "Pitcairn Islands[GB]"
},
{
    a: 16131,
    b: "http://www.lapostespm.net/",
    c: "圣皮埃尔和密克隆[法国]",
    d: "聖皮埃爾和密克隆[法國]",
    e: "St. Pierre And Miquelon[FR]"
},
{
    a: 16141,
    b: "",
    c: "帕劳",
    d: "帕勞",
    e: "Palau"
},
{
    a: 17011,
    b: "http://www.qpost.com.qa/",
    c: "卡塔尔",
    d: "卡塔爾",
    e: "Qatar"
},
{
    a: 18011,
    b: "",
    c: "留尼汪岛[法国]",
    d: "留尼汪島[法國]",
    e: "Reunion Island[FR]"
},
{
    a: 18021,
    b: "http://www.posta-romana.ro/en/posta-romana/servicii-online/track-trace.html",
    c: "罗马尼亚",
    d: "羅馬尼亞",
    e: "Romania"
},
{
    a: 18031,
    b: "http://www.russianpost.ru/rp/servise/en/home/postuslug/trackingpo",
    c: "俄罗斯",
    d: "俄羅斯",
    e: "Russia"
},
{
    a: 18033,
    b: "http://emspost.ru/",
    c: "俄罗斯",
    d: "俄羅斯",
    e: "Russia"
},
{
    a: 18041,
    b: "",
    c: "卢旺达",
    d: "盧旺達",
    e: "Rwanda"
},
{
    a: 19011,
    b: "",
    c: "圣克里斯托弗",
    d: "聖克里斯托弗",
    e: "Saint Christopher"
},
{
    a: 19021,
    b: "",
    c: "圣文森特和格林纳丁斯",
    d: "聖文森特和格林納丁斯",
    e: "Saint Vincent And Grenadines"
},
{
    a: 19031,
    b: "",
    c: "萨尔瓦多",
    d: "薩爾瓦多",
    e: "Salvador"
},
{
    a: 19041,
    b: "",
    c: "萨摩亚群岛[美国]",
    d: "薩摩亞群島[美國]",
    e: "Samoa[US]"
},
{
    a: 19051,
    b: "http://www.aasfn.sm",
    c: "圣马力诺",
    d: "聖馬力諾",
    e: "San Marino"
},
{
    a: 19061,
    b: "http://inh.st",
    c: "圣多美和普林西比",
    d: "聖多美和普林西比",
    e: "Sao Tome And Principe"
},
{
    a: 19071,
    b: "http://www.sp.com.sa/English/Saudipost/Pages/default.aspx",
    c: "沙特阿拉伯",
    d: "沙特阿拉伯",
    e: "Saudi Arabia"
},
{
    a: 19081,
    b: "http://www.skncollection.com/xml/rech_courrier.php",
    c: "塞内加尔",
    d: "塞內加爾",
    e: "Senegal"
},
{
    a: 19091,
    b: "http://www.posta.rs/struktura/eng/aplikacije/alati/posiljke.asp",
    c: "塞尔维亚",
    d: "塞爾維亞",
    e: "Serbia"
},
{
    a: 19101,
    b: "http://www.sgisland.gs",
    c: "南桑威奇群岛[英国]",
    d: "南桑威奇群島[英國]",
    e: "South Sandwich Islands[GB]"
},
{
    a: 19111,
    b: "http://www.seychellespost.gov.sc/",
    c: "塞舌尔",
    d: "塞舌爾",
    e: "Seychelles"
},
{
    a: 19121,
    b: "",
    c: "塞拉利昂",
    d: "塞拉利昂",
    e: "Sierra Leone"
},
{
    a: 19131,
    b: "http://www.singpost.com/",
    c: "新加坡",
    d: "新加坡",
    e: "Singapore"
},
{
    a: 19133,
    b: "http://www.speedpost.com.sg/",
    c: "新加坡",
    d: "新加坡",
    e: "Singapore"
},
{
    a: 19141,
    b: "http://tandt.posta.sk/en",
    c: "斯洛伐克",
    d: "斯洛伐克",
    e: "Slovakia"
},
{
    a: 19151,
    b: "http://sledenje.posta.si/Default.aspx",
    c: "斯洛文尼亚",
    d: "斯洛文尼亞",
    e: "Slovenia"
},
{
    a: 19161,
    b: "",
    c: "所罗门群岛",
    d: "所羅門群島",
    e: "Solomon Islands"
},
{
    a: 19171,
    b: "http://sms.postoffice.co.za/TrackingParcels/",
    c: "南非",
    d: "南非",
    e: "South Africa"
},
{
    a: 19181,
    b: "http://www.correos.es/comun/Localizador/2010_c1-LocalizadorE.asp",
    c: "西班牙",
    d: "西班牙",
    e: "Spain"
},
{
    a: 19191,
    b: "http://www.slpost.gov.lk/",
    c: "斯里兰卡",
    d: "斯里蘭卡",
    e: "Sri Lanka"
},
{
    a: 19201,
    b: "http://196.29.186.166:8450/ipsweb/",
    c: "苏丹",
    d: "蘇丹",
    e: "Sudan"
},
{
    a: 19211,
    b: "http://www.surpost.com/",
    c: "苏里南",
    d: "蘇里南",
    e: "Surinam"
},
{
    a: 19221,
    b: "",
    c: "斯瓦尔巴岛和扬马延岛[挪威]",
    d: "斯瓦爾巴島和揚馬延島[挪威]",
    e: "Svalbard And Jan Mayen[NO]"
},
{
    a: 19231,
    b: "http://www.sptc.co.sz/",
    c: "斯威士兰",
    d: "斯威士蘭",
    e: "Swaziland"
},
{
    a: 19241,
    b: "http://www.posten.se/en/Pages/home.aspx",
    c: "瑞典",
    d: "瑞典",
    e: "Sweden"
},
{
    a: 19251,
    b: "http://www.swisspost.ch/",
    c: "瑞士",
    d: "瑞士",
    e: "Switzerland"
},
{
    a: 19261,
    b: "http://213.178.243.17/",
    c: "叙利亚",
    d: "敘利亞",
    e: "Syrian"
},
{
    a: 19271,
    b: "http://www.nia.gov.kn",
    c: "圣基茨和尼维斯",
    d: "聖基茨和尼維斯",
    e: "Saint Kitts And Nevis"
},
{
    a: 19281,
    b: "http://samoapost.ws/",
    c: "西萨摩亚",
    d: "西薩摩亞",
    e: "Western Samoa"
},
{
    a: 19291,
    b: "",
    c: "索马里",
    d: "索馬里",
    e: "Somalia"
},
{
    a: 19301,
    b: "",
    c: "苏格兰",
    d: "蘇格蘭",
    e: "Scotland"
},
{
    a: 19311,
    b: "",
    c: "圣赫勒拿岛[英国]",
    d: "聖赫勒拿島[英國]",
    e: "Saint Helena[GB]"
},
{
    a: 20011,
    b: "http://postserv.post.gov.tw/webpost/CSController?cmd=POS0000_3&_MENU_ID=189&_ACTIVE_ID=189&_SYS_ID=D",
    c: "台湾",
    d: "台灣",
    e: "Taiwan"
},
{
    a: 20021,
    b: "http://www.tajik-gateway.org/",
    c: "塔吉克斯坦",
    d: "塔吉克斯坦",
    e: "Tajikistan"
},
{
    a: 20031,
    b: "http://41.59.0.101:1120/tracking/IPSWeb_submit.htm",
    c: "坦桑尼亚",
    d: "坦桑尼亞",
    e: "Tanzania"
},
{
    a: 20041,
    b: "http://track.thailandpost.co.th/trackinternet/Default.aspx?lang=en",
    c: "泰国",
    d: "泰國",
    e: "Thailand"
},
{
    a: 20051,
    b: "http://www.laposte.tg/",
    c: "多哥",
    d: "多哥",
    e: "Togo"
},
{
    a: 20061,
    b: "http://www.tongapost.net/",
    c: "汤加",
    d: "湯加",
    e: "Tonga"
},
{
    a: 20071,
    b: "",
    c: "特立尼达和多巴哥",
    d: "特立尼達和多巴哥",
    e: "Trinidad And Tobago"
},
{
    a: 20073,
    b: "http://www.ttpost.net/",
    c: "特立尼达和多巴哥",
    d: "特立尼達和多巴哥",
    e: "Trinidad And Tobago"
},
{
    a: 20081,
    b: "",
    c: "特里斯坦达库尼亚群岛[英国]",
    d: "特里斯坦達庫尼亞群島[英國]",
    e: "Tristan Da Cunha Islands[GB]"
},
{
    a: 20091,
    b: "",
    c: "图瓦卢",
    d: "圖瓦盧",
    e: "Tuvalu"
},
{
    a: 20101,
    b: "http://www.e-suivi.poste.tn/an/suivi.html",
    c: "突尼斯",
    d: "突尼斯",
    e: "Tunisia"
},
{
    a: 20111,
    b: "http://212.175.152.3/tr/interaktif/kayitliposta-yd_yeniweb.php",
    c: "土耳其",
    d: "土耳其",
    e: "Turkey"
},
{
    a: 20121,
    b: "",
    c: "土库曼斯坦",
    d: "土庫曼斯坦",
    e: "Turkmenistan"
},
{
    a: 20131,
    b: "",
    c: "特克斯和凯科斯群岛[英国]",
    d: "特克斯和凱科斯群島[英國]",
    e: "Turks And Caicos Islands[GB]"
},
{
    a: 20141,
    b: "",
    c: "托克劳群岛[新西兰]",
    d: "托克勞群島[新西蘭]",
    e: "Tokelau Islands[NZ]"
},
{
    a: 21011,
    b: "http://41.210.163.110:8080/postglobaltrack/",
    c: "乌干达",
    d: "烏干達",
    e: "Uganda"
},
{
    a: 21021,
    b: "http://www.ukrposhta.com/www/upost_en.nsf/search_post?openpage",
    c: "乌克兰",
    d: "烏克蘭",
    e: "Ukraine"
},
{
    a: 21023,
    b: "http://dpsz.ua/en/",
    c: "乌克兰",
    d: "烏克蘭",
    e: "Ukraine"
},
{
    a: 21031,
    b: "http://www.pochta.uz/index.php/en/2012-04-24-04-47-55",
    c: "乌兹别克斯坦",
    d: "烏茲別克斯坦",
    e: "Uzbekistan"
},
{
    a: 21033,
    b: "http://old.ems.uz/search_nodesign_en.php",
    c: "乌兹别克斯坦",
    d: "烏茲別克斯坦",
    e: "Uzbekistan"
},
{
    a: 21041,
    b: "http://www.correo.com.uy/index.asp?codPag=tyt",
    c: "乌拉圭",
    d: "烏拉圭",
    e: "Uruguay"
},
{
    a: 21043,
    b: "",
    c: "乌拉圭",
    d: "烏拉圭",
    e: "Uruguay"
},
{
    a: 21051,
    b: "https://tools.usps.com/go/TrackConfirmAction_input",
    c: "美国",
    d: "美國",
    e: "Usa"
},
{
    a: 22011,
    b: "",
    c: "维尔京群岛[美国]",
    d: "維爾京群島[美國]",
    e: "Virgin Islands[US]"
},
{
    a: 22021,
    b: "http://www.postvanuatu.com/",
    c: "瓦努阿图",
    d: "瓦努阿圖",
    e: "Vanuatu"
},
{
    a: 22023,
    b: "",
    c: "瓦努阿图",
    d: "瓦努阿圖",
    e: "Vanuatu"
},
{
    a: 22031,
    b: "http://www.ipostel.gob.ve/",
    c: "委内瑞拉",
    d: "委內瑞拉",
    e: "Venezuela"
},
{
    a: 22041,
    b: "http://www.vnpost.vn/",
    c: "越南",
    d: "越南",
    e: "Vietnam"
},
{
    a: 22051,
    b: "",
    c: "梵蒂冈",
    d: "梵蒂岡",
    e: "Vatican"
},
{
    a: 22061,
    b: "",
    c: "维尔京群岛[英国]",
    d: "維爾京群島[英國]",
    e: "Virgin Islands[GB]"
},
{
    a: 23011,
    b: "",
    c: "瓦利斯和富图纳群岛[法国]",
    d: "瓦利斯和富圖納群島[法國]",
    e: "Wallis And Futuna[FR]"
},
{
    a: 25011,
    b: "http://www.e-stamps.post.ye:8090/ips/IPSWeb_submit.htm",
    c: "也门",
    d: "也門",
    e: "Yemen"
},
{
    a: 26011,
    b: "",
    c: "赞比亚",
    d: "贊比亞",
    e: "Zambia"
},
{
    a: 26021,
    b: "http://ips-webtracking.zimpost.co.zw/trackit/",
    c: "津巴布韦",
    d: "津巴布韋",
    e: "Zimbabwe"
}];
var COUNTRY_ENUM = [{
    a: 1020,
    b: "阿富汗",
    c: "阿富汗",
    d: "Afghanistan"
},
{
    a: 1030,
    b: "阿尔巴尼亚",
    c: "阿爾巴尼亞",
    d: "Albania"
},
{
    a: 1110,
    b: "安的列斯群岛[荷兰]",
    c: "安的列斯群島[荷蘭]",
    d: "Netherlands Antilles[NL]"
},
{
    a: 1120,
    b: "阿根廷",
    c: "阿根廷",
    d: "Argentina"
},
{
    a: 1130,
    b: "亚美尼亚",
    c: "亞美尼亞",
    d: "Armenia"
},
{
    a: 1140,
    b: "阿鲁巴[荷兰]",
    c: "阿魯巴[荷蘭]",
    d: "Aruba[NL]"
},
{
    a: 1150,
    b: "澳大利亚",
    c: "澳大利亞",
    d: "Australia"
},
{
    a: 1160,
    b: "奥地利",
    c: "奧地利",
    d: "Austria"
},
{
    a: 1170,
    b: "阿塞拜疆",
    c: "阿塞拜疆",
    d: "Azerbaijan"
},
{
    a: 2020,
    b: "巴林",
    c: "巴林",
    d: "Bahrain"
},
{
    a: 2050,
    b: "白俄罗斯",
    c: "白俄羅斯",
    d: "Belarus"
},
{
    a: 2060,
    b: "比利时",
    c: "比利時",
    d: "Belgium"
},
{
    a: 2090,
    b: "百慕达[英国]",
    c: "百慕達[英國]",
    d: "Bermuda[GB]"
},
{
    a: 2100,
    b: "不丹",
    c: "不丹",
    d: "Bhutan"
},
{
    a: 2130,
    b: "博茨瓦纳",
    c: "博茨瓦納",
    d: "Botswana"
},
{
    a: 2150,
    b: "巴西",
    c: "巴西",
    d: "Brazil"
},
{
    a: 2160,
    b: "文莱",
    c: "文萊",
    d: "Brunei"
},
{
    a: 2170,
    b: "保加利亚",
    c: "保加利亞",
    d: "Bulgaria"
},
{
    a: 3010,
    b: "中国",
    c: "中國",
    d: "China"
},
{
    a: 3020,
    b: "柬埔寨",
    c: "柬埔寨",
    d: "Cambodia"
},
{
    a: 3040,
    b: "加拿大",
    c: "加拿大",
    d: "Canada"
},
{
    a: 3100,
    b: "智利",
    c: "智利",
    d: "Chile"
},
{
    a: 3120,
    b: "科特迪瓦",
    c: "科特迪瓦",
    d: "Côte d'Ivoire"
},
{
    a: 3130,
    b: "哥伦比亚",
    c: "哥倫比亞",
    d: "Colombia"
},
{
    a: 3180,
    b: "哥斯达黎加",
    c: "哥斯達黎加",
    d: "Costa Rica"
},
{
    a: 3190,
    b: "克罗地亚",
    c: "克羅地亞",
    d: "Croatia"
},
{
    a: 3210,
    b: "塞浦路斯",
    c: "塞浦路斯",
    d: "Cyprus"
},
{
    a: 3220,
    b: "捷克",
    c: "捷克",
    d: "Czech"
},
{
    a: 4010,
    b: "丹麦",
    c: "丹麥",
    d: "Denmark"
},
{
    a: 5010,
    b: "厄瓜多尔",
    c: "厄瓜多爾",
    d: "Ecuador"
},
{
    a: 5020,
    b: "埃及",
    c: "埃及",
    d: "Egypt"
},
{
    a: 5030,
    b: "阿联酋",
    c: "阿聯酋",
    d: "United Arab Emirates"
},
{
    a: 5040,
    b: "爱沙尼亚",
    c: "愛沙尼亞",
    d: "Estonia"
},
{
    a: 5050,
    b: "埃塞俄比亚",
    c: "埃塞俄比亞",
    d: "Ethiopia"
},
{
    a: 5060,
    b: "厄立特里亚",
    c: "厄立特里亞",
    d: "Eritrea"
},
{
    a: 6020,
    b: "法罗群岛[丹麦]",
    c: "法羅群島[丹麥]",
    d: "Faroe Islands[DK]"
},
{
    a: 6030,
    b: "斐济",
    c: "斐濟",
    d: "Fiji"
},
{
    a: 6040,
    b: "芬兰",
    c: "芬蘭",
    d: "Finland"
},
{
    a: 6050,
    b: "法国",
    c: "法國",
    d: "France"
},
{
    a: 7040,
    b: "德国",
    c: "德國",
    d: "Germany"
},
{
    a: 7050,
    b: "加纳",
    c: "加納",
    d: "Ghana"
},
{
    a: 7070,
    b: "希腊",
    c: "希臘",
    d: "Greece"
},
{
    a: 7080,
    b: "格陵兰[丹麦]",
    c: "格陵蘭[丹麥]",
    d: "Greenland[DK]"
},
{
    a: 7120,
    b: "危地马拉",
    c: "危地馬拉",
    d: "Guatemala"
},
{
    a: 8010,
    b: "香港",
    c: "香港",
    d: "Hong Kong"
},
{
    a: 8050,
    b: "匈牙利",
    c: "匈牙利",
    d: "Hungary"
},
{
    a: 9010,
    b: "冰岛",
    c: "冰島",
    d: "Iceland"
},
{
    a: 9020,
    b: "印度",
    c: "印度",
    d: "India"
},
{
    a: 9030,
    b: "印度尼西亚",
    c: "印度尼西亞",
    d: "Indonesia"
},
{
    a: 9040,
    b: "伊朗",
    c: "伊朗",
    d: "Iran"
},
{
    a: 9050,
    b: "爱尔兰",
    c: "愛爾蘭",
    d: "Ireland"
},
{
    a: 9060,
    b: "以色列",
    c: "以色列",
    d: "Israel"
},
{
    a: 9070,
    b: "意大利",
    c: "意大利",
    d: "Italy"
},
{
    a: 10020,
    b: "日本",
    c: "日本",
    d: "Japan"
},
{
    a: 10030,
    b: "约旦",
    c: "約旦",
    d: "Jordan"
},
{
    a: 11010,
    b: "哈萨克斯坦",
    c: "哈薩克斯坦",
    d: "Kazakhstan"
},
{
    a: 11020,
    b: "肯尼亚",
    c: "肯尼亞",
    d: "Kenya"
},
{
    a: 11030,
    b: "英国",
    c: "英國",
    d: "United Kingdom"
},
{
    a: 11050,
    b: "韩国",
    c: "韓國",
    d: "Korea"
},
{
    a: 11090,
    b: "吉尔吉斯斯坦",
    c: "吉爾吉斯斯坦",
    d: "Kirghizstan"
},
{
    a: 12020,
    b: "拉脱维亚",
    c: "拉脫維亞",
    d: "Latvia"
},
{
    a: 12030,
    b: "黎巴嫩",
    c: "黎巴嫩",
    d: "Lebanon"
},
{
    a: 12060,
    b: "利比亚",
    c: "利比亞",
    d: "Libya"
},
{
    a: 12070,
    b: "列支敦士登",
    c: "列支敦士登",
    d: "Liechtenstein"
},
{
    a: 12080,
    b: "立陶宛",
    c: "立陶宛",
    d: "Lithuania"
},
{
    a: 12100,
    b: "卢森堡",
    c: "盧森堡",
    d: "Luxembourg"
},
{
    a: 13010,
    b: "澳门",
    c: "澳門",
    d: "Macau"
},
{
    a: 13020,
    b: "马其顿",
    c: "馬其頓",
    d: "Macedonia"
},
{
    a: 13050,
    b: "马来西亚",
    c: "馬來西亞",
    d: "Malaysia"
},
{
    a: 13060,
    b: "马尔代夫",
    c: "馬爾代夫",
    d: "Maldives"
},
{
    a: 13080,
    b: "马耳他",
    c: "馬耳他",
    d: "Malta"
},
{
    a: 13130,
    b: "毛里求斯",
    c: "毛里求斯",
    d: "Mauritius"
},
{
    a: 13140,
    b: "墨西哥",
    c: "墨西哥",
    d: "Mexico"
},
{
    a: 13160,
    b: "摩尔多瓦",
    c: "摩爾多瓦",
    d: "Moldova"
},
{
    a: 13210,
    b: "摩洛哥",
    c: "摩洛哥",
    d: "Morocco"
},
{
    a: 14050,
    b: "新喀里多尼亚[法国]",
    c: "新喀裡多尼亞[法國]",
    d: "New Caledonia[FR]"
},
{
    a: 14060,
    b: "新西兰",
    c: "新西蘭",
    d: "New Zealand"
},
{
    a: 14080,
    b: "挪威",
    c: "挪威",
    d: "Norway"
},
{
    a: 14100,
    b: "尼日利亚",
    c: "尼日利亞",
    d: "Nigeria"
},
{
    a: 15010,
    b: "阿曼",
    c: "阿曼",
    d: "Oman"
},
{
    a: 16010,
    b: "巴基斯坦",
    c: "巴基斯坦",
    d: "Pakistan"
},
{
    a: 16060,
    b: "秘鲁",
    c: "秘魯",
    d: "Peru"
},
{
    a: 16070,
    b: "菲律宾",
    c: "菲律賓",
    d: "Philippines"
},
{
    a: 16080,
    b: "波兰",
    c: "波蘭",
    d: "Poland"
},
{
    a: 16100,
    b: "葡萄牙",
    c: "葡萄牙",
    d: "Portugal"
},
{
    a: 16110,
    b: "波多黎各[美国]",
    c: "波多黎各[美國]",
    d: "Puerto Rico[US]"
},
{
    a: 17010,
    b: "卡塔尔",
    c: "卡塔爾",
    d: "Qatar"
},
{
    a: 18020,
    b: "罗马尼亚",
    c: "羅馬尼亞",
    d: "Romania"
},
{
    a: 18030,
    b: "俄罗斯",
    c: "俄羅斯",
    d: "Russia"
},
{
    a: 19070,
    b: "沙特阿拉伯",
    c: "沙特阿拉伯",
    d: "Saudi Arabia"
},
{
    a: 19080,
    b: "塞内加尔",
    c: "塞內加爾",
    d: "Senegal"
},
{
    a: 19090,
    b: "塞尔维亚",
    c: "塞爾維亞",
    d: "Serbia"
},
{
    a: 19130,
    b: "新加坡",
    c: "新加坡",
    d: "Singapore"
},
{
    a: 19140,
    b: "斯洛伐克",
    c: "斯洛伐克",
    d: "Slovakia"
},
{
    a: 19150,
    b: "斯洛文尼亚",
    c: "斯洛文尼亞",
    d: "Slovenia"
},
{
    a: 19170,
    b: "南非",
    c: "南非",
    d: "South Africa"
},
{
    a: 19180,
    b: "西班牙",
    c: "西班牙",
    d: "Spain"
},
{
    a: 19200,
    b: "苏丹",
    c: "蘇丹",
    d: "Sudan"
},
{
    a: 19240,
    b: "瑞典",
    c: "瑞典",
    d: "Sweden"
},
{
    a: 19250,
    b: "瑞士",
    c: "瑞士",
    d: "Switzerland"
},
{
    a: 19260,
    b: "叙利亚",
    c: "敘利亞",
    d: "Syrian"
},
{
    a: 20010,
    b: "台湾",
    c: "台灣",
    d: "Taiwan"
},
{
    a: 20030,
    b: "坦桑尼亚",
    c: "坦桑尼亞",
    d: "Tanzania"
},
{
    a: 20040,
    b: "泰国",
    c: "泰國",
    d: "Thailand"
},
{
    a: 20100,
    b: "突尼斯",
    c: "突尼斯",
    d: "Tunisia"
},
{
    a: 20110,
    b: "土耳其",
    c: "土耳其",
    d: "Turkey"
},
{
    a: 21010,
    b: "乌干达",
    c: "烏干達",
    d: "Uganda"
},
{
    a: 21020,
    b: "乌克兰",
    c: "烏克蘭",
    d: "Ukraine"
},
{
    a: 21030,
    b: "乌兹别克斯坦",
    c: "烏茲別克斯坦",
    d: "Uzbekistan"
},
{
    a: 21040,
    b: "乌拉圭",
    c: "烏拉圭",
    d: "Uruguay"
},
{
    a: 21050,
    b: "美国",
    c: "美國",
    d: "Usa"
},
{
    a: 22020,
    b: "瓦努阿图",
    c: "瓦努阿圖",
    d: "Vanuatu"
},
{
    a: 22040,
    b: "越南",
    c: "越南",
    d: "Vietnam"
},
{
    a: 25010,
    b: "也门",
    c: "也門",
    d: "Yemen"
},
{
    a: 26020,
    b: "津巴布韦",
    c: "津巴布韋",
    d: "Zimbabwe"
}];