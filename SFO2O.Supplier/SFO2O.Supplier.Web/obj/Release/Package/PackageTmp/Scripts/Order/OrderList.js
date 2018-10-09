$(function () {

    var tabs = $("#ulDetail");
    BindLiChange(tabs, function (li, old) {
        var oldId = old.data("for");
        var liId = li.data("for");
        $("#" + oldId).hide();
        $("#Total" + oldId).hide();
        $("#" + liId).show();
        $("#Total" + liId).show();
    });


    //$("#orderStatus").chosen({ disable_search: true, width: "180px" });

    $("#search").click(function () { $(this).closest("form").submit() });



})

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

function ShowLogistics(expressCode, expressCompany) {
    var html = "";
    if (expressCompany.trim() == '德邦物流')
        {
        window.location.href = "https://m.kuaidi100.com/result.jsp?com=debangwuliu&nu=" + expressCode;
    }
    if(expressCompany.trim() == '宅急送')
    {
        window.location.href = "https://m.kuaidi100.com/result.jsp?com=zhaijisong&nu=" + expressCode;
    }
    //$.get('/Order/ShowLogistics', { ExpressCode: expressCode }, function (data) {
    //    $.dialog({
    //        title: "查看物流信息",
    //        content: data,
    //    });

    //});
}
