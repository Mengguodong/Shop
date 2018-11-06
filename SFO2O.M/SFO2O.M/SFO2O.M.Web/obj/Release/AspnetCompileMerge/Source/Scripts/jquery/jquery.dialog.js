(function ($){
	/**
	* 弹出层 version:20150623
	@targetEl           必填 弹窗对应id或class
	@closeBtnName       选填 关闭按钮的类名或id名 默认是.closeBtn 如果有多个按钮触发关闭事件，以逗号隔开
	@isfixed            选填 弹窗是否跟随滚动条滚动 默认是true
	@background         选填 遮罩层背景色
	@opacity            选填 遮罩层透明度
	@openAnimationName  选填 显示弹窗时动态效果的类名
	@closeAnimationName 选填 关闭弹窗时动态效果的类名
	@positionY          选填 弹窗的位置 默认为垂直居中显示 可选参数有 top 顶头显示 bottom 底部显示
	@openFun            选填 开始时回调函数
	@closeFun           选填 关闭时回调函数
	@content            选填  弹窗的html代码 如需动态加载时使用，如果页面已有对应的弹窗代码，此参数可以不传
	**/
	$.fn.popShow = function (options){
		var defaults = {    
			targetEl:'',
			closeBtnName:'.closeBtn' ,
			isfixed: true,    
			background: '#000',
			opacity:'30',
			openAnimationName:'openAnimation',
			closeAnimationName:'closeAnimation',
			positionY:'center',
			openFun:function(){},
			closeFun:function(){},
			content:''
		  };     
  		var opts = $.extend(defaults, options); 
		if(!$(opts.targetEl).length>0){
			$("body").append(opts.content)	
		}
		var box = $(opts.targetEl);
		var targetId=opts.targetEl.substring(1);
		if (! box[0]){
			return this;
		}
		var pageSizeArr = [
			document.documentElement.scrollWidth,
			document.documentElement.scrollHeight,
			document.documentElement.clientWidth,
			document.documentElement.clientHeight
		];
		var isIE= /msie/.test(navigator.userAgent.toLowerCase());
		var isIE6=false;
		if ('undefined' == typeof(document.body.style.maxHeight)) {
			isIE6=true;
		}
		var setBox=function(){
			if (isIE6 || ! opts.isfixed){
				box.css({
					position: 'absolute',
					zIndex: 9999,
					top: document.documentElement.scrollTop + (document.documentElement.clientHeight - box.innerHeight()) / 2,
					left: document.documentElement.scrollLeft + (document.documentElement.clientWidth - box.innerWidth()) / 2,
					marginTop: 0,
					marginLeft: 0
				});
				switch(opts.positionY){
					case "top":box.css(top,document.documentElement.scrollTop);break;
					case "bottom":box.css(top,document.documentElement.scrollTop+$(window).height()-box.innerHeight());break;	
				}
			} else {
				box.css({
					position: (opts.isfixed ? 'fixed' : 'absolute'),
					zIndex: 9999,
					left: '50%',
					marginLeft: 0 - box.innerWidth()/ 2,
					top: '50%',
					marginTop: 0 - box.innerHeight() / 2
				});
				switch(opts.positionY){
					case "top":box.css({top:0,marginTop:0});break;
					case "bottom":box.css({bottom:0,marginTop:0,top:"auto"});break;	
				}
			}	
		}
		var closeDialog=function(){
			if (isIE){
				$("body").find("#"+targetId+"dialogBackground").remove();
				$("body").find("#"+targetId+"dialogBackgroundiframe").remove();
				box.hide();
			} else {
				$("body").find("#"+targetId+"dialogBackground").remove();
				$("body").find("#"+targetId+"dialogBackgroundiframe").remove();
				box.removeClass(opts.openAnimationName).addClass(opts.closeAnimationName).fadeOut();
				//setTimeout(function(){popFlag=true;box.hide();},opts.timer)
				
			}
			box.off('click',opts.closeBtnName);
			opts.closeFun();
			$(window).unbind('scroll.dialogScroll');	
		}
		var createDialog = function (){
			//如果是IE6 则创建iframe
			if (isIE6){
				var backgroundiframe = $('<iframe>',{
						id:targetId+'dialogBackgroundiframe',
						frameborder: '0'
					}).css({
					position: 'absolute',
					background: opts.background,
					zIndex: 9991,
					top: 0,
					left: 0,
					width: pageSizeArr[2] > pageSizeArr[0] ? pageSizeArr[2] : pageSizeArr[0],
					height: pageSizeArr[3] > pageSizeArr[1] ? pageSizeArr[3] : pageSizeArr[1],
					zoom:1,
					display: 'none'
				}).on("click",function(){
					closeDialog();
				});
				
				if (isIE){
					backgroundiframe.css({'filter':'Alpha(Opacity='+opts.opacity+')','opacity':opts.opacity/100});
				} else {
					backgroundiframe.css('opacity', ''+opts.opacity/10+'');
				}
				backgroundiframe.appendTo('body');
				backgroundiframe.show();
				
			}
				
			var backgroundDiv = $('<div>',{
					id:targetId+'dialogBackground'
				}).css({
				position: 'fixed',
				background: opts.background,
				zIndex: 9992,
				top: 0,
				left: 0,
				// width: pageSizeArr[2] > pageSizeArr[0] ? pageSizeArr[2] : pageSizeArr[0],
				// height: pageSizeArr[3] > pageSizeArr[1] ? pageSizeArr[3] : pageSizeArr[1],
				right:0,
				bottom:0,
				zoom:1,
				display: 'none'
			}).on("click",function(){
				closeDialog();
				});
			if (isIE){
				backgroundDiv.css({'filter':'Alpha(Opacity='+opts.opacity+')','opacity':opts.opacity/100});
			} else {
				backgroundDiv.css('opacity', opts.opacity/100);
			}
			backgroundDiv.appendTo('body');
			backgroundDiv.show();
		};
		
		var dialogshow = function (){
			if ($('#'+targetId+'dialogBackground')[0]) {
				$('#'+targetId+'dialogBackground').show();
				$('#'+targetId+'dialogBackgroundiframe').show();
			} else {
				createDialog();
			}
			setBox();
			box.removeClass(opts.closeAnimationName).addClass(opts.openAnimationName).fadeIn();
			opts.openFun();
		};
		
		$(window).resize(function () {  
			pageSizeArr = [
			document.documentElement.scrollWidth,
			document.documentElement.scrollHeight,
			document.documentElement.clientWidth,
			document.documentElement.clientHeight
		];
			setBox();
			if($("#"+targetId+"dialogBackgroundiframe").length>0){
				
				$("#"+targetId+"dialogBackgroundiframe").css({width: pageSizeArr[2] > pageSizeArr[0] ? pageSizeArr[2] : pageSizeArr[0],
				height: pageSizeArr[3] > pageSizeArr[1] ? pageSizeArr[3] : pageSizeArr[1]})
			}
			if($("#"+targetId+"dialogBackground").length>0){
				$("#"+targetId+"dialogBackground").css({width: pageSizeArr[2] > pageSizeArr[0] ? pageSizeArr[2] : pageSizeArr[0],
				height: pageSizeArr[3] > pageSizeArr[1] ? pageSizeArr[3] : pageSizeArr[1]})	
			}
		})
		box.on('click',opts.closeBtnName, function (event){
			closeDialog();
			
		});
		if (isIE6 && opts.isfixed){
			$(window).bind('scroll.dialogScroll', function (){
				box.css({
					top: document.documentElement.scrollTop + (document.documentElement.clientHeight - box.innerHeight()) / 2,
					left: document.documentElement.scrollLeft + (document.documentElement.clientWidth - box.innerWidth()) / 2
				});
			});
		}
		dialogshow();
		return this;
	};
})(jQuery);