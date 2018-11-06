$(function () {

    $('#SettlementStatus').chosen({ disable_search: true });

    $("#search").click(function () {
        Query(1);
    });
    $("#SettlementCode").blur(function () {
        if ($(this).val() != "") {
            $("#CreateTimeStart").val("");
            $("#CreateTimeEnd").val("");
            $("#OrderCode").val("");
            $("#SettlementStatus").val("-1");
            $("#CompanyName").val("");
        }
    });
    
   Query(1);
});
function Query(pageIndex) {
    if (pageIndex == undefined)
        pageIndex = 1;

    $.ajax({
        type: 'POST',
        url: "/Settlement/SettlementList",
        data: $('#queryForm').serialize() + "&PageSize=" + 50 + "&PageNo=" + pageIndex,
        async: true,
        success: function (data) {
            $("#list").html(data);
            displayPage(Query, $("#recordCount").val(), $("#recordCount").val(), $("#pageIndex").val(), 50)
        }
    });
}
