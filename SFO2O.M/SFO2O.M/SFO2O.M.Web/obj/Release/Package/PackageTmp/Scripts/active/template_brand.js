//母亲节活动
$().ready(function(){
	var ref=document.referrer?document.referrer:window.hostname;
	$("#returnBtn").off("click").on("click",function(){
		window.location=ref;
	})
	$(".lazyloadImg").lazyload({
	    placeholder: window.hostname + "/Content/images/blank.png",
			effect : "fadeIn",
			threshold :"100"
	});
	setImgWH();
	$(window).resize(function(){
		setImgWH();
	})
})
function setImgWH(){
	var height=$("#MContainer").width()*310/750;
	$(".img310").width($("#MContainer").width()*310/750).height(height);
	$(".productDes").each(function(i){
		var wrap=$(this).parents("div.proDiv");
		var descriptHeight=parseInt(height-wrap.find("h4").outerHeight()-wrap.find(".productPrice").outerHeight()-wrap.find(".btn_temp").outerHeight());
		var clamp=Math.floor(descriptHeight/16).toString();
		$(this).height(descriptHeight);
		$(this).find("div").css({"-webkit-line-clamp":clamp});

	})
}
