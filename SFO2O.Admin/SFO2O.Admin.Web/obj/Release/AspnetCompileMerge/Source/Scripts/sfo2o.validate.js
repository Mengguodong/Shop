var validate = {
    getLength: function (val) {
        return $.trim(val).length;
    },
    required: function (that, param) {
        return this.getLength(that.val()) > 0;
    },
    email: function (that, value) {
        var email = /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i;
        return email.test(that.val()) || $.trim(that.val()) == "";

    },
    maxLength: function (that, param) {
        return this.getLength(that.val()) < param;
    },
    equalTo: function (that, param) {
        return that.val() == param.val();
    },
    passwordT: function (that, param) {
        var number = /^[0-9]{6,32}$/,
			letter = /^[a-zA-Z]{6,32}$/,
			sign = /^[@!#\$%&'\*\+\-\/=\?\^_`{\|}~]{6,32}$/,
			china = /[\u4E00-\u9FA5]/;
        if (number.test(that.val()) || letter.test(that.val()) || sign.test(that.val()) || china.test(that.val())) {
            return false;
        }
        return true;
    },
    rangelength: function (that, param) {
        var length = this.getLength($.trim(that.val()));
        return length >= param[0] && length <= param[1];
    },
    mobileEmail: function (that, value) {
        var email = /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i;

        var mob = /^(13|15|18|17)\d{9}$/;

        return email.test(that.val()) || mob.test(that.val());
    },
    mobile: function (that, value) {
        var mob = /^(13|15|18|17)\d{9}$/;
        return mob.test(that.val());
    },
    mobilephone: function (that, value) {
        var mob = /^((13|15|18|17)\d{9})$/;
        var phone = /^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$/;

        if (that.val() != "") {
            return mob.test(that.val()) || phone.test(that.val());
        } else {
            return true;
        }
    },
    phone: function (that, value) {
        var phone = /^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$/;

        if (that.val() != "") {
            return phone.test(that.val());
        } else {
            return true;
        }
    },
    decimal: function (that, param) {
        var intStr = "\^\\d{1,para1}\$";
        var doubleStr = "\^(\\d{1,para1})+(\\.)+(\\d{1,para2})\$";
        var intmob = new RegExp(intStr.replace("para1", param[0]));
        var submob = new RegExp(doubleStr.replace("para1", param[0]).replace("para2", param[1]));
        return intmob.test(that.val()) || submob.test(that.val());
    },
    number_: function (that, value) {
        var number_ = /^\d+$/;
        return number_.test(that.val());
    },
    isfloat: function (that) {
        var partrn = /^\d+(?=\.{0,1}\d+$|$)/;
        return partrn.test(that.val());
    },
    max_float: function (that, value) {
        var that_value = parseFloat(that.val());
        value = parseFloat(value);
        return that_value > value;
    },
    qq: function (that, value) {
        var qq = /^(\d{5,10}(,\d{5,10})*)?$/;
        return qq.test(that.val());
    },
    max_number: function (that, value) {
        that_value = Number(that.val());
        value = Number(value);
        return that_value <= value;
    },
    big_zero: function (that, value) {
        that_value = Number(that.val());
        return that_value >= 0;
    },
    chinese: function (that, value) {
        var match = /[\u4e00-\u9fa5]/g;
        return match.test(that.val());
    },
    min_length: function (that, param) {
        if (that.val() != "") {
            return !(this.getLength(that.val()) < param);
        } else {
            return true;
        }
    },
    unchinese: function (that, value) {
        var match = /[\u4e00-\u9fa5]/g;
        return !match.test(that.val());
    },
    ShowError: function (that, message, isRedTr, errorItem) {
        that.next().text("").text(message).show();
        if (isRedTr == true) {
            errorItem.parents("tr:eq(0)").addClass("error");
        }
    }
};
var decimalOperation = {
    /*浮点数加法 js原生态加法有bug*/
    accAdd: function (arg1, arg2) {
        var r1, r2, m, c;
        try {
            r1 = arg1.toString().split(".")[1].length;
        }
        catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg2.toString().split(".")[1].length;
        }
        catch (e) {
            r2 = 0;
        }
        c = Math.abs(r1 - r2);
        m = Math.pow(10, Math.max(r1, r2));
        if (c > 0) {
            var cm = Math.pow(10, c);
            if (r1 > r2) {
                arg1 = Number(arg1.toString().replace(".", ""));
                arg2 = Number(arg2.toString().replace(".", "")) * cm;
            } else {
                arg1 = Number(arg1.toString().replace(".", "")) * cm;
                arg2 = Number(arg2.toString().replace(".", ""));
            }
        } else {
            arg1 = Number(arg1.toString().replace(".", ""));
            arg2 = Number(arg2.toString().replace(".", ""));
        }
        return (arg1 + arg2) / m;
    },
    /*浮点数乘法*/
    accMul: function (arg1, arg2) {
        var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
        try {
            m += s1.split(".")[1].length;
        }
        catch (e) {
        }
        try {
            m += s2.split(".")[1].length;
        }
        catch (e) {
        }
        return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
    },
    /**
     ** 减法函数，用来得到精确的减法结果
     ** 说明：javascript的减法结果会有误差，在两个浮点数相减的时候会比较明显。这个函数返回较为精确的减法结果。
     ** 调用：accSub(arg1,arg2)
     ** 返回值：arg1加上arg2的精确结果
     **/
    accSub: function (arg1, arg2) {
        var r1, r2, m, n;
        try {
            r1 = arg1.toString().split(".")[1].length;
        }
        catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg2.toString().split(".")[1].length;
        }
        catch (e) {
            r2 = 0;
        }
        m = Math.pow(10, Math.max(r1, r2)); //last modify by deeka //动态控制精度长度
        n = (r1 >= r2) ? r1 : r2;
        return ((arg1 * m - arg2 * m) / m).toFixed(n);
    },
    /** 
     ** 除法函数，用来得到精确的除法结果
     ** 说明：javascript的除法结果会有误差，在两个浮点数相除的时候会比较明显。这个函数返回较为精确的除法结果。
     ** 调用：accDiv(arg1,arg2)
     ** 返回值：arg1除以arg2的精确结果
     **/
    accDiv: function (arg1, arg2) {
        var t1 = 0, t2 = 0, r1, r2;
        try {
            t1 = arg1.toString().split(".")[1].length;
        }
        catch (e) {
        }
        try {
            t2 = arg2.toString().split(".")[1].length;
        }
        catch (e) {
        }
        with (Math) {
            r1 = Number(arg1.toString().replace(".", ""));
            r2 = Number(arg2.toString().replace(".", ""));
            return (r1 / r2) * pow(10, t2 - t1);
        }
    },
    /*数字格式化（加逗号）*/
    formatNum: function (str) {
        str = str.toString();
        var newStr = "";
        var count = 0;
        if (str.indexOf(".") == -1) {
            for (var i = str.length - 1; i >= 0; i--) {
                if (count % 3 == 0 && count != 0) {
                    newStr = str.charAt(i) + "," + newStr;
                } else {
                    newStr = str.charAt(i) + newStr;
                }
                count++;
            }
            str = newStr; //自动补小数点后两位
        }
        else {
            for (var i = str.indexOf(".") - 1; i >= 0; i--) {
                if (count % 3 == 0 && count != 0) {
                    newStr = str.charAt(i) + "," + newStr;
                } else {
                    newStr = str.charAt(i) + newStr; //逐个字符相接起来
                }
                count++;
            }
            str = newStr + (str + "00").substr((str + "00").indexOf("."), 3);

        }
        return str;
    }
};