(function () {
    var wrapHtml = '<div class="win_wrap f16" style="z-index:2147483647;">' +
        '<div class="win_top clearfix"><span class="win_title f_l mg_l20"></span><a href="javascript:void(0)" title="關閉" class="win_close f_r"></a></div>' +
        '<div class="win_cont pd_l20 pd_r20"></div>' +
        '<div class="win_btm f18 t_c pd_b20 mg_t10"><a href="javascript:void(0)" class="win_btn btn_green w135">確定</a></div>' +
        '</div>';
    var $ = jQuery;
    function isArray(obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    }
    function ShowDialog (options) {
        if (!options) {
            return;
        }
        if (typeof options == "string") {
            options = { content: options };
        }
        if (!options.content) {
            return;
        }
        if (!options.title) {
            options.title = "提示"
        }
        if (options.onConfirm && typeof options.onConfirm != "function") {
            options.onConfirm = null;
        }
        if (options.init && typeof options.init != "function") {
            options.init = null;
        }
        var $body = $("body");
        var bodyOverflow = $body.css("overflow");
        if (options.denyScroll) {
            $body.css("overflow", "hidden");
        }
        var $this = $(this);
        var sw = document.documentElement.scrollWidth || document.body.scrollWidth;
        var sh = document.documentElement.scrollHeight || document.body.scrollHeight;
        var cover = $('<div style="position:absolute;top:0;left:0;background:#000;opacity:0.0;filter:alpha(opacity=0);z-index:2147483647;"/>').css("width", sw).css("height", sh);
        var wrap = $(wrapHtml).css("position", "absolute");
        wrap.find(".win_title").text(options.title);
        var fadeIn, fadeOut;
        function destroy() {
            clearInterval(fadeIn);
            var opacityOut = 0.7;
            fadeOut = setInterval(function () {
                if (opacityOut > 0) {
                    cover.css("opacity", opacityOut).css("filter", "alpha(opacity=" + opacityOut * 100 + ")");
                    opacityOut -= 0.05;
                }
                else {
                    clearInterval(fadeOut);
                    wrap.remove();
                    cover.remove();
                    if (options.denyScroll) {
                        $body.css("overflow", bodyOverflow);
                    }
                }
            }, 20);
        }
        cover.dblclick(destroy);
        wrap.find('.win_close').click(destroy);
        var $content = wrap.find(".win_cont");
        var $btns = wrap.find(".win_btm");
        if (options.buttons) {
            $btns.html('');
            if (isArray(options.buttons) && options.buttons.length > 0) {
                for (var i = 0; i < options.buttons.length; i++) {
                    var btnOption = options.buttons[i];
                    if (btnOption.onClick && typeof btnOption.onClick != "function") {
                        btnOption.onClick = null;
                    }
                    var $btn = $('<a href="javascript:void(0)" class="win_btn w135">' + btnOption.text + '</a>');
                    $btn.addClass(btnOption.width ? btnOption.width : "w135");
                    $btn.addClass(btnOption.isWhite ? "btn_white" : "btn_green");
                    if (i > 0) {
                        $btn.addClass("mg_l40");
                    }
                    $btn.appendTo($btns);
                    (function (btn, onClick) {
                        btn.click(function () {
                            if (!onClick || onClick($content)) {
                                destroy();
                            }
                        });
                    })($btn, btnOption.onClick);
                }
            }
            else {
                var btnOption = options.buttons;
                var $btn = $('<a href="javascript:void(0)" class="win_btn w135">' + btnOption.text + '</a>');
                $btn.addClass(btnOption.isWhite ? "btn_white" : "btn_green");
                $btn.appendTo($btns);
                var onClick = btnOption.onClick;
                $btn.click(function () {
                    if (!onClick || onClick($content)) {
                        destroy();
                    }
                })
            }
        }
        else {
            var onClick = options.onConfirm;
            $btns.find('.win_btn').click(function () {
                if (!onClick || onClick($content)) {
                    destroy();
                }
            });
        }
        var html;
        if (typeof options.content == "string") {
            if (options.content.indexOf('<') == -1)
                html= '<p class="t_c pd_t40 pd_b30"><span>' + options.content + '</span></p>';
            else {
                html = options.content;
            }
        }
        else {
            html = $(options.content).html();
        }
        if (html.length > 0) {
            $content.html(html);
        }
        else {
            return;
        }
        if (options.init) {
            options.init($content)
        };
        var sl = document.documentElement.scrollLeft || document.body.scrollLeft;
        var st = document.documentElement.scrollTop || document.body.scrollTop;
        var cw = document.documentElement.clientWidth || document.body.clientWidth;
        var ch = document.documentElement.clientHeight || document.body.clientHeight;
        cover.appendTo($body);
        wrap.appendTo($body);
        var ww = wrap.width();
        if (options.width) {
            ww = options.width | 0;
        }
        if (ww < 300) {
            wrap.width(300);
        } else if (options.width) {
            wrap.width(ww);
        }
        var left = sl + (cw - wrap.width() + 20) / 2;
        var top = st + (ch - wrap.height()) / 2;
        wrap.css("left", left < 0 ? 0 : left).css("top", top < 0 ? 0 : top);
        var opacityIn = 0;
        fadeIn = setInterval(function () {
            if (opacityIn < 0.7) {
                cover.css("opacity", opacityIn).css("filter", "alpha(opacity=" + opacityIn * 100 + ")");
                opacityIn += 0.05;
            }
            else {
                clearInterval(fadeIn);
            }
        }, 20);
        return { close: destroy }
    }
    $.fn.extend({
        dialog: function (options) {
            options = options || {};
            options.content = $(this);
            return ShowDialog(options);
        }
    });
    $.extend({
        dialog: function (options) {
            return ShowDialog(options);
        }
    });
})();