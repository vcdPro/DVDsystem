
       jQuery(document).ready(function ($){
           $('.theme-login').click(function () {
               $('.theme-popover-mask').fadeIn(100);
               $('.theme-popover').slideDown(200);
           })
           $('.theme-poptit .close').click(function () {
               $('.theme-popover-mask').fadeOut(100);
               $('.theme-popover').slideUp(200);
           })
           $('#txtUserName').blur(function () {
               ajax("../Hander/rememberme.ashx?serch=ture&username=" + document.getElementById('txtUserName'), function (responsetext) {
                   document.getElementById('txtPassword').value = responsetext;
               });
                
           })//当用户名失去焦点时候调用该方法
       })

window.onload = function onLoginLoaded()
{

    GetLastUser();

}
function search()
{
    var search = document.getElementById("search").value;

}

function getimg() {//我们就是通过这个函数来异步获取信息的
    document.getElementById("checkimg").src = "../Hander/yzheng2.ashx?" + Math.random();
}

function GetLastUser() {
    var id = "C005-7D5B-4231-8CEA-1693";//GUID标识符  setcookie(name,value,expire,path,domain,secure)
    var usr = GetCookie(id);
    if (usr != null) {
        document.getElementById('txtUserName').value = usr;
    } else {
        document.getElementById('txtUserName').value = "a1234";
    }
    GetPwdAndChk();
}

//------------点击登录时触发客户端事件
function slideup() {
    jQuery('#button').click();//---------触发button点击事件
}

function registercheckuser() {
    usernamecheck = new RegExp("^[a-zA-Z][a-zA-Z0-9_]{0,8}$");//字母或者@开头的最多9位账户
    passworldcheck = new RegExp("^[0-9][0-9]{0,8}$");//9位密码
    checkcode = new RegExp("[0-9]{5}$");

    var result = document.getElementById('txtUserName').value.match(usernamecheck);
    var result2 = document.getElementById('txtPassword').value.match(passworldcheck);
    var result3 = document.getElementById('verification').value.match(checkcode);

    if (result != null && result2 != null && result3 != null) {
        register();
    }
    else if (result == null) { alert("请检测用户名格式是否输入有误!!"); return false; }
    else if (result2 == null) { alert("请检查密码格式是否输入有误!!"); return false; }
    else { alert("请检查验证码格式是否输入有误!!"); return false; }

}
      

        
function checkuser() {
    usernamecheck = new RegExp("^@[a-zA-Z0-9_]{0,8}$|^[a-zA-Z][a-zA-Z0-9_]{0,8}$");//字母或者@开头的最多9位账户
    passworldcheck = new RegExp("^[0-9][0-9]{0,8}$");//9位密码
    checkcode = new RegExp("[0-9]{5}$");

    var result = document.getElementById('txtUserName').value.match(usernamecheck);
    var result2 = document.getElementById('txtPassword').value.match(passworldcheck);
    var result3 = document.getElementById('verification').value.match(checkcode);

    if (result != null && result2 != null && result3 != null)
    {
        lastcheck();
    }
    else if (result == null) { alert("请检测用户名格式是否输入有误!!"); return false; }
    else if (result2 == null) { alert("请检查密码格式是否输入有误!!"); return false; }
    else { alert("请检查验证码格式是否输入有误!!"); return false; }

}


function lastcheck()
{
    var username=document.getElementById("txtUserName").value;
    var password = document.getElementById("txtPassword").value;
    var checkcode = document.getElementById("verification").value;
    var admin = "";
    if (document.getElementById("Adminlogin").checked == true)
        var admin = "true";
    ajax("login.ashx?username=" + username + "&password=" + password + "&checkcode=" + checkcode+"&Admin="+admin, function (responsetext) {
        if (responsetext == "usererror") {
            document.getElementById("usererror").innerHTML = "该用户不存在或者密码错误！";
            document.getElementById("usererror").style.color = "#1e1e1e";
            document.getElementById("checkcodeerror").innerHTML = "";
        }

        else if (responsetext == "checkcodeerror") {
            document.getElementById("usererror").innerHTML = "";
            document.getElementById("checkcodeerror").innerHTML = "验证码输入有误!";
            document.getElementById("checkcodeerror").style.color = "#1e1e1e";
        }

        else if (responsetext == "ok")
        {
            document.getElementById("usererror").innerHTML = "";
            document.getElementById("checkcodeerror").innerHTML = "";

            var usr = document.getElementById('txtUserName').value;
            SetLastUser(usr);
            //如果记住密码选项被选中
            if (document.getElementById('chkRememberPwd').checked == true)
            {
                var pwd = document.getElementById('txtPassword').value;

                ajax("../Hander/rememberme.ashx?username="+usr+"&password="+pwd, function (responsetext){ });
            }

            slideup();

            window.location.href = "index.ashx";//刷新页面
        }
    })
}

function SetLastUser(usr) {
    var id = "C005-7D5B-4231-8CEA-1693";
    var expdate = new Date();
    //当前时间加上两周的时间
    expdate.setTime(expdate.getTime() + 14 * (24 * 60 * 60 * 1000));
    SetCookie(id, usr, expdate);
}

function GetPwdAndChk() {
    var usr = document.getElementById('txtUserName').value;
    var pwd = GetCookie(usr);
    if (pwd != null) {
        document.getElementById('chkRememberPwd').checked = true;
        document.getElementById('txtPassword').value = pwd;
    } else {
        document.getElementById('chkRememberPwd').checked = false;
        document.getElementById('txtPassword').value = "";
    }
}
//取Cookie的值

function GetCookie(name) {
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var i = 0;
    while (i < clen) {
        var j = i + alen;
        //alert(j);
        if (document.cookie.substring(i, j) == arg) return getCookieVal(j);
        i = document.cookie.indexOf(" ", i) + 1;
        if (i == 0) break;
    }
    return null;
}

function getCookieVal(offset) {
    var endstr = document.cookie.indexOf(";", offset);
    if (endstr == -1) endstr = document.cookie.length;
    return unescape(document.cookie.substring(offset, endstr));
}
//写入到Cookie

function SetCookie(name, value, expires) {

    var argv = SetCookie.arguments;
    //本例中length = 3
    var argc = SetCookie.arguments.length;
    var expires = (argc > 2) ? argv[2] : null;
    var path = (argc > 3) ? argv[3] : null;
    var domain = (argc > 4) ? argv[4] : null;
    var secure = (argc > 5) ? argv[5] : false;

    document.cookie = name + "=" + escape(value) + ((expires == null) ? "" : ("; expires=" + expires.toGMTString())) + ((path == null) ? "" : ("; path=" + path)) + ((domain == null) ? "" : ("; domain=" + domain)) + ((secure == true) ? "; secure" : "");
} //自定义属性编码


function ResetCookie() {
    var usr = document.getElementById('txtUserName').value;
    var expdate = new Date();
    SetCookie(usr, null, expdate);
}


function prevent(){
    jQuery('.theme-popover-mask').fadeIn(100);
    jQuery('.theme-popover').slideDown(200);
}

function logout() {

    ajax("login.ashx?deletesesion=true", function (responsetext){})
    window.location.href = "index.ashx";
}
