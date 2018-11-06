var initHeight, //记录导航条的初始位置
	targetTop,
	discountCon1Top,
	discountCon2Top,
	discountCon3Top,
	discountCon4Top,
	discountCon5Top;
var InterValObj,SysSecond;
// var myScroll;
var hasOcc = false;
var menuHeight;

$(function(){

	menuHeight = $("#discountTab").height();

	// 

	// 给导航条设置可以左右滑动
	// myScroll = new IScroll('#discountTab', { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false });
	
	// 计算浮动导航的位置
	$("#discountTab").css('margin-left', -$("#MContainer").width()/2 + 'px' );


	if($("#countdown").length>0){ 
		SysSecond = $("#countdown").attr("data-time");
		if(SysSecond && SysSecond>0){
			 InterValObj = window.setInterval(SetRemainTime, 1000); //间隔函数，1秒执行 
	 	}	
	}

	// 点击大菜单
	$("#menu li").on('click',function(){
		$(this).addClass('active').siblings().removeClass('active');
		$("#discountTabCon div").eq(0).addClass('cur').siblings().removeClass('cur');
	})

	// // 设置商品宽度
	// var size = parseInt($(".productList .item").width()*0.32);
	// $(".productList .saleOut").css({
	// 	'width': size,
	// 	'height':size,
	// 	'border-radius':size/2,
	// 	'line-height':size+'px',
	// 	'margin-top':-size/2+'px',
	// 	'margin-left':-size/2+'px'
	// })
	// var nWidth = ($("#MContainer").width()*0.95-5)/2-22;
	// var nHeight = 150*nWidth/127.5 + 'px';
	// $("#MContainer .productList .item").width(nWidth);
	// $("#MContainer .productList .item .imgBox").css({
	// 	'width':nWidth + 'px',
	// 	'height':nHeight,
	// 	'line-height':nHeight
	// })
	$(".productList .more").width( nWidth + 20).height( $(".productList .item").height() +25 );


	
	// 导航条菜单点击时加选中样式
	$("#discountTabCon>div").click(function(){
		$(this).addClass('cur').siblings().removeClass('cur');
		var targetElId = $(this).attr('data-href');
		var top;
		switch($(this).index())
			{
			case 0:
				top = discountCon1Top;
				break;
			case 1:
			  top = discountCon2Top;
			  break;
			case 2:
			  top = discountCon3Top;
			  break;
			case 3:
				top = discountCon4Top;
				break;
			case 4:
			    top = discountCon5Top;
				break;
			case 5:
				top = discountCon6Top;
				break;
			}

		if (hasOcc) {
			$(document).scrollTop( top - menuHeight );
		}else{
			$(document).scrollTop( top );
		}
		
	})

	$(window).scroll(function(){
	   menuFixed();
	}); 


})

function menuFixed(){

    // console.log('discountCon1Top='+discountCon1Top);
    // console.log('discountCon2Top='+discountCon2Top);
    // console.log('discountCon3Top='+discountCon3Top);
    // console.log('discountCon4Top='+discountCon4Top);
    // console.log('discountCon5Top='+discountCon5Top);
    // console.log('discountCon6Top='+discountCon6Top);

	var scrollTop=FSH.tools.getScrollTop();
	    
    if(scrollTop>=targetTop){
      fixedTop();
      hasOcc = true;
      // 计算浮动导航的位置
		$("#discountTab").css('margin-left', -$("#MContainer").width()/2 + 'px' );
    }else{
      $("#discountTab").removeClass('fixedTop').css('margin-left',0).find('.discountTab div').eq(0).addClass('cur').siblings().removeClass('cur');
      $("#discountWrap").css('padding-top',0);
      hasOcc = false;
    }

    // scrollTop -=menuHeight; 
    // console.log('scrollTop='+scrollTop);
    if (scrollTop +menuHeight>=targetTop && scrollTop+menuHeight < discountCon1Top) {
    	$("#discountTab .discountTab div").eq(0).addClass("cur").siblings("div").removeClass("cur");
    }else if(scrollTop+menuHeight>=discountCon1Top && scrollTop+menuHeight<discountCon2Top){
      $("#discountTab .discountTab div").eq(0).addClass("cur").siblings("div").removeClass("cur");
    }else if(scrollTop+menuHeight>=discountCon2Top && scrollTop+menuHeight<discountCon3Top){
     $("#discountTab .discountTab div").eq(1).addClass("cur").siblings("div").removeClass("cur");
    }else if(scrollTop+menuHeight>=discountCon3Top && scrollTop+menuHeight<discountCon4Top){
       $("#discountTab .discountTab div").eq(2).addClass("cur").siblings("div").removeClass("cur");
    }
    else if(scrollTop+menuHeight>=discountCon4Top && scrollTop+menuHeight<discountCon5Top){
       $("#discountTab .discountTab div").eq(3).addClass("cur").siblings("div").removeClass("cur");
    }else if (scrollTop+menuHeight>=discountCon5Top && scrollTop+menuHeight<discountCon6Top) {
      $("#discountTab .discountTab div").eq(4).addClass("cur").siblings("div").removeClass("cur");
    }else if ( scrollTop +menuHeight>=discountCon6Top ) {
    	$("#discountTab .discountTab div").eq(5).addClass("cur").siblings("div").removeClass("cur");
    }
}

window.onload = function(){
	targetTop=$("#discountWrap").offset().top;
	discountCon1Top=$("#discountCon1").offset().top,
    discountCon2Top=$("#discountCon2").offset().top;
    discountCon3Top=$("#discountCon3").offset().top;
    discountCon4Top=$("#discountCon4").offset().top;
    discountCon5Top=$("#discountCon5").offset().top;
    discountCon6Top=$("#discountCon6").offset().top;
    menuFixed();
    // console.log('discountCon1Top='+discountCon1Top);
    // console.log('discountCon2Top='+discountCon2Top);
    // console.log('discountCon3Top='+discountCon3Top);
    // console.log('discountCon4Top='+discountCon4Top);
    // console.log('discountCon5Top='+discountCon5Top);
    // console.log('discountCon6Top='+discountCon6Top);
}

function fixedTop(){
	
	$("#discountWrap").css('padding-top',menuHeight+'px');
	$('#discountTab').addClass('fixedTop');
	$("#discountTab .discountTab a").eq(0).addClass("cur").siblings("a.cur").removeClass("cur");
}

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
	//这里可以添加倒计时时间为0后需要执行的事件 
	window.location.reload();
} 
} 