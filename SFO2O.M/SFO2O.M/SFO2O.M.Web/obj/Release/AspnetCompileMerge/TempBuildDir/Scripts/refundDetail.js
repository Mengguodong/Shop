$(function(){
	var size = ($("#MContainer").width() * 0.95 - 30)/4 - 2; 
	$("#imgBox div").css({
		width:size,
		height:size,
		'line-height':size+'px'
	});

	$("#imgBox>div").each(function(n){
		if ( (n+1)%4 == 0 ) {
			$("#imgBox>div").eq(n).css('margin-right',0);
		};
	})


	$("#cancelRefund").one('click',function(){
		var code = FSH.tools.request('code');
		FSH.Ajax({
			type:'post',
			url:'/refund/CancelRefund',
			data:{refundCode:code},
			success:function(msg){
				FSH.commonDialog(1,[msg.Content]);
				window.location.reload();
			},
			error:function(msg){
				FSH.commonDialog(1,[msg.Content]);
			}
		})
	})
})