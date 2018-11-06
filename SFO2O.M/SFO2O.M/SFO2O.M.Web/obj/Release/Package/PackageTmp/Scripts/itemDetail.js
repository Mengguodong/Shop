$().ready(function(){
	FSH.addGoTop();
	$("#stepsTabs a").click(function(){
		if(!$(this).hasClass("cur")){
			
			var t=$("#stepsTabs a").index($(this));
			if(t==1 && $("#stepsContent1").length==0){
				var str='<div class="FAQContent stepsContent w95p bgColor3 boxShadow mt8  mb20 hide" id="stepsContent1" style="min-height:500px;">\
				<img class="mt20" src="'+window.hostname+'/Content/images/step.jpg?2016042501" >\
				<img class="mt20 mb20" src="'+window.hostname+'/Content/images/FAQ.jpg?201604082">\
			  </div>';
			  $("#stepsContent0").after(str);
			}
			$(this).addClass("cur").siblings("a.cur").removeClass("cur");
			$("#stepsContent"+$("#stepsTabs a").index($(this))).show().siblings(".stepsContent").hide();
			FSH.fixedFooter();
		}
	})
})
