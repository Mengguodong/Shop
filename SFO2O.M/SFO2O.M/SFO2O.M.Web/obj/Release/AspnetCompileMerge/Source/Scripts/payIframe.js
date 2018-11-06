$().ready(function(){
		var orderId=FSH.tools.request("orderId");
		$("#payIframe").height($(window).height());
		if (ajaxFlag && orderId) {
        //获取校验码
        ajaxFlag = false;
        FSH.Ajax({
            url: window.hostname +"/ZOrderPay.html",
            dataType: 'json',
            data: { id:orderId},
            jsonp: 'callback',
            jsonpCallback: "success_jsonpCallback",
            success: function (json) {
                if (json.Type == 1 && json.orderStatus !=5) {
							   $("#payIframe").attr("src",unescape(json.Url));                 
                }

            },
            error: function (err) {
                FSH.commonDialog(1, ['请求超时，请刷新页面']);
            }
        })
    }

		$(window).resize(function(){
			$("#payIframe").height($(window).height());
		})
	})