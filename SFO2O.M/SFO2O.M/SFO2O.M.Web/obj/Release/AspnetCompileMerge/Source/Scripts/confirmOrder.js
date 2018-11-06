// 确认订单交互
var cid; // cid为优惠券的id值，默认为0
var disCount; // 优惠券的个数
if ($("#discountInfo").length > 0) {
    cid = $("#discountInfo").attr('data-cid');
    disCount = $("#discountList li").length - 1;
}

var dislist; // 优惠券数据
var sku = $("#sku").val();
var val = $("#numberInput").val();
var pid = $("#pid").val();
var hasClickDiscount = true;
var originalPrice = $("#allMoney").attr('data'); // 选择抵用酒豆值之前的价格
if (originalPrice.indexOf(',') > 0) {
    originalPrice = originalPrice.split(',');
    var originalprice = '';
    for (var i = 0; i < originalPrice.length; i++) {
        originalprice = originalprice + originalPrice[i];
    }
    originalPrice = parseFloat(originalprice);
}
$(function () {

    addUrl();
    changeAllMoney();

    $("#MContainer").off('click', 'b.addBtn').on('click', 'b.addBtn', function () {
        //增加
        if (!$(this).hasClass("disable")) {
            var el = $(this).siblings("input"),
			val = parseFloat(el.val());
            val++;
            ajaxMarkShow();
            checkNum(val, el);
        }
    })

    $("#MContainer").off('click', 'b.reduceBtn').on('click', 'b.reduceBtn', function () {
        //减少
        if (!$(this).hasClass("disable")) {
            var el = $(this).siblings("input"),
            val = parseFloat(el.val());
            val--;
            ajaxMarkShow();
            checkNum(val, el);
        }
    })

    $("#selectValue").on('click', function () {
        var targetEl = $(this).find('i');
        if (targetEl.hasClass('selected')) {
            targetEl.removeClass('selected');
            $("#allMoney").text("￥" + FSH.tools.formatNum(originalPrice));

        } else {
            targetEl.addClass('selected');
        }

        changeAllMoney();
    })


    var aid = $("#logisticsInfo .detail").attr('addressid');
    // 立即购买提交 或 拼团过来的提交
    $("#submitBtn").on('click', function () {
        if (aid == undefined || aid == '' || aid == null) {
            // if ( $(".logisticsInfo").find('.noAdress').length > 0 ) {
            FSH.smallPrompt('请添加收货地址');
            return;
        };

        var number = parseInt($("#number").text());
        var totalPrice = $("#totalPrice").attr('data');
        var teamcode = $("#teamcode").val();

        if ($("#numberInput").val() == '') {
            FSH.smallPrompt('购买数量不能为空');
            return;
        }

        var hasActivity = 0; // 0指没有选中酒豆值
        if ($("#selectValue i").hasClass('selected')) {
            hasActivity = 1;
        }

        var Quy; // 如果是拼团确认订单页的话，Quy=1
        if (!FSH.tools.request('pid') && !FSH.tools.request('teamcode')) {
            Quy = $("#numberInput").val();
        } else {
            Quy = 1;
        }

        var GiftCardId;
        if ( $("#discount").length > 0 ) { // 如果有优惠券这一行的话
            GiftCardId = $("#discountInfo").attr('data-cid');
        }


        var postData = { Sku: sku, Quy: Quy, AddressId: aid, Type: 1, hasActivity: hasActivity, pid: pid, teamcode: teamcode, GiftCardId:GiftCardId };
        FSH.Ajax({
            Type: 'post',
            url: window.hostname + '/PlaceOrder.html', // 提交url
            data: { jsonModel: JSON.stringify(postData) },
            success: function (msg) {
                if (msg.Type == 1) {
                    window.location.replace(window.hostname + '/PayPage.html?id=' + msg.Data);
                } else {
                    FSH.commonDialog(1, [msg.Content]);
                }
            },
            error: function (msg) {
                FSH.commonDialog(1, [msg.Content]);
            }
        })
    })

    // 购物车提交 或 重新提交
    $("#submitOrderBtn").on('click', function () {
        var id = FSH.tools.request('id');
        if (aid == undefined || aid == '' || aid == null) {
            // if ( $(".logisticsInfo").find('.noAdress').length > 0 ) {
            FSH.smallPrompt('请添加收货地址');
            return;
        };

        var hasActivity = 0; // 0指没有选中酒豆值
        if ($("#selectValue i").hasClass('selected')) {
            hasActivity = 1;
        }

        var GiftCardId;
        if ( $("#discount").length > 0 ) { // 如果有优惠券这一行的话
            GiftCardId = $("#discountInfo").attr('data-cid');
        }

        var postData;
        if (id != '' || id != undefined || id != null) {
            var postData = { Sku: 0, Quy: 0, AddressId: aid, Type: 2, hasActivity: hasActivity,GiftCardId:GiftCardId };
        } else {
            var postData = { Sku: 0, Quy: 0, AddressId: aid, Type: 3, OrderCode: id, hasActivity: hasActivity,GiftCardId:GiftCardId };
        }


        FSH.Ajax({
            Type: 'post',
            url: window.hostname + '/PlaceOrder.html', // 提交url
            data: { jsonModel: JSON.stringify(postData) },
            success: function (msg) {
                if (msg.Type == 1) {
                    window.location.replace(window.hostname + '/PayPage.html?id=' + msg.Data);
                } else if (msg.Type == 2) {
                    window.location.replace(window.hostname + '/split.html?orderCode=' + msg.Data);
                } else {
                    FSH.commonDialog(1, [msg.Content]);
                }
            },
            error: function (msg) {
                FSH.commonDialog(1, [msg.Content]);
            }
        })
    })


    $("#numberInput").keyup(function () {
        checkNum($(this).val(), $(this))
    }).blur(function () {
        if ($(this).val() == '') {
            FSH.smallPrompt('购买数量不能为空');
            return;
        }
        checkNum($(this).val(), $(this))
    })

    // 返回
    $("#pageReturnBtn").on('click', function () {
        if (document.referrer.indexOf('/account/login') > 0) {
            window.history.go(-2);
        } else {
            window.history.back(-1);
        }
    })


    // 选择优惠券
    $("#MContainer").on('click','#selectDiscount li',function(){
        if ( $("#selectValue i").hasClass('selected') ) {
            $("#selectValue i").removeClass('selected');
        }
        $("#discountList .radio").removeClass('selected');
        var _this = $(this);
        _this.find('.radio').addClass('selected');
        if (_this.attr('id') != 'last') {
            $("#discountInfo").html(_this.find('.title').text() + '<br>立减￥<font id="dis">' + FSH.tools.toDecimal2(_this.find('.discountNum').attr('data-discount'))+'</font>').css('margin-top', '6px');
            // 酒豆值改变
            var targetEl = _this.find('.discountNum');
            $("#vigor").text( parseInt(targetEl.attr('data-huoli')) );
            $("#vigorValue").text( targetEl.attr('data-money') );
            $("#discountInfo").attr("data-cid",targetEl.attr('data-cid'));
        } else {
            $("#discountInfo").text('你有'+ disCount +'个可用优惠券').css('margin-top', '15px');
            var targetEl = _this.find('.none');
            $("#vigor").text( parseInt(targetEl.attr('data-huoli')) );
            $("#vigorValue").text( targetEl.attr('data-money') );
            $("#discountInfo").attr("data-cid",targetEl.attr('data-cid'));

        }

        hasClickDiscount = false;

        changeAllMoney();
    })

})

