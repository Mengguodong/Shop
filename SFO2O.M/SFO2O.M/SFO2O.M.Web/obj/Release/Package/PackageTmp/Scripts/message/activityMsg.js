var url=window.hostname+"/Information/GetActivityMsgList",
		pageIndex=0,//页码 从1开始
		count=10,//每页显示条数
		scrollPageFlag=true;//scrollPageFlag 是否有下一页标志位 true 有 false 没有
$().ready(function(){
	loadData();
	FSH.scrollPage(loadData);
})
/*
{
    "Type": 1,
    "Data": {
        "PageIndex": 1,
        "PageSize": 10,
        "TotalRecord": 37,
        "PageCount": 4,
        "MsgList": [
            {
                "date": "2016-05-12 14:30",
                "title": "放多少发送到发送到发送到放多少",
                "msgCon": "法第三方斯蒂芬都是放多少发送到费第三方第三方第三方放多少法第三方第三方都是放多少但是放多少但是放多少但是的",
                "img":"",
                "url":"#"
            }
        ]
    }
}
请求参数:
{"pageIndex":页码,"pageSize":每页显示条数}
返回值说明：
PageIndex 当前页码
PageSize 每页显示条数
TotalRecord 总公共条数
PageCount 总共页数
MsgList 列表内容
date 消息发布时间， title 消息标题 ， msgCon 消息内容，img 图片地址  url 消息正文跳转地址
 */
function loadData() {
    if (ajaxFlag && scrollPageFlag) {
        pageIndex++;
        FSH.Ajax({
            url: url,
            type: "get",
            data: { "pageIndex": pageIndex, "pageSize": count},
            loadingType:2,
            dataType: 'json',
            jsonp: 'callback',
            jsonpCallback: "success_jsonpCallback",
            success: function (json) {                
                if (json.Type==1) {
                    if (json.Data.PageIndex == json.Data.PageCount) {
                        $("#more").show().html("全部加载完");
                        scrollPageFlag = false;
                    } else {
                        $("#more").show().html("上滑加载更多");
                        scrollPageFlag = true;
                    }
                    var html='';
                    for (var i = 0; i < json.Data.MsgList.length; i++) {
                        html += '<a href="'+json.Data.MsgList[i].url+'" target="_self">\
  			<div class="FontColor4 f24 lh24 n_sysMsgTime">'+json.Data.MsgList[i].date+'</div>\
  			<div class="boxShadow bgColor3 n_sysTxtCon">\
  				<div class="f28 lh24">'+json.Data.MsgList[i].title+'</div>\
          <img class="n_actImg" src="'+json.Data.MsgList[i].img+'" alt="">\
  				<div class="f24 lh24 FontColor3">'+json.Data.MsgList[i].msgCon+'</div>\
  			</div>\
  		</a>';
                    }
                    $("#systemMsgList").append(html);
                    FSH.fixedFooter();
                }
                else {
                    FSH.commonDialog(1, [json.Content]);
                }

            },
            error: function(err) {
                    FSH.commonDialog(1,['请求超时，请刷新页面']);   
            }
        });
         
    }
}
