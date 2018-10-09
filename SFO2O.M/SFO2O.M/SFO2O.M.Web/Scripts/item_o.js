var sku="",pid,buyType,notEnoughFlag=false,//notEnoughFlag true 库存量不足  false 足够可提交
		initShowFlag=true,
		addToCarUrl=window.hostname+"1",
		buyNowUrl=window.hostname+"2",
		url="",//实际提交地址
		sku1Temp,sku2Temp;
$.views.helpers({
    jsonToString:function(obj){return  JSON.stringify(obj);}
})
$().ready(function(){
	checkSKU();
	$("#mySwipe ").show()
	pid=FSH.tools.request("pid");
	$("#MContainer").css("padding-bottom","50px")
	$("#productList .item").width(($("#MContainer").width()*0.95-5)/2-22);
	$(".itemFeature li").css("text-indent",$("#MContainer").width()*0.025)
	var swipeCount = $("#mySwipe .swipe-wrap li").length;
	var indexStr = '';
	for (var i = 0; i < swipeCount ; i++ ) {
		indexStr = indexStr + "<li></li>";
	};
	$("#mySwipeLiItems ul").append(indexStr).find('li:first').addClass('on');
	TouchSlide({slideCell:"#mySwipe",effect:"leftLoop",autoPlay:true,interTime:3500});
	sku1Temp = $.templates("#sku1Temp");
  sku2Temp = $.templates("#sku2Temp");
	$("#skuSelectA").click(function(){//选择sku
		selectSKUShow(1);
	})
	$("#MContainer").on("click",".addToCarBtn,.buyNowBtn",function(){
		FSH.gotoLogReg("log");
		if($(this).is(".addToCarBtn")){
			buyType="1";
			url=addToCarUrl;
		}else{
			buyType="2";
			url=buyNowUrl;
		}
		if($("#skuSelectA").css("display")!="none"){
			/*
			if(sku!="" && !$(this).is("#addToCarBtn")){
				if(!notEnoughFlag){
					submitFun(url);
				}else{
					FSH.smallPrompt("数量超出最大库存")
				}
			}else{
				selectSKUShow(2,buyType);
			}
			*/
				if($(this).is("#addToCarBtn2") || $(this).is("#buyNowBtn2")){
					//sku选择中的直接购买和加入购物车
						if(!sku){
							FSH.smallPrompt("请选择 "+$("#sku1Title").html()+" "+$("#sku2Title").html());
						}else if(notEnoughFlag){
							FSH.smallPrompt("数量超出最大库存")
						}else{
							submitFun(url);
						}

				}else{
					selectSKUShow(2,buyType);
				}
		}else{
				//没有sku属性直接提交
				submitFun(url);
	  }
	})
	$("#selectSKU").on("click","#sureBtn",function(){
		if(!$(this).hasClass("defaultBtn")){
			submitFun(url)
		}
	})
	$("#sku1").on("click"," li",function(){
		//主SKU控制子SKU
		if(!$(this).hasClass("selected") && !$(this).hasClass("disable")){
			$(this).addClass("selected").siblings("li.selected").removeClass("selected");
			var subSku=eval('(' + $(this).attr("_subAttributes") + ')');
			if(subSku.length>0){
				sku="";
				var htmlOutput2=sku2Temp.render(subSku);
				$("#sku2").html(htmlOutput2)
				if($("#sureBtn").length>0){
					$("#sureBtn").addClass("defaultBtn");
				}
			}else{
				$("#itemProPrice2,#itemProPrice").html("￥"+$(this).attr("_price"));
				sku=$(this).attr("_skuid")
				$("#num").attr("_max",$(this).attr("_forOrder"));
				if($("#sureBtn").length>0){
					$("#sureBtn").removeClass("defaultBtn");
				}
				checkKCL();
			}
		}
	});
	$("#sku2").on("click","li",function(){
		//子SKU选中
		if(!$(this).hasClass("selected") && !$(this).hasClass("disable")){
			$(this).addClass("selected").siblings("li.selected").removeClass("selected");
			$("#itemProPrice2,#itemProPrice").html("￥"+$(this).attr("_price"));
			sku=$(this).attr("_skuid");
			$("#num").attr("_max",$(this).attr("_forOrder"));
			if($("#sureBtn").length>0){
				$("#sureBtn").removeClass("defaultBtn");
			}
			checkKCL();
		}
	});
	$("#selectSKU .closeA").click(function(){
		$("#skuMark,#selectSKU").hide();
		saveSelect();
	})
	$("#selectSKU .addBtn").click(function(){
		if(!$(this).hasClass("disable")){
			var max=parseFloat($("#num").attr("_max")),
					val=parseFloat($("#num").val());
			if(val<max){
				val++;
				$("#num").val(val);
			}
			checkKCL();
			
		}
	})
	$("#selectSKU .reduceBtn").click(function(){
		if(!$(this).hasClass("disable")){
			var max=parseFloat($("#num").attr("_max")),
					val=parseFloat($("#num").val());
			if(val>1){
				val--;
				$("#num").val(val);
			}
			checkKCL();
			
		}
	})
	$("#num").on("keyup",function(){
		var max=parseFloat($(this).attr("_max")),
				val=parseFloat($(this).val());
		checkKCL();
	})
})
function selectSKUShow(type,buyType){//type 1 选择sku  2 加入购物车或者直接购买
	var skuJson=eval('(' + $("#skuSelectA").attr("data-json") + ')');
	if(initShowFlag){
		//初次加载渲染内容
		initShowFlag=false;
		$("#sku1Title").html(skuJson.MainName);
		$("#sku2Title").html(skuJson.SubName);
		var subStr=skuJson.SubAttributes;
		var htmlOutput1 = sku1Temp.render(skuJson.MainAttributes);
		$("#sku1").html(htmlOutput1);
		for(var i=0; i<skuJson.MainAttributes.length;i++){
			if(skuJson["MainAttributes"][i]["Flag"]==1){
				subStr=skuJson["MainAttributes"][i]["SubAttributes"];
				$("#num").attr("_max",skuJson["MainAttributes"][i]["ForOrder"]);
				$("#itemProPrice2,#itemProPrice").html("￥"+skuJson["MainAttributes"][i]["Price"]);
				if(skuJson["MainAttributes"][i]["Sku"]!=""){
					sku=skuJson["MainAttributes"][i]["Sku"];
				}else{
					for(var j=0;j<skuJson["MainAttributes"][i]["SubAttributes"].length;j++){
						if(skuJson["MainAttributes"][i]["SubAttributes"][j]["Flag"]==1){
							$("#num").attr("_max",skuJson["MainAttributes"][i]["SubAttributes"][j]["ForOrder"]);
							$("#itemProPrice2,#itemProPrice").html("￥"+skuJson["MainAttributes"][i]["SubAttributes"][j]["Price"]);
							sku=skuJson["MainAttributes"][i]["SubAttributes"][j]["Sku"];
						}
					}
				}
				checkKCL();
				break;
			}
		}
		if(subStr.length>0){
			var htmlOutput2 = sku2Temp.render(subStr);
			$("#sku2").html(htmlOutput2)
		}
		if(!skuJson.SubName){
			$("#sku2Title,#sku2").hide();
		}

	}
	if(type==1){
		$("#selectSKU .SKUBtnArea").html('<a class="addToCarBtn tc f28 boxflex1" id="addToCarBtn2">加入购物车</a><a class="buyNowBtn tc f28 boxflex1" id="buyNowBtn2">立即购买</a>')
	}else{
		var className="defaultBtn";
		if(sku){
			className="";
		}
		$("#selectSKU .SKUBtnArea").html('<a class="btn '+className+' tc f28 boxflex1" id="sureBtn">确定</a>');
	}
	checkKCL();
	$("#skuMark,#selectSKU").show().removeClass("hide");
}
function submitFun(url){//提交
	alert("sku="+sku)
	alert("num="+$("#num").val())
	alert("提交"+url)
}
function saveSelect(){//保存选择
	if(!sku){
		$("#skuSelectA").html($("#skuSelectA").attr("data-init"));
	}else{
		var saveStr='已选：“'+$("#sku1 li.selected").html()+'"';
		if($("#sku2 li.selected").length>0){
			saveStr+=',“'+$("#sku2 li.selected").html()+'”';
		}
		$("#skuSelectA").html(saveStr);
	}

}
function checkKCL(){
	notEnoughFlag=false;
	if(!sku){
		$(".addBtn,.reduceBtn").addClass("disable")
		if($("#sureBtn").length>0){
			$("#sureBtn").addClass("defaultBtn")
		}
		return false;
	}
	var val=parseFloat($("#num").val()),
			max=parseFloat($("#num").attr("_max"))
	if(val>max){
		notEnoughFlag=true;
		FSH.smallPrompt("数量超出最大库存")
		$(".addBtn").addClass("disable")
		$(".reduceBtn").removeClass("disable")
		if($("#sureBtn").length>0){
			$("#sureBtn").addClass("defaultBtn")
		}
	}else{ 
		if($("#sureBtn").length>0){
			$("#sureBtn").removeClass("defaultBtn")
		}
		if(val==max){
			$(".addBtn").addClass("disable")
			$(".reduceBtn").removeClass("disable")


		}else if(val<=1){
			$("#num").val(1);
			$(".reduceBtn").addClass("disable")
			$(".addBtn").removeClass("disable")
		}else{
			$(".reduceBtn").removeClass("disable")
			$(".addBtn").removeClass("disable")
		}
	}
}

function checkSKU(){
	var skustr=eval('(' + $("#skuSelectA").attr("data-json") + ')');
	if(!skustr.MainName && !skustr.MainCode){
		//没有sku属性
		$("#skuSelectA").hide()
		sku=skustr["MainAttributes"][0]["Sku"];
	}

}
