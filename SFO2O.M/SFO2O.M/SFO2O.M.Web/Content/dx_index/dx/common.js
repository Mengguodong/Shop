/*禁止页面被iframe嵌套*/
if(window.location.href.indexOf("login")==-1 && top != self){
    top.location = self.location;
}
/**
 * 公共类FSH 适用于M站
 */
var smallPromptTimer,/*弱提示时间戳*/
    ajaxFlag=true,/*异步请求标志位*/
    nextPageFlag=true,/*滚动加载是否由下一页*/
    pageIndex=1,/*初始页码*/
    pageSize=10;/*一页10行*/

if(!window.hostname){
	//设置域名
	window.hostname=null;
	if(window.location.hostname=="www.wdnzmt9.net"){
    window.hostname="http://www.wdnzmt9.net";   
	}else if(window.location.hostname=="www.wdnzmt9.n1et"){
	    window.hostname="http://www.wdnzmt9.n1et";   
	} else if (window.location.host =="www.wdnzmt9.c1om") {
	    window.hostname = "http://www.wdnzmt9.c1om";
	}
	else if (window.location.host == "14l8185x70.51mypc.cn") {
	    window.hostname = http://14l8185x70.51mypc.cn";
	}
	else if (window.location.host == "www.wdnzmt9.cn") {
	    window.hostname = "http://www.wdnzmt9.cn";
	}
	else {
		 window.hostname="http://www.wdnzmt9.com";// 正式环境
	}
}
FSH={
	//固定页脚位置
	fixedFooter:function(){
		if(!$("#footerWrap").length>0){
			return false;
		}

		var MContainerHeight;

		if ( $("#footerWrap").hasClass('fixedFooter') ) {
			MContainerHeight = $(".MContainer:visible").outerHeight()+$("#footerWrap").outerHeight();
		}else{
			MContainerHeight = $(".MContainer:visible").outerHeight();
		}
		if(MContainerHeight<=$(window).height()){
			$("#footerWrap").addClass("fixedFooter").css("margin-left",-$("#footerWrap").width()/2);
		}else{
			$("#footerWrap").removeClass("fixedFooter").css("margin-left",0);
		}	
	},
	/*判断登陆状态*/
	checkLogin: function() {
		var loginFlag = FSH.tools.getCookie("sfo2o_user");
		if (loginFlag == undefined || loginFlag <= 0 || loginFlag == "") {
			return -1;
		} else {
			return 1;
		}
	},
	/*无登录状态跳转登录页面*/
	gotoLogReg: function(flag) {
		if (FSH.checkLogin() == -1) {
			if(flag=="log"){
				window.location.href = window.hostname + "/account/login?return_url=" + escape(window.location.href);
			}else{
				window.location.href = window.hostname + "/account/register?return_url=" + escape(window.location.href);
			}
		}
	},
	/*
	* 返回顶部
	*/
	goTop:function(){
		 $("html,body").animate({ "scrollTop": 0 }, 200);
	},
	/*
	 *	添加圆形回到顶部按钮 
	 */
	addGoTop:function(){
		$(document).on("scroll.gotop", window, function () {
		// $(window).on('scroll',function(){
		// $(document).on("scroll", function () {
			if ($("#goTop").length > 0) {
		        var windowHeight = $(window).height();
		        var bodyHeight = $(document).height();
		        var footerHeight = $("footer").length == 0 ? 0:$('.m-footer').height(); // 判断有没有底部
		        var loginAreaHeight = $(".loginArea").height();
		        var nHeight = bodyHeight - windowHeight - footerHeight - loginAreaHeight; 
		       
		        if ($(window).scrollTop() >= 80) {
		            $("#goTop").fadeIn();

					if ( $(".fixedBottomDiv").length > 0 ) {
						$("#goTop").css("bottom","129px");
					};
		        }
		        else {
		            $("#goTop").fadeOut();
		        }
		    }else{
		    	$('body').append( '<div class="m-goTop" id="goTop"><a href="javascript:FSH.goTop()" class="ui-link"><img src="'+ window.hostname +'/Content/Images/to_top.png"></a></div>');
		    }
		})
	},
	/**
	 * 公共弹窗（依赖于jquery.dialog.js）
	 * @type 弹窗类型 1 错误提示弹窗 仅有一个确定按钮和一句提示文案 确定即关闭弹窗 2 确定 取消两个按钮的弹窗 
	 * @contentArray  弹窗的提示文案，是数组，第一个元素是指标题类的大字，第二个元素是指提示类的小字
	 * @el   如果type为2时需传入触发弹窗事件的按钮的类名或id
	 * @confirmFun  当type为2时确定按钮的回调事件
	 * @sureBtnText 可自定义确定按钮的文字
	 */
	commonDialog:function(type,contentArray,el,confirmFun,sureBtnText){
		var  elname=el;
		var titleContent = contentArray[0]?contentArray[0]:'';
		var tipContent = contentArray[1]?contentArray[1]:'';
		if(type==1){
			if(!$("#errorTipBtn").length>0){
				$("body").append('<a id="errorTipBtn"></a>');
			}
			el="#errorTipBtn";
			elname="errorTip";
		}else{
			elname=el.substring(1);
		}
		if($("#"+elname+"dialog").length>0){
			//if(type==1){
				//$("#"+elname+"dialog").find(".title").html(titleContent).end().find('.tip').html(tipContent);
			//}
			//$(el).popShow({targetEl:"#"+elname+"dialog"});	
			$("#"+elname+"dialog").remove();	
		}
		var dialogHtml='<div class="sf-pop" id="'+elname+'dialog"><p class="content title">'+titleContent+'</p><p class="content tip">'+tipContent+'</p>';
			if(type==1){
				if (sureBtnText == undefined || sureBtnText == ''){
					sureBtnText="确定";
				}
				if (confirmFun == undefined || confirmFun == ''){
					dialogHtml+='<p class="btn-box"><a class="closeBtn m-btn hover" style="width:30%; white-space:nowrap;" >'+sureBtnText+'</a></p>'
				}else{
					dialogHtml+='<p class="btn-box"><a class="closeBtn m-btn hover" style="width:30%; white-space:nowrap;" onclick="FSH.commonDialogConfirmFun('+confirmFun+')">'+sureBtnText+'</a></p>'
				}
				
			}else{
				if (sureBtnText == undefined || sureBtnText == '') {
					dialogHtml+='<p class="btn-box"><a class="closeBtn m-btn">取消</a><a class="closeBtn m-btn hover" onclick="FSH.commonDialogConfirmFun('+confirmFun+')">确定</a></p>';
				}else{
					dialogHtml+='<p class="btn-box"><a class="closeBtn m-btn">取消</a><a class="closeBtn m-btn hover" onclick="FSH.commonDialogConfirmFun('+confirmFun+')">'+ sureBtnText +'</a></p>'
				}
			}
			dialogHtml+='</div>';
		$(el).popShow({targetEl:"#"+elname+"dialog",content:dialogHtml});		
		
	},
	/**
	 * 公共弹窗中确定按钮回调事件
	 */
	commonDialogConfirmFun:function(fun){
		if(fun){
			fun()
		};
	},
	/**
	 * 弱提示
	 */
	smallPrompt:function(txt){
		if(!$("#smallPrompt").length>0){
			var html='<div class="smallPrompt" id="smallPrompt" style="height:'+FSH.tools.getClientHeight()+'px">\
								<p class="smallPromptTxt f36">'+txt+'</p>\
							</div>';
			$("body").append(html);
		}else{
			$("#smallPrompt .smallPromptTxt").html(txt);
			$("#smallPrompt").height($(window).height());
		}
		$("#smallPrompt").show();
		$("#smallPrompt .smallPromptTxt").css("left",($(window).width()-$("#smallPrompt .smallPromptTxt").outerWidth())/2+"px")
		
		smallPromptTimer=setTimeout(function(){
			$("#smallPrompt").hide();
		},3000)
		$("body").on("click","#smallPrompt",function(){
			$("#smallPrompt").hide();
			clearTimeout(smallPromptTimer);
		})
		$(window).resize(function(){
			$("#smallPrompt").height(FSH.tools.getClientHeight());
		})
	},
	menuDialog: function() {
		var diaMenuHtml = '<div class="classify_show" id="menuCon">\
												<ul class="clearfix"><a target="_self" href="' + window.hostname + '"><li item="nohide"><div class="pic_box"><img src="' + window.hostname + '/Content/Images/second_nav_03.png?v=201507091514"></div><p>首页</p></li></a>\
												<a target="_self" href="' + window.hostname + '/"><li item="nohide"><div class="pic_box"><img src="' + window.hostname + '/Content/Images/second_nav_06.png?v=201507091514"></div><p>商品列表</p></li></a>\
												<a target="_self" href="' + window.hostname + '/ShoppingCart"><li item="nohide"><div class="pic_box"><img src="' + window.hostname + '/Content/Images/second_nav_08.png?v=201507091514"></div><p>购物车</p></li></a>\
												<a target="_self"  href="' + window.hostname + '/my"><li item="nohide"><div class="pic_box"><img src="' + window.hostname + '/Content/Images/second_nav_10.png?v=201507091514"></div><p>我的</p></li></a>\
												</ul>\
												</div>';
		$("#m-menu").popShow({targetEl:"#menuCon",content:diaMenuHtml,openAnimationName:"animationUp",closeAnimationName:"hide"})	
	},
	renderList:function (data,type){ //商品列表 // type为1时

		var discountHtml='';
		if (data.DiscountRate > 0 && data.DiscountRate !=10) {
			discountHtml = '<span class="account">'+ data.DiscountRate +'折</span>';
		}

		var saleOutHtml = '';
		if (data.Qty <= 0) {
			saleOutHtml = '<div class="saleOut"><span>已售罄</span></div>';
		}

		var s = '<div class="item pr">\
					<a href="'+ window.hostname +'/item.html?productCode='+ data.SPU +'">\
						<div class="imgBox"><img class="lazyloadImg" data-original="' + data.ImagePath + '" src="../Content/images/blank.png">' + discountHtml + saleOutHtml + '</div>\
						<div class="title">'+ data.Name +'</div>';
		var a = '';


		if (data.DiscountPrice<data.MinPrice) {
			a = '<div class="price">￥'+ FSH.tools.showPrice(data.DiscountPrice) +'<span class="f20 FontColor4 ml5 original">'+ FSH.tools.showPrice(data.MinPrice) +'</span></div>';
		}else{
			a = '<div class="price">￥'+ FSH.tools.showPrice(data.MinPrice)+'</div>';
		}			
						
		var b ='				<span class="freeSign"></span>\
					</a>'

		var n = '';
		if (data.Qty > 0) {
			if ( data.IsHolidayGoods != 1 ) {
				if (type != 1) {
					if (data.SkuCount > 1) {
				        n = '<a class="addCart" onclick="AddToCartBySpu(\'' + data.SPU + '\',this)"></a>';
						//n = '<a class="addCart" href="'+ window.hostname +'/item.html?productCode='+ data.SPU +'"></a>';
					}else{
						n = '<span class="addCart" onclick="FSH.addToCart(\''+ data.SkuList[0] +'\',this)"></span>'; 
						//调一下加入购物车的异步请求
					}
				}else{
					n = '<span class="addCart" onclick="soAddToCart('+ data.SPU +',this)"></span>';
				}
			}
			
		}
						
		var x ='</div>';
		return s+a+b+n+x;
	},
	// 飞入购物车
	// @targetId 指目标元素的id
	// @targetIdPos 指飞向的购物车的位置对象{left:x,top:y}
	// @startPos 指开始位置对象{left:x,top:y}
	// ImgEl指飞行的图片元素
	// @number 是指后台返回的购物车数量
	fly: function(targetId,targetIdPos,startPos,ImgElSrc,number){
		var flyer = $('<img class="u-flyer" src="'+ImgElSrc+'">');
		flyer.fly({
			start: {
				left : startPos.left,
				top : startPos.top
			},
			end: {
				// left: offset.left+10,
				// top: offset.top+10,
				left: targetIdPos.left + 10,
				top: targetIdPos.top + 10,
				width: 0,
				height: 0
			},
			onEnd: function(){

				// if ( $("#"+targetId).find('span').length > 0 ) {
				// 	$("#"+targetId).find('span').addClass('scale2').text( number );
				// }else{
				// 	$('<span>'+ number+'</span>').appendTo("#"+targetId).addClass('scale2');
				// }
				$("#"+targetId).find('span').removeClass('hide').show().text(number).addClass('scale2');
				this.destory();

				
				window.setTimeout(function(){
					$("#"+targetId).find('span').removeClass('scale2');
				},800)
			}
		});
	},
	addToCartSuccess:function(){
		if ( $("#successInfo").length > 0 ) {
			$("#successInfo").fadeIn().delay(800).fadeOut();
		}else{
			var successInfo = '<div class="successInfo" id="successInfo">\
									<div class="success"></div>\
									<p>\
										恭喜您，商品已添加<br>至购物车\
									</p>\
								</div>';
			$(successInfo).appendTo("#MContainer").fadeIn().delay(800).fadeOut();
		}
	},
	// 加入购物车
	addToCart:function(sku,obj ){ 
		var _this = $(obj);
		FSH.Ajax({
			url:window.hostname + '/ShoppingCart/AddItem',
			type:'post',
			data:{ sku:sku,qty:1},
			success:function( msg ){
				if (msg.Type == 1) {
					FSH.addToCartSuccess();
					var startPos={};
					startPos.left = _this.offset().left;
					startPos.top = _this.offset().top - $(document).scrollTop();
					var ImgElSrc = _this.parent().find('img').attr('src');
					var targetPos = $("#headerCart").position();
					FSH.fly('headerCart',targetPos,startPos,ImgElSrc,msg.Data);
				}else{
					FSH.commonDialog(1,[msg.Content]);
				}
			},
			error:function(){
				FSH.commonDialog(1,[msg.Content]);
			}
		})
	},
	productListAddToCartSucFun:function(cartnum,_this){
		FSH.addToCartSuccess();
		var startPos=_this.offset();
		startPos.top = _this.offset().top - $(document).scrollTop();
		var ImgElSrc =_this.parent().find('img').attr('src');
		var targetPos = $("#headerCart").offset();
		targetPos.top = targetPos.top - $(document).scrollTop();
		FSH.fly('headerCart',targetPos,startPos,ImgElSrc,cartnum);
	},
	collectionFun:function(productCode,collectionStatus,fun,el){
		//收藏功能  
		//productCode:spu  
		//collectionStatus收藏当前的状态 true为已收藏准备取消收藏 false为未收藏准备收藏  
		//fun成功后回调函数
		// el是在同个页面内有多个收藏按钮时用到的
		//var json={"Type":1};
		 if(ajaxFlag){
        ajaxFlag=false;
        FSH.Ajax({
	        url: window.hostname+"/Favorite/isFavorite",
	        dataType: 'json',
	        data:{"productCode":productCode,"collectionStatus":collectionStatus},
	        jsonp: 'callback',
	        jsonpCallback: "success_jsonpCallback",
	        success: function(json) {
	        		if(json.Type==1){
	        			fun(collectionStatus,el);
	        		}else{
	        			FSH.commonDialog(1,[json.Content]);
	        		}
	        },
	        error: function(err) {
	            FSH.commonDialog(1,['请求超时，请刷新页面']);   
	        }
	    }) 
    }else{
        FSH.smallPrompt("请勿重复提交")
    }
	}


}

