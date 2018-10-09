// JavaScript Document
(function ($) {
    var conf = {
        rules: {}, //规则,
        messages: {}, //提示信息
        tipsPositionClass: '.J_validate',
        rightClassName: 'none', //正确class correct
        errorClassName: 'inline errorTip', //错误class tip_erorr inline errorTip
        defaultClassName: '', //默认class  tipBox
        righthide: false, //验证通过后是否隐藏tip 
        callback: null//回调函数
    },
	Bform;
    function ValidateFn(that, conf) {
        var self = this;
        self.self = self;
        self.that = that;
        self.conf = conf;
        self.valid = false;
        self.checked = true;
        self.init = function () {
            self.bindEvent();
        };

        self.init();

    };
    ValidateFn.prototype = {
        constructor: ValidateFn,
        bindEvent: function () {
            var self = this;
            self.that.find(':text, :password, :file, select, textarea').blur(function () {
                self.check($(this));
            });
            self.that.submit(function (e, type) {
                Bform = true;
                self.valid = false;
                $(this).find(':text, :password, :file, select, textarea').each(function () {
                    self.check($(this));
                });
                if (self.that.find('.' + self.conf.errorClassName).length > 0) {
                    return false;
                } else {
                    if (type) {
                        self.valid = true;
                        e.preventDefault();
                    }
                    return true;
                }
            }).on('setRules.setRules', function (e, newRules) {
                if (newRules == null) {
                    self.checked = false;
                } else {
                    $.extend(self.conf, newRules);
                    self.checked = true;
                }
            });

        },
        check: function (that, type) {
            var self = this;
            if (!self.checked) {
                return;
            }
            $.each(self.conf.rules, function (key, value) {
                if (key == that[0].name) {
                    $.each(value, function (k, v) {
                        if (k == 'remote' && !type) {
                            that.data('remote', 'remote');
                        }
                        if (!that.data('remote')) {
                            /*
                            {
                            thta : that //本身
                            key : k,//规则key
                            value : v, //规则value
                            self : self // 自身
                            messages : self.conf.messages[key][k] //提示信息
                            }
							
                            self.eachRules({
                            thta : that,
                            key : k,
                            value : v,
                            self : self,
                            messages : self.conf.messages[key][k]
                            })*/
                            if (self.methods[k](that, v, self)) {
                                self.hideError(that, self.conf.messages[key][k]);
                            } else {
                                //if (Bform) {
                                //var height = that.position().top;
                                //}
                                //self.showError(that, self.conf.messages[key][k], height);
                                self.showError(that, self.conf.messages[key][k]);
                                return false;
                            }
                        } else {
                            self.methods[k](that, v, self, self.conf.messages[key][k])
                        }

                    });

                }
            });

        },
        methods: {
            required: function (that, param, self) {
                return self.getLength(that.val()) > 0;
            },
            email: function (that, value) {
                //var email =/^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/;
                //return email.test(that.val()) || $.trim(that.val()) == "";
                var email = /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i;
                return email.test(that.val()) || $.trim(that.val()) == "";

            },
            remote: function (that, param, self, messages) {
                var data = {};
                data[$(that).attr('name')] = $(that).val();
                $.ajax({
                    url: param,
                    type: "post",
                    datatype: "json",
                    data: data,
                    async: false,
                    success: function (data) {
                        if (data.status == 1) {
                            self.hideError(that, messages);
                            that.removeData('remote');
                            return true;
                        } else {
                            //height = 0;
                            //if (Bform) {
                            //height = that.position().top;
                            //}
                            //self.showError(that, messages, height);
                            self.showError(that, messages);
                            if (!self.getLength(that.val())) {
                                that.removeData('remote');
                                self.check(that, true);
                            }
                            //that.removeData('remote');
                            return false;
                        }

                    },
                    error: function () {
                        return false;
                    }
                });
            },
            maxLength: function (that, param, self) {
                return self.getLength(that.val()) < param;
            },
            equalTo: function (that, param) {
                return that.val() == param.val();
            },
            passwordT: function (that, param, self) {
                var number = /^[0-9]{6,32}$/,
                    letter = /^[a-zA-Z]{6,32}$/,
                    sign = /^[@!#\$%&'\*\+\-\/=\?\^_`{\|}~]{6,32}$/,
                    china = /[\u4E00-\u9FA5]/;
                if (number.test(that.val()) || letter.test(that.val()) || sign.test(that.val()) || china.test(that.val())) {
                    return false;
                }
                return true;
            },
            rangelength: function (that, param, self) {
                var length = self.getLength($.trim(that.val()));
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
                return mob.test(that.val()) || phone.test(that.val());
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
            min_length: function (that, param, self) {
                return !(self.getLength(that.val()) < param);
            },
            unchinese: function (that, value) {
                var match = /[\u4e00-\u9fa5]/g;
                return !match.test(that.val());
            }
        },
        showError: function (that, messages) {
            var self = this;
            if (Bform) {
                //$(window).scrollTop(height);
                //if($(window).scrollTop() > that.position().top){
                //$(window).scrollTop(that.position().top);
                //}
                if ($(window).scrollTop() > that.offset().top) {
                    $(window).scrollTop(that.offset().top);
                    if (that.attr("type") == "text") { that.focus(); }
                }
                Bform = false;
            }

            var tipEle = self.getTipElement(that);
            if (tipEle) {
                tipEle.html('<div class="' + self.conf.errorClassName + ' ' + self.conf.defaultClassName + '">' + messages + '</div>');
            }
            //if (self.hasTipsDiv(that)) {
            //    that.parents().next(self.conf.tipsPositionClass).children().removeClass(self.conf.rightClassName).addClass(self.conf.errorClassName);
            //    that.parents().next(self.conf.tipsPositionClass).html(messages);
            //} else {
            //    that.parents().next(self.conf.tipsPositionClass).html('<div class="' + self.conf.errorClassName + ' tipBox">' + messages + '</div>');
            //}

        },
        hideError: function (that) {
            var self = this;
            var tipEle = self.getTipElement(that);
            if (tipEle) {
                tipEle.html('');
                tipEle.html('<div class="' + self.conf.rightClassName + ' ' + self.conf.defaultClassName + '"></div>');
            }
            //if (self.hasTipsDiv(that)) {
            //    that.parents().next(self.conf.tipsPositionClass).html('').children().removeClass(self.conf.errorClassName).addClass(self.conf.rightClassName);
            //} else {
            //    that.parents().next(self.conf.tipsPositionClass).html('<div class="' + self.conf.rightClassName + ' tipBox"></div>');
            //}
        },
        submit: function () {


        },
        hasTipsDiv: function (that) {
            var self = this;
            var tipEle = self.getTipElement(that);
            if (tipEle) { return tipEle.find('.' + self.conf.rightClassName).length > 0 || tipEle.find('.' + self.conf.errorClassName).length > 0 }
            return false;
            //if (that.parents().next(self.conf.tipsPositionClass + '.' + self.conf.rightClassName).length > 0 || that.parents().next(self.conf.tipsPositionClass + '.' + self.conf.errorClassName).length > 0) {
            //    return true;
            //} else {
            //    return false;
            //}
        },
        getLength: function (val) {
            return $.trim(val).length;
        },
        getTipElement: function (that) {
            var self = this;
            var result;
            that.parents().next().each(function (index, item) {
                if ($(item).hasClass(self.conf.tipsPositionClass.substring(1))) {
                    result = $(item);
                    return false;
                }
                if ($(item).find(self.conf.tipsPositionClass).length > 0) {
                    result = $(item).find(self.conf.tipsPositionClass);
                    return false;
                }
            });
            return result;
        }//,
        //eachRules: function (obj) {
        //if (self.methods[k](that, v, self)) {
        //self.hideError(that, self.conf.messages[key][k]);
        //} else {
        //if (Bform) {
        //height = that.position().top;
        //}
        //self.showError(that, self.conf.messages[key][k], height);
        //return false;
        //}

        //}

    };


    $.fn.validateFn = function (options) {
        var el = this.data("validateFn");
        if (this.length < 0 || el) {
            return;
        }
        options = $.extend({}, conf, options);
        this.each(function () {
            el = new ValidateFn($(this), options);
            $(this).data("validateFn", el);
        });
    };

    $.extend($.fn, {
        validSubmit: function () {
            //验证是否合格
            $(this).trigger('submit', [true]);
            return $(this).data('validateFn').valid;
        },
        setRules: function (newRules) {
            //设置规则
            $(this).trigger('setRules', [newRules]);
        }
    });

})(jQuery);

