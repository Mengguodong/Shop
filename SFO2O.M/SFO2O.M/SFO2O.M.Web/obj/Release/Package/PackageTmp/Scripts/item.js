var pid;
var myScroll;
function fun(collectionStatus){
	//if(collectionStatus){
	//	$("#collectionBtn").find("span").removeClass("selected").end().find("b").html("收　藏");

	//}else{
	//	$("#collectionBtn").find("span").addClass("selected").end().find("b").html("已收藏");
	//}
}
$().ready(function(){
	//收藏功能
	$("#collectionBtn").click(function(){
		var productCode=FSH.tools.request("productCode"),
		    collectionStatus=$(this).find("span").hasClass("selected");
		FSH.collectionFun(productCode,collectionStatus,fun);
	})
	FSH.addGoTop();
	pid=FSH.tools.request("pid");
	
	$("#MContainer").css("padding-bottom","50px");

	
	//$(".itemFeature li").css("text-indent",$("#MContainer").width()*0.025)
	/*图片轮播*/
	$("#mySwipe,#similarBrand ").show()
	if($("#mySwipe .swipe ul").length>1){
		var swipeCount = $("#mySwipe .swipe-wrap li").length;
		var indexStr = '';
		for (var i = 0; i < swipeCount ; i++ ) {
			indexStr = indexStr + "<li></li>";
		};
		$("#mySwipeLiItems ul").append(indexStr).find('li:first').addClass('on');
		TouchSlide({slideCell:"#mySwipe",effect:"leftLoop",autoPlay:true,interTime:3500});
	}
	//相似品牌推荐
	if($("#similarBrand .similarBrandItems ul").length>1){
		var similarBrandItemsCount = $("#similarBrand .similarBrandItemsWrap").length;
		var similarBrandIndexStr = '';
		for (var t = 0; t < similarBrandItemsCount ; t++ ) {
			similarBrandIndexStr = similarBrandIndexStr + "<li></li>";
		};
		$("#similarBrandIndex ul").append(similarBrandIndexStr).find('li:first').addClass('on');
		TouchSlide({slideCell:"#similarBrand",effect:"left",autoPlay:false,interTime:3500});
	}
	//同品牌推荐
	if($("#hotPro").length>0){
		var hotProItemWidth = ($('.MContainer').width()*0.975-parseInt($("#hotPro .item").css('margin-right'))*2)*0.4 - parseInt($("#hotPro .item").css('padding-left')) - parseInt( $("#hotPro .item").css('padding-right') );
		$("#hotPro .item").css({
				width:hotProItemWidth // 一屏显示两个半产品，中间有两个外间距
			})

		//console.log(Math.ceil(parseFloat(($("#hotPro").css('padding-left')))));
		$("#itemBox").css({
			width:($(".item").outerWidth())*$("#hotPro .item").length + ($("#hotPro .item").length-1)*parseInt( $(".item").css('margin-right') ) + Math.ceil(parseFloat(($("#hotPro").css('padding-left'))))
		})
		myScroll = new IScroll('#hotPro', { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false });
	}	
	
	$("#skuSelectA").click(function(){//选择sku
		var t=new SelectSKU({json:eval('(' + $(this).attr("data-json") + ')'),type:0,resultEL:$(this),addCartSuccess:function(cartnum){
			addToCartSucFun(cartnum)
		}})
	})
	$("#addToCarBtn").click(function(){
		var s=new SelectSKU({json:eval('(' + $("#skuSelectA").attr("data-json") + ')'),type:1,originalSKU: $("#skuSelectA").attr("_selectedsku"),addCartSuccess:function(cartnum){
			addToCartSucFun(cartnum)
		}})
	})
	$("#buyNowBtn").click(function(){
		var k=new SelectSKU({json:eval('(' + $("#skuSelectA").attr("data-json") + ')'),type:2,originalSKU: $("#skuSelectA").attr("_selectedsku"),addCartSuccess:function(cartnum){
			addToCartSucFun(cartnum)
		}})
	})
	if($("#skuSelectA").length>0){
		checkSKU();
	}
})
function checkSKU(){
	var skustr=eval('(' + $("#skuSelectA").attr("data-json") + ')');
	if($(".nobuyBtn").length>0){
		//已售罄
		$("#skuSelectA").hide()
	}else if(!skustr.MainName && !skustr.MainCode){
		//没有sku属性
		$("#skuSelectA").hide()
	}else{
		$("#skuSelectA").trigger("click");
		$("#selectSKU .closeA").trigger("click");
	}

}
//加入购物车成功后飞入购物车
function addToCartSucFun(cartnum){
	FSH.addToCartSuccess();
	var startPos={};
	startPos.left = $(window).width()*0.5-25;
	startPos.top = $(window).height()*0.5-25;
	var ImgElSrc =$("#mySwipe .swipe ul").eq(0).find("img").attr("src");
	var targetPos = $("#headerCart").offset();
	FSH.fly('headerCart',targetPos,startPos,ImgElSrc,cartnum);
}