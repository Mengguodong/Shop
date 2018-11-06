var myScroll;
$(function(){
	/*焦点图*/
	var swipeCount = $("#mySwipe .swipe-wrap li").length;
	var indexStr = '';
	for (var i = 0; i < swipeCount ; i++ ) {
		indexStr = indexStr + "<li></li>";
	};
	if (swipeCount > 1) {
		$("#mySwipeLiItems ul").append(indexStr).find('li:first').addClass('on');
	}
	$("#mySwipe").removeClass('hide');
	if ( $(".focusBoxIn").length > 1 ) {
		TouchSlide({slideCell:"#mySwipe",effect:"leftLoop",autoPlay:true,interTime:5500})
	}
	/*焦点图 end*/
	var hotProItemWidth = ($('.MContainer').width()*0.975-parseInt($(".indexHotPro .item").eq(0).css('margin-right'))*2)*0.4 - parseInt($(".indexHotPro .item").eq(0).css('padding-left')) - parseInt( $(".indexHotPro .item").eq(0).css('padding-right') );

	$(".indexHotPro .item").css({
		width:hotProItemWidth // 一屏显示两个半产品，中间有两个外间距
	})

	$(".indexHotPro .itemBox").each(function(i){
		$(this).css({
		width:($(".item").outerWidth())*$(this).parents(".indexHotPro").find(".item").length + ($(this).parents(".indexHotPro").find(".item").length-1)*parseInt( $(".item").css('margin-right') ) + Math.ceil(parseFloat(($(".indexHotPro").eq(0).css('padding-left'))))
	})
	})
	$(".lazyloadImg:not(.indexHotPro .lazyloadImg)").lazyload({
		placeholder:window.hostname+"/Content/images/blank.png",
		effect : "fadeIn",
		threshold :200,
		failurelimit : 100

	})
	for(var a=0;a<$(".indexHotPro").length;a++){
		$(".indexHotPro").eq(a).find(".lazyloadImg").lazyload({
			placeholder:window.hostname+"/Content/images/blank.png",
			effect : "fadeIn",
			threshold :200,
			failurelimit : 100,
		 container:$(".indexHotPro").eq(a)
		})
	}
	//搜索弹窗触发
	$("#gotoSearchPage").click(function(){

		$("#gotoSearchPage").showSearch({
			value:$(this).text(),
			placeholderFlag:true
		})
	})
})
