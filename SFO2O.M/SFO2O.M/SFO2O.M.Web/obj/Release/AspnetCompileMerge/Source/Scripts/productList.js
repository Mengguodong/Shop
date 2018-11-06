
var cateloryId = FSH.tools.request('c');
var level = FSH.tools.request('level');
var sort = 1;
	

// var screenData = {};
var screenData = decodeURI(FSH.tools.request('attrData'));
var page = 1;

var isLoading = true;
var isLastPage = false;


$(function(){

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


	// 点击筛选
	$("#select").on('click',function(){
		level = FSH.tools.request('level');
		if (level != 2) {
			var cid = FSH.tools.request('c');
			var data = { level:level,categoryid:cid};
			getSubCategory(data);
		}else{
			if ( $("#select1").length > 0  ) {
				$("#select1").show();
			}else{
				getCategoryAttributes(cateloryId);
			}
		}
		$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").hide()
		$("#MContainer").find("header").eq(0).hide()
		//$("body").css('overflow','hidden');
		//$("html").css('overflow','hidden');
	})

	

	// 点击一级分类
	$("#MContainer").on('click','#select1 .item',function(){
		var targetId = $(this).attr('data-target');
		$("#select1").hide();
		var dataSaveId = $(this).find('.selected').attr('data-saveid');

		if ( dataSaveId ) {
			$("#"+targetId).find('.item').removeClass('selected');
			var idArray = dataSaveId.split(',');
			for(var i = 0; i < idArray.length; i++){
				$("#"+targetId).find('.item').filter(function(){
					// console.log($.trim($(this).find('.sortItem').text()) == $.trim(dataSaveId[i]));
					return $.trim($(this).find('.sortItem').text()) == $.trim(idArray[i]);
				}).addClass('selected');
			}
			
		}

		$("#"+targetId).show();
		$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").hide()
		$("#MContainer").find("header").eq(0).hide()
		//$("body").css('overflow','hidden');
		//$("html").css('overflow','hidden');
	})

	//点击二级分类
	$("#MContainer").on('click','.select2Box .item',function(){
		if ($(this).hasClass('selected')) {
			$(this).removeClass('selected');
		}else{
			$(this).addClass('selected');
		}
	})

	$("#MContainer").on('click','.select2Box .returnBtn',function(){
		$(".select2Box").hide();
		$("#select1").show();
		$(".select2Box").find('.item').removeClass('selected');
	})
	
	// 点击二级分类确定
	$("#MContainer").on('click','.select2Box .sure',function(e){
		FSH.EventUtil.stopPropagation(e);
		var selectedArray = [];
		var selectedIdArray = [];
		var targetId = $(this).parents('.select2Box').attr('id');
		var targetItems = $(this).parent('.btn-box').siblings('.items').find('.selected');
		
		var targetWindow = $(this).parents('.select2Box');
		var sKey = targetWindow.attr('datavalue');
		for( var i = 0; i < targetItems.length; i++ ){
			selectedArray.push(targetItems.eq(i).find('.sortItem').text()); // right
			selectedIdArray.push( targetItems.eq(i).attr('datavalue') );
		}
		
		targetWindow.hide();
		$("#select1").show();
		$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").hide()
		$("#MContainer").find("header").eq(0).hide()
		//$("body").css('overflow','hidden');
		//$("html").css('overflow','hidden');
		if (selectedArray.length == 0) {
			selectedArray.push('不限');
			selectedIdArray = [];
		};

		$("#select1 .item[data-target="+ targetId +"]").find('.selected').html( selectedArray.join('，')).attr('data-saveId',selectedIdArray.join(','));

	})
	


	// 点击二级分类的重置
	$("#MContainer").on('click','.select2Box .reset',function(){
		var targetItems = $(this).parent('.btn-box').siblings('.items').find('.item');
		for(var i = 0; i < targetItems.length; i++){
			targetItems.eq(i).removeClass('selected');
		}
	})


	// 点击一层分类的重置
	$("#MContainer").on('click','#select1 .reset',function(){
		var targetItems = $(this).parent('.btn-box').siblings('.items').find('.item');
		for( var i = 0; i < targetItems.length; i++ ){
			targetItems.eq(i).find('.selected').text('不限').attr('data-saveid','');
		}
		$(".select2Box .item").removeClass('selected');
	})

	// 点击一层分类的确定
	$("#MContainer").on('click','#select1 .sure',function(){

		var targetItems = $("#select1 .item");
		screenData = [];

		for( var i = 0; i < targetItems.length; i++ ){
			var _this = targetItems.eq(i);
			var littleJson = {};
			littleJson.keyname = _this.attr('datavalue');
			littleJson.keyvalue = _this.find('.selected').attr('data-saveid');
			littleJson.isskuattr = _this.attr('isskuattr');
			screenData.push( littleJson );
		}
		screenData = JSON.stringify(screenData);

		if ( FSH.tools.request('level')!=2 ) {
			window.location.href = window.hostname + '/product/Index?c='+cateloryId+'&level='+level+'&attrData=' + screenData;
			// window.location.href = window.hostname + '/product/Index?c='+cateloryId+'&level='+level;
		}else{
			// $(".selectBox").remove();
			$(".selectBox").hide();
			$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").show()
			$("#MContainer").find("header").eq(0).show()
			//$("body").css('overflow-y','scroll');
			//$("html").css('overflow-y','scroll');
			page = 1;
			$("#productList>div").eq(0).empty();
			isLastPage = false;
			ajaxFlag = true;
			getData();
		}
	})

	// 一层分类的返回按钮
	$("#MContainer").on('click','#select1 .returnBtn',function(){
		$("#select1").remove();
		// $("#select1").hide();
		$(".select2Box").remove();
		// $(".select2Box").hide();
		countNoBottomHeight();
		if ( FSH.tools.request('level') == 2 ) {
			$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").show();
			$("#MContainer").find("header").eq(0).show();
		}
	})

	$("#MContainer #productList .item").width(($("#MContainer").width()*0.95-5)/2-22);


	// 滚动加载
	$(document).on('scroll','',function(){
		if (FSH.tools.getScrollTop() + FSH.tools.getClientHeight() >= FSH.tools.getScrollHeight()) {
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

})

$(function(){
	// 从二级分类请求三级的数据
	$("#MContainer").on('click','.class1 .item',function(){
		var cateloryLevel2 = $(this).attr('cateloryid');

		var data = { level:level,categoryid:cateloryLevel2};
		getSubCategory(data);
		
	})

	$("#MContainer").on('click','.class2 .item',function(){
	    var cid = $(this).attr('cateloryid');
	    cateloryId = cid;
	    level = 2;
	    
	    var title = $.trim( $(this).find('.sortItem').text() );
	    // $("#MContainer").find('header').find('b').text( title );
	    getCategoryAttributes(cid);
	})

	$("#pageInReturn").on('click',function(){
		if ( $("#classContent .class2:visible").length > 0 ) {
			$("#classContent .class2").hide();
			$(".class1").show();
		}else{
			window.history.back(-1);
		}
	})


	// 返回
	$("#MContainer").on('click','.class1 .returnBtn',function(){
		$(this).parents('.selectBox ').remove();
		$("#MContainer #sortType,#MContainer #productList,#MContainer #footerWrap").show()
		$("#MContainer").find("header").eq(0).show()
		//$("body").css('overflow-y','scroll');
		//$("html").css('overflow-y','scroll');
	})

	$("#MContainer").on('click','.class2 .returnBtn',function(){
		$(this).parents('.selectBox').remove();
		if ( $(".class1").length > 0 ) { // 如果有二级分类，则请求它所在的二级分类的数据

		}
	})

})


function renderList(data){
	var s = '<div class="item pr">\
				<a href="' + window.hostname + '/item.html?productCode=' + data.SPU + '">\
					<span class="freeSign">包<br>邮</span>\
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
                var t=new SelectSKU({
                    json:data.Data,
                    type:1});
            }
            else {
                FSH.commonDialog(1, [data.Content]);
            }
        }
    });
}

