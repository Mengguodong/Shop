var keyword = '';
$(function(){

	// 如果是从搜索结果页跳转过来的（并且在点击搜索框时里面有内容）;
	keyword = decodeURI(FSH.tools.request('keyword'));
	$("#search").val( keyword );
	changeHotStatus( $("#search") );

	if ( FSH.tools.request('p') ) {
		$("#search").attr('placeholder', decodeURI(FSH.tools.request('p')) );
	}

	$("#MContainer #search").focus(function(){
		if ( $(this).val() != '' ) {
			$("#delete").removeClass('hide');
		}
	});

	// 热搜部分的显示与隐藏
	$("#MContainer").on('keyup','#search',function(){
		changeHotStatus( $(this) );
	})

	$("#MContainer").on('change','#search',function(){
		changeHotStatus( $(this) );
	})

	// 点击删除按钮
	$("#MContainer").on('click','#delete',function(){
		$("#search").val('');
		window.location.href = window.hostname + '/so.html';
		changeHotStatus( $("#search") );
	})

	// 点击热词
	$("#MContainer").on('click','.hotSearch a',function(){
		if ( $(this).attr('id') == 'more' ) {
			window.location.href = window.hostname + '/category.html';
		}else{
			keyword = $(this).text();
			$("#search").val(keyword);
			$("#searchBtn").click();
		}
	})
	// 点击'搜索'
	$("#searchBtn").on('click',function(){
		searchWord = $("#search").val();
		if ( searchWord == '' ) {
			searchWord = $("#search").attr('placeholder');
		}
		
		recordNumber(searchWord);

		window.location.href = window.hostname + '/search.html?keyword='+searchWord;

	})

	$("#postKeyword").on('submit',function(e){
		// searchWord = $("#search").val();
		// if ( searchWord == '' ) {
		// 	 $("#search").val( $("#search").attr('placeholder') );
		// 	 searchWord = $("#search").attr('placeholder');
		// }
		
		// recordNumber(searchWord);
		e.preventDefault();
		$("#searchBtn").click();
	})

})

// 统计热词搜索次数
function recordNumber(searchWord){
	FSH.Ajax({
        url: "/Search/AddSearchHotWordRecord",
        type: "post",
        dataType: 'json',
        jsonp: 'callback',
        jsonpCallback: "success_jsonpCallback",
        data:{keyword:searchWord},
        success: function (data) {
            if (data.Type == 1) {
                console.log(data.Content);
            }
            else {
                //FSH.commonDialog(1, [data.Content]);
                console.log(data.Content);
            }
        }
    });
}

function changeHotStatus(obj){
	if ( obj.val() == '' ) {
		$("#hot").show();
		$("#delete").addClass('hide');
	}else{
		$("#hot").hide();
		$("#delete").removeClass('hide');
	}
}

