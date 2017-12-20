$(function () {
    LoadMenuNav();
    GetUser();
    bindCateClick();
    LoadCateTotal();
    refreshCateTotalList("DAY");
});
function LoadMenuNav() {
    $.ajax({
        type: 'post',
        async: false,
        dataType: 'json',
        url: '/Home/GetMenuList',
        success: function (data) {
            if (data) {
                AppendMenu(data);
            }
        }
    });
}
/**
* 加载系统菜单
*/
function AppendMenu(data) {
    var arrprt = [];
    var arrchd = [];
    for (var k = 0; k < data.length; k++) {
        if (data[k].ParentID == "0")
            arrprt.push(data[k]);
        else
            arrchd.push(data[k]);
    }
    var strHtml = "";

    for (var i = 0; i < arrprt.length; i++) {
        var prtdata = arrprt[i];
        strHtml += '<li class="layui-nav-item">';
        strHtml += '<a href="javascript:;"><i class="fa ' + prtdata.MenuIcon + '"></i>' + prtdata.MenuName + '</a>';
        strHtml += '<dl class="layui-nav-child"> ';
        for (var j = 0; j < arrchd.length; j++) {
            var chddata = arrchd[j];
            if (prtdata.MenuID == chddata.ParentID) {
                strHtml += '<dd><a href="javascript:;" data-url="' + chddata.MenuUrl + '" data-id="' + chddata.Sort + '" >' + chddata.MenuName + '</a></dd>';
            }
        }
        strHtml += '</dl>';
        strHtml += '</li>';
    }
    $(".layui-nav").append(strHtml);
}
/**
* 加载该餐厅录入菜单数
*/
function LoadCateTotal() {
    //$("#CateTotalTable tbody").html("");
    //$.ajax({
    //    type: 'post',
    //    async: false,
    //    data: { time: t == undefined ? 'DAY' : t },
    //    dataType: 'json',
    //    url: '/Home/QueryCateRecordTotal',
    //    success: function (data) {
    //        var tr = '';
    //        if (data) {
    //            for (var i = 0; i < data.length; i++) {
    //                tr += '<tr><td>' + data[i].Creator + '</td><td>' + data[i].Name + '</td><td>' + data[i].CateCount + '</td></tr>';
    //            }                
    //        }
    //        $("#CateTotalTable tbody").append(tr);
    //    }
    //});
    $('#CateTotalTable').bootstrapTable({
        method: 'POST',
        contentType: "application/x-www-form-urlencoded",
        cache: false,
        //search: true,
        uniqueId: "Id",
        pageSize: "10",
        pageNumber: "1",
        pageList: [10, 15, 20, 50],
        queryParamsType: "undefined",
        sidePagination: "client",
        pagination: true,
        height: '490',
        striped: true,
        columns: [            
            {
                field: 'Creator', title: '录入人', sortable: true,algin:"center"
            },
            {
                field: 'Name', title: '餐厅名', sortable: true
            },            
            {
                field: 'CateCount', title: '菜单总数', sortable: true
            }            
        ]
    });
}
//刷新菜单列表
function refreshCateTotalList(t) {
    var opt = {
        url: "/Home/QueryCateRecordTotal",
        silent: true,
        query: {
            time: t
        }
    };
    $('#CateTotalTable').bootstrapTable('refresh', opt);
}
function bindCateClick() {
    $('.layui-body .ht-box').click(function () {
        var time = $(this).attr("tm");
        if (time == "YEAR") {
            var element = layui.element;           
            var tabTitleDiv = $('.layui-tab[lay-filter=\'tab\']').children('.layui-tab-title');
            var exist = tabTitleDiv.find('li[lay-id=' + 1 + ']');
            if (exist.length > 0) {
                //切换到指定索引的卡片
                element.tabChange('tab', 1);
            } else {
                var index = layer.load(1);               
                setTimeout(function () {
                    //模拟菜单加载
                    layer.close(index);
                    element.tabAdd('tab', { title: "菜单录入", content: '<iframe src="/Restaurants/CateList" style="width:100%;height:100%;border:none;outline:none;"></iframe>', id: 1 });
                    //切换到指定索引的卡片
                    element.tabChange('tab', 1);
                }, 500);
            }
        } else {
            refreshCateTotalList(time)
        }
        
    });
}
function setPassWord() {
    var url = "/System/ModifyPassWord";
    var tle = "用户修改密码";
    parent.layer.open({
        type: 2,
        title: [tle, "background:#1a7bb9;color:#fff;font-weight:600;font-size:16px;"],
        //title:false,
        skin: 'layer-ext-seaning',
        area: ['769px', '450px'],
        btn: ['保存', '取消'],
        yes: function (index, layero) {
            var body = parent.layer.getChildFrame('body', index);
            var iframeWin = parent.window[layero.find('iframe')[0]['name']];

            iframeWin.submitSave(window);
        }, cancel: function (index) {
            parent.layer.close(index);
        },
        content: url
    });
}