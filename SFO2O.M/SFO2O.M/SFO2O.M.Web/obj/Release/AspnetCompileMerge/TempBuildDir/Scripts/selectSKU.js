/**
* 筛选SKU弹窗 version:20151230
@type           必填 触发类型 0 详情页sku选择  1 详情页加入购物车 2详情页立即购买 3 购物车SKU选择
@json           必填 
@resultEL       必填 显示sku选中值得对象
@numEL          选填 数量所在input对象 若为空则为弹窗内的input
@addToCarUrl    必填 加入购物车异步交互地址
@buyNowUrl      选填 立即购买异步交互地址 当type为2是可以此参数可以为空
@originalSKU    选填 立即购买时需要提供原始选中的skuid
@addCartSuccess 选填  加入购物车成功后回调事件
checkNum(el,errorShowType)  验证库存量 el待验证的数据所在input对象 errorShowType库存量不足显示的位置状态位 0 弱提示 1 输入框下面提示
reduceNum(el,errorShowType) 库存量减一 el待验证的数据所在input对象 errorShowType库存量不足显示的位置状态位
addNum(el,errorShowType)    库存量加一 el待验证的数据所在input对象 errorShowType库存量不足显示的位置状态位
setItemPagePrice(price,originalPrice,promotionLable,isDutyOnSeller,taxAmount,RealTaxType)设置价格及商品税
**/
//关闭登录页面
function closeIframe(f,cartNum){
	$("#MContainer").show();
  $("#loginIframe").remove();
  FSH.fixedFooter();
  if(f==1){
  	if(cartNum){
  		$("#cartNum").html(cartNum).show()
  	}else{
  		$("#cartNum").html(0).hide()
  	}
  	$("#footerWrap .loginArea").html('<a class="f24 FontColor3" href="/account/signout">退出</a>')
	}
  
}
/*保留两位小数 并加进位标示*/
function toNumber(num){
	var t;
	t=FSH.tools.toDecimal2(num.toString().replace(/,/g, ""));
  t=FSH.tools.formatNum(t);
  return t;
}
(function (window, undefined) {
$.views.helpers({
    jsonToString:function(obj){return  JSON.stringify(obj);},
    toDecimal2:function(num){
    	var t;
			t=FSH.tools.toDecimal2(num.toString().replace(/,/g, ""));
  		t=FSH.tools.formatNum(t);
  		return t;
    }
})
var that,selectSkuElPos;
function SelectSKU(options){
	that=this;
	var defaults = {    
		type:0,
		json:{"ImgUrl":"","ProductName":"","InitPrice":"","MainName":"","MainCode":"","SubName":"","SubCode":"","SubAttributes":[],"MainAttributes":[{"MetaCode":"","Id":"","Name":"","Flag":-1,"SubAttributes":[],"Sku":"","ForOrder":0,"Price":168.00}]},
		resultEL:"#skuSelectA" ,
		numEL:"#num",  
		deleteItemUrl:window.hostname+"/shoppingcart/deleteitems", 
		addToCarUrl:window.hostname+"/ShoppingCart/AddItem",
		buyNowUrl:window.hostname+"/buy/CheckSku",
		cartChangeSKUUrl:window.hostname+"/ShoppingCart/modifyproduct",
		changeItemNumUrl:window.hostname+"/ShoppingCart/UpdateQty",
		originalSKU:"",
		addCartSuccess:function(cartnum){}
	  };     
		that.opts = $.extend(defaults, options); 
		that.SKUID="";//选中的skuID
		that.errorShowType=that.opts.type==3?1:0;
		that.notEnoughFlag=false;//true 库存量不足  false 足够可提交
		that._init();
};
SelectSKU.prototype = {
	_init:function(){
		var t;
		if(!$("#selectSKU").length>0){
			t=0;
		}else{
			t=1;
		}
		if(!that.opts.json.MainName && !that.opts.json.MainCode){
			//没有sku属性
			that.SKUID=that["opts"]["json"]["MainAttributes"][0]["Sku"];
			if(that.opts.type==1 || that.opts.type==2){
				that._submit();
			}
		}else{
			for(var i=0;i<that["opts"]["json"]["SubAttributes"].length;i++){
				that["opts"]["json"]["SubAttributes"][i]["Flag"]=0;
			}
			that._creatDialog(t,that.opts.json);
			$("#skuMark,#selectSKU").show();
			//$("#selectSKU").css({"max-height":$(window).height(),"overflow":"auto"});
			//if(t==1){
			selectSkuElPos=$(window).scrollTop();
				$("#MContainer").outerHeight($(window).height())
												.css("overflow","hidden");
			//}
		}
	},
	_getInitSkuId:function(){
		//获取用户已经选中的skuId
		var b={'skuId':'','price':''}
		for(var i=0;i<that.opts.json.MainAttributes.length;i++){
			if(that["opts"]["json"]["MainAttributes"][i]["Flag"]==1 && that["opts"]["json"]["MainAttributes"][i]["SubAttributes"].length==0 ){
						that.numEL.attr("_max",that["opts"]["json"]["MainAttributes"][i]["ForOrder"]);
						if(that.resultEL.attr("_num") && that.opts.type!=3){
							that.numEL.val(that.resultEL.attr("_num"));	
						}
						b.skuId=that["opts"]["json"]["MainAttributes"][i]["Sku"];
						b.price=that["opts"]["json"]["MainAttributes"][i]["Price"]
						return b;
				
			}
			if(that["opts"]["json"]["MainAttributes"][i]["Flag"]==1){
				for(var j=0;j<that["opts"]["json"]["MainAttributes"][i]["SubAttributes"].length;j++){
					if(that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["Flag"]==1){
						 that.numEL.attr("_max",that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["ForOrder"]);
						 if(that.resultEL.attr("_num") && that.opts.type!=3){
							that.numEL.val(that.resultEL.attr("_num"));	
							}
							b.skuId=that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["Sku"];
							b.price=that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["Price"];
						 return b;
					}

				}	
			}
		}
		return false;
		
	},
	_creatDialog:function(t,json){
		//创建弹窗 t=0 创建整个弹窗 1弹窗内所有内容 2渲染主属性 3渲染子属性 
		var btnHtml1='<a class="addToCarBtn tc f28 boxflex1" id="addToCarBtn2">加入购物车</a><a class="buyNowBtn tc f28 boxflex1" id="buyNowBtn2">立即购买</a>',
				//className=that.SKUID?"":"defaultBtn",
				className="",
				btnHtml2='<a class="btn '+className+' tc f28 boxflex1" id="sureBtn">确定</a>',
				btnHtml=btnHtml2,
				numHtml='<!--数量-->\
										<div class="w95p pb10">\
											<p class="itemTitle f28 mt10 mb10 FontColor3">数量</p>\
											<div class=" skuNum whiteOnLine">\
												<b class="reduceBtn boxSizingB whiteOnLine disable">－</b>\
												<input type="number" class="numTxt boxSizingB tc" name="num" id="num" value="1">\
												<b class="addBtn boxSizingB whiteOnLine">＋</b>\
											</div>\
										</div>\
										<!--数量 END-->',
				outHtml="";
				if(that.opts.type==0){
					btnHtml=btnHtml1;
				}
				if(that.opts.type==3){
					numHtml="";
				}
		var temps=$.templates({
			dialogTmpl:'<div class="skuMark hide" id="skuMark"></div>\
									<div class="w100 selectSKU bgColor3 hide" id="selectSKU">\
										{{include tmpl="infTmpl"/}}\
									</div>',
			infTmpl:'<a class="closeA"></a>\
							<!--商品信息-->\
							<div class="w95p displayBox borderBottom whiteOnLine proInf">\
								<img src="{{:ImgUrl}}" class="productImg whiteOnLine mr10">\
								<div class="boxflex1 pr30">\
									<p class="f24 proName">{{:ProductName}}</p>\
									<p id="itemProPrice2" class="FontColor6 f30">￥{{:~toDecimal2(InitPrice)}} {{if OriginalPrice}}<span class="originalPrice f20 FontColor4">￥{{:OriginalPrice}}</span>{{/if}}</p>\
								</div>\
							</div>\
							<!--商品信息 END-->\
							<div class="w100 selectSKUConWrap">\
							<div class="w100 selectSKUCon">\
							<!--SKU选择区-->\
							<div class="w95p borderBottom whiteOnLine">\
								<p class="itemTitle f28 mt10 mb10 FontColor3" id="sku1Title">{{:MainName}}</p>\
								<ul id="sku1" class="skuUL clearfix">\
									{{for MainAttributes}}\
									{{include tmpl="sku1Temp"/}}\
									{{/for}}\
								</ul>\
								{{if SubName}}\
								<p class="itemTitle f28 mb10 FontColor3" id="sku2Title">{{:SubName}}</p>\
								<ul id="sku2" class="skuUL clearfix">\
								{{for SubAttributes}}\
									{{include tmpl="sku2Temp"/}}\
								{{/for}}\
								</ul>\
								{{/if}}\
							</div>\
							<!--SKU选择区END-->'+
							numHtml
							+'</div></div><div class="w100 displayBox SKUBtnArea">'+
							btnHtml
							+'</div>',
			sku1Temp:"<li class='{{if Flag==1}}selected{{else  Flag==-1}}disable{{/if}}' _Id='{{:Id}}' _skuid='{{:Sku}}' _forOrder='{{:ForOrder}}' _price='{{:~toDecimal2(Price)}}' _tariff='{{:TaxAmount}}' _RealTaxType='{{:RealTaxType}}' _isDutyOnSeller='{{:IsDutyOnSeller}}' _originalPrice='{{:OriginalPrice}}'  _subAttributes='{{:~jsonToString(SubAttributes)}}' _promotion='{{if Promotion==null}}0{{else}}1{{/if}}'  {{if Promotion!=null}}_promotionLable='{{:Promotion.PromotionLable}}'{{/if}}>{{:Name}}</li>",
			sku2Temp:"<li class='{{if Flag==1}}selected{{else  Flag==-1}}disable{{/if}}' _Id='{{:Id}}' _skuid='{{:Sku}}' _forOrder='{{:ForOrder}}' _price='{{:~toDecimal2(Price)}}' _tariff='{{:TaxAmount}}' _RealTaxType='{{:RealTaxType}}' _isDutyOnSeller='{{:IsDutyOnSeller}}' _originalPrice='{{:OriginalPrice}}'  _promotion='{{if Promotion==null}}0{{else}}1{{/if}}' {{if Promotion!=null}}_promotionLable='{{:Promotion.PromotionLable}}'{{/if}}>{{:Name}}</li>"
		});
		switch(t){
			case 0://页面追加整个弹窗
						 outHtml=$.render.dialogTmpl(json);
						 $("#MContainer").append(outHtml);	
						 break;
			case 1://渲染弹窗内所有内容
						 outHtml=$.render.infTmpl(json);
						 $("#selectSKU").html(outHtml);	
						 break;
			case 2://渲染主属性
						 outHtml=$.render.sku1Temp(json);
						 $("#sku1").html(outHtml);	
						 break;
			case 3://渲染子属性
						 outHtml=$.render.sku2Temp(json);
						 $("#sku2").html(outHtml);	
						 break;

		}
		that.numEL=$(that.opts.numEL);
		that.resultEL=$(that.opts.resultEL);
		that.dispose();
		that._bindEvent();
		
		var b=that._getInitSkuId();
		if(b.skuId && t!=2 && t!=3){
			$("#sku1 li.selected").removeClass("selected").trigger("click");
			that.SKUID=b.skuId;
			//当选中状态被带入时
			$("#itemProPrice2").html("￥"+toNumber(b.price))
			//促销价显示原始价
			if(that.opts.json.OriginalPrice){
				$("#itemProPrice2").append('<span class="originalPrice f20 FontColor4">￥'+that.opts.json.OriginalPrice+'</span>')
			}
			//促销价显示原始价 end
		}
		that.checkNum($(that.opts.numEL),that.errorShowType);
	},
	_bindEvent:function(){
		$("#MContainer").on("click","#sku1 li",function(){
				//绑定主属性选择
				if(!$(this).hasClass("selected") && !$(this).hasClass("disable")){
					if($("#sku1 li.selected").length>0 && $("#sku2 li.selected").length>0){
						var subSku2=eval('(' + $("#sku1 li.selected").attr("_subAttributes") + ')');
						for(var t=0; t<subSku2.length;t++){
							if(subSku2[t]["Flag"]==1){
								subSku2[t]["Flag"]=0;
								break;
							}
						}
						$("#sku1 li.selected").attr("_subAttributes",JSON.stringify(subSku2))
					}
					var subSku=eval('(' + $(this).attr("_subAttributes") + ')');
					if(subSku.length>0){
						that.SKUID="";
						if($("#sku2").length>0 && $("#sku2 li.selected").length>0){
							var subid=$("#sku2 li.selected").attr("_id");
							for(var i=0;i<subSku.length;i++){
								if(subSku[i]["Id"]==subid && subSku[i]["Flag"]!="-1"){
									subSku[i]["Flag"]="1";
									that.SKUID=subSku[i]["Sku"]

									$("#itemProPrice2").html("￥"+toNumber(subSku[i]["Price"]));
									//促销价显示原始价
									if(subSku[i]["Promotion"]){
										$("#itemProPrice2").append('<span class="originalPrice f20 FontColor4">￥'+subSku[i]["OriginalPrice"]+'</span>');
										that.setItemPagePrice(toNumber(subSku[i]["Price"]),subSku[i]["OriginalPrice"],subSku[i]["Promotion"]["PromotionLable"],subSku[i]["IsDutyOnSeller"],subSku[i]["TaxAmount"],subSku[i]["RealTaxType"]);
									}else{
										that.setItemPagePrice(toNumber(subSku[i]["Price"]),"","",subSku[i]["IsDutyOnSeller"],subSku[i]["TaxAmount"],subSku[i]["RealTaxType"]);
									}
									//促销价显示原始价 end
									that.numEL.attr("_max",subSku[i]["ForOrder"]);
									that.checkNum(that.numEL,that.errorShowType);
									break;
								}
							}
						}
						that._creatDialog(3,subSku);
						/*
						if($("#sureBtn").length>0){
							if(that.SKUID){
								$("#sureBtn").removeClass("defaultBtn");
							}else{
								$("#sureBtn").addClass("defaultBtn");
							}
						}
						*/
					}else{
						$("#itemProPrice2").html("￥"+toNumber($(this).attr("_price")));
						//促销价显示原始价
						if($(this).attr("_promotion")==1){
							$("#itemProPrice2").append('<span class="originalPrice f20 FontColor4">￥'+
								$(this).attr("_originalPrice")+'</span>');
							that.setItemPagePrice(toNumber($(this).attr("_price")),$(this).attr("_originalPrice"),$(this).attr("_promotionLable"),$(this).attr("_isDutyOnSeller"),$(this).attr("_tariff"),$(this).attr("_RealTaxType"));
						}else{
							that.setItemPagePrice(toNumber($(this).attr("_price")),"","",$(this).attr("_isDutyOnSeller"),$(this).attr("_tariff"),$(this).attr("_RealTaxType"));
						}
						//促销价显示原始价 end
						that.SKUID=$(this).attr("_skuid")
						that.numEL.attr("_max",$(this).attr("_forOrder"));
						/*
						if($("#sureBtn").length>0){
							$("#sureBtn").removeClass("defaultBtn");
						}
						*/
						that.checkNum(that.numEL,that.errorShowType);
					}
					$(this).addClass("selected").siblings("li.selected").removeClass("selected");
					return false;
				}
				//已选中状态下取消
				if($(this).hasClass("selected")){
					//$("#sku2 li.selected").removeClass("selected");
					$("#sku2 li.disable").removeClass("disable");
					var subSku2=eval('(' + $(this).attr("_subAttributes") + ')');
					for(var i=0; i<subSku2.length;i++){
						if(subSku2[i]["Flag"]==1){
							subSku2[i]["Flag"]=0;
							break;
						}
					}
					$("#itemProPrice2").html("￥"+toNumber(that.opts.json.InitPrice.replace(",", "")));
					//促销价显示原始价
					if(that.opts.json.OriginalPrice){
						$("#itemProPrice2").append('<span class="originalPrice f20 FontColor4">￥'+that.opts.json.OriginalPrice+'</span>');
						that.setItemPagePrice(toNumber(that.opts.json.InitPrice.replace(",", "")),that.opts.json.OriginalPrice,that.opts.json.PromotionLable,that.opts.json.IsDutyOnSeller,that.opts.json.TaxAmount,that.opts.json.RealTaxType);
					}else{
						that.setItemPagePrice(toNumber(that.opts.json.InitPrice.replace(",", "")),"","",that.opts.json.IsDutyOnSeller,that.opts.json.TaxAmount,that.opts.json.RealTaxType);
					}
					//促销价显示原始价 end
					$(this).attr("_subAttributes",JSON.stringify(subSku2)).removeClass("selected");
					that.SKUID="";
					/*
					if($("#sureBtn").length>0){
							$("#sureBtn").addClass("defaultBtn");
						}
					*/
					return false;
				}
				
			})
		.on("click","#sku2 li",function(){
			//绑定子属性选择
			if(!$(this).hasClass("selected") && !$(this).hasClass("disable")){
				if($("#sku1 li.selected").length==0){
					var subid=$(this).attr("_id");
					for(var i=0;i<that["opts"]["json"]["MainAttributes"].length;i++){
						for(var j=0;j<that["opts"]["json"]["MainAttributes"][i]["SubAttributes"].length;j++){
							if(that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["Id"]==subid && that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["Flag"]!="-1"){
								$("#sku1 li[_id="+that["opts"]["json"]["MainAttributes"][i]["Id"]+"]").removeClass("disable")
								break;
							}
							if(j==that["opts"]["json"]["MainAttributes"][i]["SubAttributes"].length-1){
								$("#sku1 li[_id="+that["opts"]["json"]["MainAttributes"][i]["Id"]+"]").removeClass("disabled").addClass("disable");
							}
						}
					}
					$(this).attr({"_skuid":"","_price":that.opts.json.InitPrice})
				}
				$(this).addClass("selected").siblings("li.selected").removeClass("selected");
				$("#itemProPrice2").html("￥"+toNumber($(this).attr("_price")));
				//促销价显示原始价
				if($(this).attr("_promotion")==1){
					$("#itemProPrice2").append('<span class="originalPrice f20 FontColor4">￥'+
				$(this).attr("_originalPrice")+'</span>');
					that.setItemPagePrice(toNumber($(this).attr("_price")),$(this).attr("_originalPrice"),$(this).attr("_promotionLable"),$(this).attr("_isDutyOnSeller"),$(this).attr("_tariff"),$(this).attr("_RealTaxType"));
				}else{
					that.setItemPagePrice(toNumber($(this).attr("_price")),"","",$(this).attr("_isDutyOnSeller"),$(this).attr("_tariff"),$(this).attr("_RealTaxType"));
				}
				//促销价显示原始价 end
				that.SKUID=$(this).attr("_skuid");
				that.numEL.attr("_max",$(this).attr("_forOrder"));
				/*
				if($("#sureBtn").length>0){
					$("#sureBtn").removeClass("defaultBtn");
				}
				*/
				that.checkNum(that.numEL,that.errorShowType);
				return false;
			}
			//已选中状态下取消
				if($(this).hasClass("selected")){
					
					$(this).removeClass("selected");
					that.SKUID="";
					$("#itemProPrice2").html("￥"+toNumber(that.opts.json.InitPrice))
					//促销价显示原始价
					if(that.opts.json.OriginalPrice){
						$("#itemProPrice2").append('<span class="originalPrice f20 FontColor4">￥'+that.opts.json.OriginalPrice+'</span>');
						that.setItemPagePrice(toNumber(that.opts.json.InitPrice),that.opts.json.OriginalPrice,that.opts.json.PromotionLable,that.opts.json.IsDutyOnSeller,that.opts.json.TaxAmount,that.opts.json.RealTaxType);
					}else{
						that.setItemPagePrice(toNumber(that.opts.json.InitPrice),"","",that.opts.json.IsDutyOnSeller,that.opts.json.TaxAmount,that.opts.json.RealTaxType);
					}
					//促销价显示原始价 end
					/*
					if($("#sureBtn").length>0){
							$("#sureBtn").addClass("defaultBtn");
						}
					*/
					return false;
				}
		})
		.on("click","#selectSKU .closeA",function(){
			//关闭弹窗
			$("#skuMark,#selectSKU").hide();
			$("#MContainer").height("auto");
			$(window).scrollTop(selectSkuElPos);
			FSH.fixedFooter();
			if(that.opts.type!=3){
				that._saveData();
			}
		})
		.on("keyup",that.opts.numEL,function(){
			if(that.opts.type!=3){
				var max=parseFloat($(this).attr("_max")),
						val=parseFloat($(this).val());
				that.checkNum(that.numEL,that.errorShowType);
			}
		})
		.on("blur",that.opts.numEL,function(){
			if(that.opts.type!=3){
				if(!FSH.string.Trim($(this).val())){
					$(this).val(1);
				}
			}
		})
		.on("click",".addBtn",function(){
			if(that.opts.type!=3){
				that.addNum(that.numEL,that.errorShowType)
			}
		})
		.on("click",".reduceBtn",function(){
			if(that.opts.type!=3){
				that.reduceNum(that.numEL,that.errorShowType)
			}
		})
		.on("click","#selectSKU #addToCarBtn2,#selectSKU #buyNowBtn2,#selectSKU #sureBtn",function(){
			/*
			if($(this).hasClass("defaultBtn")){
				return false;
			}
			*/
			if(!that.SKUID){
				var str=$("#sku1Title").html();
				if($("#sku2Title").length>0){
					str+=" "+$("#sku2Title").html()
				}
					FSH.smallPrompt("请选择 "+str);
					return false;
			}
			if(that.notEnoughFlag && that.opts.type!=3){
					FSH.smallPrompt("数量超出最大库存")
					return false
			}
			if($(this).is("#addToCarBtn2")){
				that.opts.type=1;
			}else if($(this).is("#buyNowBtn2")){
				that.opts.type=2;
			}
			
			that._submit();
			

		})
	},
	_saveData:function(){
		//将选中的sku值显示
		var saveStr;
		if(!that.SKUID){
			saveStr=that.resultEL.attr("data-init");
			//that.resultEL.html();
		}else{
			
			if(that.opts.type==3){
				saveStr=$("#sku1Title").html()+'：'+$("#sku1 li.selected").html();
			}else{
				saveStr='已选：“'+$("#sku1 li.selected").html()+'”';
			}
			if($("#sku2 li.selected").length>0){
				if(that.opts.type==3){
					saveStr+=' , '+$("#sku2Title").html()+'：'+$("#sku2 li.selected").html();
				}else{
					saveStr+=',“'+$("#sku2 li.selected").html()+'”';
				}
			}
			
		}
		if(that.opts.type!=3){
			saveStr=saveStr+'<i class="itemIcon_jt"></i>';
		}
		if($(that.opts.resultEL).length>0){
			//针对列表页添加判断 列表页加入购物车后无需在页面中显示已选中的sku属性
			if($(that.opts.resultEL).css("display")!="none"){
				that.resultEL.html(saveStr);
			}
			if(that.opts.type!=3){
				if($(that.opts.resultEL).css("display")!="none"){
					that.resultEL.attr("_num",that.numEL.val());
				}
				
			}else{
				var price=tariff=0;
				if($("#sku1 li.selected").attr("_skuid")){
					price=$("#sku1 li.selected").attr("_price");
					tariff=$("#sku1 li.selected").attr("_tariff");
				}else{
					price=$("#sku2 li.selected").attr("_price");
					tariff=$("#sku2 li.selected").attr("_tariff");
				}
				that.resultEL.parents("li").find(".proPrice").html("￥"+price);
				that.resultEL.parents("li").find(".proTax").html("￥"+tariff);
				cart.Calculation();
			}
			if($(that.opts.resultEL).css("display")!="none"){
				that.resultEL.attr("_selectedSKU",that.SKUID);
				that._changeJson()
			}
		}
	},
	_changeJson:function(){
		//根据新选中的sku属性改变opts.json值
		var selectSku1Id=$("#sku1 li.selected").attr("_Id"),
			  selectSku2Id=$("#sku2 li.selected").attr("_Id");
			  if(!that.SKUID){
			  	selectSku1Id=selectSku2Id="";
			  }
		for(var i=0;i<that.opts.json.MainAttributes.length;i++){
			if(that["opts"]["json"]["MainAttributes"][i]["Id"]==selectSku1Id){
				that["opts"]["json"]["MainAttributes"][i]["Flag"]=1;
				for(var j=0;j<that["opts"]["json"]["MainAttributes"][i]["SubAttributes"].length;j++){
					if(that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["Id"]==selectSku2Id){
						 that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["Flag"]=1;
					}else if(that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["Flag"]!=-1){
							that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][j]["Flag"]=0;
					}

				}
			}else if(that["opts"]["json"]["MainAttributes"][i]["Flag"]!=-1){
				that["opts"]["json"]["MainAttributes"][i]["Flag"]=0;
				for(var k=0;k<that["opts"]["json"]["MainAttributes"][i]["SubAttributes"].length;k++){
					if(that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][k]["Flag"]!=-1){
							that["opts"]["json"]["MainAttributes"][i]["SubAttributes"][k]["Flag"]=0;
					}

				}
			}
		}
		that.resultEL.attr("data-json",JSON.stringify(that.opts.json))
	},
	_submit:function(){
		//提交数据
		var num=1;
		if(typeof(that.numEL) == "undefined" || that.numEL.length==0 ){
			num=1;
		}else{
			num=that.numEL.val();
		}
		var url="";
		if(that.opts.type==1){
			url=that.opts.addToCarUrl
			that.ajaxFun(that.opts.type,url,that.SKUID,num);
			return false;
		}
		if(that.opts.type==2){
			url=that.opts.buyNowUrl;
			that.ajaxFun(that.opts.type,url,that.SKUID,num);
			return false;
		}
		if(that.opts.type==3){
			//购物车修改sku属性
			//console.log("原始SKU:",that.opts.originalSKU)
			//console.log("现在SKU:",that.SKUID)
			//console.log("num:",that.numEL.val())
			if(that.opts.originalSKU != that.SKUID){
		   $.ajax({
		        url: that.opts.cartChangeSKUUrl,
		        type: 'post',
		        data: {"sku":that.opts.originalSKU,"newSku":that.SKUID,"qty":that.numEL.val(),"productCode":that.numEL.parents("li").attr("data-spu")},
		        cache: true,
		        async: true,
		        timeout:100000000,
		        success: function (json) {
		          if (typeof json == "string"){
		            $("#"+depotType).find(".cartProList1").html(json);
								cart.Calculation();
							 	that._saveData()
								if($("#selectSKU").length>0){
									$("#skuMark,#selectSKU").hide();
									$("#MContainer").height("auto");
									$(window).scrollTop(selectSkuElPos);
									FSH.fixedFooter();
								}
								var pos= $("#"+depotType).find(".cartProList1 li[data-anchor=1]").offset();
								$(window).scrollTop(pos.top,500)
	           }else{
							FSH.commonDialog(1,[json.Content]);
	           }
		        },
		        error: function (err) {
		            FSH.commonDialog(1,['请求超时，请刷新页面']);  
		        }
		    });
		  }else{
		  	cart.Calculation();
			 	that._saveData()
				if($("#selectSKU").length>0){
					$("#skuMark,#selectSKU").hide();
					$("#MContainer").height("auto");
					$(window).scrollTop(selectSkuElPos);
					FSH.fixedFooter();
				}
		  }
			return false;
		}
		//针对列表页添加判断 列表页加入购物车后无需在页面中显示已选中的sku属性
		if(that.resultEL.length>0){
			that._saveData()
		}
		if($("#selectSKU").length>0){
			$("#skuMark,#selectSKU").hide();
			$("#MContainer").height("auto");
			$(window).scrollTop(selectSkuElPos);
			FSH.fixedFooter();
		}
		
	},
	checkNum:function(el,errorShowType){
		//验证库存量
		that.notEnoughFlag=false;
		if(!that.SKUID){
			el.siblings(".reduceBtn,.addBtn").addClass("disable");
			/*
			if($("#sureBtn").length>0){
				$("#sureBtn").addClass("defaultBtn")
			}
			*/
			return false;
		}
		var val=parseFloat(el.val()),
				max=parseFloat(el.attr("_max"));
		if(val>max){
			that.notEnoughFlag=true;
			el.siblings(".addBtn").addClass("disable");
			el.siblings(".reduceBtn").removeClass("disable");
			if(that.opts.type!=3){
				FSH.smallPrompt("数量超出最大库存");
				/*
				if($("#sureBtn").length>0){
					$("#sureBtn").addClass("defaultBtn")
				}
				*/
			}
		}else{ 
			/*
			if($("#sureBtn").length>0){
				$("#sureBtn").removeClass("defaultBtn")
			}
			*/
			if(val==max){
				el.siblings(".addBtn").addClass("disable")
				el.siblings(".reduceBtn").removeClass("disable")
			}else if(val<=1){
				el.val(1);
				el.siblings(".reduceBtn").addClass("disable")
				el.siblings(".addBtn").removeClass("disable")
			}else{
				el.siblings(".reduceBtn,.addBtn").removeClass("disable");
			}
		}
	},
	reduceNum:function(el,errorShowType){
		//库存量减一
		var thisEl=el.siblings(".reduceBtn");
		if(!thisEl.hasClass("disable")){
			var max=parseFloat(el.attr("_max")),
					val=parseFloat(el.val());
			if(val>1){
				val--;
				el.val(val);
			}
			that.checkNum(el,that.errorShowType);
			
		}

	},
	addNum:function(el,errorShowType){
		//库存量加一
		var thisEl=el.siblings(".addBtn");
		if(!thisEl.hasClass("disable")){
			var max=parseFloat(el.attr("_max")),
					val=parseFloat(el.val());
			if(val<max){
				val++;
				el.val(val);
			}
			that.checkNum(el,that.errorShowType);
			
		}

	},
	dispose:function(){
		$("#MContainer").off("click","#sku1 li")
		.off("click","#sku2 li")
		.off("click","#selectSKU .closeA")
		.off("keyup",that.opts.numEL)
		.off("blur",that.opts.numEL)
		.off("click",".addBtn")
		.off("click",".reduceBtn")
		.off("click","#selectSKU #addToCarBtn2,#selectSKU #buyNowBtn2,#selectSKU #sureBtn")
	},
	ajaxFun:function(type,url,sku,qty){/*1 加入购物车的 2 立即购买 */
		FSH.Ajax({
        url: url,
        dataType: 'json',
        data:{"sku":sku,"qty":qty},
        jsonp: 'callback',
        jsonpCallback: "success_jsonpCallback",
        success: function(json) {
        		if(json.Type==1){
        			 if(type==1){
        			 	 var cartnum=0;
        			 	 if(json.Data==0){
        			 	 	$("#cartNum").hide();
        			 	 	cartnum=0;
        			 	 }else if(json.Data>99){
        			 	 	$("#cartNum").show();
        			 	 	cartnum="9+";
        			 	 }else{
        			 	 	$("#cartNum").show();
        			 	 	cartnum=json.Data;
        			 	 }
        			 	 //$("#cartNum").html(cartnum);
        				 //FSH.commonDialog(1,['添加购物车成功']);
        				that.opts.addCartSuccess(cartnum);
        				
        			 }else if(type==2){
        			 	window.location=window.hostname+json.LinkUrl
        			 }else{
        			 	cart.Calculation();
        			 }
      			 	that._saveData()
							if($("#selectSKU").length>0){
								$("#skuMark,#selectSKU").hide();
								$("#MContainer").height("auto");
								$(window).scrollTop(selectSkuElPos);
								FSH.fixedFooter();
							}
        		}else{
        			FSH.commonDialog(1,[json.Content]);
        		}
        },
        error: function(err) {
            FSH.commonDialog(1,['请求超时，请刷新页面']);   
        }
    })
	},
	deleteItem:function(skus){
		var skusParam=$.param({"skus":skus}, true);
		FSH.Ajax({
        url: that.opts.deleteItemUrl,
        dataType: 'json',
        type:'post',
        data:skusParam,
        jsonp: 'callback',
        jsonpCallback: "success_jsonpCallback",
        success: function(json) {
        		if(json.Type==1){
						  that.ajaxFun(that.opts.type,that.opts.addToCarUrl,that.SKUID,that.numEL.val());
						}else{
							FSH.commonDialog(1,[json.Content]);   	
						}
        },
        error: function(err) {
            FSH.commonDialog(1,['请求超时，请刷新页面']);   
        }
    })
		
	},
	setItemPagePrice:function(price,originalPrice,promotionLable,isDutyOnSeller,taxAmount,RealTaxType){
		//设置单品页外面价格显示 isDutyOnSeller 1卖家承担商品税 0 卖家不承担商品税 taxAmount商品税金额  RealTaxType 1是综合税 2是行邮税
		if($("#itemProPrice").length>0){
			if(promotionLable){
				$("#itemProPrice").html('<div class="promotionIcon FontColor6"><span></span>'+promotionLable+'</div>￥'+price+'<span class="originalPrice f20 FontColor4">￥'+originalPrice+'</span>')
			}else{
				$("#itemProPrice").html('￥'+price)
			}
			var str="商品税：";
			if(isDutyOnSeller==1){
				str+="卖家承担";
			}else if(taxAmount<=50 && RealTaxType==2){
				str+='<span class="lineThrough">￥'+taxAmount+'</span><span class="FontColor4">（若订单总税额≤50元则免征）</span>'
			}else{
				str+="￥"+taxAmount;
			}
			$("#taxTip").html(str);
			$("#warehouse").html(RealTaxType);

		}

	}

};
window.SelectSKU=SelectSKU;

})(window);
$().ready(function(){
	$(window).resize(function(){
		if($("#selectSKU").length>0 && $("#selectSKU").css("display")!="none"){
			$("#MContainer").outerHeight($(window).height());
		}
	})
})