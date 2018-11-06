var url = window.hostname + "/GiftCard/GiftCardList",
	pageIndex = 0,//页码 从1开始
	count = 10,//每页显示条数
	scrollPageFlag = true,//scrollPageFlag 是否有下一页标志位 true 有 false 没有
    type = "0";//0未使用，1已过期, 2已使用
$().ready(function () {
    loadData();
    FSH.scrollPage(loadData);
    $("#tabs a").click(function () {
        if (!$(this).hasClass("cur")) {
            type = $(this).attr("_index");
            pageIndex = 0;
            scrollPageFlag = true;
            $(this).addClass("cur").siblings("a.cur").removeClass("cur");
            $("#couponsList").html("").hide();
            loadData();
        }
    })
    $("#couponsList").on("touchstart","li.effective a",function(){
        $(this).addClass("active");
    })
    $("#couponsList").on("touchend","li.effective a",function(){
        $(this).removeClass("active");
    })
})
function loadData() {
    if (ajaxFlag && scrollPageFlag) {
        pageIndex++;
        FSH.Ajax({
            url: url,
            type: "post",
            data: { "pageIndex": pageIndex, "type": type },
            dataType: 'json',
            jsonp: 'callback',
            cache: false,
            loadingType:2,
            jsonpCallback: "success_jsonpCallback",
            success: function (json) {
                if (json.Type == 1) {
                    if (json.Data.TotalRecords ==0) {
                        $("#couponsList,#more").hide();
                        $("#emptyTip").show();
                    } else {
                        if (json.Data.PageIndex == json.Data.PageCount) {
                            $("#more").show().html("全部加载完");
                            scrollPageFlag = false;
                        } else {
                            $("#more").show().html("上滑加载更多");
                            scrollPageFlag = true;
                        }
                        var html = statusClass = statusStr = gotourl=gotoFont='';
                        switch (type) {
                            case "0":
                                statusClass = "effective";
                                statusStr = "有效";
                                gotourl='href="'+window.hostname+'/" target="_self"';
                                gotoFont='<div class="pa gotoFont f20">去使用<span></span></div>';
                                break;
                            case "1":
                                statusClass = "failure";
                                statusStr = "已过期";
                                break;
                            case "2":
                                statusClass = "failure";
                                statusStr = "已使用";
                                break;
                        }
                        for (var i = 0; i < json.Data.Products.length; i++) {
                            //判断优惠券适用的商品
                            var giftCardType = "";
                            if (json.Data.Products[i].SatisfyProduct == 1) {
                                giftCardType = '该券只适用于原价商品';
                            } else if (json.Data.Products[i].SatisfyProduct == 2)
                            {
                                giftCardType = '该券只适用于促销商品';
                            } else if (json.Data.Products[i].SatisfyProduct == 3)
                            {
                                giftCardType = '除拼生活商品外均可用';
                            } else if (json.Data.Products[i].SatisfyProduct == 4) {
                                giftCardType = '该券只适用于拼生活商品';
                            } else if (json.Data.Products[i].SatisfyProduct == 5) {
                                giftCardType = '该券适用于原价商品和拼生活商品';
                            } else if (json.Data.Products[i].SatisfyProduct == 6) {
                                giftCardType = '该券适用于促销商品和拼生活商品';
                            } else if (json.Data.Products[i].SatisfyProduct == 7) {
                                giftCardType = '全场通用';
                            }
                            // 区分优惠券的类型 1：现金  2：满减
                            var str = '';
                            if (json.Data.Products[i].CardType == 2) {
                                str = '<p class="conditions1 f20">满' + json.Data.Products[i].SatisfyPrice + '元可用</p>'
                            } else {
                                str = '<p class="conditions1 f20">满' + json.Data.Products[i].SatisfyPrice + '元可用</p>';
                            }
                            html += '<li class="w100 ' + statusClass + '">\
                            <a '+gotourl+' class="w100 boxFlexWrap">\
        <div class="couponsLeft ">\
          <p class="status f20">'+ statusStr + '</p>\
          <p class="price1">￥' + json.Data.Products[i].CardSum + '</p>\
          ' + str + '\
        </div>\
        <div class="couponsRight boxFlex pr">\
          '+gotoFont+'\
          <p class="name f20">' + json.Data.Products[i].BatchName + '</p>\
          <p class="conditions2 f24">' + giftCardType + '</p>\
          <p class="date f20">有效期 ' + json.Data.Products[i].BeginTimeToString + ' - ' + json .Data.Products[i].EndTimeToString + '</p>\
        </div>\
        </a>\
      </li>';
                        }
                        $("#couponsList").append(html).show();
                        $("#emptyTip").hide();
                        FSH.fixedFooter();
                    }
                }
                else {
                    FSH.commonDialog(1, [json.Content]);
                }
            },
            error: function(err) {
                    FSH.commonDialog(1,['请求超时，请刷新页面']);   
            }
        });
    }
}