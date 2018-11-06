function ChangePassword() {
    var handle;
    handle = $.dialog({
        title: "重置密碼",
        width: 470,
        content: $("#changePassword"),
        buttons: {
            text: "重置密碼", isWhite: 1,
            onClick: function ($dom) {
                var oldpass = $dom.find(".oldpass");
                var newpass1 = $dom.find(".newpass1");
                var newpass2 = $dom.find(".newpass2");
                if (oldpass.val() == "") {
                    oldpass.next().text("舊密碼不能為空");
                    return false;
                }
                else {
                    oldpass.next().text("");
                }
                if (newpass1.val() == "") {
                    newpass1.next().text("新密碼不能為空");
                    return false;
                }
                else {
                    newpass1.next().text("");
                }
                if (newpass2.val() == "") {
                    newpass2.next().text("確認新密碼不能為空");
                    return false;
                }
                else {
                    newpass2.next().text("");
                }
                if (newpass1.val() != newpass2.val()) {
                    newpass2.next().text("確認新密碼與新密碼不一致，請重新輸入");
                    return false;
                }
                else {
                    newpass2.next().text("");
                }
                var oldPass = oldpass.val();
                var newpass = newpass1.val();
                $.ajax({
                    url: "/Home/ChangePassword",
                    type: "post",
                    data: { oldPassword: oldpass.val(), newPassword: newpass1.val() },
                    success: function (res) {
                        if (!res.Error) {
                            handle.close();
                            $.dialog("密碼修改成功！");
                        } else {
                            if (res.Message) {
                                $.dialog(res.Message);
                            } else {
                                $.dialog("密碼修改失敗！");
                            }
                        }
                    }
                });
                return false;
            }
        }
    });
}