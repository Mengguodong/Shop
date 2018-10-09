// 搜索结果页

var sort = 1;

var keyword = decodeURIComponent(FSH.tools.request('keyword'));

var postData = [];

var page = 1;

var isLastPage = false;

var brands = [];

$(function(){

	$("#showSearch").val( keyword );

	$("#showSearch").on('focus',function(){
		$(this).showSearch({
			value:$(this).val()
		});
	})

	var nWidth = parseFloat($(".returnBtn").offset().left) + parseFloat( $(".returnBtn").outerWidth(true) ) + parseFloat($(".headerCart").css('right')) + parseFloat( $(".headerCart").outerWidth(true) );

	if ( (parseFloat( $("#MContainer").width() ) - parseFloat( $("header").eq(0).find('b').width() ))/2 < parseFloat($(".headerCart").width()) + parseFloat($(".headerCart").css('right')) ) {
	
		$("header").eq(0).find('b').css({
			'width': parseFloat($("#MContainer").width())-nWidth - 50,
			'overflow':'hidden',
			'text-overflow':'ellipsis',
			'white-space':'nowrap',
			'display':'inline-block'
		})
	}
	
	getData();
	
	
	$("#sortType>div").eq(0).addClass('active');
	
	// 点击上新排序
	$("#new").on('click',function(){
		sort = 1;
		page = 1;
		$(this).addClass('active').siblings().removeClass('active');
		$("#price").attr('class','price');
		$("#productList>div").eq(0).empty();
		isLastPage = false;
		ajaxFlag = true;
		getData();
		loadMoreEmpty();
	})

	// 点击按价格排序
	$("#price").on('click',function(){
		page = 1;
		$(this).siblings().removeClass('active');
		if ( $(this).hasClass('asc') ) {
			sort = 3;
			$(this).removeClass('asc').addClass('desc');
		}else if ( $(this).hasClass('desc') ) {
			sort = 2;
			$(this).removeClass('desc').addClass('asc');
		}else{
			$(this).addClass('active').addClass('asc');
			sort = 2;
		}
		$("#productList>div").eq(0).empty();
		isLastPage = false;
		ajaxFlag = true;
		getData();
		loadMoreEmpty();

	})

	// 点击折扣
	$("#discount").on('click',function(){
		page = 1;
		$("#sortType>div").removeClass('active');
		$("#price").attr('class','price');
		sort=4;
		$(this).addClass('active');
		$("#productList>div").eq(0).empty();
		isLastPage = false;
		ajaxFlag = true;
		getData();
		loadMoreEmpty();

	})


	// 点击筛选
	$("#select").on('click',function(){

		if ( $("#brands").length > 0 ) {
			$("#brands").show();
		}else{
			getAllBrands();
		}

		$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").hide();
		$("#MContainer").find("header").eq(0).hide();
		loadMoreEmpty();
	})

	// 一层分类的返回按钮
	$("#MContainer").on('click','#select1 .returnBtn',function(){
		$("#select1").remove();
		$(".select2Box").remove();
		countNoBottomHeight();
		if ( FSH.tools.request('level') == 2 ) {
			$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").show();
			$("#MContainer").find("header").eq(0).show();
		}
	})

	// $("#MContainer #productList .item").width(($("#MContainer").width()*0.95-5)/2-22);

	FSH.scrollPage(getData);

})

