$(function () {
    var x = 200;
    var y = 200;   //定义放大后图片的显示位置与鼠标位置的关系
    $("a.imgPho").click(function (e) {   //也可以根据需要改为mouseover事件
        $("#tooltip").remove();
        var pageWidth = document.body.scrollWidth;
        var pageHeight = document.body.scrollHeight;
        var tleft = e.pageX - x;
        var ttop = e.pageY - y;
        tleft = tleft < 5 ? 5 : tleft;
        tleft = tleft > pageWidth - 520 ? pageWidth - 520 : tleft;
        ttop = ttop < 150 ? 150 : ttop;
        ttop = ttop > pageHeight - 500 ? pageHeight - 500 : ttop;
        var top = (ttop) + "px";
        var left = (tleft) + "px";//以上这几行代码是为了避免放大后的图片超出页面范围
        var imgTitle = this.title;
        var ni = new Image();
        ni.src = imgTitle;
        ni.width = "730";
        ni.height = "450";
        var tooltip = "<div id='tooltip' align='center'>" + ni.outerHTML + "</div>";    //可以是任意希望显示的html代码
        $("body").append(tooltip);
        ni.onload = function () {  //保证图片加载完毕，否则得到的图片大小可能为0
            //var twidth = ni.width;   //得到放大后图片的大小，让div宽度与图片宽度相同
            //var pwidth = twidth + "px";
            $("#tooltip")   //为添加的div增加位置、宽度属性
                .css({
                    "top": (top),
                    "left": (50),
                    "width": "735px",//(pwidth)
                    "height": "455px",
                }).show("fast");
        }
        $("#tooltip").mouseleave(function () { //鼠标离开div区域后即清除它
            $(this).remove();
        });
        return false;
    });
});