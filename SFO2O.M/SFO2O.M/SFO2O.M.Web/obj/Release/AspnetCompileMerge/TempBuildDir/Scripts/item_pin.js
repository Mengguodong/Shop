$().ready(function () {
    if(FSH.share.is_weixin()){
        //增加微信分享
        FSH.share.getWXConfig("PinLifeDetailSharedFlag");
    }
    FSH.addGoTop();
    if($("#MContainer .overdueJump").length>0){
        $("#footerWrap").css("padding-bottom", "50px")
    }
    /*图片轮播*/
    $("#mySwipe,#similarBrand ").show()
    if ($("#mySwipe .swipe ul").length > 1) {
        var swipeCount = $("#mySwipe .swipe-wrap li").length;
        var indexStr = '';
        for (var i = 0; i < swipeCount ; i++) {
            indexStr = indexStr + "<li></li>";
        };
        $("#mySwipeLiItems ul").append(indexStr).find('li:first').addClass('on');
        TouchSlide({ slideCell: "#mySwipe", effect: "leftLoop", autoPlay: true, interTime: 3500 });
    }
    $("#taxTip_pin").click(function () {
        $("#skuMark,#taxDialog").show();
    })
    $("#taxDialog .closeA,#skuMark").click(function () {
        //关闭弹窗
        $("#skuMark,#taxDialog").hide();
    })
    //售罄下点击购买按钮
    $(".soldOutStatu a").click(function (event) {
        FSH.EventUtil.preventDefault(event);
        FSH.commonDialog(1, ['该商品已售罄，商家正在补货中', '请耐心等待...'], '', '', '知道了');
    })
    //已过期下点击购买按钮
    $(".overdueStatu a").click(function (event) {
        FSH.EventUtil.preventDefault(event);
    })
})
