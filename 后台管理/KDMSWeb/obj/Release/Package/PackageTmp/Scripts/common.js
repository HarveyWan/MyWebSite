//combobox 默认选择第一项   .combobox('selectedIndex')
$.extend($.fn.combobox.methods, {
    selectedIndex: function (jq, index) {
        if (!index)
            index = 0;
        var data = $(jq).combobox('options').data;
        var vf = $(jq).combobox('options').valueField;
        $(jq).combobox('select', eval('data[index].' + vf));
    }
});
// 设置grid 为第一页
function setFirstPage(ids) {
    var opts = $(ids).datagrid('options');
    var pager = $(ids).datagrid('getPager');
    opts.pageNumber = 1;
    opts.pageSize = opts.pageSize;
    pager.pagination('refresh', {
        pageNumber: 1,
        pageSize: opts.pageSize
    });
}
$.extend($.fn.validatebox.defaults.rules, {
    checkFloat: {
        validator: function (value, param) {
            return /^[+|-]?([0-9]+\.[0-9]+)|[0-9]+$/.test(value);
        },
        message: '请输入合法数字'
    }
});
//设置grid 编辑列
$.extend($.fn.datagrid.defaults.editors, {
    numberspinner: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            return input.numberspinner(options);
        },
        destroy: function (target) {
            $(target).numberspinner('destroy');
        },
        getValue: function (target) {
            return $(target).numberspinner('getValue');
        },
        setValue: function (target, value) {
            $(target).numberspinner('setValue', value);
        },
        resize: function (target, width) {
            $(target).numberspinner('resize', width);
        }
    }
});
$.extend($.fn.datagrid.methods, {
    addEditor: function (jq, param) {
        if (param instanceof Array) {
            $.each(param, function (index, item) {
                var e = $(jq).datagrid('getColumnOption', item.field);
                e.editor = item.editor;
            });
        } else {
            var e = $(jq).datagrid('getColumnOption', param.field);
            e.editor = param.editor;
        }
    },
    removeEditor: function (jq, param) {
        if (param instanceof Array) {
            $.each(param, function (index, item) {
                var e = $(jq).datagrid('getColumnOption', item);
                e.editor = {};
            });
        } else {
            var e = $(jq).datagrid('getColumnOption', param);
            e.editor = {};
        }
    }
});
//combobox 清楚选择 css样式在common.css
(function ($) {

    //初始化清除按钮  
    function initClear(target) {
        var jq = $(target);
        var opts = jq.data('combo').options;
        var combo = jq.data('combo').combo;
        var arrow = combo.find('span.combo-arrow');

        var clear = arrow.siblings("span.combo-clear");
        if (clear.size() == 0) {
            //创建清除按钮。  
            clear = $('<span class="combo-clear" style="height: 20px;"></span>');

            //清除按钮添加悬停效果。  
            clear.unbind("mouseenter.combo mouseleave.combo").bind("mouseenter.combo mouseleave.combo",
                function (event) {
                    var isEnter = event.type == "mouseenter";
                    clear[isEnter ? 'addClass' : 'removeClass']("combo-clear-hover");
                }
            );
            //清除按钮添加点击事件，清除当前选中值及隐藏选择面板。  
            clear.unbind("click.combo").bind("click.combo", function () {
                jq.combo("setValue", "").combo("setText", "");
                jq.combo('hidePanel');
            });
            arrow.before(clear);
        };
        var input = combo.find("input.combo-text");
        input.outerWidth(input.outerWidth() - clear.outerWidth());

        opts.initClear = true;//已进行清除按钮初始化。  
    }
    function _16(_37, _38) {
        var _39 = $.data(_37, "combo");
        var _3a = _39.options;
        var _3b = _39.combo;
        if (_38) {
            _3a.disabled = true;
            $(_37).attr("disabled", true);
            _3b.find(".combo-value").attr("disabled", true);
            _3b.find(".combo-text").attr("disabled", true);
        } else {
            _3a.disabled = false;
            $(_37).removeAttr("disabled");
            _3b.find(".combo-value").removeAttr("disabled");
            _3b.find(".combo-text").removeAttr("disabled");
        }
    };
    function _17(_3c, _3d) {
        var _3e = $.data(_3c, "combo");
        var _3f = _3e.options;
        _3f.readonly = _3d == undefined ? true : _3d;
        var _40 = _3f.readonly ? true : (!_3f.editable);
        _3e.combo.find(".combo-text").attr("readonly", _40).css("cursor", _40 ? "pointer" : "");
    };
    function _1c(_1d) {
        $(_1d).find(".combo-f").each(function () {
            var p = $(this).combo("panel");
            if (p.is(":visible")) {
                p.panel("close");
            }
        });
    };
    function _1e(_1f) {
        var _20 = $.data(_1f, "combo");
        var _21 = _20.options;
        var _22 = _20.panel;
        var _23 = _20.combo;
        var _24 = _23.find(".combo-text");
        var _25 = _23.find(".combo-arrow");
        $(document).unbind(".combo").bind("mousedown.combo", function (e) {
            var p = $(e.target).closest("span.combo,div.combo-p");
            if (p.length) {
                _1c(p);
                return;
            }
            $("body>div.combo-p>div.combo-panel:visible").panel("close");
        });
        _24.unbind(".combo");
        _25.unbind(".combo");
        if (!_21.disabled && !_21.readonly) {
            _24.bind("click.combo", function (e) {
                if (!_21.editable) {
                    _26.call(this);
                } else {
                    var p = $(this).closest("div.combo-panel");
                    $("div.combo-panel:visible").not(_22).not(p).panel("close");
                }
            }).bind("keydown.combo paste.combo drop.combo", function (e) {
                switch (e.keyCode) {
                    case 38:
                        _21.keyHandler.up.call(_1f, e);
                        break;
                    case 40:
                        _21.keyHandler.down.call(_1f, e);
                        break;
                    case 37:
                        _21.keyHandler.left.call(_1f, e);
                        break;
                    case 39:
                        _21.keyHandler.right.call(_1f, e);
                        break;
                    case 13:
                        e.preventDefault();
                        _21.keyHandler.enter.call(_1f, e);
                        return false;
                    case 9:
                    case 27:
                        _27(_1f);
                        break;
                    default:
                        if (_21.editable) {
                            if (_20.timer) {
                                clearTimeout(_20.timer);
                            }
                            _20.timer = setTimeout(function () {
                                var q = _24.val();
                                if (_20.previousValue != q) {
                                    _20.previousValue = q;
                                    $(_1f).combo("showPanel");
                                    _21.keyHandler.query.call(_1f, _24.val(), e);
                                    $(_1f).combo("validate");
                                }
                            }, _21.delay);
                        }
                }
            });
            _25.bind("click.combo", function () {
                _26.call(this);
            }).bind("mouseenter.combo", function () {
                $(this).addClass("combo-arrow-hover");
            }).bind("mouseleave.combo", function () {
                $(this).removeClass("combo-arrow-hover");
            });
        }
        function _26() {
            if (_22.is(":visible")) {
                _27(_1f);
            } else {
                var p = $(this).closest("div.combo-panel");
                $("div.combo-panel:visible").not(_22).not(p).panel("close");
                $(_1f).combo("showPanel");
            }
            _24.focus();
        };
    };
    function _27(_31) {
        var _32 = $.data(_31, "combo").panel;
        _32.panel("close");
    };
    function removeClear(target) {
        var jq = $(target);
        var opts = jq.data('combo').options;
        var combo = jq.data('combo').combo;
        var arrow = combo.find('span.combo-arrow');

        var clear = arrow.siblings("span.combo-clear");

        var input = combo.find("input.combo-text");
        input.outerWidth(input.outerWidth() + clear.outerWidth());
        $(clear).remove();

    }
    //扩展easyui combo添加清除当前值。  
    //var oldResize = $.fn.combo.methods.resize;
    //$.extend($.fn.combo.methods, {
    //    initClear: function (jq) {
    //        return jq.each(function () {
    //            initClear(this);
    //        });
    //    },
    //    resize: function (jq) {
    //        //调用默认combo resize方法。  
    //        var returnValue = oldResize.apply(this, arguments);
    //        var opts = jq.data("combo").options;
    //        if (opts.initClear) {
    //            jq.combo("initClear", jq);
    //        }
    //        return returnValue;
    //    },
    //    disable: function (jq) {
    //        return jq.each(function () {
    //            removeClear(this);
    //            _16(this, true);
    //            _1e(this);
    //        });
    //    },
    //    readonly: function (jq, _69) {
    //        return jq.each(function () {
    //            removeClear(this);
    //            _17(this, _69);
    //            _1e(this);
    //        });
    //    }
    //});

    var defaults = $.fn.combo.extensions.defaults = {
        initClear: true
    };


    function initialize(target) {
        var t = $(target), state = $.data(target, "combo"),
            opts = t.combo("options"), panel = state.panel,
            combo = state.combo, //arrow = combo.find(".combo-arrow"),
            exts = opts.extensions ? opts.extensions : opts.extensions = {};
        if (!opts.disabled || !opts.readonly)
            if (opts.initClear) { initClear(target); }
    }


    var _combo = $.fn.combo;
    $.fn.combo = function (options, param) {
        if (typeof options == "string") {
            return _combo.apply(this, arguments);
        }
        options = options || {};
        return this.each(function () {
            var jq = $(this), hasInit = $.data(this, "combo") ? true : false,
                opts = hasInit ? options : $.extend({}, $.fn.combo.parseOptions(this), $.parser.parseOptions(this, [
                    "initClear"
                ]), options);
            _combo.call(jq, opts);
            //initialize(this);
        });
    };
    $.union($.fn.combo, _combo);
    $.extend($.fn.combo.defaults, defaults);
}(jQuery));


