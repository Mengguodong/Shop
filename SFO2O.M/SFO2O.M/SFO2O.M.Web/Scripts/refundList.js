$(function(){

	ajaxData();


	// 加载数据
	function ajaxData(){
		if(ajaxFlag){
			ajaxFlag=false;
			$.ajax({
				url: '/refund/getrefundlist',
				type: 'get',
				data: {"pageIndex":pageIndex},
				success: function(json) {
					// var json = {"Type":1,"Data":{"List":[{"RefundCode":"R773-004","OrderCode":"773","RefundStatus":1,"RefundType":1,"Name":"OXBRIDGE TOWN 綁帶鞋(白色圓點)","unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":"颜色分类","MainValue":"粉红色","SubDicValue":"净含量","SubValue":"50g","TotalRecord":21},{"RefundCode":"R773-004","OrderCode":"773","RefundStatus":1,"RefundType":1,"Name":"OXBRIDGE TOWN - Lace-Ups Espedrilles(White Dots)","RefundTotalAmount":565.00,"unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":null,"MainValue":null,"SubDicValue":null,"SubValue":null,"TotalRecord":21},{"RefundCode":"R773-005","OrderCode":"773","RefundStatus":1,"RefundType":1,"Name":"OXBRIDGE TOWN 绑带鞋(白色圆点)","RefundTotalAmount":1000.00,"unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":null,"MainValue":null,"SubDicValue":null,"SubValue":null,"TotalRecord":21},{"RefundCode":"R773-005","OrderCode":"773","RefundStatus":1,"RefundType":1,"Name":"OXBRIDGE TOWN 綁帶鞋(白色圓點)","RefundTotalAmount":1000.00,"unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":null,"MainValue":null,"SubDicValue":null,"SubValue":null,"TotalRecord":21},{"RefundCode":"R773-005","OrderCode":"773","RefundStatus":1,"RefundType":1,"Name":"OXBRIDGE TOWN - Lace-Ups Espedrilles(White Dots)","RefundTotalAmount":1000.00,"unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":null,"MainValue":null,"SubDicValue":null,"SubValue":null,"TotalRecord":21},{"RefundCode":"R772-001","OrderCode":"772","RefundStatus":6,"RefundType":1,"Name":"OXBRIDGE TOWN 绑带鞋(白色圆点)","RefundTotalAmount":1000.00,"unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":null,"MainValue":null,"SubDicValue":null,"SubValue":null,"TotalRecord":21},{"RefundCode":"R772-001","OrderCode":"772","RefundStatus":6,"RefundType":1,"Name":"OXBRIDGE TOWN 綁帶鞋(白色圓點)","RefundTotalAmount":1000.00,"unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":null,"MainValue":null,"SubDicValue":null,"SubValue":null,"TotalRecord":21},{"RefundCode":"R772-001","OrderCode":"772","RefundStatus":6,"RefundType":1,"Name":"OXBRIDGE TOWN - Lace-Ups Espedrilles(White Dots)","RefundTotalAmount":1000.00,"unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":null,"MainValue":null,"SubDicValue":null,"SubValue":null,"TotalRecord":21},{"RefundCode":"R772-002","OrderCode":"772","RefundStatus":1,"RefundType":1,"Name":"OXBRIDGE TOWN 绑带鞋(白色圆点)","RefundTotalAmount":500.00,"unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":null,"MainValue":null,"SubDicValue":null,"SubValue":null,"TotalRecord":21},{"RefundCode":"R772-002","OrderCode":"772","RefundStatus":1,"RefundType":1,"Name":"OXBRIDGE TOWN 綁帶鞋(白色圓點)","RefundTotalAmount":500.00,"unitPrice":500.00,"TaxRate":13.00,"ImagePath":"http://o2oImages.dssfxt.net/LIB/Product/Footspot/CLO00001000-1.jpg","OrderTotalAmount":6596.40,"MainDicValue":null,"MainValue":null,"SubDicValue":null,"SubValue":null,"TotalRecord":21}],"TotalRecord":21,"PageIndex":2,"PageSize":10,"PageCount":3}};
					ajaxFlag=true;
					if ( json.Type == 1 ){
						

						if (json.Data.PageIndex == json.Data.PageCount) {
		        			isLastPage = true;
		        			$("#loadMore").text('全部加载完');
		        			ajaxFlag=false;
		        		}else{
		        			$("#loadMore").text('上滑加载更多');
		        			pageIndex +=1;
		        		}


						var temp = $.templates("#tmp");
						var htmlOutput = "";
						if (json.Data.List.length > 0) {
						    htmlOutput = temp.render(json.Data.List);
						}
						else {
						    htmlOutput = "<div class=\"noData\"><img src=\"../../Content/Images/refundList/noData.png?v=20151224\" alt=\"无数据\"><p class=\"mainTips\">您还没有退款/退货商品</p></div>";
						}
						$("#loadMore").before( htmlOutput );
						

						FSH.fixedFooter();
					}else{
						FSH.commonDialog(1,[json.Content]);
					}
				},
				error: function(err) {
					FSH.commonDialog(1,['请求超时，请刷新页面']);   
				}
			});
		}
	}


	FSH.scrollPage(ajaxData);

	$("#refundList").one('click',".cancelBtn",function(){
		var code = $(this).attr("refundcode");
		var _this = $(this);
		FSH.Ajax({
			type:'post',
			url:'/refund/CancelRefund',
			data:{refundCode:code},
			success:function(msg){
				FSH.commonDialog(1,[msg.Content]);
				// setTimeout("location.reload();",2000); 
				_this.parents('.item').find('.status').text('退款关闭');
				_this.remove();
			},
			error:function(msg){
				FSH.commonDialog(1,[msg.Content]);
			}
		})
	})



})

$.views.helpers({
	showStatus: function (status) {//自动保留两位小数
		var statusText;
		if (status == 1) {
			statusText = '待审核';
		}
		if (status == 2) {
			statusText = '上门取件';
		};
		if (status == 3) {
			statusText = '待退款';
		};
		if (status == 4) {
			statusText = '退款成功';
		};
		if (status == 5) {
			statusText = '退款关闭';
		};
		if (status == 6) {
			statusText = '退款关闭';
		};
		return statusText;
	},
	showSpec:function( mainKey,mainValue,subKey,subValue ){
		if ( mainKey != null && subKey == null ) {
			return mainKey + '：' + mainValue;
		}else if (mainKey != null && subKey != null) {
			return mainKey + '：' + mainValue + '，' + subKey + '：' + subValue;
		};
	},
	showByStatus:function( status ){
		if (status == 1) {
			return true;
		};
	},
	returnStatus:function(status){
		if (status != 1) {
			return true;
		}
	},
    showWeightUnit:function(str) {
        if (str == '净重' || str == '淨重' ) {
            return true;
        }
    },
    showContentUnit:function(str) {
        if (str == '净含量' || str == '淨含量') {
            return true;
        }
    }
    
});