//弹出选择sku的框
function AddToCartBySpu(spu,obj) {
	var _this = $(obj);
    FSH.Ajax({
        url: "/ShoppingCart/modifyproduct?productcode=" + spu,
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

function soAddToCart(spu,obj){ 
	FSH.Ajax({
		url:'/product/GetStockInfoBySpu?spu='+spu,
		type: 'get',
		dataType:'json',
		jsonp:'callback',
		jsonpCallback:'success_jsonpCallback',
		success:function(data){
			if ( data.Type == 1 ) {
				if (data.Data.IsHaveStock == 0) {
					FSH.smallPrompt(data.Content);
				}else{
					if (data.Data.SkuCount > 1) {
						AddToCartBySpu(spu,obj);
					}else{
						FSH.addToCart(data.Data.Skus[0] ,obj);
					}
				}
			}else{
				FSH.commonDialog(1,[data.Content]);
			}
		}
	})
}

function getData( ){
	if(ajaxFlag && !isLastPage){
		isLoading = false;
		FSH.Ajax({
			url: "/Search/GetProductListByOptionsAndPageIndex",
	        type:"post",
	        dataType: 'json',
	        loadingType: 2,
	        data: {pageIndex:page,
					sort:sort, // sort:1上新，2升序，3降序,4折扣升序
					keyword:encodeURIComponent(keyword),
					brands:postData
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
															<p class="mainTips">抱歉，没有找到符合条件的商品</p>\
															<p class="secTips">换换其他的看看吧</p>\
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

		        		brands = data.Data.Brands;

	        			var html = '';
	        			for(var i = 0; i < data.Data.Products.length; i++){
	        				html = html + FSH.renderList(data.Data.Products[i],1);
	        			};

	        			$("#productList>div").eq(0).append(html);
	        			$(".lazyloadImg").lazyload({
	        			    placeholder: window.hostname + "/Content/images/blank.png",
							effect : "fadeIn"
						})
	        			// var nWidth = ($("#MContainer").width()*0.95-5)/2-22;
	        			// var nHeight = 150*nWidth/127.5 + 'px';
	        			// $("#MContainer #productList .item").width(nWidth);

	        		}
	        	}else{
	        		FSH.commonDialog(1,[data.Content])
	        	}

    			FSH.fixedFooter();
	        	isLoading = true;
	        }
	    })
	}
}

function getWindowHeight(){
	return $(window).height() - $("header").outerHeight();
}

function countContentHeight(){
	var boxHeight = getWindowHeight() - 45;
	$(".selectBox .items").css('height', boxHeight );
}
function countNoBottomHeight(){
	var boxHeight = getWindowHeight();
	$(".selectBox .items").css('height', boxHeight );
}

window.onresize = function(){
	countContentHeight();
}

function getAllBrands(){
	var html = renderBrands(brands);
	$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").hide();
	$(html).appendTo("#MContainer").show();
	countContentHeight();
}

function renderBrands(data){
	var html = '<div class="MContainer selectBox showBrands" id="brands">'				
					+'<header class="pageHeader w100 pr tc overflowH">'
						+'<a class="returnBtn"></a>'
						+'<b class="f36 FontColor1">筛选</b>'
					+'</header>'
					+'<div class="selectContent">'
						+'<div class="items">'
							+'<div class="w95p boxShadow bgColor3">'
								+'<p class="f28 FontColor3 pt10 mb10">品牌</p>'
								+'<ul class="brands clearfix">';
	var lists = '';
	for( var i = 0; i < data.length; i++ ){
		lists = lists+'<li>'+ data[i] +'</li>';
	}
	var htmlend = 				'</ul>'
							+'</div>'
						+'</div>'
						+'<div class="btn-box">'
							+'<a class="reset">重置</a>'
							+'<a class="sure">确定</a>'
						+'</div>'
					+'</div>'
				+'</div>';
	return html + lists + htmlend;
}

$(function(){

	// 点品牌'重置'
	$("#MContainer").on('click','#brands .reset',function(){
		$("#brands").find('.brands').find('li').removeClass('selected');
		$("#MContainer").find("#brands .sure").click();
	})

	// 点品牌'确定'
	$("#MContainer").on('click','#brands .sure',function(){
		postData = [];
		var getTargetEl = $("#brands .brands li.selected");
		for( var i = 0; i < getTargetEl.length; i++ ){
			postData.push( $.trim( getTargetEl.eq(i).text() ) );
		}
		postData = JSON.stringify(postData);
		page = 1;
		$("#productList>div").eq(0).empty();
		isLastPage = false;
		ajaxFlag = true;
		getData( );
		$("#brands").hide();
		mainPartShow();
	})

	// 品牌选择
	$("#MContainer").on('click','.brands li',function(){
		if ($(this).hasClass('selected')) {
			$(this).removeClass('selected');
		}else{
			$(this).addClass('selected');
		}
	})

	// 品牌筛选浮层返回按钮
	$("#MContainer").on('click','#brands .returnBtn',function(){
		$("#brands").hide();
		mainPartShow();
	})

})

function mainPartShow(){
	$("#MContainer header").eq(0).show();
	$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").show();
}

function loadMoreEmpty(){
	$("#loadMore").text('');
}