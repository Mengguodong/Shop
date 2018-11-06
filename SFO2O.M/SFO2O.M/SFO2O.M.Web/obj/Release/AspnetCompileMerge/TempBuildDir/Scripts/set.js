$(function(){

	$(".item i").on('click',function(){
		var _this = $(this);
		var value = (_this.hasClass('active'))?2:1;
		FSH.Ajax({
			url:window.hostname + '/my/updateIsPush',
			type:'post',
			data:{type:value},
			success:function( msg ){
				if (msg.Type == 1) { // type=1 允许推送 type=2 不允许推送
					if (_this.hasClass('active')) {
						_this.removeClass('active');
					}else{
						_this.addClass('active');
					}
				}
			},
			error:function( msg ){
				FSH.commonDialog(1,[msg.Content]);
			}
		})
	})
})