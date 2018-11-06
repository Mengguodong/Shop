$(document).ready(function () {

    var tabs = $("#ulDetail");
    BindLiChange(tabs, function (li, old) {
        $("#" + old.data("for")).hide();
        $("#" + li.data("for")).show();
    });

    $("#savebrand").click(function () {
        $(".wrong_tips").text("").hide();
        if (validateInput() == true) {
            SaveBrand();
        }
    });
    $("#category").chosen({ disable_search: true, width: "150px" });
    $("#country").chosen({ disable_search: true, width: "120px" });

    new AjaxUpload("logoFile", {
        action: '/Utility/UploadSupplierLogo',//提交post
        autoSubmit: true,
        name: 'imgFile',
        fileType: '.jpg,.jpeg,.png',
        responseType: "json",
        onSubmit: function (file, extension) {
            var ext = extension[0].toLowerCase();
            if (ext != "jpg" && ext != "jpeg" && ext != "png") {
                {
                    $.dialog("只支持上传PNG、JPG 或 JPEG格式的图片！");
                    return false;
                }
            }
        },
        onComplete: function (file, response) {
            var retValue = response;
            uploadImageResult("logo", retValue);
        }
    });

    new AjaxUpload("relogoFile", {
        action: '/Utility/UploadSupplierLogo',//提交post
        autoSubmit: true,
        name: 'imgFile',
        fileType: '.jpg,.jpeg,.png',
        responseType: "json",
        onSubmit: function (file, extension) {
            var ext = extension[0].toLowerCase();
            if (ext != "jpg" && ext != "jpeg" && ext != "png") {
                {
                    $.dialog("只支持上传PNG、JPG 或 JPEG格式的图片！");
                    return false;
                }
            }
        },
        onComplete: function (file, response) {
            var retValue = response;
            uploadImageResult("logo", retValue);
        }
    });

    new AjaxUpload("bannerFile", {
        action: '/Utility/UploadSupplierLogo',//提交post
        autoSubmit: true,
        name: 'imgFile',
        fileType: '.jpg,.jpeg,.png',
        responseType: "json",
        onSubmit: function (file, extension) {
            var ext = extension[0].toLowerCase();
            if (ext != "jpg" && ext != "jpeg" && ext != "png") {
                {
                    $.dialog("只支持上传PNG、JPG 或 JPEG格式的图片！");
                    return false;
                }
            }
        },
        onComplete: function (file, response) {
            var retValue = response;
            uploadImageResult("banner", retValue);
        }
    });

    new AjaxUpload("reBannerFile", {
        action: '/Utility/UploadSupplierLogo',//提交post
        autoSubmit: true,
        name: 'imgFile',
        fileType: '.jpg,.jpeg,.png',
        responseType: "json",
        onSubmit: function (file, extension) {
            var ext = extension[0].toLowerCase();
            if (ext != "jpg" && ext != "jpeg" && ext != "png") {
                {
                    $.dialog("只支持上传PNG、JPG 或 JPEG格式的图片！");
                    return false;
                }
            }
        },
        onComplete: function (file, response) {
            var retValue = response;
            uploadImageResult("banner", retValue);
        }
    });
});

function CopyEditInfo(copyto, copyfrom) {
    var info = $("#" + copyfrom).val();

    if (copyto == "content2") {
        $("#" + copyto).val(info);
    }
    else if (copyto == "content3") {
        $("#" + copyto).val(info);
    }
}

function BindLiChange(ul, func) {
    ul.delegate("li", "click", function () {
        var li = $(this);
        var old;
        if (li.hasClass("current")) {
            return;
        }
        else {
            old = ul.find("li.current");
            old.removeClass("current");
            li.addClass("current");
        }
        func(li, old);
    });
}
function SaveBrand() {
    var json = {};
    json.Id = $("#Id").val();
    json.NameCN = $("#nameCN").val();
    json.NameHK = $("#nameHK").val();
    json.NameEN = $("#nameEN").val();
    json.Logo = $("#logoPath").val();
    json.Banner = $("#barnnerPath").val();
    json.CategoryId = $("#category").val();
    json.CategoryName = $("#category").find("option:selected").text();
    json.CountryId = $("#country").val();
    json.CountryName = $("#country").find("option:selected").text();
    json.IntroductionHK = $("#content2").val();
    json.IntroductionCN = $("#content1").val();
    json.IntroductionEN = $("#content3").val();


    if (json.NameCN == "") {
        $("#nameCN").closest("td").find(".wrong_tips").html("请输入简体中文品牌名称").show();
        return false;
    }
    if (json.NameCN.length > 50) {
        $("#nameCN").closest("td").find(".wrong_tips").html("简体中文品牌名称最多不超过50个字符").show();
        return false;
    }
    //if (json.NameHK == "") {
    //    $("#nameHK").closest("td").find(".wrong_tips").html("请输入繁体中文品牌名称").show();
    //    return false;
    //}
    //if (json.NameHK.length > 50) {
    //    $("#nameHK").closest("td").find(".wrong_tips").html("繁体中文品牌名称最多不超过50个字符").show();
    //    return false;
    //}
    //if (json.NameEN == "") {
    //    $("#nameEN").closest("td").find(".wrong_tips").html("请输入英文品牌名称").show();
    //    return false;
    //}
    //if (json.NameEN.length > 50) {
    //    $("#NameEN").closest("td").find(".wrong_tips").html("英文品牌名称最多不超过50个字符").show();
    //    return false;
    //}

    if (json.Logo == "") {
        $("#logoPath").closest("td").find(".wrong_tips").html("请上传品牌LOGO").show();
        return false;
    }
    if (json.Banner == "") {
        $("#barnnerPath").closest("td").find(".wrong_tips").html("请上传Banner图").show();
        return false;
    }

    //if (json.IntroductionHK == "") {
    //    $("#content1").closest("td").find(".wrong_tips").html("请输入繁体版品牌简介").show();
    //    return false;
    //}


    if (json.IntroductionCN == "") {
        $("#content2").closest("td").find(".wrong_tips").html("请输入简体中文版品牌简介").show();
        return false;
    }

    //if (json.IntroductionEN == "") {
    //    $("#content3").closest("td").find(".wrong_tips").html("请输入英文版品牌简介").show();
    //    return false;
    //}


    $.ajax({
        type: 'POST',
        url: "/Brand/SaveBrand",
        data: "brand=" + escape(JSON.stringify(json)),
        async: true,
        success: function (data) {
            if (data.result) {
                $.dialog({
                    width: 470,
                    title: "提示",
                    content: "品牌信息保存成功",
                    buttons: [
                        {
                            text: "确定",
                            onClick: function () {
                                window.location.href = "/Brand/BrandList";
                                return true;
                            }
                        }]
                });
            }
            else {
                $.dialog("品牌信息保存失败！");
            }
        }
    });
}

function uploadImageResult(type, resultJson) {
    if (resultJson.Error == 1) {
        $.dialog(resultJson.Message);
    }
    else {
        $.dialog("图片保存成功");
        if (type == "logo") {
            $("#logoFileLi").hide();
            $("#logoImage").show();
            $("#logoImage").children("img").attr("src", resultJson.Url);
            $("#reLogoFileLi").show();
            $("#logoPath").val(resultJson.Path);
        }
        else {
            $("#bannerFileLi").hide();
            $("#bannerImage").show();
            $("#bannerImage").children("img").attr("src", resultJson.Url);
            $("#reBannerFileLi").show();
            $("#barnnerPath").val(resultJson.Path);
        }


    }
}

function validateInput() {
    return true;
}

