var ArrInput = [
    { "title": "有无分店", "text": "0", "type": "radio" }, { "title": "是否需要走楼梯(无电梯)", "text": ["是", "否"], "type": "radio" },
    { "title": "候位区", "text": ["无座", "店外座位", "店内座位"], "type": "radio" }, { "title": "衣帽间", "text": "0", "type": "radio" },
    { "title": "园景(饭店区域内的花园)", "text": "0", "type": "radio" }, { "title": "背景音乐", "text": "0", "type": "radio" },
    { "title": "空气清新度", "text": ["异味", "香味", "无味"], "type": "radio" }, { "title": "服务人员形象", "text": ["服装统一", "服装整洁", "长相令人舒服", "无特色"], "type": "checkbox" },
    { "title": "邻桌动线有无干扰", "text": "0", "type": "radio" },{ "title": "传菜有无干扰", "text": "0", "type": "radio" },
    { "title": "空间感(高度)", "text": "红外测距 设计师——标准高度", "type": "text" }, { "title": "照明", "text": ["背景照明", "泛照明", "聚光"], "type": "checkbox" },
    { "title": "采光", "text": "0", "type": "radio" }, { "title": "软装品质", "text": ["高", "中", "低"], "type": "radio" },
    { "title": "硬装质感", "text": ["高", "中", "低"], "type": "radio" }, { "title": "地域特征", "text": ["江南", "新疆", "日式", "欧式"], "type": "radio" },
    { "title": "文化艺术", "text": ["现代", "怀旧"], "type": "radio" }, { "title": "餐具档次", "text": ["品牌餐具", "定制餐具", "普通餐具"], "type": "radio" },
    { "title": "餐具特色", "text": ["有图案", "有花纹", "有主题", "异形", "无特色"], "type": "checkbox" },
    { "title": "菜单呈现", "text": ["有照片", "有簿册", "有电子簿册", "有设计感（封皮质地，纸张，VI结构）", "简易菜单"], "type": "checkbox" },
    { "title": "桌子舒适度", "text": ["拥挤", "适中", "宽松"], "type": "radio" },
    { "title": "椅子舒适度", "text": ["椅子位置可调", "有腰撑托背感", "手肘有支撑", "有靠垫", "符合人体力学", "有碰撞阻碍", "普通"], "type": "radio" },
    { "title": "移动拼桌", "text": ["支持", "不支持"], "type": "radio" },
    { "title": "衣服椅套", "text": "0", "type": "radio" },
    { "title": "御冷披肩", "text": "0", "type": "radio" },
    { "title": "舞台", "text": "0", "type": "radio" },
    { "title": "吧台", "text": "0", "type": "radio" },
    { "title": "演奏曲", "text": "0", "type": "radio" },
    { "title": "播放区", "text": "0", "type": "radio" },
    { "title": "露台", "text": "0", "type": "radio" },
    { "title": "露台景观", "text": "0", "type": "radio" },
    { "title": "儿童娱乐区", "text": "0", "type": "radio" },
    { "title": "厨房", "text": "0", "type": "radio" },
    { "title": "包房", "text": "0", "type": "radio" },
    { "title": "包房卫生间", "text": "0", "type": "radio" },
    { "title": "卫生间距离", "text": ["店外", "店内"], "type": "radio" },
    {
        "title": "卫生间室内",
        "text": ["有香味", "有异味", "无味", "有服务", "有显示屏", "有设计感", "有背景音乐"            
            ],
        "type": "checkbox"
    },
    {
        "title": "卫生间洗手台", "text": ["有热水", "有洗手液", "有纸巾", "有干手器", "有梳妆镜"], "type": "checkbox"
    },
    { "title": "卫生间坐厕间", "text": ["智能马桶", "普通马桶", "蹲便", "挂衣钩", "手机架", "卫生纸", "性别区分", "有独立功能区", "有独立母婴残疾区"], "type": "checkbox" },
    { "title": "儿童椅", "text": "0", "type": "radio" },
    { "title": "儿童餐具", "text": "0", "type": "radio" },
    { "title": "可带宠物", "text": "0", "type": "radio" },
    { "title": "免费WIFI", "text": "0", "type": "radio" },
    { "title": "停车(推荐地)", "text": "地点", "type": "text" },
    { "title": "停车福利", "text": "0", "type": "radio" },
    { "title": "娱乐演艺", "text": "0", "type": "radio" },
    { "title": "厨师表演", "text": "0", "type": "radio" },
    { "title": "招牌菜", "text": "", "type": "textarea" },
    { "title": "人气菜", "text": "", "type": "textarea" },
    { "title": "新菜", "text": "", "type": "textarea"  },
    { "title": "有无隐藏菜单", "text": "0", "type": "radio" },
    { "title": "能否接受预订", "text": ["能", "不能"], "type": "radio" },
    { "title": "最大单桌人数", "text": "", "type": "text"},
    { "title": "接待桌数", "text": "", "type": "text" },
    { "title": "店内面积", "text": "", "type": "text"}
];//, "mstinput": "1"
function getVal() {
    //var arrName = strTxt.split(',');
    //var arr = [];
    //var arr1 = [];
    //for (var i = 0; i < arrName.length; i++) {   

    //    if (arrName[i] == "0" || arrName[i] == "1") {
    //        arr1.push(arrName[i]);
    //    } else {
    //        if (arr1.length > 0 ) {
    //            arr.push(arr1);
    //        }
    //        arr.push(arrName[i]);
    //    }        

    //}    
    //var input_list = [
    //    { "title": "标题1", "text": ["值1", "值2"] },
    //    { "title": "标题2", "text": "取值2" }
    //];
    //for (var i in input_list) {
    //    var txt = input_list[i]       
    //    if (txt.text.length > 1 && typeof (txt.text) == "object") {
    //        var txt1 = txt.text;
    //        for (var j in txt1) {

    //            console.log(txt1[j]);
    //        }
    //    }
    //}
    

}

