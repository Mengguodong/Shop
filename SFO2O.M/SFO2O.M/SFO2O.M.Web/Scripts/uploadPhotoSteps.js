$().ready(function(){
	$("#stepsTabs a").click(function(){
		if(!$(this).hasClass("cur")){
			$(this).addClass("cur").siblings("a.cur").removeClass("cur");
			$("#stepsContent"+$("#stepsTabs a").index($(this))).show().siblings(".stepsContent").hide();
		}
	})
})