$(document).ready(function () {
    $("#refundstatus").chosen({ disable_search: true, width: "140px" });

    $("#refundType").chosen({ disable_search: true, width: "140px" });

    $("#search").click(function () { $(this).closest("form").submit() });

    var tabs = $("#ulDetail");
    BindLiChange(tabs, function (li, old) {
        $("#" + old.data("for")).hide();
        $("#" + li.data("for")).show();
    });

});

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