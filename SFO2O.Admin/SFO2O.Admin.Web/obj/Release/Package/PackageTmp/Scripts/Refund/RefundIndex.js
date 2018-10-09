$(function () {

    $('#RefundStatus').chosen({ disable_search: true });
    $('#RefundType').chosen({ disable_search: true });
    $("#search").click(function () {
        Query(1);
       
    });   
    Query(1);
   
});
function InitTab()
{
    $(".check_title li a").each(function (index) {
        $(this).parent().removeClass('current');
        if ($(this).attr("code") == $("#RegionCode").val()) {
            $(this).parent().addClass('current');          
        }
    });
}
function Query(pageIndex) {
    if (pageIndex == undefined)
        pageIndex = 1;

    $.ajax({
        type: 'POST',
        url: "/Refund/RefundList",
        data: $('#queryForm').serialize() + "&PageSize=" + 50 + "&PageNo=" + pageIndex,
        async: false,
        success: function (data) {
            $("#list").html(data);
            displayPage(Query, $("#recordCount").val(), $("#recordCount").val(), $("#pageIndex").val(), 50);
            InitTab();
        }
    });   
}
