var InterValObj,SysSecond,teamUrl;

$(function(){
	//配置微信
	var teamcode = $('#TeamCode').val(),
			Flag = $("#Flag").val(),
			RestTeamMemberNum=$("input[name='RestTeamMemberNum']").val();//拼团剩余人数
  FSH.share.getWXConfig("TeamShared");
	var clientWidth = $("#MContainer").width();
	var nWidth = parseInt(clientWidth)*0.95*0.18;
	$("#product .imgBox").height( nWidth+'px' );
	var infoWidth = (parseInt( clientWidth )*0.95*0.82 - 7 - 2)*0.4;
	var infoHeight = infoWidth*35/184;
	// $("#info").height( infoHeight + 'px' )
	// 		  .css({'line-height':infoHeight+'px','border-radius':infoHeight/2+'px'})

	var avastarWidth = parseInt(clientWidth)*0.95*0.6/5;
	$("#people li").width( avastarWidth + 'px' ).height( avastarWidth + 'px' );

	$("#share").on('click', function () {
		var isWeiXin = FSH.share.is_weixin();
		if (isWeiXin) {
			if ( $(".shareInWX").length < 1 ) {
  			$("#MContainer").append('<div class="shadow shareInWX">\
				    <div class="guide">\
				        <img src="../../Content/Images/groupon/shareInWX.png?v=20160414" />\
				    </div>\
				    <div class="content f30 fontWhite tc">\
				        还差 '+ RestTeamMemberNum+' 人就组团成功啦~<br>赶快呼唤小伙伴们参加吧！\
				    </div>\
				</div>');
  		}
		}else{
			if ( $(".shareInBrowser").length < 1 ) {
			 	$("#MContainer").append('<div class="shadow shareInBrowser">\
									    <div class="f30 tc ">\
									        请使用浏览器自带的分享功能<br><br>\
									        分享给微信好友<br><br>\
									        或直接复制链接发送给好友\
									    </div>\
									</div>').scrollTop(0).height( $(window).height() ).css('overflow-y','hidden');
			 }
		}
	})

	// 点击透明背景消失
	$("#MContainer").on('click','.shadow',function(){
		$(this).remove();
		$("#MContainer").height('auto').css('overflow-y','scroll');
	})

	if($("#duration").length>0){ 
		SysSecond = $("#duration").attr("data-time");
		$("#duration").show();
		SetRemainTime();
		if(SysSecond && SysSecond>0){
			 InterValObj = window.setInterval(SetRemainTime, 1000); //间隔函数，1秒执行 
	 	}	
	}


	// 返回按钮
	$("#pageReturn").on('click',function(){
		var a = window.location.href;
		console.log(a.indexOf('ZFBReturnPage.html'));
		if ( a.indexOf('ZFBReturnPage.html') > 0 ) { // 如果不是从订单详情来的(开团支持成功后)
			window.location.href = window.hostname + '/product/FightIndex';
		}else{
			window.history.back(-1);
		}
	})

})

//将时间减去1秒，计算天、时、分、秒 
function SetRemainTime() { 
if (SysSecond > 0) { 
	SysSecond = SysSecond-1; 
	var second = Math.floor(SysSecond % 60);// 计算秒     
	var minite = Math.floor((SysSecond / 60) % 60);      //计算分 
	var hour = Math.floor((SysSecond /60/60)%24);      //计算小时
	var day  = Math.floor((SysSecond /60/60/24));      //计算天
	if(second<10){
		second="0"+second;
	}
	if(minite<10){
		minite="0"+minite;
	}
	if(hour<10){
		hour="0"+hour;
	}
	if(day<10){
		day="0"+day;
	}
	$("#day").html(day);
	$("#hour").html(hour);
	$("#minite").html(minite);
	$("#second").html(second);
} else {//剩余时间小于或等于0的时候，就停止间隔函数 
    window.clearInterval(InterValObj);
    
    $("#hour").html("00");
    $("#minite").html("00");
    $("#second").html("00");
	//这里可以添加倒计时时间为0后需要执行的事件 
	//window.location.reload();
} 
} 