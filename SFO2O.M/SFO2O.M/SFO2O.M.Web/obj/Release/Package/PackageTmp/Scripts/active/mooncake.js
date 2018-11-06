$(function(){
	$(".mooncakeList .imgBox").height( $(".mooncakeList .item").width() );
	$(".mooncakeList .more").height( $(".mooncakeList .item").height() );
	$(".mooncakeList .more div").css({
		width : $(".mooncakeList .item").width() - 14,
		height : $(".mooncakeList .more").height() - 14
	})

	

})