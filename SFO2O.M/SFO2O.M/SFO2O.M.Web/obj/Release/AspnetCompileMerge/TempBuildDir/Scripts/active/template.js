$(function(){
	$(".productList .imgBox").height( $(".productList .imgBox").width()  );

	$(".lazyloadImg").lazyload({
	    placeholder: window.hostname + "/Content/images/blank.png",
		effect : "fadeIn"
	})

	$("#inductiveTab").tabView();

	$("#returnBtn").off('click').on('click',function(){
		window.location.href = document.referrer;
	})

})
