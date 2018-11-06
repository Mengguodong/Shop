//母亲节活动
$().ready(function(){
	$("#returnBtn").off("click").on("click",function(){
		window.location=window.hostname;
	})
	//微信分享
	FSH.share.getWXConfig("activity0701Shared");
	$(".lazyloadImg").lazyload({
			placeholder : window.hostname+"/Content/Images/active/0615/lazyload2.png", 
			effect : "fadeIn",
			threshold :"100"
	});
})
