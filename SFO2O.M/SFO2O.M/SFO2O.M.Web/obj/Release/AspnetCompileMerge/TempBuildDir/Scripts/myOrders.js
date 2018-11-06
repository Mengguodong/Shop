var url=window.hostname+"/my/list",
		status,scrollPageFlag=true;//scrollPageFlag 是否有下一页标志位 true 有 false 没有
$().ready(function(){
	FSH.addGoTop();
	status=FSH.tools.request("status");
	init(0);
	FSH.scrollPage(ajaxData);
	//$("#orderList").scrollPage("",ajaxData)
	$("#orderList").off("click","li .more").on("click","li .more",function(){
		if($(this).hasClass("up")){
			var len=$(this).parents(".proList").find("a").length,
					other=len-2;
			$(this).parents(".proList").find("a:gt(1)").hide();
			$(this).removeClass("up").html('展示其余'+other+'件<span></span>');
		}else{
			$(this).parents(".proList").find("a:gt(1)").show();
			$(this).addClass("up").html('收起部分商品<span></span>');
		}
	})
	
})	

function init(t){
	var len,other;
	for(var i=t;i<$("#orderList li").length; i++){
		len=$("#orderList li").eq(i).find(".proList a").length;
		if(len>2)
		{
			other=len-2;
				$("#orderList li").eq(i).find(".proList a:gt(1)").hide();
				$("#orderList li").eq(i).find(".more").removeClass("up").html('展示其余'+other+'件<span></span>').show();
		}else{
				$("#orderList li").eq(i).find(".more").hide()
		}
	}
}
function ajaxData(){
	if(ajaxFlag && scrollPageFlag){
		ajaxFlag=false;
		pageIndex++;
		$.ajax({
	      url: url,
	      type: 'get',
	      data: {"pageIndex":pageIndex,"pageSize":pageSize,"status":status},
	      cache: true,
	      async: true,
	      timeout:100000000,
	      success: function(json) {
	      	$("#moreTip").hide();
	      	ajaxFlag=true;
	      	 if (typeof json == "string"){
	      	 		if(!FSH.string.Trim(json)){
	      	 			scrollPageFlag=false;
	      	 			$("#moreTip").show().html("全部加载完");
	      	 			return false;
	      	 		}
	      	 		scrollPageFlag=true;
	      	 		$("#moreTip").show().html("上滑加载更多");
	      	     $("#orderList").append(json);
	      	     FSH.fixedFooter();
	      	     init(pageSize*(pageIndex-1));
	         }else  if (json.Type == 3) { //未登录
              var returnUrl = "";
              var currentUrl = window.location.href;
              if (json.returnUrl != undefined && json.returnUrl != null) {
                  returnUrl = json.returnUrl;
              }
              if (returnUrl == "")
                  returnUrl = window.location.href;
              location.href =window.hostname+'/account/login?return_url=' + escape(returnUrl);
          }else{
						FSH.commonDialog(1,[json.Content]);
	        }
	      },
	      error: function(err) {
	          FSH.commonDialog(1,['请求超时，请刷新页面']);   
	      }
	  });
  }
}