var modalDialog = function (options) {
    var opts = $.extend({
        title: '&nbsp;',
        width: 640,
        height: 480,
        modal: true,
        autoCloseOnEsc: true,
        onClose: function () {
            $(this).dialog('destroy');
        }
    }, options);
    //opts.modal = true;// 强制此dialog为模式化，无视传递过来的modal参数
    if (options.url) {
        opts.content = '<iframe id="" src="' + options.url + '" allowTransparency="true" scrolling="auto" width="100%" height="98%" frameBorder="0" name=""></iframe>';
    }
    return $('<div/>').dialog(opts);
};

var getQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};
//combobox  onLoadSuccess事件
var onLoadSuccessCombo = function () {
    var target = $(this);
    var data = target.combobox("getData");
    var options = target.combobox("options");
    if (data && data.length > 0) {
        var fs = data[0];
        target.combobox("setValue", fs[options.valueField]);
    }
}

// 禁用 验证
function InitValidataboxClose() {
    $('input.easyui-validatebox').validatebox('disableValidation')//////////
              .focus(function () {
                  $(this).validatebox('enableValidation');
              })
              .blur(function () {
                  $(this).validatebox('validate')
              });
    $('input.easyui-numberbox').validatebox('disableValidation')//////////
            .focus(function () {
                $(this).validatebox('enableValidation');
            })
            .blur(function () {
                $(this).validatebox('validate')
            });
    $('input.easyui-combobox').combobox('disableValidation')//////////
           .focus(function () {
               $(this).combobox('enableValidation');
           })
           .blur(function () {
               $(this).combobox('validate')
           });
}
 
