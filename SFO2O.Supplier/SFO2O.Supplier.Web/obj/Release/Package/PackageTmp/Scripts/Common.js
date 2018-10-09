(function (window) {
    var sfo2o = {};

    sfo2o.IsIeBrower = function () {

        if (sfo2o.getBrowerType() == "msie")
            return true;
        else {
            return false;
        }
    };

    sfo2o.getBrowerType = function () {
        var brower = $.browser;
        if (brower.msie) return "msie";
        if (brower.mozilla) return "mozilla";
        if (brower.safari) return "safari";
        if (brower.opera) return "opera";
        return "";
    };

    //取得文件名称，去掉扩展名
    sfo2o.GetFileName = function (fileName) {
        return fileName.substring(0, fileName.lastIndexOf("."));
    };

    //通过Url。取得文件名称，去掉扩展名
    sfo2o.GetFileNameByUrl = function (url) {
        while (url.indexOf("/") > -1) {
            url = url.substring(url.indexOf("/") + 1, url.length);
        }

        return url.substring(0, url.lastIndexOf("."));

    };

    //跳转到登录页
    sfo2o.GotoLogin = function () {
        location.href = "/Account/Login";
    };

  

    //取得随机数
    sfo2o.GetRandom = function (n) {
        var chars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];

        var res = "";
        for (var i = 0; i < n; i++) {
            var id = Math.ceil(Math.random() * 35);
            res += chars[id];
        }
        return res;
    };

    //取得文件的后缀名
    sfo2o.GetExtensionFileName = function (pathfilename) {
        var reg = /(\\+)/g;
        var pfn = pathfilename.replace(reg, "#");
        var arrpfn = pfn.split("#");
        var fn = arrpfn[arrpfn.length - 1];
        var arrfn = fn.split(".");
        return arrfn[arrfn.length - 1];
    };

    //格式化价格
    sfo2o.FormatMoney = function (fileName) {
        var fileNameTmp = fileName + "";

        if (fileNameTmp.indexOf(".") > -1) {
            var aryName = fileNameTmp.split(".");
            if (aryName[1].length == 1) {
                return fileNameTmp + "0";
            }
            else {
                return fileNameTmp;
            }

        } else {
            return fileNameTmp + ".00";
        }
    };

    //显示遮罩层
    sfo2o.ShowMaskDiv = function () {
        $(".pop_mask").show();
    };

    //隐藏遮罩层
    sfo2o.HideMaskDiv = function () {
        $(".pop_mask").hide();
    };

    sfo2o.IsEmail = function (str) {
        var pattern = /^([a-zA-Z0-9_-])+(([a-zA-Z0-9_-]|[.])+([a-zA-Z0-9_-])+)?@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
        return pattern.test(str);
    };

    sfo2o.GetPassLevel = function (s) {
        if (s.length == 0) {
            return 0;
        }
        var level = 0;
        if (s.match(/([a-z])+/)) {
            level++;
        }

        if (s.match(/([0-9])+/)) {
            level++;
        }
        if (s.match(/([A-Z])+/)) {
            level++;
        }
        if (s.match(/[^a-zA-Z0-9]+/)) {
            level++;
        }
        return level;
    }
    window.sfo2o = sfo2o;

})(window);
$(document).ready(function () {
    var hplink = $(".homepage");
    hplink.css("cursor", "pointer");
    hplink.click(function () {
        location.href = "/Home/Index";
    })
})