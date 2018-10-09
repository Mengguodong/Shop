$(document).ready(function () {
    $("#submitInfo").click(function () {
        $(".wrong_tips").text("").hide();

        if (validateInput() == true) {
            submitSupplier();
        }
    });
    BindingBlurEvents();
});

function submitSupplier() {
    var json = {};

    json.SupplierID = $("#supplierID").val();
    json.UserName = $("#userName").val();
    json.passWord = $("#passWordFirst").val();

    json.CompanyName = $("#companyName").val();
    json.CompanynameSample = $("#companynameSample").val();
    json.CompanyNameEnglish = $("#companyNameEnglish").val();

    json.Address = $("#address").val();
    json.AddressSample = $("#addressSample").val();
    json.AddressEnglish = $("#addressEnglish").val();

    json.ContactTel = $("#contactTel").val();
    json.ContactPhone = $("#contactPhone").val();
    json.ContactFax = $("#contactFax").val();

    json.ConnectPeople = $("#connectPeople").val();
    json.ConnectPeopleSample = $("#connectPeopleSample").val();
    json.ConnectPeopleEnglish = $("#connectPeopleEnglish").val();

    $.ajax({
        type: 'POST',
        url: "/supplier/SaveSupplier",
        data: "json=" + escape(JSON.stringify(json)),
        async: true,
        success: function (data) {
            showOverlay();
            $("#warning").show();
            adjust("#warning");

            if (data.Success == true) {
                $("#message").text("").text("商家保存成功");

                $("#temBtn").click(function () {
                    MessageButtonClick(true);
                });
            }
            else {

                $("#message").text("").text("商家保存失敗");

                $("#temBtn").click(function () {
                    MessageButtonClick(false);
                });
            }
        }
    });
}

function MessageButtonClick(isCloseWindow) {
    closeWarning();

    this

    if (isCloseWindow == true) {
        window.location.href = "/Supplier/SupplierQuery";
    }
}

function closeWarning() {
    hideOverlay();
    $("#warning").hide();
    $("#message").text("");
}

function BindingBlurEvents() {
    $("#userName").blur(function () {
        validateUserName();
    }).focus(function () {
        $("#userName").next().text("").hide();
    });

    $("#passWordFirst").blur(function () {
        validatePws();
    }).focus(function () {
        $("#passWordFirst").next().text("").hide();
    });

    $("#passWordSecond").blur(function () {
        validatePws();
    }).focus(function () {
        $("#passWordSecond").next().text("").hide();
    });

    //公司名称
    $("#companyName").blur(function () {
        validateCompanyName();
    }).focus(function () {
        $("#companyTable").next().text("").hide();
    });

    $("#companynameSample").blur(function () {
        validateCompanyName();
    }).focus(function () {
        $("#companyTable").next().text("").hide();
    });

    $("#companyNameEnglish").blur(function () {
        validateCompanyName();
    }).focus(function () {
        $("#companyTable").next().text("").hide();
    });

    //公司地址
    $("#address").blur(function () {
        validateCompanyAddress();
    }).focus(function () {
        $("#addressTable").next().text("").hide();
    });

    $("#addressSample").blur(function () {
        validateCompanyAddress();
    }).focus(function () {
        $("#addressTable").next().text("").hide();
    });

    $("#addressEnglish").blur(function () {
        validateCompanyAddress();
    }).focus(function () {
        $("#addressTable").next().text("").hide();
    });

    //电话
    $("#contactTel").blur(function () {
        validatePhone("contactTel", "公司電話");
    }).focus(function () {
        $("#contactTel").next().text("").hide();
    });

    //手机
    $("#contactPhone").blur(function () {
        validatePhone("contactPhone", "手提電話");
    }).focus(function () {
        $("#contactPhone").next().text("").hide();
    });

    //传真
    $("#contactFax").blur(function () {
        validatePhone("contactFax", "傳真號碼");
    }).focus(function () {
        $("#contactFax").next().text("").hide();
    });

    //联络人
    $("#connectPeople").blur(function () {
        validateConnectPeople();
    }).focus(function () {
        $("#connectTable").next().text("").hide();
    });

}

