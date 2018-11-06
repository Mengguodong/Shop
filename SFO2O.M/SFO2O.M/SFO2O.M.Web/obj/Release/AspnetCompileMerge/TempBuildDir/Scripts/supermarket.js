$(function () {
	var swipeCount = $("#mySwipe .swipe-wrap li").length;
	var indexStr = '';
	for (var i = 0; i < swipeCount ; i++ ) {
		indexStr = indexStr + "<li></li>";
	};
	if (swipeCount > 1) {
		$("#mySwipeLiItems ul").append(indexStr).find('li:first').addClass('on');
	}
	$("#mySwipe").removeClass('hide');
	if ( $(".focusBoxIn").length > 1 ) {
		TouchSlide({slideCell:"#mySwipe",effect:"leftLoop",autoPlay:true,interTime:5500})
	}
})
$(function(){
	var hotProItemWidth = ($('.MContainer').width()*0.975-parseInt($(".indexHotPro .item").eq(0).css('margin-right'))*2)*0.4 - parseInt($(".indexHotPro .item").eq(0).css('padding-left')) - parseInt( $(".indexHotPro .item").eq(0).css('padding-right') );

	$(".indexHotPro .item").css({
		width:hotProItemWidth // 一屏显示两个半产品，中间有两个外间距
	})
	$(".indexHotPro .imgBox").css({
		'width':hotProItemWidth,
		'height':hotProItemWidth,
		'overflow':'hidden'

	})

	$(".itemBox").each(function(i){
		$(this).css({
		width:($(".item").outerWidth())*$(this).parents(".indexHotPro").find(".item").length + ($(this).parents(".indexHotPro").find(".item").length-1)*parseInt( $(".item").css('margin-right') ) + Math.ceil(parseFloat(($(".indexHotPro").eq(0).css('padding-left'))))
	})
	})

	$(".lazyloadImg:not(.indexHotPro .lazyloadImg:gt(3))").lazyload({
	  placeholder: window.hostname+"/Content/images/blank.png",
		effect : "fadeIn",
		threshold :"200"
	})

	$(".lazyloadImg_rm").lazyload({
	    placeholder: window.hostname + "/Content/Images/default_brand.png",
		effect : "fadeIn",
		event : "sporty"  
	})
	for(var s=1;s<=$(".indexHotPro").length;s++){
		var t=$(".indexHotPro").eq(s-1).attr("id");
		eval("var myScroll"+s+"= new IScroll('#"+t+"', { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false })");
		eval("myScroll"+s+".on('scrollStart', function () {showImg(this.wrapper.id)})");
	}

})
$(window).on("load",function(){
	$(".lazyloadImg_rm").trigger("sporty");
})
function showImg(Id){
	//左右活动时加载图片
	var el=$("#"+Id);
	for(var i=0;i<el.find("img.lazyloadImg").length;i++){
		el.find(".lazyloadImg").eq(i).attr("src",el.find(".lazyloadImg").eq(i).attr("data-original"));
	}
	el.find("img.lazyloadImg").removeAttr("data-original").removeClass("lazyloadImg")
}

/* Moi Start */

var myScroll;
var sort = 1;
var level;
var cateloryId;
var page = 1;
var isLastPage = false;
var targetTop;
var menuHeight;
var hasOcc = false;
var cartHtml;
$(function(){

	menuHeight = $("#nav").height();

	level = $("#itemBox .itemBox>div").eq(0).attr('data-level');
	cateloryId = $("#itemBox .itemBox>div").eq(0).attr('data-id');


	navResize();
	
	$("#MContainer").on('resize',function(){
		navResize();
	})

	myScroll = new IScroll("#nav",{
	    eventPassthrough: true,
	    scrollX: true,
	    scrollY: false,
	    preventDefault: false
	})

	$("#itemBox").css('width',$("#itemBox .itemBox>div").outerWidth(true)*6+ 25)
	

	getData();

	FSH.scrollPage(getData);

	$("#itemBox .itemBox div").on('click', function () {
		var _this = $(this);
		$("#itemBox .itemBox div").removeClass('active');
		_this.addClass('active');
		$("#tags>div").hide();
		if ( $("#"+_this.attr('data-target')).length > 0 ) {
			$("#"+ _this.attr('data-target')).css('display','block');
			$("#productList").css('padding-top','0px');
		}else{
			$("#productList").css('padding-top','20px');
		}
		
		cateloryId = _this.attr('data-id');
		level = _this.attr('data-level');
		page = 1;
		isLastPage = false;
		ajaxFlag = true;
		$("#productList>div").eq(0).empty().height(1300);
		loadMoreEmpty();

		if($("#navContainer").hasClass("fixedTop")){
			getData("fixedNav");
		}else{
			getData();
		}
		
	})

	$(window).scroll(function(){
		targetTop = $("#placeholder").offset().top+8; // 把占位符放在需要浮动的元素前面。
		menuFixed();
	});

	$("#gotoSearch").click(function(e){
		$("#searchH").remove();
		$("body").prepend( '<input style="height: 0px;width: 0px;" type="search" id="searchH" name="">' )
		$("#searchH").focus();
		// e.stopPropagation();
		$(this).showSearch();
	})

	

})
//弹出选择sku的框
function AddToCartBySpu(spu,obj) {
	var _this = $(obj);
    FSH.Ajax({
        url: "/ShoppingCart/modifyproduct?productcode=" + spu + "&sku=",
        type: "get",
        dataType: 'json',
        jsonp: 'callback',
        jsonpCallback: "success_jsonpCallback",
        success: function (data) {
            if (data.Type == 1) {
                var t=new SelectSKU({
                    json:data.Data,
                    type:1,
                	addCartSuccess:function(cartnum){
                		FSH.productListAddToCartSucFun(cartnum,_this);
                	}});
        
            }
            else {
                FSH.commonDialog(1, [data.Content]);
            }
        }
    });
}