FSH.string={
	/**
	 * 去除空格
	 */
	Trim:function(str){
		var pattern1 = new RegExp("^[\\s]+", "gi");
		var pattern2 = new RegExp("[\\s]+$", "gi");
		var t=str.replace(pattern1, "");
		var t2=t.replace(pattern2, "");
		return t2;
	},
	/**
	 * 自动添加...
	 */
	getByteLen:function(str,maxNum){
		var len = 0;
		var newStr="";
		for (var i = 0; i < str.length; i++) {
					if(str.sub(i,i+1)!=undefined){
						if (str.substring(i,i+1).match(/[^\x00-\xff]/ig) != null){ //全角
							len =len+2;
						}else{
							len =len+1;
						}
						if(len<=maxNum){
							newStr=newStr+str.substring(i,i+1);	
						}else{
							newStr=	newStr+"...";
							break;
						}
					}
				}
			return newStr;	
	}

}
/**
	*事件处理
	*getEvent(event)返回对event对象的引用
	*getTarget(event)返回事件的目标
	*preventDefault(event)取消事件的默认行为
	*stopPropagation(event)阻止事件冒泡
 */
FSH.EventUtil={
	getEvent:function(event){
		return event ? event:window.event;	
	},
	getTarget:function(event){
		var ev=this.getEvent(event);
		return ev.target || ev.srcElement;	
	},
	preventDefault:function(event){
		var ev=this.getEvent(event);
		if(ev.preventDefault){
			ev.preventDefault();	
		}else{
			ev.returnValue=false;	
		}
	},
	stopPropagation:function(event){
		var ev=this.getEvent(event);
		if(ev.stopPropagation){
			ev.stopPropagation();	
		}else{
			ev.cancelBubbles=true;	
		}	
	}
}
/*
 * M站用到的公共方法
 */
