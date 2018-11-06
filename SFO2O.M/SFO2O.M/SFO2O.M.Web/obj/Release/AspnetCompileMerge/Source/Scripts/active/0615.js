//母亲节活动
$().ready(function(){
	$("#returnBtn").off("click").on("click",function(){
		window.location=window.hostname;
	})
	//微信分享
	FSH.share.getWXConfig("activity0615Shared");
	$(".lazyloadImg").lazyload({
					effect : "fadeIn",
					threshold :"200"
	});
})
