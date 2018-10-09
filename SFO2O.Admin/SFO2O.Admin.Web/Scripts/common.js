//显示分页
function displayPage(search, recordCount,rowCount, pageIndex, pageSize) {
    var j = recordCount % pageSize;

    if (j > 0)
        j = recordCount / pageSize + 1;
    else {
        j = recordCount / pageSize;
    }
    //$(".pagination").show();

    MLGPager(search, rowCount, pageIndex, j, search);
}

function displayPage1(search, recordCount, rowCount, pageIndex, pageSize, obj) {
    var j = recordCount % pageSize;

    if (j > 0)
        j = recordCount / pageSize + 1;
    else {
        j = recordCount / pageSize;
    }
    //$(".pagination").show();

    MLGPager1(search, rowCount, pageIndex, j, search, obj);
}

function MLGPager1(search, rowCount, pageIndex, pageCount, search, pageObj) {

    $(pageObj).pager({
        pagenumber: pageIndex,
        pagecount: pageCount,
        buttonClickCallback: function (pagenumber) {
            search(pagenumber);
        }

    });

    $(pageObj).append("<p class=\"f14 mg_l20 mg_b5 inline\">共<em class=\"color_green pd_l10 pd_r10\"> " + rowCount + " </em>條數據</p>");

    $(pageObj).find("ul").addClass("inline")

}
//显示分页器
function MLGPager(search, rowCount, pageIndex, pageCount, search) {

    $(".page").pager({
        pagenumber: pageIndex,
        pagecount: pageCount,
        buttonClickCallback: function (pagenumber) {
            search(pagenumber);
        }
    });
    var pageObj = $(".page");

    $(pageObj).append("<p class=\"f14 mg_l20 mg_b5 inline\">共<em class=\"color_green pd_l10 pd_r10\"> " + rowCount + " </em>條數據</p>");

    $(pageObj).find("ul").addClass("inline")
}
$(document).ready(function () {
    var hplink = $(".homepage");
    hplink.css("cursor", "pointer");
    hplink.click(function () {
        location.href = "/Home/Index";
    })
})