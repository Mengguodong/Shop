var page = 1;
var brandId = $("#brandId").val();
var sort = 1;
var isLastPage = false;
var isLoading = true;
$(function(){
	// banner图给一个默认高度
	$("#banner").css('height',303*parseFloat($("#MContainer").width())/640);

	getData();
	FSH.addGoTop();
	


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

	FSH.scrollPage(getData);

	// 点击收藏
	$(".collect").click(function(){
		var _this = $(this);
		var productCode=_this.parents('.item').attr('data-procode'),
		    collectionStatus=_this.hasClass("active");
		    ajaxFlag = true;
		FSH.collectionFun(productCode,collectionStatus,fun,_this);
	})

	
	
})

function fun(collectionStatus,el){
	if(collectionStatus){
		el.removeClass("active").html("<i></i>收&nbsp;&nbsp;藏");

	}else{
		el.addClass("active").html("<i></i>已收藏");
	}
}

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
                var t = new SelectSKU({
                    json: data.Data,
                    type: 1,
                    addCartSuccess:function(cartnum){
                		FSH.productListAddToCartSucFun(cartnum,_this);
                	}
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
			url: "/brand/productlist",
	        type:"post",
	        dataType: 'json',
	        loadingType: 2,
	        data: {page:page,
					sort:sort, // sort:1上新，2升序，3降序
					id:brandId, //level：0一级分类，1二级分类，2三级分类,level=2时，c的值就是三级分类的id,你可以直接用
				},
	        jsonp: 'callback',
	        jsonpCallback: "success_jsonpCallback",
	        success: function (data) {
	        	if (data.Type == 1) {
	        		if (data.Data.TotalRecord == 0) {
	        			// 无数据
	        			if ( $(".noData").length < 1 ) {
		        			$("#loadMore").before('<div class="noData">\
															<img src="../../Content/Images/noData.png?v=20151224" alt="无数据">\
															<p class="mainTips">该品牌下暂无商品</p>\
														</div>');
		        			
	        			};
	        		}else{
        				$(".products").removeClass('hide');
	        			$("#screen").removeClass('hide');
	        			$("#totalCount").text('（'+ data.Data.TotalRecord+'）' );
	        			

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
	        			$("#productList>div").eq(0).append(html);
	        			$(".lazyloadImg").lazyload({
	        			    placeholder: window.hostname + "/Content/images/blank.png",
							effect : "fadeIn"
						})
	        			
	        			
	        		}
	        		FSH.fixedFooter();
	        	}else{
	        		FSH.commonDialog(1,[data.Content])
	        	}
	        	isLoading = true;
	        }
	    })
	}
}