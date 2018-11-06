/*
 * 用于感应式的标签
 * 功能包含：滚动条滚动到导航条所在的位置时，导航条固定在顶部
 * 滚动到某部分内容时，其对应的导航是选中状态
*/

(function($){

	$.fn.tabView = function(options){
		var defaults = {    
			
		  };     
  		var opts = $.extend(defaults, options); 

  		var targetTop;
		var menuHeight;
		var scrollTop;
		var myScroll;
		var elementsTopArray= [];
		var elementsTopOriginal = [];
		var length = $("#doArea>div").length;
		var windowHeight = $(window).height();
		var hasNoClick = true;
		
			
		menuHeight = $("#navContainer").outerHeight(true);

		fixed();


		$(window).scroll(function(){
			fixed();

			elementsTopArray = [];
			elementsTopOriginal = [];
			getElementTop();
			autoSelect( length );
		})

		// 点击导航条
		$("#doArea a").on('click',function(e){
			e.preventDefault();
		})
		$("#doArea>div").on('click',function(){
			elementsTopOriginal = [];
			getElementTop();
			$(this).addClass('active').siblings().removeClass('active');

			var _index = $(this).index();

			if (  elementsTopOriginal[_index] > windowHeight ) {
				$("#navContainer").addClass('fixedTop');
			}
			
			if (hasNoClick) {
				$(window).scrollTop( elementsTopOriginal[_index] - 2*menuHeight )
			}else{
				$(window).scrollTop(elementsTopOriginal[_index] );
			}

			hasNoClick = false;
			

		})


		if ( (parseInt($("#doArea>div").css("max-width"))+parseInt($("#doArea>div").css('margin-right')))*length <= $(window).width() && $(window).width() <= 640 ) { // 如果导航条未占满一行,让单个导航的宽度平分总宽度
			$("#nav").css('width','100%');
			$("#doArea").addClass('average');
		}else{
			$("#doArea").removeClass('average');
			$("#nav").css('width',$("#doArea>div").outerWidth(true)*length);
		}

		myScroll = new IScroll("#navBox",{
		    eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false
		})


		function fixed(){
			targetTop = getTargetTop();
			scrollTop = FSH.tools.getScrollTop();
			if ( scrollTop >= targetTop ) {
				$('#navContainer').addClass('fixedTop');
				$("#navContainer").css('margin-left', -$("#MContainer").width()/2 + 'px' );
				hasNoClick = false;
			}
			else{
				$("#navContainer").removeClass('fixedTop').css('margin-left',0);
				hasNoClick = true;
			}
			
		}


		function getTargetTop(){
			return $("#placeholder").offset().top;
		}

		function getElementTop(){
			if ( $("#navContainer").hasClass('fixedTop') ) {
				$("#navContent>div").each(function(i){
					elementsTopOriginal.push( $(this).find('.placeholder').offset().top - menuHeight );
					elementsTopArray.push( $(this).find('.placeholder').offset().top + $(this).outerHeight(true) - menuHeight );
				})
			}else{
				$("#navContent>div").each(function(i){
					elementsTopOriginal.push( $(this).find('.placeholder').offset().top );
					elementsTopArray.push( $(this).find('.placeholder').offset().top + $(this).outerHeight(true) );
				})
			}
			
			
		}

		function autoSelect(n){ // n为导航的个数
			$("#doArea>div").removeClass('active');


			var jscode="if ( scrollTop >= elementsTopArray[0] && scrollTop < elementsTopArray[1]) {\
				$('#doArea>div').eq(1).addClass('active');\
				myScroll.scrollToElement($('#doArea>div').eq(1).get(0),null,true,true)\
			}";
			for( var i = 2; i <= n-1; i++ ){ // i 为 index
				jscode +="else if( scrollTop >= elementsTopArray["+(i-1)+"] && scrollTop < elementsTopArray["+i+"] ){\
					$('#doArea>div').eq("+i+").addClass('active');\
					myScroll.scrollToElement( $('#doArea>div').eq("+i+").get(0),null,true,true );\
				}";
			}
			var jscodeend = "else{\
				$('#doArea>div').eq(0).addClass('active');\
				myScroll.scrollToElement( $('#doArea>div').eq(0).get(0),null,true,true )\
			}"

			eval( jscode + jscodeend );

			
		}

	}

}(jQuery));