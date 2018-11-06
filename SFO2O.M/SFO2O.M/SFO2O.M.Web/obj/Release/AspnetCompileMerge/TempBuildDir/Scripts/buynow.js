
// 立即购买来的确认订单

$(function(){

    // 选择优惠券
    $("#MContainer").on('click','#discount',function(){

        if ( dislist ) {
            appendDisList(dislist);
             
            if (hasClickDiscount) {
                $("#selectDiscount li").eq(0).find('i').addClass('selected');
            }else{
                $("#discountList").find('i').removeClass('selected');
                var cid = $("#discountInfo").attr('data-cid');
                $("#discountList").find(".discountNum").filter(function(){
                    return $(this).attr('data-cid') == cid
                }).parents('li').find('.radio').addClass('selected');
            }
           
         }

        disListShow( $(this) )

    })




})

function appendDisList(dislist){
    disCount = dislist.disList.length;

            $("#selectDiscount").remove();

            var list = '';
            // 重新追加优惠券列表
            for( var i = 0; i < disCount; i++ ){
                
                list += '<li><table><tr><td width="20%" class="tc" style="border-right:1px solid #e0e0e0; "><span class="f34 textOrange discountNum" data-discount="'+ FSH.tools.toDecimal2(dislist.disList[i].CardSum) +'" data-huoli="'+ parseInt(dislist.disList[i].Huoli) +'" data-cid="'+ dislist.disList[i].Id +'" data-Money="'+ dislist.disList[i].Money +'">￥'+ dislist.disList[i].CardSum +'</span></td><td><div class="f28 title pl15">'+ dislist.disList[i].Name +'</div><div class="f20 pl15">'+ dislist.disList[i].BeginTime +'-'+ dislist.disList[i].EndTime +'</div></td><td width="15%" class="tc"><i class="radio mr5"></i></td></tr></table></li>';
            }
            var html = ''
            html = '<div class="selectDiscount FontColor3" id="selectDiscount"><a class="closeBtn"><img src="../Content/Images/closeBtn.png?v=20160519" /></a><p class="f28 mb16">选择优惠券</p><ul class="discountList" id="discountList">'+list+'<li id="last"><table><tr><td width="85%"><div class="f28 pl15 none discountNum" data-cid="0" data-huoli="'+ parseInt(dislist.Huoli) +'" data-Money="'+ dislist.Money +'" >有钱任性，不使用优惠券</div></td><td width="15%" class="tc"><i class="radio mr5"></i></td></tr></table></li></ul></div>';

            $("#MContainer").append(html);
}
