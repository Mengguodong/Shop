var deleteItemUrl=window.hostname+"/shoppingcart/deleteitems",
		changeItemNumUrl=window.hostname+"/ShoppingCart/UpdateQty",
		getSKUUrl=window.hostname+"/ShoppingCart/modifyproduct",
		submitUrl=window.hostname+"/shoppingcart/CheckOut",
		closeTipUrl=window.hostname+"/shoppingcart/ClearTip",
		selectedItemUrl=window.hostname+"/shoppingcart/SelectedItems",
		editBtn=$("#cartHeaderBtn"),
		//taxTipClose=$("#taxTipClose"),
		totalPrice=$("#totalPrice"),
		totalTariff=$("#totalTariff"),
		shoppingCartBtn=$("#shoppingCartBtn"),
		editStatus=false,
		itemel,
		depotType;//编辑商品sku属性时 记录该商品属于哪个仓库
/*解决safari浏览器下返回到购物车不刷新页面的bug*/
function isSafari() {
    if (navigator.userAgent.indexOf("Safari") > -1) {        return true;
    }    return false;
}
/*解决safari浏览器下返回到购物车不刷新页面的bug END*/
$().ready(function(){
/*解决safari浏览器下返回到购物车不刷新页面的bug*/
	if (isSafari()) {
    $(window).bind("pageshow", function (event) {
        if (event.originalEvent.persisted) {
            document.body.style.display = "none";
            window.location.reload();
        }
    });
}
/*解决safari浏览器下返回到购物车不刷新页面的bug END*/
	cart.init();
})		
var cart={
	init:function(){
			that=this;
			
			that.bindEvent()
	},
	bindEvent:function(){
		/*
		taxTipClose.click(function(){
			that.ajaxMarkShow();
			//关闭温馨提示
			FSH.Ajax({
          url: closeTipUrl,
          dataType: 'json',
          jsonp: 'callback',
          jsonpCallback: "success_jsonpCallback",
          success: function(json) {
          	if(json.Type==1){
          		$("#taxTip").hide();
          	}else{
          		FSH.commonDialog(1,[json.Content]);   
          	}

          },
          error: function(err) {
              FSH.commonDialog(1,['请求超时，请刷新页面']);   
          }
      })
		})
		*/
		$("#MContainer").off("click","div.cartCheckBoxWrap").on('click','div.cartCheckBoxWrap',function(){
			var selected,el=$(this).find(".cartCheckBox"),strs=[],str={},checkList;
			var depot=el.parents(".productBox").attr("id"),depotEl=$("#"+depot);

			//单选框
			if(el.hasClass("checked")){
				el.removeClass("checked")
				selected=0;
			}else{
				el.addClass("checked");
				selected=1;
			}
			if(editStatus){
				//编辑状态下失效商品也可选中  失效商品列表不包含.cartProList1
				checkList=".cartProList";
			}else{
				checkList=".cartProList1";
			}
			if(selected){
				//不同仓库的商品不能同时被选中
				$(".productBox:not(#"+depot+")").find(".allCheck .cartCheckBox").removeClass("checked");
				$(".productBox:not(#"+depot+")").find("ul"+checkList+" .checked").removeClass("checked");
			}
			if($(this).is(".allCheck")){
				//全选
				for(var i=0;i<depotEl.find("ul"+checkList+" li div.cartCheckBoxWrap").length;i++){
					str={"selected":selected,"sku":depotEl.find("ul"+checkList+" li div.cartCheckBoxWrap").eq(i).parents("li").find(".cartSelectedSku").attr("_selectedsku")};
					strs.push(str)
					if(selected==0){
						depotEl.find("ul"+checkList+" li div.cartCheckBoxWrap").eq(i).find(".cartCheckBox").removeClass("checked");
					}else{
						depotEl.find("ul"+checkList+" li div.cartCheckBoxWrap").eq(i).find(".cartCheckBox").addClass("checked");
					}
				}
			}else{
				//单选
				str={"selected":selected,"sku":$(this).parents("li").find(".cartSelectedSku").attr("_selectedsku")};
				strs.push(str)
				if(selected==0){
					depotEl.find(".allCheck .cartCheckBox").removeClass("checked");
				}else if(selected==1 && depotEl.find("ul"+checkList+" li div.cartCheckBoxWrap .checked ").length== depotEl.find("ul"+checkList+" li div.cartCheckBoxWrap ").length){
					depotEl.find(".allCheck .cartCheckBox").addClass("checked");
				}
			}
			FSH.Ajax({
	        url: selectedItemUrl,
	        type:'post',
	        dataType: 'json',
	        data:JSON.stringify(strs),
	        jsonp: 'callback',
	        jsonpCallback: "success_jsonpCallback",
	        success: function(json) {
	        		if(json.Type==1){
	        			 that.Calculation()
								 if(!editStatus){
									 shoppingCartBtn.html("结算("+$(".cartProList1 li b.cartCheckBox.checked").length+")");
								 }
	        		}else{
	        			FSH.commonDialog(1,[json.Content]);
	        		}
	        },
	        error: function(err) {
	            FSH.commonDialog(1,['请求超时，请刷新页面']);   
	        }
	    })
				
		})
		editBtn.click(function(){
			//编辑按钮
			if($(this).html()=="编辑"){
				//编辑状态
				for(var i=1;i<=$(".depot").length;i++){
					if($("#depot"+i).find("ul.cartProList li div.cartCheckBoxWrap .checked ").length== $("#depot"+i).find("ul.cartProList li div.cartCheckBoxWrap ").length){
						$("#allCheck"+i).find(".cartCheckBox").addClass("checked")
					}else{
						$("#allCheck"+i).find(".cartCheckBox").removeClass("checked")
					}
				}
				shoppingCartBtn.html("删除")
				editStatus=true;
				$(this).html("完成")
				$(".cartProList1,.cartProList2").addClass("editStatus");
			}else{
				for(var i=1;i<=$(".depot").length;i++){
					if($("#depot"+i).find("ul.cartProList1 li div.cartCheckBoxWrap .checked ").length== $("#depot"+i).find("ul.cartProList1 li div.cartCheckBoxWrap ").length){
						$("#allCheck"+i).find(".cartCheckBox").addClass("checked")
					}else{
						$("#allCheck"+i).find(".cartCheckBox").removeClass("checked")
					}
				}
				shoppingCartBtn.html("结算("+$(".cartProList1 li b.cartCheckBox.checked").length+")")
				editStatus=false;
				$(this).html("编辑")
				$(".cartProList1,.cartProList2").removeClass("editStatus");
			}
		})
		$("#MContainer").off("click","ul.cartProList li a").on("click","ul.cartProList li a",function(event){
			if(editStatus && !$(this).parents("ul").hasClass("disabledProList")){
				FSH.EventUtil.preventDefault(event);
				var el=FSH.EventUtil.getTarget(event);
				itemel=$(this);
				if(!$(el).hasClass("addBtn") && !$(el).hasClass("reduceBtn") && !$(el).hasClass("numTxt")){
					FSH.Ajax({
		        url: getSKUUrl,
		        dataType: 'json',
		        data:{"productcode":$(this).parents("li").attr("data-spu"),"sku":$(this).find(".cartSelectedSku").attr("_selectedsku")},
		        jsonp: 'callback',
		        jsonpCallback: "success_jsonpCallback",
		        success: function(json) {
		        		if(json.Type==1){
		        			if(!json.Data.MainName && !json.Data.MainCode){
		        			 //没有sku
		        			 return false;
		        			}
	        				itemel.find(".cartSelectedSku").attr("data-json",JSON.stringify(json.Data));
	        				depotType=itemel.parents(".depot").attr("id");
	        			 var t=new SelectSKU({
									json:eval('(' + itemel.find(".cartSelectedSku").attr("data-json") + ')'),
									type:3,
									numEL:itemel.parents("li").find("input"),
									originalSKU: itemel.find(".cartSelectedSku").attr("_selectedsku"),
								  resultEL:itemel.find(".cartSelectedSku")})
		        			
		        		}else{
		        			FSH.commonDialog(1,[json.Content]);
		        		}
		        },
		        error: function(err) {
		            FSH.commonDialog(1,['请求超时，请刷新页面']);   
		        }
		    })
					
			  }
			}
		})
		shoppingCartBtn.click(function(){
			if(editStatus){
				//删除操作
				if($(".cartProList li b.cartCheckBox.checked").length==0){
					FSH.smallPrompt("您还没有选择商品哦！")
					return false;
				}
				if($("#deleteBtn_hide").length==0){
					$("body").append('<a class="hide" id="deleteBtn_hide"></a>');
				}
				FSH.commonDialog(2,["确定要删除这个商品吗？"],"#deleteBtn_hide","cart.deleteItem");
			}else{
				//结算操作
				if($(".cartProList1 li b.cartCheckBox.checked").length==0){
					FSH.smallPrompt("您还没有选择商品哦！")
					return false;
				}
				//提交后台
					FSH.Ajax({
		        url: submitUrl,
		        dataType: 'json',
		        jsonp: 'callback',
		        jsonpCallback: "success_jsonpCallback",
		        success: function(json) {
		        	if(json.Type==1){
		        		window.location="OrderSumit.html";
		            //alert("提交后台")
		           }else if(json.Type==2){
		           	FSH.commonDialog(1,['<b class="tl  f30 lh16 show"><SPAN class="f24 FontColor3" style="font-weight:normal;">根据物流贸易规范规定，单笔订单金额不可超过￥'+json.Data+'，如订单内仅有一件商品且不可分割，订单金额可以超过￥'+json.Data+'。</SPAN></b>'],"","","知道了");
		           } else if (json.Type == 4) {
		               FSH.commonDialog(1, ['<b class="tl  f30 lh16 show"><SPAN class="f24 FontColor3" style="font-weight:normal;">根据物流贸易规范规定，单笔订单商品金额不可超过￥' +FSH.tools.formatNum(json.Data) + '。<br>健康绿氧建议您分开下单，就可以购买到所有商品啰！</SPAN></b>'], "", "", "知道了");
		           }else if (json.Type == 0) {
		               FSH.commonDialog(1, ['<b class="tl  f30 lh16 show"><SPAN class="f24 FontColor3" style="font-weight:normal;">基于不同商品的特性，商品须个别进行清关。健康绿氧建议您将1号仓和2号仓的商品分开下单，就可以购买到所有商品啰！</SPAN></b>'], "", "", "知道了");
		           }
		           else {
								FSH.commonDialog(1,[json.Content]);
		           }

		        },
		        error: function(err) {
		            FSH.commonDialog(1,['请求超时，请刷新页面']);   
		        }
		    })
				
			}
		})
		$("#MContainer").off("click","b.addBtn").on('click','b.addBtn',function(){
			//增加
			if(!$(this).hasClass("disable")){
				var el=$(this).siblings("input"),
						val=parseFloat(el.val());
				val++;
				that.ajaxMarkShow();
				that.checkNum(val,el);
			}
		})
		$("#MContainer").off("click","b.reduceBtn").on('click','b.reduceBtn',function(){
			//减少
			if(!$(this).hasClass("disable")){
				var el=$(this).siblings("input"),
						val=parseFloat(el.val());
				val--;
				if(val<=0){
					val=1;
					el.val(1);
				}
				that.ajaxMarkShow();
				that.checkNum(val,el);
			}
		})
		$("#MContainer").off("change","input.numTxt").on('change','input.numTxt',function(){
			if(!FSH.string.Trim($(this).val()) || $(this).val()<=0){
					$(this).val(1);
			}
			that.ajaxMarkShow();
			that.checkNum($(this).val(),$(this));
		})
	},
	ajaxMarkShow:function(){
		//显示异步请求时遮罩层
		if(!$("#ajaxMark").length>0){
			$("body").append('<div class="ajaxMark tc" id="ajaxMark" style="height:500px; display:block"><img src="../Content/images/loading.png?V=201601071111" class="animationLoading"></div>');
		}
		$("#ajaxMark").height($(window).height()).css("line-height",$(window).height()+"px").show();
	},
	checkNum:function(num,inputEl){
		FSH.Ajax({
        url: changeItemNumUrl,
        dataType: 'json',
        data:{"sku":inputEl.parents("li").find(".cartSelectedSku").attr("_selectedsku"),"qty":num},
        jsonp: 'callback',
        jsonpCallback: "success_jsonpCallback",
        success: function(json) {
        	if(json.Type==1){
            inputEl.val(num);
			      inputEl.parents("li").find(".proNum").html("×"+num);
			      that.Calculation();
			      if(num==1){
							inputEl.siblings(".reduceBtn").addClass("disable");
			      }else{
			      	inputEl.siblings(".reduceBtn,.addBtn").removeClass("disable");
			      }
           }else{
           	inputEl.val(json.Data);
           	inputEl.parents("li").find(".proNum").html("×"+json.Data);
           	that.Calculation();
           	inputEl.siblings(".addBtn").addClass("disable");
						FSH.commonDialog(1,[json.Content]);
           }

        },
        error: function(err) {
            FSH.commonDialog(1,['请求超时，请刷新页面']);   
        }
    })
	},
	Calculation:function(){
		var totalPrice=totalTariff=thisPrice=thisTariff=0
		for(var i=0;i<$(".cartProList1 li b.cartCheckBox.checked").length;i++){
			thisPrice=FSH.tools.accMul(parseFloat($(".cartProList1 li b.cartCheckBox.checked").eq(i).parents("li").find(".proPrice").attr("data-price")),parseInt($(".cartProList1 li b.cartCheckBox.checked").eq(i).parents("li").find(".proNum").html().substring(1)));
			thisTariff=FSH.tools.accMul(parseFloat($(".cartProList1 li b.cartCheckBox.checked").eq(i).parents("li").find(".proTax").attr("data-price")),parseInt($(".cartProList1 li b.cartCheckBox.checked").eq(i).parents("li").find(".proNum").html().substring(1)));
			totalPrice=FSH.tools.accAdd(totalPrice,thisPrice);
			//if($(".cartProList1 li b.cartCheckBox.checked").eq(i).parents("li").attr("data-isdutyonseller")!=1){
			totalTariff=FSH.tools.accAdd(totalTariff,thisTariff);
			//}
		}
		$("#totalPrice").html("￥"+toNumber(totalPrice));
		$("#totalTariff").html("￥"+toNumber(totalTariff));
	},
	deleteItem:function(){
		var skus=[];
		for(var i=0; i<$(".cartProList li b.cartCheckBox.checked").length;i++){
			skus.push($(".cartProList li b.cartCheckBox.checked").eq(i).parents("li").find(".cartSelectedSku").attr("_selectedsku"))
		}	
		var skusParam=$.param({"skus":skus}, true);
		FSH.Ajax({
        url: deleteItemUrl,
        dataType: 'json',
        type:'post',
        data: skusParam,
        jsonp: 'callback',
        jsonpCallback: "success_jsonpCallback",
        success: function(json) {
        		for(var i=0; i<$(".cartProList li b.cartCheckBox.checked").length;i++){
							$(".cartProList li b.cartCheckBox.checked").eq(i).parents("li").addClass("deleteItem");
						}
						$(".cartProList li.deleteItem").remove();
						
						//判断仓库中是否还有商品
						if($("#depot1").find("li").length==0){
							$("#depot1").hide();
						}
						if($("#depot2").find("li").length==0){
							$("#depot2").hide();
						}
						var len=$(".cartProList li").length;
						if(len==0){
							$("#MContainer header b").html("购物车");
							$("#emptyDiv").show();
							$("#taxTip, #cartHeaderBtn,.fixedBottomDiv").hide();
						}else{
							$("#MContainer header b").html("购物车(<span>"+len+"</span>)")
						}
						cart.Calculation();
        },
        error: function(err) {
            FSH.commonDialog(1,['请求超时，请刷新页面']);   
        }
    })
		
	}
}
function toNumber(num){
	var t;
	t=FSH.tools.toDecimal2(num.toString().replace(/,/g, ""));
  t=FSH.tools.formatNum(t);
  return t;
}