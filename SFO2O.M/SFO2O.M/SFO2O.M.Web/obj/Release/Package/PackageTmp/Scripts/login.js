// 登录页
var cart=0,//是否需要获取购物车数量 1需要 0不需要
    returnUrl="";//注册成功回跳页面
$(document).ready(function () { 
    returnUrl=FSH.tools.request("Return_Url");
    //登录页以弹窗形式内嵌到其他页面时
    var isIframe=FSH.tools.request("closeIframe");
    if(isIframe==1 && window.parent.closeIframe!= null){
        cart=1;
        $(".links a").attr("target","_parent");
    }else{
        if(returnUrl){
        $(".links a").eq(0).attr("href",$(".links a").eq(0).attr("href")+"?Return_Url="+returnUrl);
        }
    }
    $("#loginReturnBtn").click(function(){
        if(isIframe==1 && window.parent.closeIframe!= null){
           window.parent.closeIframe(0,0); 
        }else{
            window.history.back(-1);
        }
    })
    //登录页以弹窗形式内嵌到其他页面时 end
    var RegionCode = 86;
    function loginBtnStatus(){ // 账户和密码输入完之后登录按钮字体变白
        var username = $("#tel").val();
        var password = $("#psw").val();
        if ( username != '' && password != '' ) {
            $("#loginBtn").addClass('active');
        }else{
            $("#loginBtn").removeClass('active');
        }
    }

    $("#select").on('click',function(){
        $(this).popShow({positionY:"center",openAnimationName:"bottomShow",closeAnimationName:"bottomHide",targetEl:"#areaCode",closeBtnName:"#cancel,#sure"});
    })
    

    $("#login").on('blur','#psw',function(){
        loginBtnStatus();
    }).on('blur','#tel',function(){
        loginBtnStatus();
    })

    $("#areaCode").on('click','li',function(){
        // $(this).addClass('current').siblings().removeClass('current');
        $("#areaCode i").removeClass('checked');
        $(this).find('i').addClass('checked');
        RegionCode = $(this).attr('data-code');
        
    }).on('click','#sure',function(){
        $("#select span").text("+"+RegionCode);
    })

    $("#loginBtn").on("click", function (e) {
        // FSH.EventUtil.preventDefault()
        e.preventDefault();

        window.ReturnUrl = unescape(FSH.tools.request("return_url"));

        

        var username = $("#tel").val();
        var password = $("#psw").val();

        if (username == "") {
            FSH.smallPrompt('请输入手机号');
            return false;
        }
        if (!FSH.tools.isPhone(username)) {
            FSH.smallPrompt('手机号码不正确，重新填下吧');
            return false;
        }
        if (password == "") {
            FSH.smallPrompt('请输入账户密码');
            return false;
        }


        FSH.Ajax({
            url: "/account/login",
            type:"post",
            dataType: 'json',
            data: {userName:username,password:password,RegionCode:RegionCode,cart:cart},
            jsonp: 'callback',
            jsonpCallback: "success_jsonpCallback",
            success: function (htmlobj) {

                if (htmlobj.Type == 1) {
                    if(isIframe==1 && window.parent.closeIframe!= null){
                            window.parent.closeIframe(1,htmlobj.Data);
                            return false;
                    }
                    if (window.ReturnUrl != "" && window.ReturnUrl != null && window.ReturnUrl != "null") {
                       
                        location.href = window.ReturnUrl;
                    }else {
                        //登录成功，跳转
                        location.href = window.hostname;
                        return;
                    }
                } else {
                    FSH.commonDialog(1,[htmlobj.Content]);
                }

            },
            timeout: 30000
        });

    });
});
