//显示分页
function displayPage(search, rowCount, pageIndex, pageSize) {
    var j = rowCount % pageSize;

    if (j > 0)
        j = rowCount / pageSize + 1;
    else {
        j = rowCount / pageSize;
    }
    //$(".pagination").show();

    MLGPager(search, pageIndex, j, search);
}

function displayPage1(search, rowCount, pageIndex, pageSize, obj) {
    var j = rowCount % pageSize;

    if (j > 0)
        j = rowCount / pageSize + 1;
    else {
        j = rowCount / pageSize;
    }
    //$(".pagination").show();

    MLGPager1(search, pageIndex, j, search, obj);

    $(obj).append("<p class=\"f14 mg_l20 mg_b5 inline\">共<em class=\"color_green pd_l10 pd_r10\"> " + rowCount + " </em>条数据</p>");

    $(obj).find("ul").addClass("inline")
}

function MLGPager1(search, pageIndex, pageCount, search, pageObj) {

    $(pageObj).pager({
        pagenumber: pageIndex,
        pagecount: pageCount,
        buttonClickCallback: function (pagenumber) {
            search(pagenumber);
        }
    });
}
//显示分页器
function MLGPager(search, pageIndex, pageCount, search) {

    $(".page").pager({
        pagenumber: pageIndex,
        pagecount: pageCount,
        buttonClickCallback: function (pagenumber) {
            search(pagenumber);
        }
    });
}