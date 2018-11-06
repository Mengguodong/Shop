$(function(){
	if( $("#hotPro").length > 0 ){

		var hotProItemWidth = ($('#MContainer').width()*0.975-parseInt($("#hotPro .item").css('margin-right'))*3)*0.28 - parseInt($("#hotPro .item").css('padding-left')) - parseInt( $("#hotPro .item").css('padding-right') );
		$("#hotPro .item").css({
			width:hotProItemWidth // 一屏显示两个半产品，中间有两个外间距
		})
		$("#hotPro .imgBox").css({
			'width':hotProItemWidth,
			'height':hotProItemWidth,
			'overflow':'hidden'

		})

		//console.log(Math.ceil(parseFloat(($("#hotPro").css('padding-left')))));
		$("#itemBox").css({
			width:($(".item").outerWidth())*$("#hotPro .item").length + ($("#hotPro .item").length-1)*parseInt( $(".item").css('margin-right') ) + Math.ceil(parseFloat(($("#hotPro").css('padding-left'))))
		})
		myScroll = new IScroll('#hotPro', { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false });
	}
})