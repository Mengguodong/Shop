function subStrByByte(str, len) {
    /// <signature>
    ///   <summary>按字节截取字符串.</summary>
    ///   <param name="str" type="String">要截取的字符串.</param>
    ///   <param name="len" type="Number">要保留的字节数.</param>
    ///   <returns type="String" />
    /// </signature>
    if (!str || len < 0) { return undefined }
    if (!len) { return '' }
    if (str.length <= (len / 2)) { return str }
    var suffix = '...' + str[str.length - 1];
    var sufByteLength = suffix.length + (suffix.charCodeAt(suffix.length - 1) > 255 ? 1 : 0);
    if (len <= sufByteLength) {
        throw new Error("截取的長度太短");
    }
    //预期计数：中文2字节，英文1字节
    var bytes = 0;
    //临时字串 
    var temp = '';
    for (var i = 0; i < str.length; i++) {
        bytes += (str.charCodeAt(i) > 255 ? 2 : 1);
        //如果增加计数后长度大于限定长度，就直接返回临时字符串
        if (bytes > len) { return temp; }
        if (temp == '' && (bytes + sufByteLength) > len) { temp = str.substr(0, i) + suffix; }
    }
    //如果全部是单字节字符，就直接返回源字符串
    return str;
}
$(document).ready(function () {
    $("#btnSearch").click(function () {
        $("#searchForm").submit();
    })
    var btnNextStep = $("#btnNextStep");
    btnNextStep.click(function () {
        if (!btnNextStep.hasClass("btn_green_off")) {
            $("#nextStepForm").submit();
        }
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
    var CategoryID = $("#CategoryID");
    function SetCategoryID(id) {
        CategoryID.val(id);
        btnNextStep.removeClass("btn_green_off")
    }
    function ClearCategoryID() {
        CategoryID.val("");
        btnNextStep.addClass("btn_green_off")
    }
    var tabs = $("#tabs");
    BindLiChange(tabs, function (li, old) {
        var tabID = li.data("for");
        var showTab = $("#" + tabID);
        if (showTab.find("li").length == 0) {
            $(".yixuan").hide();
            $(".btn_wrap").hide();
        }
        else {
            $(".yixuan").show();
            $(".btn_wrap").show();
            if (tabID == "tab2" && $("#CategoryID").val() == "") {
                showTab.find("li:first").click();
            }
        }
        $("#" + old.data("for")).hide();
        showTab.show();
    });
    var path = $("#CategoryPath");

    var tab1UL = $("#tab1").find("ul:first");
    if (tab1UL.hasClass("check_tab1")) {
        var ul0 = $("#sel0");
        var ul1 = $("#sel1");
        var ul2 = $("#sel2");
        function GetChild(pNode, level, node) {
            var id = pNode.find("li.current").data("id");
            if (id) {
                $.post("GetCategoryList", { level: level, parentID: id }, function (json) {
                    node.html("");
                    $.each(json, function (i, data) {
                        $("<li/>").data("id", data.Id).attr("title", data.Name).html("<span>" + subStrByByte(data.Name, 28) + "</span>").appendTo(node);
                    });
                }, "json")
            }
        }
        BindLiChange(ul0, function (li) {
            ul2.html("");
            ClearCategoryID();
            path.text(li.attr("title"));
            GetChild(ul0, 1, ul1);
        });
        BindLiChange(ul1, function (li) {
            ClearCategoryID();
            path.text(ul0.find("li.current").attr("title") + " >> " + li.attr("title"));
            GetChild(ul1, 2, ul2);
        });
        BindLiChange(ul2, function (li) {
            //设置类目ID
            SetCategoryID(li.data("id"));
            path.text(ul0.find("li.current").attr("title") + " >> " + ul1.find("li.current").attr("title") + " >> " + li.attr("title"));
        });
        CategoryID.val("");
        path.text("");
    }
    else {
        if (tab1UL.length != 0) {
            BindLiChange(tab1UL, function (li) {
                li.find("input").prop("checked", true);
                //设置类目ID
                SetCategoryID(li.data("id"));
                path.text(li.text());
            })
            var li = tab1UL.find("li.current");
            if (li.data("id") != CategoryID.val()) {
                li.removeClass("current").click();
            }
        }
    }
    var usually = $("#tab2").find("ul:first");
    if (usually.length != 0) {
        BindLiChange(usually, function (li) {
            li.find("input").prop("checked", true);
            //设置类目ID
            SetCategoryID(li.data("id"));
            path.text(li.text());
        })
    }
})