function changeAllMoney() {

    var vagorValue = 0,discountMoney=0,nowPrice=0;
    if ( $("#selectValue i").hasClass('selected') ) { // 如果酒豆值是选中状态
        vagorValue = parseFloat($("#vigorValue").text());
        nowPrice = FSH.tools.toDecimal2(FSH.tools.accSub(originalPrice, vagorValue));
        if ( $("#dis").length > 0 ) {
            discountMoney = parseFloat($("#dis").text());
            nowPrice = FSH.tools.toDecimal2( FSH.tools.accSub( nowPrice, discountMoney ) );
        }
    }else{
        if ( $("#dis").length > 0 ) { // 如果选择了优惠券且含面值
            discountMoney = parseFloat($("#dis").text());
            nowPrice = FSH.tools.toDecimal2( FSH.tools.accSub( originalPrice, discountMoney ) );
        }else{
            nowPrice = originalPrice;
        }

    } 

    $("#allMoney").text("￥" + FSH.tools.formatNum(nowPrice));

}

function checkNum(num, inputEl) {
    if (num) {
        FSH.Ajax({
            url: '/buy/BuyNowUpdateQty?sku=' + sku + '&qty=' + num , 
            // 接口返回数据eg:
            // {"Type":1,"Content":"592.80","Huoli":113522,"Money":"1,135.22",
            // disList:[{Id:11,Name:'新人奖励',FullCutNum:500,CardSum:20,Type:1,BeginTime:'2016.05.05',EndTime:'2016.12.12',Huoli:113522,Money:'1,135.22'}]}
            // disList中的Type 1.新人奖励 2.满减...3.不使用优惠券
            // Id : 优惠券id
            // Name : 优惠券名称
            // FullCutNum: 满fullCutNum可使用优惠券
            // CardSum: 优惠券面额
            // Huoli: 当前商品总额，使用当前优惠券以后可用的酒豆值
            // Money: 当前商品总额，使用当前优惠券以后可用的酒豆值对应的人民币
            dataType: 'json',
            jsonp: 'callback',
            jsonpCallback: "success_jsonpCallback",
            success: function (json) {
                hasClickDiscount = true;

                if (json.Type == 1) {
                    inputEl.val(num);
                    inputEl.parents("li").find("#number").html(num);
                    Calculation(json.Content);
                    if (num == 1) {
                        inputEl.siblings(".reduceBtn").addClass("disable");
                    } else {
                        inputEl.siblings(".reduceBtn").removeClass("disable");
                    }
                    inputEl.siblings(".addBtn").removeClass("disable");

                    if ( json.disList.length> 0 ) {

                        $("#vigor").text(json.disList[0].Huoli);
                        $("#vigorValue").text(json.disList[0].Money);
                        if ( $("#discount").length < 1 ) {
                            $(".aboutMoney").append('<p class="discount" id="discount"><b class="w100 pr"><span>优惠</span><em id="discountInfo" data-cid="'+ json.disList[0].Id+'" style=" margin-top:6px;">'+json.disList[0].Name+'<br>立减￥<font id="dis">'+ FSH.tools.toDecimal2( json.disList[0].CardSum ) +'</font></em><i class="rightArrow"></i></b></p>')
                        }else{
                            $("#discountInfo").html(json.disList[0].Name+'<br>立减￥<font id="dis">'+FSH.tools.toDecimal2( json.disList[0].CardSum )+'</font>').attr('data-cid',json.disList[0].Id).css('margin-top','6px');
                        }
                    }else{

                        $("#vigor").text(json.Huoli);
                        $("#vigorValue").text(json.Money);
                        $("#discount").remove();
                    }


                    dislist = json;
                    changeAllMoney();

                } else {
                    inputEl.val($("#number").text());
                    inputEl.siblings(".addBtn").addClass("disable");
                    FSH.commonDialog(1, [json.Content]);
                }

            },
            error: function (err) {
                FSH.commonDialog(1, ['请求超时，请刷新页面']);
            }
        })
    }
}