FSH.tools={
	getRef:function (){
			//获取来源页面
		 var ref = '';  
		 if (document.referrer.length > 0) {  
		  ref = document.referrer;  
		  return ref;
		 }  
		 try {  
		  if (ref.length == 0 && opener.location.href.length > 0) {  
		   ref = opener.location.href;  
		   return ref;
		  }  
		 } catch (e) {
		 } 
	},
	request:function(paras){ /*获取参数*/
	    var url = location.href; 
	    var paraString = url.substring(url.indexOf("?")+1,url.length).split("&"); 
	    var paraObj = {} 
	    for (i=0; j=paraString[i]; i++){ 
	    	paraObj[j.substring(0,j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=")+1,j.length); 
	    } 
	    var returnValue = paraObj[paras.toLowerCase()]; 
	    if(typeof(returnValue)=="undefined"){ 
	    	return ""; 
	    }else{ 
	    	return returnValue; 
	    } 
	},
	generateRandomMixed:function(n) {//随机数 返回n位随机数
	    var random_chars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];

	    var res = "";
	    for (var i = 0; i < n ; i++) {
	        var id = Math.ceil(Math.random() * 35);
	        res += random_chars[id];
	    }
	    return res;
	},
	getCookie:function(name) {//取cookie
	    if (document.cookie.length > 0) {
	        var start = document.cookie.indexOf(name + "=");
	        if (start != -1) {
	            start = start + name.length + 1;
	            var end = document.cookie.indexOf(";", start);
	            if (end == -1) end = document.cookie.length;
	            return unescape(document.cookie.substring(start, end));
	        }
	    }
	    return "";
	},
	setCookie:function(name, value, expiredays) {//设置cookie
	    var exdate = new Date();
	    exdate.setDate(exdate.getDate() + expiredays);
	    document.cookie = name + "=" + escape(value) +
	        ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString()) + ";domain=.sf-o2o.com";
	},
	isPhone:function (phone) { // 支持手机号、中华人民共和国大陆地区号的
	   
	     var reg1= /^((13|15|17|18|14)\d{9})$/;// 支持手机号
	     var reg2 = /^((5|6|9)\d{7})$/;// 中华人民共和国大陆地区
	     phone=FSH.string.Trim(phone);
	   // var reg1 = /^((13|15|17|18|14)\d{9})$||^((5|6|9)\d{7})$/
	   
	    if (reg1.test(phone) || reg2.test(phone)) {
	    	return true;
	    }else{
	    	return false;
	    }
	},
	isPassword: function (pwd) {
			pwd=FSH.string.Trim(pwd);
      var number = /^[0-9]{6,32}$/,
          letter = /^[a-zA-Z]{6,32}$/,
          sign = /^[@!#\$%&'\*\+\-\/=\?\^_`{\|}~]{6,32}$/,
          china = /[\u4E00-\u9FA5]/;
      if(!pwd){
      	return false;
      }
      if(pwd.length<6 || pwd.length>32){
      	return false;
      }
      if (number.test(pwd) || letter.test(pwd) || sign.test(pwd) || china.test(pwd)) {
          return false;
      }
      return true;
  },
  isEmail:function(str){
  	str=FSH.string.Trim(str);
  	if(!str){
			return false;
		}  
		if(!(/^([0-9a-zA-Z-._]+)@(([0-9a-zA-Z-]+\.)+[a-zA-Z]{2,4})$/).test(str))
		{
			return false;
		} 
		return true;
  },
	checkIsNumber:function(str) {//验证是不是数字
	    var partrn = /^[0-9]+$/;
	    return partrn.test(str);
	},
 	isExitsFunction:function(funcName) {//是否存在指定函数 
    try {
        if (typeof(eval(funcName)) == "function") {
            return true;
        }
    } catch(e) {}
    return false;
	},
	isExitsVariable:function (variableName) {//是否存在指定变量 
    try {
        if (typeof(variableName) == "undefined") {
            //alert("value is undefined"); 
            return false;
        } else {
            //alert("value is true"); 
            return true;
        }
    } catch(e) {}
    return false;
	},
	toDecimal2:function(x){//保留两位小数，如：2，会在2后面补上00.即2.00 
		var f = parseFloat(x);
    if (isNaN(f)) {
        return 0;
    }
    var f = Math.round(x * 100) / 100;
    var s = f.toString();
    var rs = s.indexOf('.');
    if (rs < 0) {
        rs = s.length;
        s += '.';
    }
    while (s.length <= rs + 2) {
        s += '0';
    }
    return parseFloat(s).toFixed(2);
	},
	isEmptyObject:function (obj){
	    for(var n in obj){return false} 
	    return true; 
	},
	showPrice:function(number){ // 如果是整数直接显示整数，如果是小数则四舍五入显示小数点后两位
		if (number == parseInt(number)) {
			return number;
		}else{
			return parseFloat(number).toFixed(2);
		}
	},
	/*获取滚动条当前的位置 */
    getScrollTop:function(id) {
        var scrollTop = 0;
        if(!!id){
            scrollTop =  $("#" + id)[0].scrollTop
        }else{
            if (document.documentElement && document.documentElement.scrollTop) {
                scrollTop = document.documentElement.scrollTop;
            }
            else if (document.body) {
                scrollTop = document.body.scrollTop;
            }
        }

        return scrollTop;
    },

    /*获取当前可是范围的高度 */
    getClientHeight: function(id) {
        var clientHeight = 0;
        if(!!id){
            clientHeight =  $("#" + id)[0].clientHeight
        }else {
            if (document.body.clientHeight && document.documentElement.clientHeight) {
                clientHeight = Math.min(document.body.clientHeight, document.documentElement.clientHeight);

            }
            else {
                clientHeight = Math.max(document.body.clientHeight, document.documentElement.clientHeight);
            }
            clientHeight=Math.max(clientHeight,window.innerHeight)
        }
        return clientHeight;
    },
    /*获取文档完整的高度 */
    getScrollHeight:function(id) {
        var scrollHeight = 0;
        if(!!id){
            scrollHeight =  $("#" + id)[0].scrollHeight
        }else {
            scrollHeight = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
        }
        return scrollHeight;
    },
    /*浮点数加法 js原生态加法有bug*/
    accAdd:function (arg1, arg2) {
	    var r1, r2, m, c;
	    try {
	         r1 = arg1.toString().split(".")[1].length;
	     }
	     catch (e) {
	         r1 = 0;
	     }
	     try {
	        r2 = arg2.toString().split(".")[1].length;
	     }
	     catch (e) {
	         r2 = 0;
	     }
	     c = Math.abs(r1 - r2);
	     m = Math.pow(10, Math.max(r1, r2));
	     if (c > 0) {
	         var cm = Math.pow(10, c);
	         if (r1 > r2) {
	             arg1 = Number(arg1.toString().replace(".", ""));
	             arg2 = Number(arg2.toString().replace(".", "")) * cm;
	         } else {
	             arg1 = Number(arg1.toString().replace(".", "")) * cm;
	             arg2 = Number(arg2.toString().replace(".", ""));
	         }
	     } else {
	         arg1 = Number(arg1.toString().replace(".", ""));
	         arg2 = Number(arg2.toString().replace(".", ""));
	     }
	     return (arg1 + arg2) / m;
	 },
	 accSub:function(arg1, arg2) { /* 浮点数减法 */
	    var r1, r2, m, n;
	    try {
	        r1 = arg1.toString().split(".")[1].length;
	    }
	    catch (e) {
	        r1 = 0;
	    }
	    try {
	        r2 = arg2.toString().split(".")[1].length;
	    }
	    catch (e) {
	        r2 = 0;
	    }
	    m = Math.pow(10, Math.max(r1, r2)); //last modify by deeka //动态控制精度长度
	    n = (r1 >= r2) ? r1 : r2;
	    return ((arg1 * m - arg2 * m) / m).toFixed(n);
	},
	 IDCardCheck:function (sIdCard) {
	    //sIdCard = sIdCard.toUpperCase();
	   //Wi 加权因子 Xi 余数0~10对应的校验码 Pi省份代码  
		  var Wi=[7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2],  
		  Xi=[1,0,"X",9,8,7,6,5,4,3,2],  
		  Pi=[11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91]; 
			sIdCard=sIdCard.replace(/^\s+|\s+$/g,"");//去除字符串的前后空格，允许用户不小心输入前后空格  
		  if (sIdCard.match(/^\d{14,17}(\d|X)$/gi)==null) {//判断是否全为18或15位数字，最后一位可以是大小写字母X  
		     // alert("身份证号码须为18位或15位数字");
		     return false;    
		  }
		  //检验输入的省份编码是否有效  
		  var p2=sIdCard.substr(0,2),
		  		provinceFlag=false;
		  for (var i = 0; i < Pi.length; i++) {  
		      if(Pi[i]==p2){  
		          provinceFlag=true;  
		      }
		  }
		  if(!provinceFlag){
		  	return false;
		  }
		  if (sIdCard.length==18) {  
		  		//检验18位身份证号码出生日期是否有效  
		      //parseFloat过滤前导零，年份必需大于等于1900且小于等于当前年份，用Date()对象判断日期是否有效。
		      var brithday18Flag=false;  
		      var year=parseFloat(sIdCard.substr(6,4));  
		      var month=parseFloat(sIdCard.substr(10,2));  
		      var day=parseFloat(sIdCard.substr(12,2));  
		      var checkDay=new Date(year,month-1,day);  
		      var nowDay=new Date();  
		      if (1900<=year && year<=nowDay.getFullYear() && month==(checkDay.getMonth()+1) && day==checkDay.getDate()) {  
		          brithday18Flag=true;
		      }
		      if(!brithday18Flag){
		      	return false;
		      }
		      //检验校验码是否有效  
		      var aIdCard=sIdCard.split("");  
		      var sum=0;  
		      for (var i = 0; i <Wi.length; i++) {  
		          sum+=Wi[i]*aIdCard[i]; //线性加权求和  
		      };  
		      var index=sum%11;//求模，可能为0~10,可求对应的校验码是否于身份证的校验码匹配  
		      if (Xi[index]==aIdCard[17].toUpperCase()) {  
		          return true; 
		      }else{
		      	return false;
		      }
		  }  
		  if (sIdCard.length==15) {  
		  	//检验15位身份证号码出生日期是否有效  
		    var year=parseFloat(sIdCard.substr(6,2));  
		    var month=parseFloat(sIdCard.substr(8,2));  
		    var day=parseFloat(sIdCard.substr(10,2));  
		    var checkDay=new Date(year,month-1,day);  
		    if (month==(checkDay.getMonth()+1) && day==checkDay.getDate()) {  
		        return true;  
		    }else{
		    	return false;
		    } 
		  }
	},
	isZipCode:function(num){
		var reg =  /^[0-9]{6}$/;
		return reg.test(num+'');
	},
	 /*浮点数乘法*/
	 accMul:function (arg1, arg2) {
	    var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
	    try {
	        m += s1.split(".")[1].length;
	    }
	    catch (e) {
	    }
	    try {
	        m += s2.split(".")[1].length;
	    }
	    catch (e) {
	    }
	    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
	},
	/*数字格式化（加逗号）*/
	formatNum:function (str){
		str=str.toString();
		var newStr = "";
		var count = 0;
		if(str.indexOf(".")==-1){
		   for(var i=str.length-1;i>=0;i--){
				 if(count % 3 == 0 && count != 0){
				   newStr = str.charAt(i) + "," + newStr;
				 }else{
				   newStr = str.charAt(i) + newStr;
				 }
				 count++;
		   }
		   str = newStr; //自动补小数点后两位
		}
		else{
		   for(var i = str.indexOf(".")-1;i>=0;i--){
				 if(count % 3 == 0 && count != 0){
				   newStr = str.charAt(i) + "," + newStr;
				 }else{
				   newStr = str.charAt(i) + newStr; //逐个字符相接起来
				 }
		 		count++;
			}
		   str = newStr + (str + "00").substr((str + "00").indexOf("."),3);
		   
		}
		return str;
	},
	isInclude:function (name){
	    var js= /js$/i.test(name);
	    var es=document.getElementsByTagName(js?'script':'link');
	    for(var i=0;i<es.length;i++) 
	    if(es[i][js?'src':'href'].indexOf(name)!=-1)return true;
	    return false;
	},
	addLoadEvent:function (func) {
		var oldonload = window.onload;
		if (typeof window.onload != 'function') {
			window.onload = func;
		} else {
			window.onload = function() {
				oldonload();
				func();
			}
		}
	} 
}
/*设置图片尺寸*/
FSH.setImgSize={
	setBannerSize:function(el,height){
		el.css({'width':$("#MContainer").width(),'height':$("#MContainer").width()*height/640});
	}
}
FSH.share={
	//判断是不是在微信中打开
	is_weixin:function(){
	    var ua = navigator.userAgent.toLowerCase();
	    if(ua.match(/MicroMessenger/i)=="micromessenger") {
	        return true;
	    } else {
	        return false;
	    }
	},
	//微信平台下的分享
	getWXConfig:function(type){
		var a=window.location.href.indexOf("#")==-1?window.location.href.length:window.location.href.indexOf("#"),
		    shareHref=encodeURI(window.location.href.substring(0,a));
		FSH.Ajax({
					url: window.hostname+"/Shared/GetSharedInfo",
	        type:"get",
	        data:{"type":type,"URL":shareHref},
	        dataType: 'json',
	        jsonp: 'callback',
	        jsonpCallback: "success_jsonpCallback",
	        success: function (json) {
	        	if (json.type == 1) {
	        		if (json.data.appId) {
	        			FSH.share.setWX(json.data.appId,json.data.timestamp,json.data.nonceStr,json.data.signature,json.data.Title,json.data.ImagePath,json.data.Description,decodeURI(shareHref));
	        			
	        		}

	        	}
	        },
	        error:function(json){
	        	//FSH.commonDialog(1,[json.Content]);
	        }
		})
	},
	setWX:function(appId,timestamp,nonceStr,signature,title,img,desc,src){
		wx.config({
		    debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
		    appId: appId, // 必填，公众号的唯一标识
		    timestamp: timestamp, // 必填，生成签名的时间戳
		    nonceStr: nonceStr, // 必填，生成签名的随机串
		    signature: signature,// 必填，签名，见附录1
		    jsApiList: ['onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
		});
		wx.ready(function(){
			//分享到朋友圈
			wx.onMenuShareTimeline({
			    title: title, // 分享标题
			    link: src, // 分享链接
			    imgUrl: img, // 分享图标
			    trigger: function (res) {

			    },
			    success: function () { 
			        // 用户确认分享后执行的回调函数
			    },
			    cancel: function () { 
			        // 用户取消分享后执行的回调函数
			    }
			});
			//分享给朋友
			wx.onMenuShareAppMessage({
			    title: title, // 分享标题
			    desc: desc, // 分享描述
			    link: src, // 分享链接
			    imgUrl: img, // 分享图标
			    trigger: function (res) {

			    },
			    success: function () { 
			        // 用户确认分享后执行的回调函数
			    },
			    cancel: function () { 
			        // 用户取消分享后执行的回调函数
			    }
			});
			//分享到QQ
			wx.onMenuShareQQ({
			    title: title, // 分享标题
			    desc: desc, // 分享描述
			    link: src, // 分享链接
			    imgUrl: img, // 分享图标
			    success: function () { 
			       // 用户确认分享后执行的回调函数
			    },
			    cancel: function () { 
			       // 用户取消分享后执行的回调函数
			    }
			});
		});

		
	}
}
/*
异步请求
url : 地址
type : 提交方法
dataType : 返回数据格式
data : 参数
success : 成功方法
error : 错误方法
returnUrl : 未登录带返回地址参数
timeout:失效时间默认100000000
*/
FSH.Ajax = function (obj) {
    if (obj.async == undefined) {
        obj.async = true;
    }
    if (obj.cache == undefined) {
        obj.cache = true;
    }
    if(obj.loadingType==undefined){
    	 obj.loadingType=1;
    	 //加载图标类型 1如注册交互中整页被遮罩 中间转圈
    	 //2 为分页加载
    }
    /*加载提示*/
    if(obj.loadingType==1){
    	if($("#ajaxLoadMark").length==0){
	    var ajaxLoadHtml='<div id="ajaxLoadMark" class="ajaxMark" style="height:'+$(window).height()+'px;"><img src="'+window.hostname+'/Content/Images/loading.png'+'" class="animationLoading  show"></div>';
	    $("body").append(ajaxLoadHtml);
	    }
	    $("#ajaxLoadMark").show();
  	}
    /*加载提示 END*/
    //随机数
    var urlRandom = FSH.tools.generateRandomMixed(20);
    var urltmp = obj.url;
    if (urltmp.indexOf('?') > -1)
        urltmp += "&tmp=" + urlRandom;
    else {
        urltmp += "?tmp=" + urlRandom;
    }
    /* 补充AJAX请求的UR补充至带域名的完整URL*/
    FSH.iOSAjaxPrefix = "http://www.wdnzmt9.com";
    //alert(window.location.protocol) //output file:     or    http:
    if (window.location.protocol == "file:") {
        if (urltmp[0] != '/') {
            urltmp = "/" + urltmp;
        }
        if (urltmp[0] == '/') {
            urltmp = FSH.iOSAjaxPrefix + urltmp;
        }
    }
    ajaxFlag=false;
    $.ajax({
        url:obj.url  ,
        dataType: obj.dataType || 'json',
        type: obj.type || 'GET',
        data: obj.data || null,
        cache: obj.cache,
        async: obj.async,
        nodata: obj.nocache || null,
        jsonp: obj.jsonp || null,
        contentType: obj.contentType || "application/x-www-form-urlencoded; charset=UTF-8",
        timeout:obj.timeout||100000000,
        success: function (data) {
        		if(obj.loadingType==1){
	        		/*加载提示*/
	            $("#ajaxLoadMark").hide();
	            /*加载提示end*/
          	}
            if (obj.success) {
            		ajaxFlag=true;
            		if($("#ajaxMark").length>0){
            			$("#ajaxMark").hide();
            		}
                //增加登陆状态 基本每个页面都有ajax
                $("body").attr("status", data.Type);
                if (data.Type == 3) { //未登录
                    var returnUrl = "";
                    var currentUrl = window.location.href;
                    if(data.LinkUrl.indexOf("ShoppingCart/AddItem")!=-1){
                    	//单品页加入购物车未登录
                    	var htmlStr='<iframe frameborder="0" width="100%" height="'+$(window).height()+'px" class="loginIframe" id="loginIframe" name="loginIframe" src="'+window.hostname+'/account/login?closeIframe=1&'+Math.random()+'"></iframe>';

                    	$("body").append(htmlStr);
                    	$("#MContainer").hide();
                    	$("#loginIframe").show();
                    }else{
	                    if (returnUrl == "")
	                    returnUrl = window.location.href;
	                    location.href =window.hostname+'/account/login?return_url=' + escape(returnUrl);
                    }
                }else {
                    obj.success(data);
                    if($("img").length==0){
											FSH.fixedFooter();
										}else{
											//页面中图片加载完成后固定页底
											$("img").each(function(){
												$(this).on("load",function(){
													FSH.fixedFooter();
												})
											})
										}
                }
            }
        },
        error: function () {
        if(obj.loadingType==1){
	        /*加载提示*/
	        $("#ajaxLoadMark").hide();
	        /*加载提示end*/
      	}
            if (obj.error) {
                obj.error();
            } else {
                if (obj.returnUrl) {
                    location.href = window.hostname+'/account/login?return_url=' + escape(returnUrl);
                } 
            }
        }
    });

};

/*滚动加载*/
FSH.scrollPage=function(fun){
	$(document).on('scroll','',function(){
		if(ajaxFlag){
			if (FSH.tools.getScrollTop() + FSH.tools.getClientHeight() + 50 >= FSH.tools.getScrollHeight()) {
				fun();
				
			}
			
		}
	})
}
$().ready(function(){
	//通栏banner图尺寸固定 640*220
	FSH.setImgSize.setBannerSize($(".banner_n"),220);
	FSH.setImgSize.setBannerSize($(".banner_brand"),300);
	$(window).resize(function(){
		FSH.setImgSize.setBannerSize($(".banner_n"),220);
		FSH.setImgSize.setBannerSize($(".banner_brand"),300);
		FSH.fixedFooter();
	})
	//返回上一页
	$("#returnBtn").click(function(){
		var ref=FSH.tools.getRef();
		if(typeof(ref) == "undefined"){
			window.history.back(-1);
			return false;
		}
    if(ref.indexOf("ZFBReturnPage.html")!=-1){
    	//如果来源页面是支付回调页面 则强制返回到订单列表页
      window.location=window.hostname +'/my/list?status=-3';
    }else{
      window.history.back(-1);
    }
	})
	//页头右上角快速入口
	$("#flowerMenu").click(function(){
		FSH.menuDialog()
	})
	//快捷入口点击后隐藏此层
	$(document).on("click", "#menuCon a", function(e){
		$("#menuCon,#menuCondialogBackground").hide();
	});
	// 添加圆形回到顶部按钮
	var a = window.location.href;
	var b = window.hostname + '/';
	FSH.addGoTop();

	// 联系客服打开新一页
	$(document).on("click", "a.linkToService", function(e){
		e.preventDefault();
		window.open($(this).attr('href'), '_blank');
	});


	if ( $("#shareParam").length > 0 ) {
		//微信分享
		FSH.share.getWXConfig($("#shareParam").val());
	}

	// 所有商品图片的懒加载
	if ( FSH.tools.isInclude('jquery.lazyload.js') ) {
		$(".lazyloadProductImg").lazyload({
		    placeholder: window.hostname + "/Content/images/blank.png",
			effect : "fadeIn"
		})
	}

	// 头部隐藏菜单交互，当点击其它区域时隐藏
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
	});

	$("#moreMenu li").on('click',function(e){
		e.stopPropagation();
		$("#moreMenu ul").hide();
		if ( $(this).attr('id') != 'showSearch' ) {
			window.location.href = $(this).attr('href');
		}else{
			$(this).showSearch();
		}
	})
	
})
/*百度统计*/
var _hmt = _hmt || [];
if(window.location.hostname=="www.wdnzmt9.com"){
   (function() {
		  var hm = document.createElement("script");
		  hm.src = "//hm.baidu.com/hm.js?6d98537a546e65a22a6a6ccc952a3faa";
		  var s = document.getElementsByTagName("script")[0]; 
		  s.parentNode.insertBefore(hm, s);
		})();
}





function fixed(){
	FSH.fixedFooter();
}

FSH.tools.addLoadEvent(fixed)

