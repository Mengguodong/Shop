var InterValObj;
	$(document).ready(function() {
    FSH.addGoTop();
	if($("#countdown").length>0){ 
	   SysSecond = $("#countdown").attr("data-time");
	   if(SysSecond && SysSecond>0){
	  	 InterValObj = window.setInterval(SetRemainTime, 1000); //间隔函数，1秒执行 
	 	 }
 		}
  }); 
  
  //将时间减去1秒，计算天、时、分、秒 
  function SetRemainTime() { 
   if (SysSecond > 0) { 
    SysSecond = SysSecond-1; 
    var second = Math.floor(SysSecond % 60);// 计算秒     
    var minite = Math.floor((SysSecond / 60) % 60);      //计算分 
    var hour = Math.floor((SysSecond /60/60)%24);      //计算小时
    var day  = Math.floor((SysSecond /60/60/24));      //计算天
    if(second<10){
    	second="0"+second;
    }
    if(minite<10){
    	minite="0"+minite;
    }
    if(hour<10){
    	hour="0"+hour;
    }
    if(day<10){
    	day="0"+day;
    }
    $("#countdown").html("还有"+day+"天"+ hour + "小时" + minite + "分" + second + "秒"+" 自动确认收货"); 
   } else {//剩余时间小于或等于0的时候，就停止间隔函数 
    window.clearInterval(InterValObj); 
    //这里可以添加倒计时时间为0后需要执行的事件 
    window.location.reload();
   } 
  }

  $(function () {
      $("#chakanwuliu").click(function () {
          var wlype = $("#chakanwuliu").attr("wlype");

          if (wlype.trim() == "德邦物流") {
              wlype = 'debangwuliu'
          }
          if (wlype.trim() == "宅急送") {
              wlype = 'zhaijisong'
          }
          var wlcode = $("#chakanwuliu").attr("wlcode");
          var ordercode = $("#chakanwuliu").attr("ordercode");
          window.location.href = "https://m.kuaidi100.com/index_all.html?type=" + wlype + "&postid=" + wlcode + "&callbackurl=http://www.discountmassworld.com/my/detail?orderCode=" + ordercode;
      })
  })