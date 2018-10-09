var mobile, regioncode, validcode, email, password, confirmPassword, gotoUrl, referrerNumber, mobilePhone,token,
    getCodeUrl = window.hostname + "/Account/RegistGeneratorCode",//获取验证码地址
    checkCodeUrl = window.hostname + "/Account/CheckValidateCode",//验证校验码地址
    regSubmitUrl = window.hostname + "/Account/registersave",//完成注册地址
    findPwdUrl = window.hostname + "/Account/FindPassword",//找回密码地址
    returnUrl = "";//注册成功回跳页面
function gotoPage() {
    if ($("#errorTipdialog").length > 0) {
        $("#errorTipdialog,#errorTipdialogdialogBackground").remove();
    }
    if (returnUrl) {
        gotoUrl = unescape(returnUrl);
    }
    window.location.href = gotoUrl;
}
$().ready(function () {
    mobile = FSH.tools.request("mobile");
    regioncode = FSH.tools.request("regionCode");
    validcode = FSH.tools.request("validCode");
    returnUrl = FSH.tools.request("Return_Url");
    token = $("#token").val();
    mobilePhone = $("#mobilePhone").val();

    $("#selectedP").click(function () {
        $(this).siblings("#selectItems").slideDown();
    })
    $("#selectItems p").click(function () {
        regioncode = $(this).attr("_selectedId");
        $(this).parents("#selectItems").siblings("#selectedP").find(".selectItem").html($(this).find(".selectItem").html());
        $(this).addClass("selected").siblings(".selected").removeClass("selected");
        $("#selectItems").slideUp();
    })
    $("#checkbox").click(function () {
        if ($(this).hasClass("checked")) {
            $(this).removeClass("checked");
            //$("#getCode").addClass("defaultBtn");
        } else {
            $(this).addClass("checked");
            if (FSH.string.Trim($("#tel").val())) {
                $("#getCode").removeClass("defaultBtn");
            }
        }
    })
    //keyup验证 按钮点击验证 针对各个项验证
    $("#MContainer").on("keyup", "input,#checkbox", function () {
        //验证手机号
        if ($(this).is("#tel")) {
            if (FSH.string.Trim($(this).val())) {
                $("#getCode").removeClass("defaultBtn");
            } else {
                $("#getCode").addClass("defaultBtn");
            }
            return false;
        }
        //验证校验码
        if ($(this).is("#code")) {
            if (FSH.string.Trim($(this).val())) {
                $("#checkCodeBtn").removeClass("defaultBtn");
            } else {
                $("#checkCodeBtn").addClass("defaultBtn");
            }
            return false;
        }
        //验证电子邮箱
        if ($(this).is("#email")) {
            if (FSH.string.Trim($("#email").val()) && FSH.string.Trim($("#pwd").val()) && FSH.string.Trim($("#pwd2").val())) {
                $("#submitBtn").removeClass("defaultBtn");
            } else {
                $("#submitBtn").addClass("defaultBtn");
            }
            return false;
        }
        //验证登录密码
        if ($(this).is("#pwd")) {
            if ($("#email").length > 0) {
                //注册验证
                if (FSH.string.Trim($("#email").val()) && FSH.string.Trim($("#pwd").val()) && FSH.string.Trim($("#pwd2").val())) {
                    $("#submitBtn").removeClass("defaultBtn");
                } else {
                    $("#submitBtn").addClass("defaultBtn");
                }
            } else {
                //忘记密码验证
                if (FSH.string.Trim($("#pwd").val()) && FSH.string.Trim($("#pwd2").val())) {
                    $("#submitBtn").removeClass("defaultBtn");
                } else {
                    $("#submitBtn").addClass("defaultBtn");
                }
            }
            return false;
        }
        //验证确认密码
        if ($(this).is("#pwd2")) {
            if ($("#email").length > 0) {
                //注册验证
                if (FSH.string.Trim($("#email").val()) && FSH.string.Trim($("#pwd").val()) && FSH.string.Trim($("#pwd2").val())) {
                    $("#submitBtn").removeClass("defaultBtn");
                } else {
                    $("#submitBtn").addClass("defaultBtn");
                }
            } else {
                //忘记密码验证
                if (FSH.string.Trim($("#pwd").val()) && FSH.string.Trim($("#pwd2").val())) {
                    $("#submitBtn").removeClass("defaultBtn");
                } else {
                    $("#submitBtn").addClass("defaultBtn");
                }
            }
            return false;
        }
    })
    $("#getCode").click(function () {
        //第一步提交
        if (!$(this).hasClass("defaultBtn")) {
            if (!FSH.tools.isPhone($("#tel").val())) {
                FSH.smallPrompt("请输入正确的手机号码");
                return false;
            } else {
                mobile = FSH.string.Trim($("#tel").val());
            }
            if (!$("#checkbox").hasClass("checked") && $("#checkbox").length > 0) {
                FSH.smallPrompt("请阅读并接受相关协议");
                return false;
            }
            if (!regioncode) {
                regioncode = 86;
            }
            getCode();
        }
    })
    //填写校验码页面
    if ($("#getCodeBtn2").length > 0) {
        var len = mobile.length,
            substr1 = mobile.substring(0, 2),
            substr2 = mobile.substring(len - 2, len);
        for (var i = 0; i < len - 2 - 2; i++) {
            substr1 += "*"
        }
        $("#telnum").html(substr1 + substr2)
        countdown();
    }
    $("#getCodeBtn2").click(function () {
        if (!$(this).hasClass("defaultBtn")) {
            getCode();
        }
    })
    //第二步提交
    $("#checkCodeBtn").click(function () {
        if (!$(this).hasClass("defaultBtn")) {
            if (!FSH.string.Trim($("#code").val())) {
                FSH.smallPrompt("请输入短信校验码");
                return false;
            }
            validcode = $("#code").val();
            checkCode();
        }
    })
    //第三步提交
    $("#submitBtn").click(function () {
        if (!$(this).hasClass("defaultBtn")) {
            if ($("#email").length > 0) {
                if (!FSH.tools.isEmail($("#email").val())) {
                    FSH.smallPrompt("请输入正确的电子邮箱");
                    return false;
                }
            }
            if (!FSH.tools.isPassword($("#pwd").val())) {
                FSH.smallPrompt("请输入正确的登录密码");
                return false;
            }
            if (FSH.string.Trim($("#pwd2").val()) != FSH.string.Trim($("#pwd").val())) {
                FSH.smallPrompt("确认密码与登录密码不同，请重新输入");
                return false;
            }
            
            if (source == 0) {
                referrerNumber = FSH.string.Trim($("#referrerNumber").val());
                if (referrerNumber != '') {
                    if (!FSH.tools.isPhone(referrerNumber)) {
                        FSH.smallPrompt("请输入正确的推荐人手机号");
                        return false;
                    }
                }
                //完成注册
                submitFun(regSubmitUrl);
            } else {
                //完成找回密码
                submitFun(findPwdUrl);
            }

        }
    })


})
function getCode() {
    if (ajaxFlag) {
        //获取校验码
        ajaxFlag = false;
        FSH.Ajax({
            url: getCodeUrl,
            dataType: 'json',
            data: { mobile: mobile, source: source, regioncode: regioncode, mobilePhone: mobilePhone,token:token },
            jsonp: 'callback',
            jsonpCallback: "success_jsonpCallback",
            success: function (json) {
                if (json.Type == 1) {
                    if (!$("#getCodeBtn2").length > 0) {
                        var t = "";
                        if (returnUrl) {
                            t = "&Return_Url=" + returnUrl;
                        }
                        window.location.href = window.hostname + json.LinkUrl + t;
                    } else {
                        countdown();
                    }

                } else {
                    FSH.commonDialog(1, [json.Content]);
                }

            },
            error: function (err) {
                FSH.commonDialog(1, ['请求超时，请刷新页面']);
            }
        })
    } else {
        FSH.smallPrompt("正在努力请求中，请耐心等待")
    }

}
function checkCode() {
    //验证校验码
    if (ajaxFlag) {
        //获取校验码
        ajaxFlag = false;
        FSH.Ajax({
            url: checkCodeUrl,
            dataType: 'json',
            data: { mobile: mobile, source: source, regionCode: regioncode, validcode: FSH.string.Trim(validcode), mobilePhone: mobilePhone, token: token },
            jsonp: 'callback',
            jsonpCallback: "success_jsonpCallback2",
            success: function (json) {
                if (json.Type == 1) {
                    var t = "";
                    if (returnUrl) {
                        t = "&Return_Url=" + returnUrl;
                    }
                    window.location.href = window.hostname + json.LinkUrl + t;
                } else {
                    FSH.commonDialog(1, [json.Content]);
                }

            },
            error: function (err) {
                FSH.commonDialog(1, ['请求超时，请刷新页面']);
            }
        })
    } else {
        FSH.smallPrompt("正在努力请求中，请耐心等待")
    }
}
function submitFun(url) {
    if (source == 1) {
        email = "";
    } else {
        email = FSH.string.Trim($("#email").val());
        referrerNumber = FSH.string.Trim($("#referrerNumber").val());
    }
    password = FSH.string.Trim($("#pwd").val());
    confirmPassword = FSH.string.Trim($("#pwd2").val());

    //提交数据
    if (ajaxFlag) {
        //获取校验码
        ajaxFlag = false;
        var jsonModel = { "mobilecode": mobile, "regioncode": regioncode, "validcode": validcode, "password": password, "confirmPassword": confirmPassword, "email": email, "referrerNumber": referrerNumber };
        FSH.Ajax({
            url: url,
            type: 'post',
            dataType: 'json',
            data: { jsonModel: JSON.stringify(jsonModel),token:token },
            jsonp: 'callback',
            jsonpCallback: "success_jsonpCallback3",
            success: function (json) {
                if (json.Type == 1) {
                    gotoUrl = window.hostname;
                    if (json.LinkUrl) {
                        gotoUrl = json.LinkUrl;
                    }
                    if ($("#errorTipdialog").length > 0) {
                        $("#errorTipdialog,#errorTipdialogdialogBackground").remove();
                    }
                    if (source == 0) {
                        var str = "恭喜，" + mobile + " 已注册成功！";
                        var str2 = "";
                        if (json.ReturnGift == 1) {
                            //str2='爱玖网送您 '+ json.GiftNum +' 张 <span style="color:#f75e26;">'+ json.GiftAmount +'</span> 元优惠券，快去使用吧。';
                            str2 = '您收到 <span style="color:#f75e26;">' + json.GiftAmount + '</span> 元优惠券礼包，快去使用吧。';
                        } else {
                            str2 = '你可以在爱玖网平台购买商品。';
                        }
                        FSH.commonDialog(1, [str, str2], '', 'gotoPage', '知道了');
                    } else {
                        var str = "恭喜，新密码设置成功！";
                        FSH.commonDialog(1, [str, '请牢记您新设置的密码'], '', 'gotoPage', '重新登录');
                    }

                } else {
                    FSH.commonDialog(1, [json.Content]);
                }

            },
            error: function (err) {
                FSH.commonDialog(1, ['请求超时，请刷新页面']);
            }
        })
    } else {
        FSH.smallPrompt("正在努力请求中，请耐心等待")
    }
}
//倒计时
function countdown() {
    $("#getCodeBtn2").addClass("defaultBtn").html('重新获取(<span id="num">179</span>)');
    countdownTimer = setInterval(function () { numreduce(); }, 1000)
}
function numreduce() {
    var num = $("#num").html();
    if (num > 1) {
        num--;
        $("#num").html(num);
    } else {
        clearInterval(countdownTimer);
        $("#getCodeBtn2").removeClass("defaultBtn").html("重新获取");
    }
}