var strType = "textChk,radio,checkbox";
/**
 * 加载HTML表单
 */
function SetFormHtml() {    
    var input_html = "";
    for (var i in ArrInput) {
        var arrtext = ArrInput[i];
        if (arrtext.type == "text" || arrtext.type == "textarea") {
            input_html += getTextHtml(arrtext.title, arrtext.text, arrtext.type);
        } else if (arrtext.type =="radio") {
            input_html += getRadioHtml(arrtext.title, arrtext.text);
        } else if (arrtext.type == "checkbox") {
            input_html += getCheckedHtml(arrtext.title, arrtext.text);
        }
        
    }
    $('.weui-cells_form').append(input_html);
}
/**
*  修改表单赋值
*   
*/
function loadShopData(resid) {
    $.post("/App/LoadShopRecord", { id: resid }, function (data) {
        if (data.ReturnData.res) {
            var dt = data.ReturnData.envinfo;
            var basedt = data.ReturnData.res;
            //餐厅基础数据
            $("#RestaurantName").val(basedt.Name);
            $("#DPId").val(basedt.DPId);
            $("#MeanPrice").val(basedt.MeanPrice);
            $("#Address").val(basedt.Address);
            if (action == "blk_det") {
                $("#UserName").html("录入人：" + basedt.Creator);
            }
            if (data.ReturnData.envinfo) {
                //餐厅环境数据
                for (var i in ArrInput) {
                    var arrdata = ArrInput[i];
                    setFormHtml(arrdata.title, arrdata.text, arrdata.type, dt)
                }
            } else {
                $.toptip('该餐厅暂无环境数据');
            }            
        } else {
            $.toptip('暂无数据');
        }
    })
}
/**
*  赋值页面表单
*   
*/
function setFormHtml(name, valarr, t, dt) {
    name = repText(name);
    if (t == "text" || t == "textarea") {
        $('#' + name).val(dt[name]);
    } else if (t == "radio") {
        $(':radio[name="' + name + '"][value="' + dt[name] + '"]').prop("checked", "checked");
    } else if (t == "checkbox") {
        for (var j in valarr) {
            var _val = Math.pow(2, parseInt(j));//得到2的N次方
            //按与位判断相等的复选框选中
            var exp = dt[name] & _val;//需要赋值再判断，否则条件放一起判断不会返回bool
            if (exp>0) {
                $(':checkbox[name="' + name + '"][value="' + _val + '"]').prop("checked", "checked");
            }
        }
        
    }
}
function repText(txt) {
    //var newtxt = txt;
    if (txt.indexOf('(') > -1 || txt.indexOf(')') > -1) {
        return txt.replace('(', '_').replace(')', '_');
    } else {
        return txt;
    }
}
//function groupJson() {
//    var name = [];
//    var val = [];
//    for (var i in ArrInput) {
//        var arrtext = ArrInput[i];
//        name.push(arrtext.title);
//        if (arrtext.text == "0") {
            
//        } else {
//            var arrtexts = arrtext.text[i];
//            for (var j in arrtexts) {

//            }
//        }
              
//        params.push({ "group": i, "param": param });  
//    }
//    $('#hidArr').val();
//}
/**
 * 获取input TextHtml
 * @param {any} name
 * @param {any} txt
 */
function getTextHtml(name, txt, t) {
    var textT = "";
    var newName = repText(name);
    var strText = "<input class=\"weui-input\" type=\"text\" id=\"" + newName + "\" name=\"" + newName + "\" placeholder=\"" + txt + "\">";
    if (t == "textarea") {
        strText = "<textarea class=\"weui-textarea\" type=\"textarea\" id=\"" + newName + "\" name=\"" + newName + "\" placeholder=\"" + txt + "\"></textarea>";
    }
    var mstInput = mstInput = "<span style=\"color:red;\">*</span>";
    if (name == "新菜" || name == "店内面积") {
        mstInput = "";
    }
    textT += " <div class=\"weui-cell\">";
    textT += "                <div class=\"weui-cell__hd\">";
    textT += "                    <label class=\"weui-label\" style=\"width:124px;\">" + mstInput + "" + name + "</label>";
    textT += "                </div>";
    textT += "                <div class=\"weui-cell__bd\">";
    textT += strText;
    textT += "                </div>";
    textT += "            </div>";
    return textT;
}
function shwTxt(c) {
    var att = $(c).find('p').text();
    if (att == "其他")
        $("#txtcell").show();
    else
        $("#txtcell").hide();
}
/**
 * 获取input RadioHtml
 * @param {any} name
 * @param {any} txtarr
 * @param {any} t
 */
