var url = window.hostname + "/Favorite/FavoriteListIndex",
	pageIndex = 0,//页码 从1开始
	count = 10,//每页显示条数
	scrollPageFlag = true,//scrollPageFlag 是否有下一页标志位 true 有 false 没有
    selectedSPU = selectedBrandUrl = "";
function fun(t) {
    $("#skuMark,#collectionDialog").hide();
    $("#collectionList li[_spu='" + selectedSPU + "']").remove();
    FSH.fixedFooter();
    if ($("#collectionList li").length == 0) {
        pageIndex = 0;
        scrollPageFlag = true;
        loadData();
    }
}
$().ready(function () {
    loadData();
    FSH.scrollPage(loadData);
    $("#collectionList").on("click", "a.setBtn", function () {
        //显示弹窗
        if ($("#collectionDialog").length == 0) {
            var html = '<div class="skuMark hide" id="skuMark"></div>\
    <div class="w100 selectSKU bgColor3  hide" id="collectionDialog">\
        <div class="w100 tc collectionDiaCon">\
            <a id="cancleCollection"><span class="collectionIcon1 "></span><br><span class="f28">取消收藏</span></a>\
            <a id="gotoBrand"><span class="collectionIcon2 "></span><br><span class="f28">进入品牌页</span></a>\
        </div>\
        <a id="cancleBtn" class="btn  tc f28 boxflex1">取消</a>\
    </div>';
            $("#MContainer").append(html);
        }
        selectedSPU = $(this).attr("_spu");
        selectedBrandUrl = $(this).attr("_brandUrl");
        $("#skuMark,#collectionDialog").show();

    })
    $("#MContainer").on("click", "#cancleBtn", function () {
        //关闭弹窗
        $("#skuMark,#collectionDialog").hide();
    })
    $("#MContainer").on("click", "#cancleCollection", function () {
        //取消收藏
        FSH.collectionFun(selectedSPU, true, fun);
    })
    $("#MContainer").on("click", "#gotoBrand", function () {
        //进入品牌页
        $("#skuMark,#collectionDialog").hide();
        window.location = selectedBrandUrl;

    })
})
function loadData() {
    if (ajaxFlag && scrollPageFlag) {
        pageIndex++;
        FSH.Ajax({
            url: url,
            type: "get",
            data: { "pageIndex": pageIndex },
            dataType: 'json',
            jsonp: 'callback',
            cache:false,
            loadingType:2,
            jsonpCallback: "success_jsonpCallback",
            success: function (json) {
                if (json.Type == 1) {
                    if (json.Data.PageIndex == json.Data.PageCount) {
                        $("#more").show().html("全部加载完");
                        scrollPageFlag = false;
                    } else {
                        $("#more").show().html("上滑加载更多");
                        scrollPageFlag = true;
                    }
                    if (json.Data.Products == null) {
                        $("#more").hide();
                        $("#emptyTip").show();
                        //$("#footerWrap,#collectionList").hide()
                    } else {
                        var html = className = str = href = '';
                        for (var i = 0; i < json.Data.Products.length; i++) {
                            if (json.Data.Products[i].fitype == 0) {
                                className = 'failure';
                                str = '<p class="collectionListTip FontColor6"><span>失效</span></p>';
                                href = 'javascript:void(0)';
                            } else {
                                href = window.hostname + '/item.html?productCode=' + json.Data.Products[i].spu;
                                className = '';
                                if (json.Data.Products[i].isDiscount == 1) {
                                    str = '<p class="collectionListTip FontColor6"><span>降</span>比收藏时降价￥' + json.Data.Products[i].DiscountPrice.toFixed(2) + '</p>';
                                } else {
                                    str = '<p class="collectionListTip FontColor6" style="visibility:hidden;"><span>失效</span></p>';
                                }

                            }
                            html += '<li _spu="' + json.Data.Products[i].spu + '" class="boxShadow   w95p bgColor3 overflowH pt15 pb15 mb8 pr ' + className + '">\
            <a href="'+ href + '" target="_self" class="displayBox w100 ">\
                <img class="collectionListImg lazyloadImg" data-original="' + json.ImageServer +"/"+ json.Data.Products[i].ImagePath + '">\
                <div class="boxflex1 ">\
                    <p class="collectionListName f24">' + json.Data.Products[i].productName + '</p>' + str + '\
                    <p class="collectionListPrice f28 FontColor6">￥' + json.Data.Products[i].originalPrice.toFixed(2) + '</p>\
                </div>\
            </a>\
            <a class="setBtn pa" _brandUrl=' +window.hostname+"/brand/"+ json.Data.Products[i].BrandId + '.html  _spu="' + json.Data.Products[i].spu + '"><img src="../Content/images/collection/icon.png"></a>\
        </li>';
                        }
                        $("#collectionList").append(html);
                        $(".lazyloadImg").lazyload({
                            placeholder: window.hostname + "/Content/Images/default_product.png",
                            effect : "fadeIn",
                            threshold :200

                        });
                        $("#emptyTip").hide();
                        $("#footerWrap,#collectionList").show()
                        FSH.fixedFooter();
                    }
                }
                else {
                    FSH.commonDialog(1, [json.Content]);
                }

            },
            //error: function(err) {
              //      FSH.commonDialog(1,['请求超时，请刷新页面']);   
          //  }
        });

    }
}