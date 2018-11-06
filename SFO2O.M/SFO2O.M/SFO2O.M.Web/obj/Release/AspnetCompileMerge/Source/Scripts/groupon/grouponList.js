

var page = 1;
var isLastPage=false;
$(function(){
	



	getData();

	// 滚动加载
	// $(document).on('scroll','',function(){
	// 	if (FSH.tools.getScrollTop() + FSH.tools.getClientHeight() >= FSH.tools.getScrollHeight()) {
	// 		if (isLoading) {
	// 			if (isLastPage) {
	// 				$("#loadMore").text('全部加载完');
	// 				return;
	// 			}else{
	// 				$("#loadMore").text('滑动加载更多');
	// 				page +=1;
	// 				getData();
	// 			}
	// 		}
	// 	}
	// })
	FSH.scrollPage(getData);

}) 

function returnStatus(qty){
	var returnStatusArray = [];
	if ( qty > 0) {
		returnStatusArray = ['goon','去开团'];
	}else{
		returnStatusArray = ['saleOut_n','待开团'];
	}
	return returnStatusArray;
}
function returnText(text,str) {
    var returnStatusArray = [];
    if (text == str) {
        returnStatusArray = [''];
    } else {
        returnStatusArray = ['('+text+')'];
    }
    return returnStatusArray;
}

function renderGrouponList(data, ImageServer) {
	var html = '<li class="w95p boxShadow bgColor3">\
		            <a href="/product/FightDetail?sku=' + data.Sku + '&pid='+data.pid + '">\
		                <div class="displayBox product mb10 w100">\
		                    <div class="imgBox">\
		                        <img src=" '+ImageServer+ data.ImagePath + '" />\
		                        <!--状态-->\
		                        <span class="status ' + returnStatus(data.ForOrderQty)[0] + '"></span>\
		                        <!--状态end-->\
		                    </div>\
		                    <div class="boxflex1 pt8">\
		                        <div class="title f28 FontColor1 mb8">' + data.Name + ' ' + data.MainValue + ' '  + data.SubValue+data.NetWeightUnit+'</div>\
		                        <div class="f24 FontColor4 mb10">单独购买：￥' + data.MinPrice.toFixed(2) + '</div>\
		                        <div class="btn-box fontWhite pr">\
		                            <span class="f24" style="margin-left: 17%;">'+data.TuanNumbers+'人团</span>\
		                            <span class="f30" style="margin-left: 2.5%; position:relation; top:5px;">￥' + data.DiscountPrice.toFixed(2) + '</span>\
		                            <span class="f24" style="position: absolute; right: 5%;"><span style=" padding-left:8px; border-left:1px solid #fff;">'+ returnStatus(data.ForOrderQty)[1] +'</span></span>\
		                        </div>\
		                    </div>\
		                </div>\
		                <div class="brand FontColor4 f24">\
		                    <span class="brandLogo mr5"><img src="' + ImageServer + data.Logo + '" /></span><span>' + data.CountryOfManufacture + '  ' + data.Brand + ' ' + returnText(data.BrandEN, data.Brand)[0] + '</span>\
		                </div>\
		            </a>\
		        </li>'
    return html;

}

// 未完待续 规格如何返回待定
function getData( ){ 
	if(ajaxFlag && !isLastPage){
		isLoading = false;
		FSH.Ajax({
			url: "/product/FightListIndex?PageIndex="+page ,
	        type:"get",
	        dataType: 'json',
	        jsonp: 'callback',
	        // data:{'page':page},
	        jsonpCallback: "success_jsonpCallback",
	        loadingType:2,
	        success: function (data) {
	  			if ( !data.IsTrue) {
	  				window.location.href = window.hostname + '/product/PinEnd';
	  			}
	        	if (data.Type == 1 ) {

	        		if (data.Data.TotalRecords == 0) {
	        			// 无数据
	        			if ( $(".noData").length < 1 ) {
		        			$("header").after(' <div class="noData">\
											        <img src="../../Content/Images/null.png?v=20160111" title="" alt="" />\
											        <p class="mainTips">暂无进行中的拼生活活动</p>\
											        <p class="secTips">先去看看其他的商品吧</p>\
											    </div>\
											    <div class="btnBox mt20">\
											        <a href="'+ window.hostname +'" class="btn default">回首页</a>\
											    </div>');
	        			};
	        		}else{
	        			
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
	        			    html = html + renderGrouponList(data.Data.Products[i], data.ImageServer);
	        			};

	        			$("#grouponList").append(html);

	        			// var nWidth = parseInt($("#MContainer").width())*0.95*0.3;
						// $(".grouponList .imgBox").height( nWidth );
						// $(".grouponList .status").height( nWidth*0.35 );
						var btnWidth = (parseFloat( $("#MContainer").width() )*0.95*0.7-7-2);
						var btnHeight = btnWidth*54/395;
						$(".grouponList .btn-box").height( btnHeight+'px'  ).css('line-height',btnHeight+'px');
						$(".grouponList .btn-box span").eq(0).css('margin-left',btnWidth*0.17+'px');
						$(".grouponList .btn-box span").eq(1).css('margin-left',btnWidth*0.05+'px');

	        			
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