// 修改申请时，需要根据后台的值显示文字和选中状态
$(function () {
    var reasonArray = ['商品颜色与订单不符', '商品尺寸与订单不符', '送达的商品与订单商品不符', '商品已超过使用期限', '商品不能正常运作', '商品已破损', '商品品质问题（请描述）', '其他原因（请描述）'];
    var statusArray = ['未开封', '已开封未使用', '已开封已使用'];
    var reasonEl = $("#reason");
    var statusEl = $("#status");
    var reasonId = reasonEl.attr('reasonid');
    var statusId = statusEl.attr('statusid');
    var size;
    var uploadImage;
    reasonEl.attr('value', reasonArray[parseInt(reasonId) - 1]);
    statusEl.attr('value', statusArray[parseInt(statusId) - 1]);
    $("#appealReason .radio").removeClass('checked');
    if (reasonId > 0) {
        $("#appealReason li").eq(parseInt(reasonId) - 1).find('i').addClass('checked');
    }
    $("#statusBox .radio").removeClass('checked');
    if (statusId > 0) {
        $("#statusBox li").eq(parseInt(statusId) - 1).find('i').addClass('checked');
    };
    // var refundType = $("#refondType").val();
    // if (refundType == 1) {
    //     $("#selectRefundType i").removeClass('checked');
    //     $("#selectRefundType i").eq(0).addClass('checked');
    // }
    // else {
    //     $("#selectRefundType i").removeClass('checked');
    //     $("#selectRefundType i").eq(1).addClass('checked');
    // }
})
// 申请申诉
$(function () {
    size = Math.floor(($("#MContainer").width() * 0.95 - 15 * 3) / 4) - 2;
    $("#fileList .item").css({ 'width': size, 'height': size, 'line-height': size + 'px' });

    $("#selectReason").on('click', function () {
        $(this).popShow({ positionY: "center", openAnimationName: "bottomShow", closeAnimationName: "bottomHide", targetEl: "#appealReason", closeBtnName: ".cancel,.sure" });
    })

    // 选择申诉原因
    var reasonText, reasonId;
    $("#appealReason li").click(function () {
        reasonText = FSH.string.Trim($(this).text());
        reasonId = $(this).attr('val');
        $("#appealReason .radio").removeClass('checked');
        $(this).find('.radio').addClass('checked');
    })

    $("#appealReason .sure").on('click', function () {
        $("#reason").attr('value', reasonText).attr('reasonId', reasonId);
    })

    // 选择退款类型
    $("#selectRefundType span").on('click', function () {
        if ($("#isReturn").val() == 0 && $(this).index() == 0) {
            FSH.smallPrompt("该商品不能退货！");
            $(this).find('.radio').removeClass('checked');
            return;
        };

        $("#selectRefundType i").removeClass('checked');
        $(this).find('i').addClass('checked');
        var type = $(this).attr('val');
        $("#refondType").attr('value', type);



    })

    // 商品状态
    $("#selectStatus").on('click', function () {
        $(this).popShow({ positionY: "center", openAnimationName: "bottomShow", closeAnimationName: "bottomHide", targetEl: "#statusBox", closeBtnName: ".cancel,.sure" });
    })
    var statusText, statusId;
    $("#statusBox li").click(function () {
        statusText = FSH.string.Trim($(this).text());
        statusId = $(this).attr('val');
        $("#statusBox .radio").removeClass('checked');
        $(this).find('.radio').addClass('checked');
    })

    $("#statusBox .sure").on('click', function () {
        $("#status").attr('value', statusText).attr('statusId', statusId);
    })

    // 照片
    $("#fileList ").on('click', '.close', function () {
        $(this).parent('.item').remove();
        checkPhotoNum();
    })

    // 上传图片
    var btn = $("#add");
    btn.on('click', function () {
        if ($("#fileList .item").length >= 4) {
            FSH.smallPrompt('最多上传10张图片');
        }
    })
    uploadImage = new AjaxUpload(btn, {
        action: '/refund/UploadImage',
        fileType: '.jpg,.jpeg,.png',
        name: 'userfile',
        autoSubmit: true,
        responseType: "json",
        onSubmit: function (file, extension) {
            var ext = extension[0].toLowerCase();
            if (ext != "jpg" && ext != "jpeg" && ext != "png") {
                {
                    FSH.smallPrompt("只支持上传PNG、JPG 或 JPEG格式的图片！");
                    return false;
                }
            }
        },
        onComplete: function (file, json) {
            if (json.Type == 1) {
                var itemHtml = '<div class="item" style="width:' + size + 'px; height:' + size + 'px; line-height:' + size + 'px">\
						            <i class="close"></i>\
						            <div>\
						            	<a href="/refund/refundimage/?path=' + json.Data.Path + '" target="_self"><img data-path="' + json.Data.Path + '" src="' + json.Data.Url + '" /></a>\
						            </div>\
						        </div>';
                $("#fileList").append(itemHtml);
                checkPhotoNum();

            } else {
                FSH.commonDialog(1, [json.Content]);
            }
        }
    });

    

    $("#submitBtn").on('click', function () {
        // model={RefundType:退款类型，RefundReason:退款原因，RefundDescription:退款详细描述，ImagePath:上传的图片地址，ProductStatus:商品状态，OrderCode:订单号}
        var RefundType = 0;
        var RefundReason = $("#reason").attr('reasonid');
        var RefundDescription = $("#detailReason").val();
        var ProductStatus = $("#status").attr('statusid');
        var ImagePath = getAllImages();
        var sku = $("#sku").val();
        var OrderCode = $("#orderCode").val();
        var refundCode = $("#refundCode").val();
        var isUserdGiftCard = $("#isUserdGiftCard").val()
        if (RefundReason == 0) {
            FSH.smallPrompt('请选择退款原因');
            return;
        }

        if (RefundDescription == '') {
            FSH.smallPrompt('请填写详细理由');
            return;
        };

        if (ProductStatus == 0) {
            FSH.smallPrompt('请选择商品状态');
            return;
        };

        if (ImagePath.length < 2) {
            FSH.smallPrompt('请至少上传两张图片');
            return;
        }
        if (isUserdGiftCard==0) {

            FSH.commonDialog(2,['确定要提交退款申请吗？'],'#submitBtn','confirmFun');
        } else {
            FSH.commonDialog(2,['该商品下单时使用了优惠券,退款时只退实际支付的金额.\n确定要提交退款申请吗？'],'#submitBtn','confirmFun');
        }


    })



})

