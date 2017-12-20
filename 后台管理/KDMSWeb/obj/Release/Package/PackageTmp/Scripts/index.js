$(function () {
    LoadMenuNav();
    GetUser();
    bindCateClick();
    LoadCateTotal("DAY");    
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
function LoadCateTotal(t) {
    $("#CateTotalTable tbody").html("");
    $.ajax({
        type: 'post',
        async: false,
        data: { time: t == undefined ? 'DAY' : t },
        dataType: 'json',
        url: '/Home/QueryCateRecordTotal',
        success: function (data) {
            var tr = '';
            if (data) {
                for (var i = 0; i < data.length; i++) {
                    tr += '<tr><td>' + data[i].Creator + '</td><td>' + data[i].Name + '</td><td>' + data[i].CateCount + '</td></tr>';
                }                
            }
            $("#CateTotalTable tbody").append(tr);
        }
    });    
}
function bindCateClick() {
    $('.layui-body .ht-box').click(function () {
        var time = $(this).attr("tm");
        if (time == "DAY") {

        } else {
            var element = layui.element;
            //var tabTitleDiv = $('.layui-tab[lay-filter=\'tab\']').children('.layui-tab-title');
            //var exist = tabTitleDiv.find('li[lay-id=' + 1 + ']');
            var tag = $('.layui-tab[lay-filter=\'tab\']').find('iframe').attr("tm");
            if (tag==time) {
                //切换到指定索引的卡片
                element.tabChange('tab', 1);
            } else {
                element.tabDelete('tab', 1);
                var index = layer.load(1);
                setTimeout(function () {
                    //模拟菜单加载
                    layer.close(index);
                    element.tabAdd('tab', { title: "菜单录入", content: '<iframe id="ifrCate" tm="'+time+'" src="/Restaurants/CateList?tm=' + time + '" style="width:100%;height:100%;border:none;outline:none;"></iframe>', id: 1 });
                    //切换到指定索引的卡片
                    element.tabChange('tab', 1);
                }, 500);
            }
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