function getRadioHtml(name,txtarr) {
    var textRdo = "";
    var newName = repText(name);
    textRdo += "<div class=\"weui-cells__title\" style=\"font-size:16px;\"><span style=\"color:red;\">*</span>" + name + "</div>";
    textRdo += "    <div class=\"weui-cells weui-cells_radio\">";
    if (txtarr.length > 2) {
        var idx = 0;
        for (var i in txtarr) {
            //var other = "";
            //var oclick = "";
            //if (txtarr[i] == "其他") {
            //    oclick = 'onclick="shwTxt(this)"';
            //    other += "<div class=\"weui-cell\" name=\"" + newName + "\" id=\"txtcell\" style=\"display:none;\">";
            //    other += "                <div class=\"weui-cell__bd\">";
            //    other += "                    <input class=\"weui-input\" type=\"text\" placeholder=\"请输入\" >";
            //    other += "                </div>";
            //    other += "            </div>";
            //}
            idx += 1;
            textRdo += "            <label class=\"weui-cell weui-check__label\" for=\"" + newName + idx + "\" >";
            textRdo += "                <div class=\"weui-cell__bd\">";
            textRdo += "                    <p>" + txtarr[i] + "</p>";
            textRdo += "                </div>";
            textRdo += "                <div class=\"weui-cell__ft\">";
            textRdo += "                    <input type=\"radio\" class=\"weui-check\" name=\"" + newName + "\" id=\"" + newName + idx + "\" value=\"" + i + "\">";
            textRdo += "                    <span class=\"weui-icon-checked\"></span>";
            textRdo += "                </div>";
            textRdo += "            </label>";
            //textRdo += other; 
        }
    } else {
        var ptxt = "<p>" + txtarr[0] + "</p>";
        var ptxt1 = "<p>" + txtarr[1] + "</p>";
        if (txtarr == "0") {
            var ptxt = "<p>有</p>";
            var ptxt1 = "<p>无</p>";
        }
        textRdo += "            <label class=\"weui-cell weui-check__label\" for=\"" + newName + "1\">";
        textRdo += "                <div class=\"weui-cell__bd\">";
        textRdo += ptxt;
        textRdo += "                </div>";
        textRdo += "                <div class=\"weui-cell__ft\">";
        textRdo += "                    <input type=\"radio\" class=\"weui-check\" name=\"" + newName + "\" id=\"" + newName + "1\" value=\"1\">";
        textRdo += "                    <span class=\"weui-icon-checked\"></span>";
        textRdo += "                </div>";
        textRdo += "            </label>";
        textRdo += "            <label class=\"weui-cell weui-check__label\" for=\"" + newName + "2\">";
        textRdo += "                <div class=\"weui-cell__bd\">";
        textRdo += ptxt1;
        textRdo += "                </div>";
        textRdo += "                <div class=\"weui-cell__ft\">";
        textRdo += "                    <input type=\"radio\" name=\"" + newName + "\" class=\"weui-check\" id=\"" + newName + "2\" value=\"0\">";//checked=\"checked\"
        textRdo += "                    <span class=\"weui-icon-checked\"></span>";
        textRdo += "                </div>";
        textRdo += "            </label>";
    }
    
    textRdo += "        </div> ";
    return textRdo;
}
/**
 * 获取input CheckedHtml
 * @param {any} name
 * @param {any} txtarr
 * @param {any} t
 */
function getCheckedHtml(name, txtarr) {
    var textChk = "";
    var newName = repText(name);
    textChk += "<div class=\"weui-cells__title\" style=\"font-size:16px;\"><span style=\"color:red;\">*</span>" + name + "</div>";
    textChk += "    <div class=\"weui-cells weui-cells_checkbox\">";
    var idx = 0;
    for (var i in txtarr) { 
        idx += 1;
        var _val = Math.pow(2, parseInt(i));//计算value：2的N次方
        textChk += "        <label class=\"weui-cell weui-check__label\" for=\"" + newName + idx + "\">";
        textChk += "            <div class=\"weui-cell__hd\">";
        textChk += "                <input type=\"checkbox\" class=\"weui-check\" name=\"" + newName + "\" id=\"" + newName + idx + "\" value=\"" + _val +"\">";
        textChk += "                <i class=\"weui-icon-checked\"></i>";
        textChk += "            </div>";
        textChk += "            <div class=\"weui-cell__bd\">";
        textChk += "                <p>" + txtarr[i] + "</p>";
        textChk += "            </div>";
        textChk += "        </label>";       
    }
    textChk += "    </div>";
    return textChk;
}

