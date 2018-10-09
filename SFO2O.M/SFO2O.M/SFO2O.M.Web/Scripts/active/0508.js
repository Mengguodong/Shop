//母亲节活动
$().ready(function(){
	$("#returnBtn").off("click").on("click",function(){
		window.location=window.hostname;
	})
	FSH.share.getWXConfig("MothersdayShared");
})