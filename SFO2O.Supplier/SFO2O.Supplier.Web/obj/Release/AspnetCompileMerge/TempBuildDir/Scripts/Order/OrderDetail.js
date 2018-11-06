var InterValObj;
$(document).ready(function () {
    if ($("#countdown").length > 0) {
        SysSecond = $("#countdown").val();
        if (SysSecond && SysSecond > 0) {
            InterValObj = window.setInterval(SetRemainTime, 1000); //间隔函数，1秒执行 
        }
    }
});

//将时间减去1秒，计算天、时、分、秒 
function SetRemainTime() {
    if (SysSecond > 0) {
        SysSecond = SysSecond - 1;
        var second = Math.floor(SysSecond % 60);// 计算秒     
        var minite = Math.floor((SysSecond / 60) % 60);         //计算分 
        var hour = Math.floor((SysSecond / 60 / 60) % 24);      //计算小时
        var day = Math.floor((SysSecond / 60 / 60 / 24));       //计算天
        if (second < 10) {
            second = "0" + second;
        }
        if (minite < 10) {
            minite = "0" + minite;
        }
        if (hour < 10) {
            hour = "0" + hour;
        }
        if (day < 10) {
            day = "0" + day;
        }
        $("#spanCountdown").html("还有" + day + "天" + hour + "小时" + minite + "分" + second + "秒" + " 自动确认收货");
    } else {//剩余时间小于或等于0的时候，就停止间隔函数 
        window.clearInterval(InterValObj);
        //这里可以添加倒计时时间为0后需要执行的事件 
        window.location.reload();
    }
}


function ShowLogistics(expressCode, expressCompany) {
    var html = "";
    if (expressCompany.trim() == '德邦物流') {
        window.location.href = "https://m.kuaidi100.com/result.jsp?com=debangwuliu&nu=" + expressCode;
    }
    if (expressCompany.trim() == '宅急送') {
        window.location.href = "https://m.kuaidi100.com/result.jsp?com=zhaijisong&nu=" + expressCode;
    }
    //$.get('/Order/ShowLogistics', { ExpressCode: "604128844391" }, function (data) {
    //    $.dialog({
    //        title: "查看物流信息",
    //        content: data,
    //    });

    //});
}