function validateInput() {
    if (validateUserName() == false) {
        return false;
    }

    if (validatePws() == false) {
        return false;
    }

    if (validateCompanyName() == false) {
        return false;
    }

    if (validateCompanyAddress() == false) {
        return false;
    }

    if (validatePhone("contactTel", "公司電話") == false && validatePhone("contactPhone", "手提電話") == false && validatePhone("contactFax", "傳真號碼") == false) {
        return false;
    }

    if (validateConnectPeople() == false) {
        return false;
    }

    return true;
}

function validateUserName() {
    var userName = $("#userName");
    if ($("#supplierID").val() <= 0) {
        if (validate.required(userName) <= 0) {
            validate.ShowError(userName, "商家登入賬號不能為空");
            return false;
        }

        if (validate.email(userName) == false) {
            validate.ShowError(userName, "商家登入賬號必須為有效的Email地址");
            return false;
        }

        if (checkOnlyEmail(userName.val()) == false) {
            validate.ShowError(userName, "此賬號已被占用");
            return false;
        }
    }

    return true;
}

function validatePws() {

    var pwF = $("#passWordFirst");
    var isCheckPw = false;
    if ($("#supplierID").val() <= 0) {
        isCheckPw = true;
    }

    if ($("#supplierID").val() > 0 && validate.required(pwF) > 0) {
        isCheckPw = true;
    }

    if (isCheckPw == true) {

        if (validate.required(pwF) <= 0) {
            validate.ShowError(pwF, "密碼不能為空");
            return false;
        }

        if (validate.getLength(pwF) < 6) {
            validate.ShowError(pwF, "請輸入6-32位數字、字母、符號兩種以上組合");
            return false;
        }

        if (validate.getLength(pwF) > 32) {
            validate.ShowError(pwF, "請輸入6-32位數字、字母、符號兩種以上組合");
            return false;
        }

        if (validate.passwordT(pwF) == false) {
            validate.ShowError(pwF, "請輸入6-32位數字、字母、符號兩種以上組合");
            return false;
        }

        var pwS = $("#passWordSecond")
        if (validate.required(pwS) <= 0) {
            validate.ShowError(pwS, "密碼不能為空");
            return false;
        }

        //if (validate.passwordT(pwS) == false) {
        //    validate.ShowError(pwS, "請輸入6-32位數字、字母、符號兩種以上組合");
        //    return false;
        //}

        if (validate.equalTo(pwF, pwS) == false) {
            validate.ShowError(pwS, "兩次密碼不一致");
            return false;
        }
    }

    return true;
}

function validateCompanyName() {
    if ($("#supplierID").val() <= 0) {

        var companyName = $("#companyName");
        var companyName_S = $("#companynameSample");
        var companyName_E = $("#companyNameEnglish");
        var companyTable = $("#companyTable");

        if (validate.required(companyName) == false) {
            validate.ShowError(companyTable, "公司名稱 繁體 不能為空");
            //companyName.parents("tr:eq(0)").addClass("error");
            return false;
        }

        if (validate.maxLength(companyName, 200) == false) {
            validate.ShowError(companyTable, "公司名稱 繁體 最多不能超過200個字符");
            companyName.parent("tr").addClass("error");
            return false;
        }


        if (validate.required(companyName_S) == false) {
            validate.ShowError(companyTable, "公司名稱 簡體 不能為空");
            return false;
        }

        if (validate.maxLength(companyName_S, 200) == false) {
            validate.ShowError(companyTable, "公司名稱 簡體 最多不能超過200個字符");
            return false;
        }

        if (validate.required(companyName_E) == false) {
            validate.ShowError(companyTable, "公司名稱 英文 不能為空");
            return false;
        }

        if (validate.maxLength(companyName_E, 200) == false) {
            validate.ShowError(companyTable, "公司名稱 英文 最多不能超過200個字符");
            return false;
        }

        if (checkOnlyCompany(companyName.val()) == false) {
            validate.ShowError(companyTable, "此公司名稱已存在");
            return false;
        }
    }

    return true;
}

