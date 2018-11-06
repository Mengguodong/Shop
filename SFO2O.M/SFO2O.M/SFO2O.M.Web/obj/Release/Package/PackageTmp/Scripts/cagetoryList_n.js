$.views.helpers({
    setUrl:function(id,L){return categoryListUrl+"level="+L+"&c="+id;},
    setImg:function(url){
        var u="http://i1image.sf-o2o.com/";
        if(window.location.hostname!="www.discountmassworld.com"){
            u="http://o2oImages.dssfxt.net/";
        }
        return u+url;
    }
})
var categoryListUrl=window.hostname+"/product/index?";
   $().ready(function(){
    if(json.Type==1){
        var categoryListTemp = $.templates("#categoryListTemp");
        var htmlOutput = categoryListTemp.render(json.Data);
        $("#categoryListWrap").html(htmlOutput);
        /*图片延迟加载*/
        $(".lazyloadImg").lazyload({
            effect : "fadeIn" 
        });
        FSH.fixedFooter();
    }else{
        $("#categoryListWrap").append('<p class="f28 tc" style="padding:50px 0px;">'+json.Content+'</p>')
    }


    $("#gotoSearch").on('click',function(e){
        e.preventDefault();
        $(this).showSearch();
    })
   })

