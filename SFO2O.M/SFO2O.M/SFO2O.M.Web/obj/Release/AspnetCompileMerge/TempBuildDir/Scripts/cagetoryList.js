$.views.helpers({
    getUrl:function(id){return categoryList3Url+id;}
})
var categoryList1Temp ,categoryList2Temp,categoryList3Url=window.hostname+"/product/index?level=2&c=";
   $().ready(function(){
    
    categoryList1Temp = $.templates("#categoryList1Temp");
    categoryList2Temp = $.templates("#categoryList2Temp");
    $("#categoryWrap,#categoryList1,#categoryList2").height($(window).height()-$(".pageHeader").outerHeight());
    if(json.Type==1){
        
        var htmlOutput1 = categoryList1Temp.render(json.Data);
        $("#categoryList1 .pb").html(htmlOutput1);
        var categoryList1 = new IScroll('#categoryList1', { mouseWheel: true, click: true });
        loadCategoryList2($("#categoryList1 .pb a").eq(0));
    }else{
        $("#categoryWrap").append('<p class="f28 tc" style="padding:50px 0px;">'+json.Content+'</p>')
    }
    document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
    $("#MContainer").on("click","#categoryList1 .pb a",function(){
        loadCategoryList2($(this))
    })
   })
   function loadCategoryList2(el){
        var id =el.attr("_CategoryId");
        if(!el.hasClass("cur")){
            el.addClass("cur").siblings(".cur").removeClass("cur")
            for(var i=0; i<=json.Data.length; i++){
                if(json.Data[i].CategoryId==id){
                    var htmlOutput2 = categoryList2Temp.render(json.Data[i].Items);
                    setTimeout(function(){
                    	$("#categoryList2 .pb").html(htmlOutput2);
                    var categoryList2 = new IScroll('#categoryList2', { mouseWheel: true, click: true,startY:0 });
                    /*图片延迟加载*/
                            $(".lazyloadImg").lazyload({
                                effect : "fadeIn",
                                event : "sporty"   
                            });
                    $(".lazyloadImg").trigger("sporty")
                    document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
                    },500)
                    
                    break;
                }
            }
        }

   }