function getData( ){
	if(ajaxFlag){
		isLoading = false;
		FSH.Ajax({
			url: "/product/productlist",
	        type:"post",
	        dataType: 'json',
	        data: {page:page,
					sort:sort, // sort:1上新，2升序，3降序
					level:level, //level：0一级分类，1二级分类，2三级分类,level=2时，c的值就是三级分类的id,你可以直接用
					c:cateloryId,
					attrData:screenData},
					// attrData:screenData},
	        jsonp: 'callback',
	        jsonpCallback: "success_jsonpCallback",
	        success: function (data) {
	        	if (data.Type == 1) {

	        	    if (data.Data.TotalRecord == 0) {
	        	        $("#sortType").removeClass('hide');
	        			// 无数据
	        			if ( $(".noData").length < 1 ) {
		        			$("#productList>div").eq(0).empty().append('<div class="noData">\
															<img src="../../Content/Images/noData.png?v=20151224" alt="无数据">\
															<p class="mainTips">没有找到您要找的商品</p>\
															<p class="secTips">换换其他类目吧</p>\
														</div>');
		        			$("#loadMore").remove();
	        			};
	        		}else{
	        			$("#sortType").removeClass('hide');
	        			if (data.Data.PageIndex == data.Data.PageCount) {
		        			isLastPage = true;
		        		}

	        			var html = '';
	        			for(var i = 0; i < data.Data.Products.length; i++){
	        				html = html + renderList(data.Data.Products[i]);
	        			};

	        			$("#productList>div").eq(0).append(html);

						

	        			// 设置“已售罄”字样的位置
	        			var size = parseInt($("#productList .item").width()*0.32);
	        			$("#productList .saleOut").css({
	        				'width': size,
	        				'height':size,
	        				'border-radius':size/2,
	        				'line-height':size+'px',
	        				'margin-top':-size/2+'px',
	        				'margin-left':-size/2+'px'
	        			})

	        			var nWidth = ($("#MContainer").width()*0.95-5)/2-22;
	        			var nHeight = 150*nWidth/127.5 + 'px';
	        			$("#MContainer #productList .item").width(nWidth);
	        			$("#MContainer #productList .item .imgBox").css({
	        				'width':nWidth + 'px',
	        				'height':nHeight,
	        				'line-height':nHeight
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
// 问题在以下
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

function renderLevelHtml(level,dataArray,title){
	var html = '';
	var int = parseInt(level);
	var classLevel = int + 1;
	var htmlstart = '<div class="MContainer selectBox class'+ classLevel +'" id="level'+ int +'">\
									<header class="pageHeader w100 pr tc overflowH">\
								        <a class="returnBtn"></a>\
								        <b class="f36 FontColor1">'+ title +'</b>\
								    </header>\
								    <div class="selectContent">\
								    	<div class="items boxShadow">';
					var list = ''
					for( var i = 0; i < dataArray.length; i++){
						list +=             '<div class="item" cateloryid="'+ dataArray[i].CategoryId +'">\
								    			<a href="javascript:;">\
								    				<i></i>\
													<span class="sortItem">'+ dataArray[i].CategoryName +'</span>\
								    			</a>\
								    		</div>';
					}
					
								    		
					var htmlend='		</div>\
										<!--<div class="btn-box">\
								    		<a class="reset">重置</a>\
								    		<a class="sure">确定</a>\
								    	</div>-->\
								    </div>\
								</div>';
	html = htmlstart + list + htmlend;
	return html;
}

function getSubCategory(data){
	FSH.Ajax({
		url:window.hostname + '/product/GetSubCategory',
		type:'post',
		data:data,
		success:function( msg ){
			if (msg.Type == 1) {
				level = msg.Data[0].CategoryLevel - 1;
				var html = renderLevelHtml(level,msg.Data,msg.Data[0].ParentName);
				
				$(html).appendTo("#MContainer").show();
				countNoBottomHeight();

			}else{
				FSH.commonDialog(1,['获取数据失败'])
			}
		},
		error:function(){
			FSH.commonDialog(1,[msg.Content]);
		}
	})
}


function getCategoryAttributes(cid){
	FSH.Ajax({
		url:window.hostname + '/product/GetCategoryAttributes',
		type:'post',
		data:{categoryid:cid},
		success:function( msg ){
			if ( msg.Type == 1 ) {
				var level1html = selectLevel1(msg.Data);
				var level2html = selectLevel2(msg.Data);
				$(level1html).appendTo('#MContainer').show();
				$(level2html).appendTo('#MContainer');
				countContentHeight();
			}
		},
		error:function(){
			FSH.commonDialog(1,[msg.Content]);
		}
	})
}


function getSelected(){
	var attrData = FSH.tools.request('attrData');
	var available = [];
	if ( attrData ) {
		attrData = JSON.parse(decodeURI(attrData));
		for( var i = 0; i < attrData.length; i++ ){
			if ( attrData[i].keyvalue ) {
				available.push({'index':i,'keyvalue':attrData[i].keyvalue})
			}
		}
	}
	return available;
}

var getSelected = getSelected();

function selectLevel1(data){
	var htmlstart = '<div class="MContainer selectBox" id="select1">\
					<header class="pageHeader w100 pr tc overflowH">\
				        <a class="returnBtn"></a>\
				        <b class="f36 FontColor1">筛选</b>\
				    </header>\
				    <div class="selectContent">\
				    	<div class="items boxShadow">'
	var list='';
	for( var i = 0; i < data.length; i++){
		var selectedValue = '<span class="selected">不限</span>';
		for( var j = 0; j < getSelected.length; j++ ){
			if ( i ==  getSelected[j].index ) {
				selectedValue = '<span class="selected" data-saveid="'+ getSelected[j].keyvalue +'">'+ getSelected[j].keyvalue +'</span>';
			}
		}
		list += '<div class="item" data-target="itemData'+ data[i].KeyName +'" datavalue="'+ data[i].KeyName +'" isSkuAttr="'+ data[i].IsSkuAttr +'">\
                    <a href="javascript:;">\
                        <i></i>\
                        <span class="sortItem">'+data[i].Name+'</span>'+ selectedValue +'</a>\
                </div>';
	}			    		
	var htmlend='</div>\
				    	<div class="btn-box">\
				    		<a class="reset">重置</a>\
				    		<a class="sure">确定</a>\
				    	</div>\
				    </div>\
				</div>';
	var html = htmlstart + list + htmlend;
	return html;
}

function selectLevel2(data){
	var html = '';
	for( var i = 0; i < data.length; i++ ){
		var htmlstart = '<div class="MContainer selectBox select2Box" id="itemData'+ data[i].KeyName +'" datavalue="'+ data[i].KeyName +'" isSkuAttr="@aa.IsSkuAttr">\
		                <header class="pageHeader w100 pr tc overflowH">\
		                    <a class="returnBtn"></a>\
		                    <b class="f36 FontColor1">'+ data[i].Name +'</b>\
		                </header>\
		                <div class="selectContent">\
		                    <div class="items boxShadow">'
		var list = '';
		for(var j = 0; j < data[i].KeyValues.length; j++){   
	                    list+= '<div class="item" datavalue="'+ data[i].KeyValues[j] +'">\
	                                <a href="javascript:;">\
	                                    <i></i>\
	                                    <span class="sortItem">'+ data[i].KeyValues[j] +'</span>\
	                                </a>\
	                            </div>'
		                        
		}
		var htmlend='       </div>\
		                    <div class="btn-box">\
		                        <a class="reset">重置</a>\
		                        <a class="sure">确定</a>\
		                    </div>\
		                </div>\
		            </div>';
		html += htmlstart + list + htmlend;
	}
	return html;
	

}