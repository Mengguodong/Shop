/*
 * 弹出搜索框
*/

(function($){


    $.fn.showSearch = function(options){
        var defaults = {    
            value:'', // 要显示在搜索框里面的字
            placeholderFlag:false // 决定是以placeholder的方式显示，还是以value的形式显示在搜索框里
          };     
        var opts = $.extend(defaults, options); 

        $(".MContainer").not( $("#so_wrapper") ).hide();


        showSearch(); // 显示搜索框

        if ( FSH.tools.request('keyword') ) {
            $("#search").val( decodeURIComponent(FSH.tools.request('keyword')) );
            changeHotStatus( $("#search") );
        }

        // 热搜部分的显示与隐藏
        $("#so_wrapper").on('keyup','#search',function(){
            changeHotStatus( $(this) );
            if ( $(this).val() == '' && $(this).attr('placeholder') == '' ) {
                loadHotwords(1);
            }

        })

        $("#so_wrapper").on('change','#search',function(){
            changeHotStatus( $(this) );

        })

        $("#so_wrapper").on('focus','#search',function(){
            changeHotStatus( $(this) );
        })

        // 点击删除按钮
        $("#so_wrapper").on('click','#delete',function(){
            $("#search").val('');
            opts.value = '';
            loadHotwords();
            changeHotStatus( $("#search") );
        })

        // 点击热词
        $("#so_wrapper").on('click','.hotSearch a',function(){
            if ( $(this).attr('id') == 'more' ) {
                window.location.href = window.hostname + '/category.html';
            }else{
                keyword = $(this).text();
                $("#search").val(keyword);
                $("#searchBtn").click();
            }
        })

        // 点击'搜索'
        $("#so_wrapper").on('click','#searchBtn',function(){
            searchWord = $("#search").val();
            if ( searchWord == '' ) {
                searchWord = $("#search").attr('placeholder');
            }
            
            recordNumber(searchWord);

            $("#so_wrapper").remove();
            $(".MContainer").not('#so_wrapper').show();

            window.location.href = window.hostname + '/search.html?keyword='+encodeURIComponent(searchWord);

        })

        $("#so_wrapper").on('submit','#postKeyword',function(e){
            e.preventDefault();
            $("#searchBtn").click();
        })

        $("#so_wrapper").on('click','#pageInReturn',function(){
            $("#search").val('');
            $("#so_wrapper").remove();
            $(".MContainer").not('#so_wrapper').show();
            window.location.reload();
        })


        function showSearch(){

            if ( $("#so_wrapper").length < 1 ) {
                var searchHtml = '<div id="so_wrapper" class="MContainer so_wrapper">\
                                    <form method="get" action="" id="postKeyword">\
                                        <header class="pageHeader searchHeader w100 pr">\
                                            <a class="returnBtn" id="pageInReturn"></a>\
                                            <div class="searchBox">\
                                                <i></i>\
                                                <input type="search" name="keyword" id="search" placeholder="" autofocus />\
                                                <em class="hide" id="delete"></em>\
                                            </div>\
                                            <a class="headerRightText FontColor6" id="searchBtn">搜索</a>\
                                        </header>\
                                    </form>\
                                    <div class="hot hide" id="hot">\
                                        <div id="hotwords"></div>\
                                        <p class="FontColor3 f24 w95m mt5 mb5">热门类目搜索</p>\
                                        <div class="boxShadow bgColor3 w95p hotSearch mb8">\
                                            <a href="/product/index?level=0&c=1">保健品</a>\
                                            <a href="/product/index?level=0&c=2">母婴产品</a>\
                                            <a href="/product/index?level=0&c=3">个护化妆</a>\
                                            <a id="more">更多类目</a>\
                                        </div>\
                                    </div>\
                                </div>';
                $("body").append( searchHtml );
                
            }else{
                $("#search").val('');
                $("#so_wrapper").show();
            }

            loadHotwords();
        }

        //加载热词
        function loadHotwords(type) {
            if (type != 1) {
                postData = {p:opts.value};
            }else{
                postData = {p:''};
            }
            FSH.Ajax({
                url: "/search/GetHotKeywords",
                type: "get",
                dataType: 'json',
                jsonp: 'callback',
                jsonpCallback: "success_jsonpCallback",
                data:postData,
                success: function (data) {
                    // {"Type":1,"Data":{"placeholder":"儿童护肤","hotwords":[{"hotword":"奶粉","isRed":true},{"hotword":"牛仔裤","isRed":false},{"hotword":"greenday","isRed":true},{"hotword":"安慕希","isRed":true},{"hotword":"爽肤水","isRed":false},{"hotword":"连衣裙","isRed":true},{"hotword":"erere","isRed":false},{"hotword":"清凉解渴","isRed":false},{"hotword":"是奔跑十大","isRed":false},{"hotword":"89","isRed":false}]}}
                    if (data.Type == 1) {
                        if (type == 1) {
                            $("#search").attr('placeholder', data.Data.placeholder );
                        }else{
                            if ( $.trim(opts.value)) {
                                if (opts.placeholderFlag) {
                                    $("#search").attr( 'placeholder',$.trim(opts.value) );
                                }else{
                                    $("#search").val($.trim(opts.value));
                                }
                            }else{
                                $("#search").attr('placeholder', data.Data.placeholder );
                            }
                        }

                        changeHotStatus( $("#search") );
                        if (data.Data.hotwords.length>0) {
                            var html=" <p class=\"FontColor3 f24 w95m mt5 mb5\">大家都在搜</p><div class=\"boxShadow bgColor3 w95p hotSearch mb8\">";
                            for( var i = 0; i < data.Data.hotwords.length; i++ ){
                                var isred = '';
                                if ( data.Data.hotwords[i].isRed ) {
                                    isred ="class=FontColor6";
                                }
                                html += "<a "+ isred +">"+ data.Data.hotwords[i].hotword +"</a>"
                            }
                            $("#hotwords").html(html);
                        }
                    }
                }
            });
        }


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
            if ( $.trim(obj.val()) == '' ) {
                $("#hot").show();
                $("#delete").addClass('hide');
            }else{
                $("#hot").hide();
                $("#delete").removeClass('hide');
            }

        }





    }

}(jQuery));