var url = window.hostname + "/my/HLDetailList",
		pageIndex = 0,//页码 从1开始
		type = 0,//0 全部酒豆 1获得酒豆 2消费酒豆
		count = 20,//每页显示条数
		scrollPageFlag = true;//scrollPageFlag 是否有下一页标志位 true 有 false 没有
$().ready(function () {
    loadData();
    FSH.scrollPage(loadData);
    $("#selectedItem").click(function () {
        if ($("#selectUL").css("display") == "none") {
            if ($("#selectUlMask").length == 0) {
                $("body").append('<div id="selectUlMask" class="ajaxMark"></div>');
            }
            $("#selectUlMask").height(FSH.tools.getClientHeight()).show();
            $("#selectUL").slideDown();
        }
    })
    $("body").on("click", "div#selectUlMask", function () {
        $("#selectUL").slideUp();
        $("#selectUlMask").hide();
    })
    $("#selectUL li").click(function () {
        if (!$(this).hasClass("cur")) {
            $(this).addClass("cur").siblings("li.cur").removeClass("cur");
            $("#selectedItem").html($(this).find("b").html() + '<i class="itemIcon5"></i>');
            $("#selectUL").slideUp();
            $("#selectUlMask").hide();
            pageIndex = 0;
            type = $(this).attr("data-type");
            scrollPageFlag = true;
            loadData();
        }
    })
})
/*
{
    "Type": 1,
    "Data": {
        "PageIndex": 1,
        "PageSize": 10,
        "TotalRecord": 37,
        "PageCount": 4,
        "Products": [
            {
                "Description": "购物抵扣",
                "CurrentHuoLi": 100,
                "ChangedHuoLi": 100,
                "Direction": 1,
                "CreateTime": "2014-08-06 12:12:12",
                "TradeCode":"s13513135335153"
            }
        ]
    }
}
请求参数:
{"pageIndex":页码,"pageSize":每页显示条数,"type":0 全部酒豆 1获得酒豆 2消费酒豆}
返回值说明：
PageIndex 当前页码
PageSize 每页显示条数
TotalRecord 总公共条数
PageCount 总共页数
Products 列表内容
Description 项目名称， CurrentHuoLi 剩余的酒豆数量， ChangedHuoLi 变更数量 ，Direction -1是- 1是+， CreateTime 进出时间, TradeCode 订单号
其中 Remaining ChangeNum 需要格式化进位
 */
function loadData() {
    if (ajaxFlag && scrollPageFlag) {
        pageIndex++;
        if (type == null) {
            type = 0;
        }
        FSH.Ajax({
            url: url,
            type: "get",
            data: { "pageIndex": pageIndex, "pageSize": count, "type": type },
            dataType: 'json',
            jsonp: 'callback',
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
                    if (json.Data.PageIndex == 1) {
                        $("#integralList").html("");
                    }
                    if (!json.Data.Products) {
                        $("#more").hide();
                        $("#emptyTip").show();
                        $("#footerWrap,#integralList").hide()
                    } else {
                        var html = className = str = orderStr= '';
                        for (var i = 0; i < json.Data.Products.length; i++) {
                            if (json.Data.Products[i].Direction == 1) {
                                className = 'addColor';
                                str = '+';
                                
                            } else {
                                className = '';
                                str = '-';
                                
                            } 
                            if(!json.Data.Products[i].TradeCode){
                                orderStr="";
                            }else if(json.Data.Products[i].TradeCode.substr(0,1).toUpperCase()=="S"){
                                orderStr ='<p class="f20 fontcolor">订单号：' + json.Data.Products[i].TradeCode + '</p>';
                            }else if(json.Data.Products[i].TradeCode.substr(0,1).toUpperCase()=="R"){
                                orderStr ='<p class="f20 fontcolor">退款单号：' + json.Data.Products[i].TradeCode + '</p>';
                            }
                           
                            html += '<li class="whiteOnLine borderBottom pr">\
														<div>\
                                                            <p class="f28">' + json.Data.Products[i].Description + '</p>'+orderStr+'<p class="f20 fontcolor">剩余：' + parseFloat(json.Data.Products[i].CurrentHuoLi).toLocaleString() + '</p>\
															<p class="f20 FontColor4">' + json.Data.Products[i].CreateTime + '</p>\
														</div>\
														<p class="pa f28 ' + className + ' integralNum">' + str + parseFloat(json.Data.Products[i].ChangedHuoLi).toLocaleString() + '</p>\
													</li>';
                        }
                        $("#integralList").append(html);
                        $("#emptyTip").hide();
                        $("#footerWrap,#integralList").show()
                        FSH.fixedFooter();
                    }
                }
                else {
                    FSH.commonDialog(1, [json.Content]);
                }

            },
           // error: function (err) {
             //   FSH.commonDialog(1, ['请求超时，请刷新页面']);
           // }
        });


    }
}