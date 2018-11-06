var url = window.hostname + "/brand/GetBrandStreetList",
	pageIndex = 0,//页码 从1开始
	count = 10,//每页显示条数
	scrollPageFlag = true;//scrollPageFlag 是否有下一页标志位 true 有 false 没有
$().ready(function () {
    loadData();
    FSH.scrollPage(loadData);
})
function loadData() {
    if (ajaxFlag && scrollPageFlag) {
        pageIndex++;
        FSH.Ajax({
            url: url,
            type: "post",
            data: { "pageIndex": pageIndex },
            dataType: 'json',
            jsonp: 'callback',
            cache: false,
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
                    var html = descriptionHtml = '';
                    for(var i=0;i<json.Data.Products.length;i++){
                        if (json.Data.Products[i].Slogan) {
                            descriptionHtml = '<p class="w100 overflowH f20 FontColor1">' + json.Data.Products[i].Slogan + '</p>';
                        }else{
                            descriptionHtml="";
                        }
                        html+='<li class="w100 mb8 ">\
        <a href="/brand/' + json.Data.Products[i].Id + '.html" target="_self" class="w100 show pr">\
            <img data-original="' + json.ImageServer + json.Data.Products[i].Banner + '" class="w100 show banner_brand lazyloadImg ">\
            <div class="brandInf mc tc pa boxSizingB">\
                <div class="brandInfCon ">\
                    <h2 class="w100 overflowH f24">' + json.Data.Products[i].NameCN + ' ' + json.Data.Products[i].CountryName + '</h2>\
                    '+descriptionHtml+'\
                </div>\
            </div>\
        </a>\
    </li>';
                    }
                    $("#brandsList").append(html);
                    for(var t=0;t<$("#brandsList li").length;t++){
                        var h=$("#brandsList li").eq(t).find(".brandInf").outerHeight();
                        $("#brandsList li").eq(t).find(".brandInf").css("margin-top",-h/2+"px");
                    }
                    $(".lazyloadImg").lazyload({
                        placeholder: window.hostname + "/Content/Images/default_brand.png",
                        effect : "fadeIn",
                        threshold :200

                    });
                    FSH.fixedFooter();
                    FSH.setImgSize.setBannerSize($(".banner_brand"),300);
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