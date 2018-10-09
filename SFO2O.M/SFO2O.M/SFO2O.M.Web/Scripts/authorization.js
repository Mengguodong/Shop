$(function(){

	function getReturnUrl(paras){ /*获取return_url，条件是return_url必须是最后一项*/
	    var url = location.href; 
	    var nIndex = url.indexOf(paras);
	    var paraString = url.substring(nIndex);
	    var returnValue = paraString.substring(11);
	    return returnValue;
	}

	$("#btn").on('click',function(){
		var returnUrl = unescape(getReturnUrl('return_url'));
		FSH.Ajax({
			type:'get',
			url:window.hostname + '/FirstOrderAuthorize.html',
			success:function(msg){
				window.location.href = returnUrl;
			}
		})
	})

})