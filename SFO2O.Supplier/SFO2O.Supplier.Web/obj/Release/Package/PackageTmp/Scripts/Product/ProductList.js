$(function () {
    // 全选按钮
    $("#selectAll").unbind("click").click(function () {
        $('label[name="chkProductID"]').each(function () {
            $(this).toggleClass("active");
        });
    });

    $(".chbx_wrap").click(function () {
        $(this).toggleClass("active");
    });
});

//
function DeleteProduct(productId, returnUrl)
{
    if (productId == null || productId == "")
    {
        return false;
    }

    $.dialog({
        width: 470,
        title: "提示",
        content: "您確認要將這條待上傳的商品信息刪除嗎？",
        buttons: [{ text: "取消", isWhite: 1 },
            {
                text: "確定",
                onClick: function () {
                    $.ajax({
                        url: "/Product/DeleteProduct",
                        type: "post",
                        data: { productId: productId },
                        success: function (data) {
                            if (data.result) {
                                $.dialog({
                                    content: "刪除成功！",
                                    onConfirm: function () {
                                        window.location.href = returnUrl;
                                    }
                                });
                            } else {
                               
                                    $.dialog("刪除失敗！");
                            }
                        }
                    });
                    return true;
                }
            }]
    });
}

// 批量删除
function DeleteAllProduct(returnUrl) {
    var productIds = "";
    $('label[name="chkProductID"]').each(function () {
        if ($(this).hasClass("active")) {
            productId = $(this).attr("data-value");
            productIds += productId + ",";
        }
    });
    if (productIds.length > 0) {
        productIds = productIds.substring(0, productIds.length - 1);
    }
    if (productIds == null || productIds == "") {
        return false;
    }

    $.dialog({
        width: 470,
        title: "提示",
        content: "您確認要將這些待上傳的商品信息刪除嗎？",
        buttons: [{ text: "取消", isWhite: 1 },
            {
                text: "確定",
                onClick: function () {
                    $.ajax({
                        type: 'POST',
                        url: "/Product/DeleteAllProduct",
                        data: { productIds: productIds },
                        async: false,
                        dataType: "json",
                        success: function (data) {
                            if (data.result) {
                                $.dialog({
                                    content: "刪除成功！",
                                    onConfirm: function () {
                                        window.location.href = returnUrl;
                                    }
                                });
                            } else {

                                $.dialog("刪除失敗！");
                            }
                        }
                    });
                    return true;
                }
            }]
    });
}