function validateCompanyAddress() {
    var address = $("#address");
    var address_S = $("#addressSample");
    var address_E = $("#addressEnglish");
    var addressTable = $("#addressTable");

    if (validate.required(address) == false) {
        validate.ShowError(addressTable, "公司詳細地址 繁體 不能為空");
        return false;
    }

    if (validate.maxLength(address, 200) == false) {
        validate.ShowError(addressTable, "公司詳細地址 繁體 最多不能超過200個字符");
        return false;
    }


    if (validate.required(address_S) == false) {
        validate.ShowError(addressTable, "公司詳細地址 簡體 不能為空");
        return false;
    }

    if (validate.maxLength(address_S, 200) == false) {
        validate.ShowError(addressTable, "公司詳細地址 簡體 最多不能超過200個字符");
        return false;
    }

    if (validate.required(address_E) == false) {
        validate.ShowError(addressTable, "公司詳細地址 英文 不能為空");
        return false;
    }

    if (validate.maxLength(address_E, 200) == false) {
        validate.ShowError(addressTable, "公司詳細地址 英文 最多不能超過200個字符");
        return false;
    }

    return true;
}

function validatePhone(id, name) {
    var phone = $("#" + id);
    if (validate.maxLength(phone, 20) == false) {
        validate.ShowError(phone, name + "不能超過20個字符");
        return false;
    }

    return true;
}

function validateConnectPeople() {
    var connectPeople = $("#connectPeople");
    if (validate.required(connectPeople) <= 0) {
        validate.ShowError(connectPeople, "聯絡人不能為空");
        return false;
    }
    if (validate.maxLength(connectPeople, 20) == false) {
        validate.ShowError(connectPeople, "聯絡人不能超過20個字符");
        return false;
    }

    return true;
}

//todo
function checkOnlyEmail(value) {
    var count = 0;

    $.ajax({
        type: 'POST',
        url: "/supplier/CheckOnlyEmail",
        data: "email=" + value,
        async: false,
        success: function (data) {
            count = data.EmailCount;
        }
    });

    if (count > 0) {
        return false;
    }

    return true;
}

//todo
function checkOnlyCompany(value) {
    var count = 0;

    $.ajax({
        type: 'POST',
        url: "/supplier/CheckOnlyCompany",
        data: "companyName=" + value,
        async: false,
        success: function (data) {
            count = data.CompanyCount;
        }
    });

    if (count > 0) {
        return false;
    }

    return true;
}

var validate = {
    required: function (that) {
        return this.getLength(that) > 0;
    },
    getLength: function (that) {
        return $.trim(that.val()).length;
    },
    email: function (that) {
        var email = /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i;
        return email.test(that.val()) || $.trim(that.val()) == "";

    },
    passwordT: function (that) {
        var number = /^[0-9]{6,32}$/,
            letter = /^[a-zA-Z]{6,32}$/,
            sign = /^[@!#\$%&'\*\+\-\/=\?\^_`{\|}~]{6,32}$/,
            china = /[\u4E00-\u9FA5]/;
        if (number.test(that.val()) || letter.test(that.val()) || sign.test(that.val()) || china.test(that.val())) {
            return false;
        }
        return true;
    },
    number_: function (that) {
        var number_ = /^\d+$/;
        return number_.test(that.val());
    }, equalTo: function (that, param) {
        return that.val() == param.val();
    },
    maxLength: function (that, param) {
        return this.getLength(that) <= param;
    },
    ShowError: function (that, message, isRedTr, errorItem) {
        that.next().text("").text(message).show();
        if (isRedTr == true) {
            errorItem.parents("tr:eq(0)").addClass("error");
        }
    }
};