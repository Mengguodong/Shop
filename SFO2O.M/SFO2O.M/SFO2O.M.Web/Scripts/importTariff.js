$(function(){

	

	$("#customReturn").on('click',function(){
		var currentUrl = window.location.href;
		if ( currentUrl.indexOf("return_url") < 0 ) {
			window.history.back(-1);
		}else{
			var returnUrl = unescape(getReturnUrl( 'return_url' ));
			window.location.href = returnUrl;
		}
	})

	

})

function getReturnUrl(paras){ /*获取return_url，条件是return_url必须是最后一项*/
    var url = location.href; 
    var nIndex = url.indexOf('return_url');
    var paraString = url.substring(nIndex);
    var returnValue = paraString.substring(11);
    return returnValue;
}