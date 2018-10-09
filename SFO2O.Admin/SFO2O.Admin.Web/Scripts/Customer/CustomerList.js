$(function () {
  
    $('#CountryArea').chosen({ disable_search: true });

    $("#search").click(function () {
        Query(1);
    });
    $("#export").click(function () {
        Export();
    });
    Query(1);
});



function Query(pageIndex) {
    if (pageIndex == undefined)
        pageIndex = 1;

    $.ajax({
        type: 'POST',
        url: "/Customer/CustomerList",
        data: $('#queryForm').serialize() + "&PageSize=" + 50 + "&PageNo=" + pageIndex,
        async: true,
        success: function (data) {
            $("#CustomerList").html(data);
            displayPage(Query, $("#recordCount").val(), $("#recordCount").val(), $("#pageIndex").val(), 50)
        }
    });
}
function Export() {
     window.location.href = "/Customer/ExportCustomerList?startTime=" + $("#CreateTimeStart").val() + "&endTime=" + $("#CreateTimeEnd").val() + "&mobile=" + $("#Mobile").val() + "&email=" + $("#Email").val() + "&countryArea=" + $("#CountryArea").val();
}