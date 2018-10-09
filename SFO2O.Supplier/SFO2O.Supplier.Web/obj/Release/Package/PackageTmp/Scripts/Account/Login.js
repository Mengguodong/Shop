$(document).ready(function () {
    var $form = $("#loginForm");
    var $username = $("#username");
    var $password = $("#password");
    $form.submit(function () {
        var flag = true;
        var $focus = null;
        if ($username.val().length == 0) {
            $("#login_wrong").text("请输入账号");
            flag = false;
            $focus = $focus || $username;
        } else {
            $("#login_wrong").text("");
        }
        if ($password.val().length == 0) {
            $("#password_wrong").text("请输入密码");
            flag = false;
            $focus = $focus || $password;
        } else {
            $("#password_wrong").text("");
        }
        if ($focus) {
            $focus.focus();
        }
        return flag;
    });
})