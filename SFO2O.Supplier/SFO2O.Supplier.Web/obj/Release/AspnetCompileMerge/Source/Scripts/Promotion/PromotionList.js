$(document).ready(function () {

    $("#PromotionStatus").chosen({ disable_search: true, width: "127px" });

    $("#promotionSearch").click(function () { PromotionSearch(1); })

    $("#promotionSearch").click();
})

function PromotionSearch(pageIndex) {
    $.ajax({
        type: 'POST',
        url: "/Promotion/GetPromotionList",
        data: $('#promotionQuery').serialize() + "&PageSize=" + 20 + "&PageNo=" + pageIndex,
        async: true,
        success: function (data) {
            $("#promotionList").html(data);
            displayPage1(PromotionSearch, $("#recordCount").val(), $("#pageIndex").val(), 20, $("#pager"));
        }
    });
}

function EndPromotion(promotionId) {
    var result = $.dialog({
        content: "您確定要終止該活動嗎？",
        buttons: [{ text: "取消", isWhite: true },
                {
                    text: "確認",
                    onClick: function () {
                        $.ajax({
                            type: 'POST',
                            url: "/Promotion/CanclePromotion",
                            data: "&promotionId=" + promotionId,
                            async: true,
                            success: function (data) {
                                result.close();
                                if (data.IsSuccess == true) {
                                    $.dialog({
                                        content: "終止成功！",
                                        onConfirm: function () {
                                            window.location.reload();
                                        }
                                    });
                                }
                                else {
                                    $.dialog({
                                        content: data.Message,
                                        onConfirm: function () {
                                            return true;
                                        }
                                    });
                                }
                            }
                        });
                    }
                }]
    });



}