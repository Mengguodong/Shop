$(document).ready(function () {
    $("#PromotionStatusType").chosen({ disable_search: true, width: "276px" });
    $("#SupplierID").chosen({ disable_search: true, width: "276px" });
    $("#search").click(function () { Query(1) });
    Query(1);
});

function Query(pageIndex) {
    if (!pageIndex) { pageIndex = 1; }

    $.ajax({
        type: 'POST',
        url: "/Promotion/GetList",
        data: $('#queryForm').serialize() + "&PageSize=20&PageIndex=" + pageIndex,
        async: true,
        success: function (data) {
            $("#promotions").html(data);
            displayPage1(QueryMain, $("#recordCount").val(), $("#recordCount").val(), $("#pageIndex").val(), 20, $("#pager"));
            $("#recordCount").remove();
            $("#pageIndex").remove();
        }
    });
}

function QueryMain(pageIndex) {
    if (!pageIndex){ pageIndex = 1; }

    $.ajax({
        type: 'POST',
        url: "/Promotion/GetList",
        data: $('#queryForm').serialize() + "&PageSize=20&PageIndex=" + pageIndex ,
        async: true,
        success: function (data) {
            $("#promotions").html(data);
            displayPage1(QueryMain, $("#recordCount").val(), $("#recordCount").val(), $("#pageIndex").val(), 20, $("#pager"));
            $("#recordCount").remove();
            $("#pageIndex").remove();
        }
    });
}
