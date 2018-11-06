var page = 1;
var storeId = FSH.tools.request('id');
var sort = 1;
var isLastPage = false;
var isLoading = true;
$(function(){
	// banner图给一个默认高度
	$("#banner").css('height',303*parseFloat($("#MContainer").width())/640);

	getData();
	FSH.addGoTop();
	// 头部隐藏菜单交互 未完待续，当点击其它区域时隐藏
	$("#moreMenu").on('click',function(e){
		e.stopPropagation();
		if($(this).find('ul').is(":hidden")){
			$(this).find('ul').show();
		}else{
			$(this).find('ul').hide();
		}
		
	});

	$(document).on('click',function(e){
		if ( e.target != $("#moreMenu ul").get(0)) {
			$("#moreMenu ul").hide();
		};
	})

	$("#shopName").css({
		'padding-left':$("#logo").outerWidth(true)
	})

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

	})

	// 滚动加载
	$(document).on('scroll','',function(){
		if (FSH.tools.getScrollTop() + FSH.tools.getClientHeight() + 20 >= FSH.tools.getScrollHeight()) {
			if (isLoading) {
				if (isLastPage) {
					$("#loadMore").text('全部加载完');
					return;
				}else{
					$("#loadMore").text('滑动加载更多');
					page +=1;
					getData();
				}
			}
		}
	})
	// FSH.scrollPage(getData);
})

function renderList(data){
	var s = '<div class="item pr">\
				<a href="'+ window.hostname +'/item.html?productCode='+ data.SPU +'">\
					<span class="freeSign"></span>\
					<div class="imgBox"><img src="'+ data.ImagePath +'"></div>\
					<div class="title">'+ data.Name +'</div>\
					<div class="price">￥'+ FSH.tools.showPrice(data.MinPrice) +'</div>\
				</a>'
			var n = '';
			if (data.Qty>0) {
				
			    if (data.SkuCount > 1) { // 跳转到详情页
			        n = '<a class="addCart" onclick="AddToCartBySpu(\'' + data.SPU + '\')"></a>';
					//n = '<a class="addCart" href="'+ window.hostname +'/item.html?productCode='+ data.SPU +'"></a>';
				}else{
					n = '<span class="addCart" onclick="addToCart(\''+ data.SkuList[0] +'\')"></span>'; 
					//调一下加入购物车的异步请求
				}
			}else{
				n = '<div class="saleOut">已售罄</div>';
			}
					
	var x ='</div>';
	return s+n+x;
}

// 加入购物车
function addToCart(sku ){
	FSH.Ajax({
		url:window.hostname + '/ShoppingCart/AddItem',
		type:'post',
		data:{ sku:sku,qty:1},
		success:function( msg ){
			if (msg.Type == 1) {
				FSH.smallPrompt('添加购物车成功');
				$(".headerCart").find('span').text( parseInt( $(".headerCart").find('span').text() ) + 1 );
			}else{
				FSH.commonDialog(1,[msg.Content]);
			}
		},
		error:function(){
			FSH.commonDialog(1,[msg.Content]);
		}
	})
}
//弹出选择sku的框
function AddToCartBySpu(spu) {
    FSH.Ajax({
        url: "/ShoppingCart/modifyproduct?productcode=" + spu + "&sku=",
        type: "get",
        dataType: 'json',
        jsonp: 'callback',
        jsonpCallback: "success_jsonpCallback",
        success: function (data) {
            if (data.Type == 1) {
                var t = new SelectSKU({
                    json: data.Data,
                    type: 1
                });
            }
            else {
                FSH.commonDialog(1, [data.Content]);
            }
        }
    });

}

function getData( ){
	if(ajaxFlag && !isLastPage ){
		isLoading = false;
		FSH.Ajax({
			url: "/Store/ProductList",
	        type:"post",
	        dataType: 'json',
	        data: {page:page,
					sort:sort, // sort:1上新，2升序，3降序
					sId:storeId, //level：0一级分类，1二级分类，2三级分类,level=2时，c的值就是三级分类的id,你可以直接用
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
		        			$("#loadMore").text('');
	        			};
	        		}else{
	        			$("#totalCount").text('（'+ data.Data.TotalRecord+'）' );
	        			if (data.Data.PageIndex == data.Data.PageCount) {
		        			isLastPage = true;
		        		}

	        			var html = '';
	        			for(var i = 0; i < data.Data.Products.length; i++){
	        				html = html + renderList(data.Data.Products[i]);
	        			};

	        			$("#productList>div").eq(0).append(html);

	        			

	        			var nWidth = ($("#MContainer").width()*0.95-5)/2-22;
	        			var nHeight = 150*nWidth/127.5 + 'px';
	        			$("#MContainer #productList .item").width(nWidth);
	        			$("#MContainer #productList .item .imgBox").css({
	        				'width':nWidth + 'px',
	        				'height':nHeight,
	        				'line-height':nHeight
	        			})
	        		    // $("#MContainer #productList .item").width(($("#MContainer").width()*0.95-5)/2-22);
	        		    // 设置“已售罄”字样的位置
	        			var size = parseInt($("#productList .item").width() * 0.32);
	        			$("#productList .saleOut").css({
	        			    'width': size,
	        			    'height': size,
	        			    'border-radius': size / 2,
	        			    'line-height': size + 'px',
	        			    'top': (150 * nWidth / 127.5 - size) / 2 + 'px',
	        			    'margin-top': '0px',
	        			    'margin-left': -size / 2 + 'px'
	        			})
	        			$("#loadMore").text('');
	        			FSH.fixedFooter();
	        		}
	        	}else{
	        		FSH.commonDialog(1,[data.Content])
	        	}
	        	isLoading = true;
	        }
	    })
	}
}