function addUrl() {
    var currentUrl = document.location.href;
    if ($("#jump").length > 0) { // 说明地址列表为空
        if (window.location.href.indexOf('buynow') > 0) {
            var pid = FSH.tools.request('pid');
            if (pid) {
                $("#jump").attr('href', '').attr('href', window.hostname + "/buy/addaddress?return_url=" + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val() + '&pid=' + pid);
            } else {
                var teamcode = FSH.tools.request('teamcode');
                if (teamcode) {
                    $("#jump").attr('href', '').attr('href', window.hostname + "/buy/addaddress?return_url=" + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val() + '&teamcode=' + teamcode);
                } else {
                    $("#jump").attr('href', '').attr('href', window.hostname + "/buy/addaddress?return_url=" + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val());
                }
            }

        } else {
            $("#jump").attr('href', '').attr('href', window.hostname + "/buy/addaddress?return_url=" + window.hostname + "/OrderSumit.html");
        }

    } else {
        var aidStr = FSH.tools.request('aid');
        if (window.location.href.indexOf('buynow') > 0) {
            var pid = FSH.tools.request('pid');
            if (pid) {
                if (aidStr) {
                    $(".goSelect").attr('href', '').attr('href', window.hostname + "/buy/chooseaddress?return_url=" + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val() + '&pid=' + pid + '&aid=' + aidStr);
                } else {
                    $(".goSelect").attr('href', '').attr('href', window.hostname + "/buy/chooseaddress?return_url=" + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val() + '&pid=' + pid);
                }
            } else {

                var teamcode = FSH.tools.request('teamcode');
                if (teamcode) {
                    if (aidStr) {
                        $(".goSelect").attr('href', '').attr('href', window.hostname + "/buy/chooseaddress?return_url=" + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val() + '&teamcode=' + teamcode + '&aid=' + aidStr);
                    } else {
                        $(".goSelect").attr('href', '').attr('href', window.hostname + "/buy/chooseaddress?return_url=" + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val() + '&teamcode=' + teamcode);
                    }

                } else {
                    if (aidStr) {
                        $(".goSelect").attr('href', '').attr('href', window.hostname + "/buy/chooseaddress?return_url=" + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val() + '&aid=' + aidStr)
                    } else {
                        $(".goSelect").attr('href', '').attr('href', window.hostname + "/buy/chooseaddress?return_url=" + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val())
                    }

                }
            }

        } else {
            if (aidStr) {
                $(".goSelect").attr('href', '').attr('href', window.hostname + "/buy/chooseaddress?aid=" + aidStr + "&return_url=" + window.hostname + "/OrderSumit.html");
            } else {
                $(".goSelect").attr('href', '').attr('href', window.hostname + "/buy/chooseaddress?return_url=" + window.hostname + "/OrderSumit.html");
            }
        }
    }
}