function confirmFun(){
    var RefundType = 0;
    var RefundReason = $("#reason").attr('reasonid');
    var RefundDescription = $("#detailReason").val();
    var ProductStatus = $("#status").attr('statusid');
    var ImagePath = getAllImages();
    var sku = $("#sku").val();
    var OrderCode = $("#orderCode").val();
    var refundCode = $("#refundCode").val();
    ImagePath = ImagePath.join(',');
    var model = {
        RefundType: RefundType,
        RefundReason: RefundReason,
        RefundDescription: RefundDescription,
        ImagePath: ImagePath,
        ProductStatus: ProductStatus,
        orderCode: OrderCode,
        refundCode: refundCode
    }


    FSH.Ajax({
        url: '/refund/submitComplain',
        data: { sku: sku, model: JSON.stringify(model) },
        type: 'post',
        success: function (msg) {
            if (msg.Type == 1) {
                window.location.href = window.hostname + msg.LinkUrl;
            } else {
                FSH.commonDialog(1, [msg.Content]);
            }
        }
    })
}

// 检查上传图片是否超过20张
function checkPhotoNum() {
    if ($("#fileList .item").length >= 10) {
        uploadImage._disabled = true;
    } else {
        uploadImage._disabled = false;
    }
    //页面中图片加载完成后固定页底
    if ($("#fileList img").length == 0) {
        FSH.fixedFooter();
    } else {
        $("#fileList img").each(function () {
            $(this).on("load", function () {
                FSH.fixedFooter();
            })
        })
    }
}

function getAllImages() {
    var imgPaths = [];
    var items = $("#fileList .item:not(#add)");
    for (var i = 0; i < items.length; i++) {
        imgPaths.push(items.eq(i).find('img').attr('data-path'))
    }
    return imgPaths;
}
