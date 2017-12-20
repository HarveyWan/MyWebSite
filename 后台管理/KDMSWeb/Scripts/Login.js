$(function () {
    var success = getQueryString('success');
    var msg = "请求地址错误！";
    if (success == "0") {
        msg = "登陆失败，登录帐号或密码错误！";
    } else if (success == "1") {
        msg = "用户已被管理员禁用，请联系管理员！";
    }
    if (success) {
        //Alert(msg);
        $(".alert").fadeIn("slow").children("strong").html(msg);        
        $(".alert").fadeOut(3000);
    }    
   
});
function Login() {    
    $('#LoginForm').submit();
}
function KeyDown() {
    document.onkeydown = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {
            Login();
        }
    }
}
function checklogin() {
    if ($.trim(LoginForm.LoginName.value) != "" && $.trim(LoginForm.Password.value) != "") {
        return true
    } else {
        LoginForm.LoginName.focus();
        if ($.trim(LoginForm.LoginName.value) != "" && $.trim(LoginForm.Password.value) == "") LoginForm.Password.focus();
        $(".alert").fadeIn("slow").children("strong").html("请输入登录帐号或密码");
        $(".alert").fadeOut(2000);
        return false
    }
}var getQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};