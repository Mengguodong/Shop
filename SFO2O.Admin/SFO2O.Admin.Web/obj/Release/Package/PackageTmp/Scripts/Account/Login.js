$(document).ready(function () {
    var $form = $("#loginForm");
    var $username = $("#username");
    var $password = $("#password");
    $form.submit(function () {
        var flag = true;
        var $focus = null;
        if ($username.val().length == 0) {
            $("#login_wrong").html("請輸入賬號");
            flag = false;
            $focus = $focus || $username;
        } else {
            $("#login_wrong").html("");
        }
        if ($password.val().length == 0) {
            $("#password_wrong").html("請輸入密碼");
            flag = false;
            $focus = $focus || $password;
        } else {
            $("#password_wrong").html("");
        }
        if ($focus) {
            $focus.focus();
        }
        return flag;
    });
})