function Calculation(totalTariff) {
    var unitPrice = $("#unitPrice").html();
    var number = $("#number").html();
    var totalPrice = FSH.tools.accMul(unitPrice, number);
    var allMoney = FSH.tools.accAdd(totalTariff, totalPrice);
    originalPrice = allMoney;
    var allMoneyNum = parseFloat(totalTariff) + parseFloat(totalPrice);
    $("#totalTariff").text(totalTariff);
    $("#totalPrice").text('￥' + FSH.tools.formatNum(totalPrice)).attr('data', totalPrice);
    $("#allMoney").text("￥" + FSH.tools.formatNum(allMoney)).attr('data', allMoneyNum);
    $("#totalNumber").text(number);
    $("#qty").val(number);
    addUrl();
    $("#goTariff").attr('href', window.hostname + '/buy/importTariff?sku=' + $("#sku").val() + '&qty=' + number + '&pid=' + pid + '&return_url=' + window.hostname + "/buy/buynow?sku=" + $("#sku").val() + "&qty=" + $("#qty").val()) + '&pid=' + pid;
}

function checkTotalTariff(number) {
    if (number <= 50) {
        return 0;
    } else {
        return number;
    }
}

function ajaxMarkShow() {
    //显示异步请求时遮罩层
    if (!$("#ajaxMark").length > 0) {
        $("body").append('<div class="ajaxMark tc" id="ajaxMark" style="height:500px; display:block"><img src="../Content/images/loading.png?V=201601071111" class="animationLoading"></div>');
    }
    $("#ajaxMark").height($(window).height()).css("line-height", $(window).height() + "px").show();
}

function disListShow(obj){
    $(obj).popShow({ positionY: "bottom", targetEl: "#selectDiscount", closeBtnName: ".closeBtn,.discountList li" });
}