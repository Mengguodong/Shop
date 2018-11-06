
function pay(type, url, payType) {

    var orderId = $("#orderId").val();
    //1,微信支付  2 支付宝支付  3. 易宝快捷支付  


    if (payType == 1 || payType == 2) {

        $.post(window.hostname + "/TwoDimensionCode.html", { PayWay: payType, orderId: orderId }, function (data) {

            if (data.result == true) {
                $('#payImg').attr("src", data.imageUrl);
                $('#div_img').show();
                GetOrderState(orderId);
                $('#div_form').hide();

            } else {
                alert(data.Msg);


            }
        });
    } else {

        ajaxFlag && (ajaxFlag = !1,
FSH.Ajax({
    url: window.hostname + url,
    dataType: "json",
    data: {
        id: $("#orderId").val(),
        payType: payType
    },
    jsonp: "callback",
    jsonpCallback: "success_jsonpCallback",
    success: function (data) {
        if (data.Type == 1)
            window.location.href = type == "wx" ? "/Pay/WxPay/" + $("#orderId").val() : type == "yeePay" ? data.Url : "/Pay/WxPay/" + $("#orderId").val();
        else if (data.Type == 0)
            FSH.commonDialog(1, [data.Content]),
            $("body").off("click", "#errorTipdialog .closeBtn");
        else {
            FSH.commonDialog(1, [data.Content]);
            $("body").on("click", "#errorTipdialog .closeBtn", function () {
                window.location = window.hostname + "/OrderSumit.html?id=" + $("#orderId").val()
            })
        }

    }
}))

    }


}
function GetOrderState(orderCode) {
    //定时请求  支付状态，请求50次，1秒
    var index = 0;
    var setIntervalOut = setInterval(function () {
        index++;
        $.ajax({
            type: "post",
            url: window.hostname + "/Order/GetOrderInfoByOrderCode",
            data: { OrderCode: orderCode },
            async: false,
            success: function (result) {
                if (result.OrderStatus == 1) {
                    window.clearInterval(setIntervalOut);
                    //$("#center").find("img").attr("src", gpath.static_path + "Show/images/success.png");
                    //$("#center").find("span").text("成功!");
                    //popCenterWindow();
                    location.href = window.hostname + "/Pay/ZFBReturnPage";
                }
                //if (result.OrderStatus == 0) {
                //    window.clearInterval(setIntervalOut);
            
                //    //popCenterWindow();
                //    location.href = window.hostname + "/ZFBReturnPage";
                //}

            }
        });


        if (index >= 60) {
            window.clearInterval(setIntervalOut);
        }
    }, 1000);
}


$(function () {
    $("#returnRefundList").click(function () {
        window.location.href = window.hostname + '/my/list?status=0';
    })

    $("#more").on('click', function () {
        $("#wxBtn").removeClass('hide');
        $("#testPay").removeClass('hide');
        $(this).fadeOut();
    })

    $(".payMentList ").on('click', function () {
        $(this).find('i').addClass('radioRight').end().addClass('selected').siblings('.payMentList').removeClass('selected').find('i').removeClass('radioRight');
    })

    $("#submitBtn").on('click', function () {
        if ($("#zfbBtn").hasClass('selected')) {
            pay("zfb", "/ZOrderPay.html", 2);
        }
        else if ($("#wxBtn").hasClass('selected')) {
            pay("wx", "/ZOrderPay.html", 1);
        }
        else if ($("#testPay").hasClass('selected')) {
            pay("testPay", "/ZOrderPay.html", 1);

        } else if ($("#yeePay").hasClass('selected')) {
            pay("yeePay", "/YOrderPay.html", 3);
        } else if ($("#yeePay").hasClass('selected'))
        {
            pay("yeePay", "/ScoreOrderPay.html", 4);
        }
    });

});