//母亲节活动
$().ready(function(){
	$("#returnBtn").off("click").on("click",function(){
		window.location=window.hostname;
	})
	//微信分享
	FSH.share.getWXConfig("childrenDayShared");
	$(".itemImg").lazyload({
					effect : "fadeIn",
					threshold :"200"
	});
})

var initHeight, //记录导航条的初始位置
	targetTop,
	discountCon1Top,
	discountCon2Top,
	discountCon3Top;
var menuHeight;
var hasOcc = false;

$(function(){

	menuHeight = $("#discountTab").height();

	// 导航条菜单点击时加选中样式
	$("#discountTabCon>a").click(function(){
		$(this).addClass('cur').siblings().removeClass('cur');	
		
	})

	$(window).scroll(function(){
		menuFixed();
	    autoSelect();
	});
})

function menuFixed(){

	var scrollTop=FSH.tools.getScrollTop();
	    
    if(scrollTop>=targetTop){
      fixedTop();
      hasOcc = true;
      // 计算浮动导航的位置
		$("#discountTab").css('margin-left', -$("#MContainer").width()/2 + 'px' );
    }else{
      $("#discountTab").removeClass('fixedTop').css('margin-left',0).find('a').eq(0).addClass('cur').siblings().removeClass('cur');
      $("#discountWrap").css('padding-top',0);
      hasOcc = false;
    }

    
}

function autoSelect(sendTop){
	var scrollTop;
	if (sendTop) {
		scrollTop = sendTop;
	}else{
		scrollTop=FSH.tools.getScrollTop();
	}
	if( scrollTop+menuHeight<discountCon2Top){
      $("#discountTabCon a").eq(0).addClass("cur").siblings("a").removeClass("cur");
    }else if(scrollTop+menuHeight>=discountCon2Top && scrollTop+menuHeight<discountCon3Top){
     $("#discountTabCon a").eq(1).addClass("cur").siblings("a").removeClass("cur");
    }else if(scrollTop+menuHeight>=discountCon3Top && scrollTop+menuHeight< discountCon3Top + $("#discountCon3").outerHeight(true)){
       $("#discountTabCon a").eq(2).addClass("cur").siblings("a").removeClass("cur");
    }
}

window.onload = function(){
	targetTop=$("#discountWrap").offset().top;
	discountCon1Top=$("#discountCon1").offset().top,
    discountCon2Top=$("#discountCon2").offset().top;
    discountCon3Top=$("#discountCon3").offset().top;
    menuFixed();
}

function fixedTop(){
	
	$("#discountWrap").css('padding-top',menuHeight+'px');
	$('#discountTab').addClass('fixedTop');
	$("#discountTabCon a").eq(0).addClass("cur").siblings("a").removeClass("cur");
}