function fixedTop(){
	
	$('#navContainer').addClass('fixedTop');
	$("#navContent").css('margin-top',menuHeight+16);
	if ( $("#cartBox").length < 1 ) {
		$("#navContainer").prepend('<div class="cartBox" id="cartBox"></div>');
		$("#cartBox").append( $("#headerCart") );
	}
	$("#nav").css('width','86%');
	myScroll.refresh();
	
}

function menuFixed(){

	var scrollTop=FSH.tools.getScrollTop();
	    
    if(scrollTop>=targetTop){
      fixedTop();
      hasOcc = true;
      // 计算浮动导航的位置
		$("#navContainer").css('margin-left', -$("#MContainer").width()/2 + 'px' );
    }else{
      $("#navContainer").removeClass('fixedTop').css('margin-left',0);
      $("#navContent").css('margin-top',0);
      $("header").append( $("#headerCart") );
      $("#cartBox").remove();
      $("#nav").css('width','auto');
      hasOcc = false;
      myScroll.refresh();
    }

    
}

function navResize(){
	if ( $("#itemBox span").outerWidth(true)*6 + 25 < 640 && $("#MContainer").width() == 640 ) {
		$("#itemBox .itemBox div").css('width','98px');
	}
}

function loadMoreEmpty(){
	$("#loadMore").text('');
}

function getData(callbackFun){
	if(ajaxFlag && !isLastPage){
		isLoading = false;
		FSH.Ajax({
		    url: "/SuperMarket/MarketProductList",
	        type:"post",
	        dataType: 'json',
	        data: {page:page,
					sort:1, // sort:1上新，2升序，3降序
					level:level, //level：0一级分类，1二级分类，2三级分类,level=2时，c的值就是三级分类的id,你可以直接用
					c:cateloryId,
					attrData:'',
				},
	        jsonp: 'callback',
	        jsonpCallback: "success_jsonpCallback",
	        success: function (data) {
	        	if (data.Type == 1) {

	        		if (data.Data.TotalRecord == 0) {
	        			// 无数据
	        			if ( $(".noData").length < 1 ) {
		        			$("#productList>div").eq(0).empty().append('<div class="noData">\
															<img src="../../Content/Images/noData.png?v=20151224" alt="无数据">\
															<p class="mainTips">没有找到您要找的商品</p>\
															<p class="secTips">换换其他类目吧</p>\
														</div>');
		        			
	        			};
	        		}else{
	        			$("#sortType").removeClass('hide');
	        			if (data.Data.PageIndex == data.Data.PageCount) {
		        			isLastPage = true;
		        			$("#loadMore").text('全部加载完');
		        			ajaxFlag=false;
		        		}else{
		        			$("#loadMore").text('上滑加载更多');
		        			page +=1;
		        		}

	        			var html = '';
	        			for(var i = 0; i < data.Data.Products.length; i++){
	        				html = html + FSH.renderList(data.Data.Products[i]);
	        			};

	        			$("#productList>div").eq(0).height('auto').append(html);
	        			
	      	        	$("#productList .lazyloadImg").lazyload({
	      	        	    placeholder: window.hostname + "/Content/Images/default_product.png",
											effect : "fadeIn"
										})

	        		}
	        	}else{
	        		FSH.commonDialog(1,[data.Content])
	        	}

	    			FSH.fixedFooter();
		        isLoading = true;
		        if(callbackFun=="fixedNav"){
		        	$(window).scrollTop(targetTop+8+8);
		        	$("#navContent").css('margin-top',0);
		        }
	      
	        }
	    })
	}
}

