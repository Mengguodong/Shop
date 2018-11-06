$().ready(function(){
	var index=FSH.tools.request("index");
	if(index==""){
		index=0;
	}
	$("#mySwipe ").show()
		if($("#mySwipe .swipe ul").length>1){
		var swipeCount = $("#mySwipe .swipe-wrap li").length;
		var indexStr = '';
		for (var i = 0; i < swipeCount ; i++ ) {
			indexStr = indexStr + "<li></li>";
		};
		$("#mySwipeLiItems ul").append(indexStr).find('li').eq(index).addClass('on');
		TouchSlide({slideCell:"#mySwipe",effect:"leftLoop",autoPlay:true,interTime:3500,defaultIndex:index});
	}
})