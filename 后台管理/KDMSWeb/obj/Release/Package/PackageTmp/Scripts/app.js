// 日期格式化
Date.prototype.Dateformat = function (format) //author: meizz
{
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
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
//字符串转为日期
function getDate(strDate) {
    var date = eval('new Date(' + strDate.replace(/\d+(?=-[^-]+$)/,
        function (a) { return parseInt(a, 10) - 1; }).match(/\d+/g) + ')');
    return date;
}
function formatTime(val) {
    var re = /-?\d+/;
    var m = re.exec(val);
    var d = new Date(parseInt(m[0]));
    // 按【2012-02-13 09:09:09】的格式返回日期
    return d.Dateformat("yyyy-MM-dd hh:mm:ss");
}

function formatDate(val) {
    var re = /-?\d+/;
    var m = re.exec(val);
    var d = new Date(parseInt(m[0]));
    // 按【2012-02-13】的格式返回日期
    return d.Dateformat("yyyy-MM-dd");
}
/**
 * 获取当前日期是当年的第多少天
 */
function getDaysByYear() {
    var dateArr = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
    var date = new Date();
    var day = date.getDate();
    var month = date.getMonth(); //getMonth()是从0开始  
    var year = date.getFullYear();
    var days = 0;
    for (var i = 0; i < month; i++) {
        days += dateArr[i];
    }
    days += day;
    //判断是否闰年  
    if (month > 1 && (year % 4 == 0 && year % 100 != 0) || year % 400 == 0) {
        days += 1;
    }
    return days;
}
/**
 * 根据日期范围(时间间隔天数)，获取具体日期区间
 * @param days 时间间隔天数
 * @returns {string}
 */
function getStartAndEndDate(bet) {
    var now = new Date();
    var days;
    //得到本周，本月，当年相差天数
    //switch (bet) {
    //    case "WEEK":
    //        days = now.getDay() == 0 ? 7 : now.getDay();
    //        break;
    //    case "MONTH":
    //        days = now.getDate();
    //        break;
    //    case "YEAR":
    //        days = getDaysByYear();
    //        break;       

    //}
    var end = now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + now.getDate();

    //var nowAfterDays = now.setDate(now.getDate() - days + 1);//
    //nowAfterDays = new Date(nowAfterDays);
    //var start = nowAfterDays.getFullYear() + "/" + (nowAfterDays.getMonth() + 1) + "/" + nowAfterDays.getDate();
    var start;
    switch (bet) {
        case "WEEK":
            days = now.getDay() == 0 ? 7 : now.getDay();
            days = now.getDate() - days + 1;
            start = now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + days;
            break;
        case "MONTH":
            start = now.getFullYear() + "/" + (now.getMonth() + 1) + "/1";
            break;
        case "YEAR":
            start = now.getFullYear() + "/1" + "/1";
            break;

    }
    return start + "~" + end;
}
var getQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};
/**
*   跳转到指定页面
*/
function goAppPage(msg,url,t,cle) {
    $.toptip(msg);
    if (t) {
        setTimeout(function () {
            if (cle) {
                window.opener = null;
                window.open('', '_self');
                window.close();
            } else {
                top.location.href = url;
            }            
        }, t);
    } else {
        top.location.href = url;
    }
}
/**
 * 判断一个元素是否存在于一个数组中
 * @param {Object} arr 数组
 * @param {Object} value 元素值
 */
function isInArray(arr, value) {
    for (var i = 0; i < arr.length; i++) {
        if (value === arr[i].ResourcesCode) {
            return true;
        }
    }
    return false;
}
//获取权限元素
function getRoleRs() {
    var hreflink = window.location.pathname;
    var arr = [];
    $.ajax({
        type: 'post',
        async: false,
        dataType: 'json',
        data: { url: hreflink },
        url: '/Home/GetRoleByPage',
        success: function (data) {
            arr = data;
        }
    });
    return arr;
}
/**
*根据权限元素删除按钮
*
*/
(function ($) {
    $.fn.extend({
        checkRole: function () {
            
            $(this).each(function () {
                var code = $(this).attr("code");
                if (code) {
                    if (getRoleRs()) {
                        var DT = getRoleRs();

                        if (!isInArray(DT, code)) {
                            $(this).remove();
                        }
                    }
                }
                //if (code) {
                //    var queryResult = Enumerable.From(l).Where(function (x) { return x.ResourcesCode.replace(/(^\s*)|(\s*$)/g, "") == code.replace(/(^\s*)|(\s*$)/g, "") }).Select(function (x) { return x.ResourcesName + ':' + x.text }).ToArray();
                //    if (queryResult.length == 0) {
                //        $(this).remove();
                //    }
                //}
            });
        }
    });
})(jQuery);