var InitValidataboxOpen = function () {
    $('input.easyui-validatebox').validatebox('enableValidation');
    $('input.easyui-validatebox').validatebox('validate');
    $('input.easyui-numberbox').validatebox('enableValidation');
    $('input.easyui-numberbox').validatebox('validate');
    $('input.easyui-combobox').combobox('enableValidation');
    $('input.easyui-combobox').combobox('validate');
}

//$(function () { InitValidataboxClose(); });


var cityall = function (param, success, error) {
    var q = param.q || '';
    if (q.length < 1) { return false }
    $.ajax({
        type: 'post',
        url: '/Common/GetCityAll',
        dataType: 'json',
        data: { q: q },
        success: function (data) {
            var items = $.map(data, function (item) {
                return {
                    Code: item.Code,
                    Name: item.Name
                };
            });
            success(items);
        },
        error: function () {
            error.apply(this, arguments);
        }
    });
}
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

//初始化grid
//id 控件ID,option grid属性json
var selectIndex = 1;
function CreateGrid(id, options) {
    var opts = $.extend({
        type: "post",
        datatype: "json",
        fit: true,
        autoRowHeight: true,
        pagination: true,
        pageSize: 20,
        singleSelect: true,
        sortable: true,
        rownumbers: true,
        onLoadSuccess: function (data) {
            if (data.rows.length <= 0) {
                var body = $(this).data().datagrid.dc.body2;
                body.find('table tbody').append('<tr><td width="' + body.width() + '" style="height: 25px; text-align: center;">没有数据</td></tr>');
            }
            $('#' + id).datagrid('scrollTo', 0);
            var fields = $('#' + id).datagrid('getColumnFields', false);
            //datagrid头部 table 的第一个tr 的td们，即columns的集合
            var panel = $('#' + id).datagrid("getPanel");
            var headerTds = panel.find(".datagrid-view2 .datagrid-header .datagrid-header-inner table tr:first-child").children();

            //重新设置列表头的对齐方式
            headerTds.each(function (i, obj) {
                var col = $('#' + id).datagrid('getColumnOption', fields[i]);
                if (!col.hidden && !col.checkbox) {
                    // var headalign = col.headalign || col.align || 'left';
                    $("div:first-child", obj).css("text-align", 'center'); $("div:first-child", obj).css("font-weight", 'bold');
                }
            })
            //操作功能按钮
            var gridbtn = $('.gridBtn');
            $.each(gridbtn, function (i, r) {
                var label = $(r).attr("label");
                $(r).linkbutton({
                    plain: true,
                    text: label == "false" ? "" : $(r).attr("title"),
                    iconCls: $(r).attr("iconCls")
                });
            })

            $('#' + id).datagrid('fixRowHeight');

        },
        onClickRow: function (rowIndex, rowData) {
            if (rowIndex == selectIndex) {
                $(this).datagrid('unselectRow', selectIndex);
                selectIndex = -1;
            }
            else {
                selectIndex = rowIndex;
            }

        }
    }, options);
    $("#" + id).datagrid(opts);
    $("#" + id).datagrid('getPager').pagination({
        pageSize: 20,
        pageList: [10, 20, 50, 100], //可以设置每页记录条数的列表
        afterPageText: "页，共{pages}页",
        displayMsg: '当前{from}到{to}条，总共{total}条'
    });
    return $("#" + id);
};

