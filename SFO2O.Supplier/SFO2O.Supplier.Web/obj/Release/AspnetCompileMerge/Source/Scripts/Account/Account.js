function ChangePassword() {
    var handle;
    handle = $.dialog({
        title: "重置密码",
        width: 470,
        content: $("#changePassword"),
        buttons: {
            text: "重置密码", isWhite: 1,
            onClick: function ($dom) {
                var $oldpass = $dom.find(".oldpass");
                var $newpass1 = $dom.find(".newpass1");
                var $newpass2 = $dom.find(".newpass2");
                var oldpass = $oldpass.val();
                var newpass = $newpass1.val();
                if (oldpass == "") {
                    $oldpass.next().text("舊密碼不能為空");
                    return false;
                }
                else {
                    $oldpass.next().text("");
                }
                if (newpass == "") {
                    $newpass1.next().text("新密碼不能為空");
                    return false;
                }
                else if (newpass.length < 6 || newpass.length > 50 || sfo2o.GetPassLevel(newpass) < 2) {
                    $newpass1.next().text("請輸入6-32位數字、字母、符號兩種以上組合");
                    return false;
                }
                else {
                    $newpass1.next().text("");
                }
                if ($newpass2.val() == "") {
                    $newpass2.next().text("確認新密碼不能為空");
                    return false;
                }
                else {
                    $newpass2.next().text("");
                }
                if ($newpass1.val() != $newpass2.val()) {
                    $newpass2.next().text("確認新密碼與新密碼不一致，請重新輸入");
                    return false;
                }
                else {
                    $newpass2.next().text("");
                }
                $.ajax({
                    url: "/Home/ChangePassword",
                    type: "post",
                    data: { oldPassword: oldpass, newPassword: newpass },
                    success: function (res) {
                        if (!res.Error) {
                            handle.close();
                            $.dialog("密碼修改成功！");
                        } else {
                            if (res.Message) {
                                $.dialog(res.Message);
                            } else {
                                if (res.OldPassword) {
                                    oldpass.next().text(res.OldPassword);
                                }
                                if (res.NewPassword) {
                                    newpass1.next().text(res.NewPassword);
                                }
                            }
                        }
                    }
                });
                return false;
            }
        }
    });
}