//表单提交

function SubmitForm(formid, $dialog, $grid, $pjq, options) {
    if ($(('#' + formid)).form('enableValidation').form('validate')) {
        var opts = $.extend({
            success: function (data) {
                data = JSON.parse(data);
                if (data.ReturnCode == 1) {
                    $grid.datagrid('load');
                    $dialog.dialog('destroy');
                    $pjq.messager.show({
                        icon: "info",
                        title: '消息',
                        msg: data.ReturnMsg,
                        timeout: 5000,
                        showType: 'slide', position: "bottomRight"
                    });
                } else {
                    $.messager.alert('错误信息', data.ReturnMsg, 'error');
                }
            }
        }, options);
        $('#' + formid).form(opts);
        $('#' + formid).submit();
    }
}
//tooltip 
$.extend($.fn.datagrid.methods, {
    tooltip: function (jq, fields) {
        return jq.each(function () {
            var panel = $(this).datagrid('getPanel');
            if (fields && typeof fields == 'object' && fields.sort) {
                $.each(fields, function () {
                    var field = this;
                    bindEvent($('.datagrid-body td[field=' + field + '] .datagrid-cell', panel));
                });
            } else {
                bindEvent($(".datagrid-body .datagrid-cell", panel));
            }
        });

        function bindEvent(jqs) {
            jqs.mouseover(function () {
                var content = $(this).text();
                if (content) {
                    $(this).tooltip({
                        content: content,
                        trackMouse: true,
                        onHide: function () {
                            $(this).tooltip('destroy');
                        }
                    }).tooltip('show');
                }
            });

        }

    }
});
function repHtmlTag(str) {
    if (str) {
        //替换html标记
        return str.replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/&/g, "&amp;");
    }
    else
        return str;
}
function repVal(s) {   
    //if (/<\/?[^>]*>/g.test($(this).val()))
    //    $(this).val("");        
    if (s.indexOf("'") > -1) {
        return s.replace(/\'/g, "''");
    } else {
        return s
    }
}
/* 
* 解析matrix矩阵，0°-360°，返回旋转角度 
* */
function getmatrix(mtx) {    
    if (mtx == "none") return 0;
    var values = mtx.split('(')[1].split(')')[0].split(',');
    var a = values[0];
    var b = values[1];
    var c = values[2];
    var d = values[3];
    var scale = Math.sqrt(a * a + b * b);
    console.log('Scale: ' + scale);
    // arc sin, convert from radians to degrees, round
    var sin = b / scale;
    // next line works for 30deg but not 130deg (returns 50);
    // var angle = Math.round(Math.asin(sin) * (180/Math.PI));
    var angle = Math.round(Math.atan2(b, a) * (180 / Math.PI));
    return angle;
}