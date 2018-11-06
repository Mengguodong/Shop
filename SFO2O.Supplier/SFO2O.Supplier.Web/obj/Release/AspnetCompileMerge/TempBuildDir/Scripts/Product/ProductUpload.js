
$(document).ready(function () {
    //替换window.alert
    window._alert = window.alert;
    window.alert = function (msg) { $.dialog("" + msg); }

    // 品牌
    // $("#brand").selectFn();

    var tabs = $("#ulDetail");
    BindLiChange(tabs, function (li, old) {
        $("#" + old.data("for")).hide();
        $("#" + li.data("for")).show();
    });

    // 绑定下拉框
    $(".select_pinpai").each(function () {
        var obj = $(this);
        obj.find("select").chosen({ disable_search: true, width: "100px" });
        if (obj.attr("data-value") == "0") {
            obj.find("select").change(function () {
                var selecttext = obj.find("option:selected").text();
                if (selecttext.indexOf("其他") > -1 || selecttext.indexOf("其它") > -1) {
                    obj.parent().parent().find("div.add_wrap").show();
                }
                else {
                    obj.parent().parent().find("div.add_success_list").empty();
                    obj.parent().parent().find("div.add_wrap").hide();
                }
            });
        }
    });

    var spuNo = $("#Spu").val();

    if (spuNo != undefined && spuNo != "") {
        $("#updatecategory").remove();
    }

    $("#BrandIdSel").chosen({ disable_search: true, width: "229px" });

    $(".chbx_wrap").not("label.unactive").click(function () {
        $(this).toggleClass("active");
    });

    $(".add_success_li,add_success_label").find("a.close").click(function () {
        $(this).parent().remove();
    });

    $("#SalesTerritory").find("label.chbx_wrap").click(function () {
        if ($("#SalesTerritory").find("label.active").length == 0 || $("#SalesTerritory").find("label.active").length == 2) {
            $("#commissionCHINA").show();
            $("#commissionHK").show();

            $("#exchangeInCHINA").show();
            $("#exchangeInHK").show();
        }
        else {
            var obj = $("#SalesTerritory").find("label.active");
            if (obj.attr("data-value") == "1") {
                $("#commissionCHINA").show();
                $("#commissionHK").hide();
                $("#exchangeInCHINA").show();
                $("#exchangeInHK").hide();
                var input = $("#commissionHK").find("input");
                input.val("");

            }
            else if (obj.attr("data-value") == "2") {
                $("#commissionCHINA").hide();
                var input = $("#commissionCHINA").find("input").val("");
                $("#commissionHK").show();
                $("#exchangeInCHINA").hide();
                $("#exchangeInHK").show();
            }
        }

    });

    // 绑定拼接sku 二维表的方法
    $(".main,.sku").find("label").not("label.unactive").click(function () {
        var Id = $(this).attr("id");
        var Name = $(this).find("span").html();
        var active = $(this).hasClass("active");
        UpdateSkuInfo(Id, Name, active);
    });
});


function transforId(skuId) {
    skuId = skuId.replace(".", "\\.").replace("*", "\\*").replace(" ", "\\ ").replace("(", "\\(").replace(")", "\\)").replace(":", "\\:");
    return skuId;
}


function DeleteSku(obj, mainId, skuId, maxItems) {
    if (maxItems == undefined) {
        maxItems = 10;
    }

    mainId = transforId(mainId);
    skuId = transforId(skuId);
    var skuType = $("#skuInfoType").val();

    var mainlength = $("#" + mainId).parent().parent().find("div.add_success_li").length - 1;
    var mianlength2 = $("#" + mainId).parent().parent().find("label.add_success").length - 1;

    if (mainlength >= maxItems || mianlength2 >= maxItems) {
        $("#" + mainId).parent().find("div.add_wrap").hide();
    }
    else {
        $("#" + mainId).parent().find("div.add_wrap").show();
    }

    var skulength = $("#" + skuId).parent().parent().find("div.add_success_li").length - 1;
    var skulength2 = $("#" + skuId).parent().parent().find("label.add_success").length - 1;

    if (skulength >= maxItems || skulength2 >= maxItems) {
        $("#" + skuId).parent().parent().find("div.add_wrap").hide();
    }
    else {
        $("#" + skuId).parent().parent().find("div.add_wrap").show();
    }

    if (skuType == "1") {
        var len = $(obj).parent().parent().find("div").length;
        if (len <= 1) {
            $(obj).parent().parent().parent().remove();

            if ($("#" + mainId).hasClass("active")) {
                $("#" + mainId).removeClass("active");
            }
            else {
                $("#" + mainId).remove();
            }
            var skus = $("#skuInfo table").find("div[name='" + skuId + "']").length;
            if (skus < 1) {
                if ($("#" + skuId).hasClass("active")) {
                    $("#" + skuId).removeClass("active");
                }
                else {
                    $("#" + skuId).remove();
                }
            }
        }
        else {
            $(obj).parent().remove();
            var skus = $("#skuInfo table").find("div[name='" + skuId + "']").length;
            if (skus < 1) {
                if ($("#" + skuId).hasClass("active")) {
                    $("#" + skuId).removeClass("active");
                }
                else {
                    $("#" + skuId).remove();
                }
            }
        }
    }
    else {
        $(obj).parent().parent().parent().remove();

        $("#" + skuId).remove();
    }



    var trs = $("#skuInfo table tbody").find("tr").length;
    if (trs < 1) {
        $("#skuInfo").hide();
    }
}

function DeleteValue(obj, addAmount, maxItems) {
    var skuType = $("#skuInfoType").val();
    var id = $(obj).parent().attr("id");
    if (skuType == "1") {
        var skuType = id.split('-')[0];
        if (skuType == "main") {
            $("#skuInfo").find("tr[name='" + id + "']").remove();
        }
        else {
            $("#skuInfo").find("div[name='" + id + "']").remove();
        }
    }
    else {
        $("#skuInfo").find("tr[name='" + id + "']").remove();
    }
    var length = $(obj).parent().parent().parent().find("div.add_success_li").length - 1;
    var length2 = $(obj).parent().parent().parent().find("label.add_success").length - 1;

    if (length >= maxItems || length2 >= maxItems) {
        $(obj).parent().parent().parent().find("div.add_wrap").hide();
    }
    else {
        $(obj).parent().parent().parent().find("div.add_wrap").show();
    }
    $(obj).parent().remove();
    var trs = $("#skuInfo table tbody").find("tr").length;
    if (trs < 1) {
        $("#skuInfo").hide();
    }
}

function UpdateSkuInfo(id, name, active) {

    var skuType = $("#skuInfoType").val();
    var basePrice = $("#Price").val();

    if (skuType == "1") {

        var main = $(".main");
        var sku = $(".sku");
        var type = id.split('-')[0];
        if ((sku.find("label.active,label.unactive").length <= 0 && sku.find("div.add_success_li,label.add_success").length <= 0)
            || (main.find("label.active,label.unactive").length <= 0 && main.find("div.add_success_li").length <= 0)) {
            $("#skuInfo").find("tbody").empty();
            $("#skuInfo").hide();
            return false;
        }

        $("#skuInfo").show();
        if (type == "main") {
            var trHtml = "";
            var titleM = name;
            var mainId = id;
            var tr = '<tr name=' + mainId + ' data-value=' + mainId + '>'
                     + '<td>' + titleM + '</td>'
                     + '<td colspan=5> ';

            if (active) {
                sku.find("label.active,label.unactive").each(function () {
                    var skuId = $(this).attr("id");
                    var titleS = $(this).find("span").html();
                    var td = '<div class="fuhe_color clearfix" name=' + skuId + ' data-value=' + skuId + '>'
                                + '<span class="f_l w58">' + titleS + '</span>'
                                + '<input type="text" name="BarCode" class="bor f_l" />'
                                + '<input type="text" name="Price" class="bor f_l" value="' + basePrice + '"/>'
                                + '<input type="text" name="AlarmStockQty" class="bor f_l" />'
                                + '<a href="javascript:void(0)" class="close" onclick="DeleteSku(this,\'' + mainId + '\',\'' + skuId + '\')"></a>'
                                + '<p class="wrong_tips_h0 w58" style="margin-left:-10px;height:1px;"></p><p class="wrong_tips_h0 color_red t_l BarCode"></p><p class="wrong_tips_h0 color_red t_l Price"></p><p class="wrong_tips_h0 color_red t_l AlarmStockQty"></p></div>';

                    tr += td;
                });

                sku.find("div.add_success_li").each(function () {
                    var skuId = $(this).attr("id");
                    var titleS = $(this).find("p.traditional").html();
                    var td = '<div class="fuhe_color clearfix" name=' + skuId + ' data-value=' + skuId + '>'
                                + '<span class="f_l w58">' + titleS + '</span>'
                                + '<input type="text" name="BarCode" class="bor f_l" />'
                                + '<input type="text" name="Price" class="bor f_l" value="' + basePrice + '"/>'
                                + ' <input type="text" name="AlarmStockQty" class="bor f_l" />'
                                + '<a href="javascript:void(0)" class="close" onclick="DeleteSku(this,\'' + mainId + '\',\'' + skuId + '\')"></a>'
                                + '   <p class="wrong_tips_h0 w58" style="margin-left:-10px;height:1px;"></p><p class="wrong_tips_h0 color_red t_l BarCode"></p><p class="wrong_tips_h0 color_red t_l Price"></p><p class="wrong_tips_h0 color_red t_l AlarmStockQty"></p></div>';

                    tr += td;
                });

                trHtml += tr;

                $("#skuInfo table").append(trHtml);
            }
            else {
                $("#skuInfo table").find("tr[name='" + mainId + "']").remove();
            }
        }
        else {
            var skuId = id;
            var titleS = name;

            if (active) {
                main.find("label.active,label.unactive").each(function () {

                    var mainId = $(this).attr("id");
                    var obj = $("#skuInfo").find("tr[name='" + mainId + "']");
                    if (obj.length > 0) {

                        var td = '<div class="fuhe_color clearfix" name="' + skuId + '" data-value="' + skuId + '">'
                                    + '<span class="f_l w58">' + titleS + '</span>'
                                    + '<input type="text" name="BarCode" class="bor f_l" />'
                                    + '<input type="text" name="Price" class="bor f_l" value="' + basePrice + '"/>'
                                    + '<input type="text" name="AlarmStockQty" class="bor f_l" />'
                                    + '<a href="javascript:void(0)" class="close" onclick="DeleteSku(this,\'' + mainId + '\',\'' + skuId + '\')"></a>'
                                    + '   <p class="wrong_tips_h0 w58" style="margin-left:-10px;height:1px;"></p><p class="wrong_tips_h0 color_red t_l BarCode"></p><p class="wrong_tips_h0 color_red t_l Price"></p><p class="wrong_tips_h0 color_red t_l AlarmStockQty"></p></div>';
                        obj.find("td:eq(1)").append(td);
                    }
                    else {
                        var titleM = $(this).find("span").html();
                        var trHtml = "";
                        var tr = '<tr name=' + mainId + ' data-value=' + mainId + '>'
                                 + '<td>' + titleM + '</td>'
                                 + '<td colspan=5> ';
                        sku.find("label.active,label.unactive").each(function () {
                            var skuId = $(this).attr("id");
                            var titleS = $(this).find("span").html();

                            var td = '<div class="fuhe_color clearfix" name=' + skuId + '  data-value=' + skuId + '>'
                                        + '<span class="f_l w58">' + titleS + '</span>'
                                        + '<input type="text" name="BarCode" class="bor f_l" />'
                                        + '<input type="text" name="Price" class="bor f_l" value="' + basePrice + '"/>'
                                        + '<input type="text" name="AlarmStockQty" class="bor f_l" />'
                                        + '<a href="javascript:void(0)" class="close" onclick="DeleteSku(this,\'' + mainId + '\',\'' + skuId + '\')"></a>'
                                        + '<p class="wrong_tips_h0 w58" style="margin-left:-10px;height:1px;"></p><p class="wrong_tips_h0 color_red t_l BarCode"></p><p class="wrong_tips_h0 color_red t_l Price"></p><p class="wrong_tips_h0 color_red t_l AlarmStockQty"></p></div>';

                            tr += td;
                        });
                        /*当首次添加自定义的尺码属性时没有遍历*/
                        sku.find("div.add_success_li").each(function () {
                            var skuId = $(this).attr("id");
                            var td = '<div class="fuhe_color clearfix" name=' + skuId + '  data-value=' + skuId + '>'
                                        + '<span class="f_l w58">' + titleS + '</span>'
                                        + '<input type="text" name="BarCode" class="bor f_l" />'
                                        + '<input type="text" name="Price" class="bor f_l" value="' + basePrice + '"/>'
                                        + '<input type="text" name="AlarmStockQty" class="bor f_l" />'
                                        + '<a href="javascript:void(0)" class="close" onclick="DeleteSku(this,\'' + mainId + '\',\'' + skuId + '\')"></a>'
                                        + '<p class="wrong_tips_h0 w58" style="margin-left:-10px;height:1px;"></p><p class="wrong_tips_h0 color_red t_l BarCode"></p><p class="wrong_tips_h0 color_red t_l Price"></p><p class="wrong_tips_h0 color_red t_l AlarmStockQty"></p></div>';

                            tr += td;
                        });
                        /*当首次添加自定义的尺码属性时没有遍历 end*/
                        trHtml += tr;
                        $("#skuInfo table").append(trHtml);
                    }
                });

                main.find("div.add_success_li").each(function () {
                    var mainId = $(this).attr("id");
                    var obj = $("#skuInfo").find("tr[name='" + mainId + "']");
                    if (obj.length > 0) {
                        var td = '<div class="fuhe_color clearfix" name="' + skuId + '" data-value="' + skuId + '">'
                                    + '<span class="f_l w58">' + titleS + '</span>'
                                    + '<input type="text" name="BarCode" class="bor f_l" />'
                                    + '<input type="text" name="Price" class="bor f_l" value="' + basePrice + '"/>'
                                    + ' <input type="text" name="AlarmStockQty" class="bor f_l" />'
                                    + '<a href="javascript:void(0)" class="close" onclick="DeleteSku(this,\'' + mainId + '\',\'' + skuId + '\')"></a></div>';
                        obj.find("td:eq(1)").append(td);
                    }
                    else {
                        var titleM = $(this).find("p.traditional").html();
                        var trHtml = "";
                        var tr = '<tr name=' + mainId + ' data-value=' + mainId + '>'
                                 + '<td>' + titleM + '</td>'
                                 + '<td colspan=5> ';

                        sku.find("label.active,label.unactive").each(function () {
                            var skuId = $(this).attr("id");
                            var titleS = $(this).find("span").html();

                            var td = '<div class="fuhe_color clearfix" name=' + skuId + '  data-value=' + skuId + '>'
                                        + '<span class="f_l w58">' + titleS + '</span>'
                                        + '<input type="text" name="BarCode" class="bor f_l" />'
                                        + '<input type="text" name="Price" class="bor f_l" value="' + basePrice + '"/>'
                                        + '<input type="text" name="AlarmStockQty" class="bor f_l" />'
                                        + '<a href="javascript:void(0)" class="close" onclick="DeleteSku(this,\'' + mainId + '\',\'' + skuId + '\')"></a>'
                                        + '<p class="wrong_tips_h0 w58" style="margin-left:-10px;height:1px;"></p><p class="wrong_tips_h0 color_red t_l BarCode"></p><p class="wrong_tips_h0 color_red t_l Price"></p><p class="wrong_tips_h0 color_red t_l AlarmStockQty"></p></div>';

                            tr += td;
                        });
                        /*当首次添加自定义的尺码属性时没有遍历*/
                        sku.find("div.add_success_li").each(function () {
                            var skuId = $(this).attr("id");
                            var td = '<div class="fuhe_color clearfix" name=' + skuId + '  data-value=' + skuId + '>'
                                        + '<span class="f_l w58">' + titleS + '</span>'
                                        + '<input type="text" name="BarCode" class="bor f_l" />'
                                        + '<input type="text" name="Price" class="bor f_l" value="' + basePrice + '"/>'
                                        + '<input type="text" name="AlarmStockQty" class="bor f_l" />'
                                        + '<a href="javascript:void(0)" class="close" onclick="DeleteSku(this,\'' + mainId + '\',\'' + skuId + '\')"></a>'
                                        + '<p class="wrong_tips_h0 w58" style="margin-left:-10px;height:1px;"></p><p class="wrong_tips_h0 color_red t_l BarCode"></p><p class="wrong_tips_h0 color_red t_l Price"></p><p class="wrong_tips_h0 color_red t_l AlarmStockQty"></p></div>';

                            tr += td;
                        });
                        /*当首次添加自定义的尺码属性时没有遍历 end*/
                        trHtml += tr;
                        $("#skuInfo table").append(trHtml);
                    }
                });
            }
            else {
                $("#skuInfo table").find("div[name='" + skuId + "']").remove();
            }
        }
    }
    else if (skuType == "2") {
        var mainId = id;
        var titleM = name;
        if (active) {
            var tr = '<tr name=' + mainId + ' data-value=' + mainId + '>'
                     + '<td>' + titleM + '</td>'
                     + '<td colspan=4>'
                     + '<div class="fuhe_color clearfix" name=' + mainId + '  data-value=' + mainId + '>'
                     + '<input type="text" name="BarCode" class="bor f_l" />'
                     + '<input type="text" name="Price" class="bor f_l" value="' + basePrice + '"/>'
                     + '<input type="text" name="AlarmStockQty" class="bor f_l" />'
                     + '<a href="javascript:void(0)" class="close" onclick="DeleteSku(this,\'' + mainId + '\',\'' + mainId + '\')"></a>'
                     + '</p><p class="wrong_tips_h0 color_red t_l BarCode"></p><p class="wrong_tips_h0 color_red t_l Price"></p><p class="wrong_tips_h0 color_red t_l AlarmStockQty"></p></div>';

            $("#skuInfo table").append(tr);
        }
        else {
            $("#skuInfo table").find("tr[name='" + mainId + "']").remove();
        }

        if ($("#skuInfo tbody tr").length > 0) {
            $("#skuInfo").show();
        }
        else {
            $("#skuInfo").hide();
        }
    }

}

function EditValue(obj, addAmount, maxLength, maxItems) {
    if (maxLength == undefined) {
        maxLength = 200;
    }
    var divId = $(obj).parent().attr("id");

    var traditional = $("#" + divId).find("p.traditional").html();
    var simplified = $("#" + divId).find("p.simplified").html();
    var english = $("#" + divId).find("p.english").html();

    var windowsId = divId.substring(0, divId.lastIndexOf('-'));

    if ($("#" + windowsId).length > 0) {

        //$(obj).parent().remove();

        $("#" + windowsId).show();
        $("#" + windowsId).parent().show();

        $("#" + windowsId).find("textarea[name='Traditional']").val(traditional);
        $("#" + windowsId).find("textarea[name='Simplified']").val(simplified);
        $("#" + windowsId).find("textarea[name='English']").val(english);

        $("#" + windowsId).find("span[name='Traditional']").val(traditional);
        $("#" + windowsId).find("span[name='Simplified']").val(simplified);
        $("#" + windowsId).find("span[name='English']").val(english);
    }
    else {
        var html = "";

        html = "<div id=\"" + windowsId + "\" class=\"add_window\" style=\"display:none\"> "
                    + " <i class=\"wind_jj\"></i>"
                    + " <div class=\"bor_green\">"
                    + "  <div class=\"wrap_window\">"
                    + "<table class=\"\">  "
                    + " <tr> "
                    //+ "  <td class=\"vtop t_c bg_eee\" width=\"55\" style=\"border-right-color: #e5e5e5!important;\"> <p>繁体 :</p></td> "
                    //+ "  <td style=\"border-left-color: #e5e5e5!important;\"><div class=\"div_area\"><span name=\"Traditional\">" + traditional + "</span><textarea name=\"Traditional\" onkeyup=\"SwapTxt('" + windowsId + "','Traditional')\">" + traditional + "</textarea></div></td>  "
                    + " </tr>"
                    + " <tr> "
                    //+ "  <td class=\"vtop t_c bg_eee\" style=\"border-right-color: #e5e5e5!important;\"><p>简体 :</p></td>  "
                    + "  <td style=\"border-left-color: #e5e5e5!important;\"><div class=\"div_area\"><span  name=\"Simplified\">" + simplified + "</span><textarea name=\"Simplified\"  onkeyup=\"SwapTxt('" + windowsId + "','Simplified')\">" + simplified + "</textarea></div></td>  "
                    + " </tr>"
                    + " <tr> "
                    //+ "  <td class=\"vtop t_c bg_eee\" style=\"border-right-color: #e5e5e5!important;\"><p>英文 :</p></td>  "
                    //+ "  <td style=\"border-left-color: #e5e5e5!important;\"><div class=\"div_area\"><span  name=\"English\" >" + english + "</span><textarea name=\"English\"  onkeyup=\"SwapTxt('" + windowsId + "','English')\">" + english + "</textarea></div></td>  "
                    + " </tr>"
                    + "</table> "
                    + "<p class=\"wrong_tips color_red\"></p> "
                    + "<div class=\"t_c pd_t20\"><a href=\"javascript:void(0)\" class=\"btn btn_ok\" onclick=\"SubmitTips('name','" + windowsId + "','1','1','0'," + maxLength + "," + maxItems + ")\">确定</a><a href=\"javascript:void(0)\" class=\"btn btn_cancel mg_l5\" onclick=\"HideAddTips('" + windowsId + "')\">取消</a></div>  "
                    + "  </div> "
                    + " </div>  "
                    + "</div>";

        var aObj = $(obj);
        var pop = $("#" + windowsId);
        if (pop.length < 1) {
            aObj.after(html);
            pop = $("#" + windowsId);
        }


        var msgObj = pop.find("div:eq(0)");

        var x = aObj.offset().left;
        var y = aObj.offset().top;

        pop.show();

        //var msgW = msgObj.width();
        //var msgH = msgObj.height();

        //x -= (msgW - 145 - aObj.width() / 2);
        //if (type == 1) {
        //    y -= (msgH - 135 - msgObj.height() / 2);
        //}
        //else {
        //    y -= (msgH - 65 - msgObj.height() / 2);
        //}

        //zIndex = zIndex + 1;
        //pop.css("left", x).css("top", y);
        //pop.css("z-index", zIndex);
    }
}

var zIndex = 10000;
function ShowAddTips(obj, type, key, addAmount, skutype, maxLength, maxItems) {
    if (maxLength == undefined) {
        maxLength = 200;
    }

    var windowsId = skutype + "-" + key;
    var html = "";
    if (type == 1) {
        html = "<div id=\"" + windowsId + "\" class=\"add_window\" style=\"display:none\"> "
                    + " <i class=\"wind_jj\"></i>"
                    + " <div class=\"bor_green\">"
                    + "  <div class=\"wrap_window\">"
                    + "<table class=\"\">  "
                    + " <tr> "
                    //+ "  <td class=\"vtop t_c bg_eee\" width=\"55\" style=\"border-right-color: #e5e5e5!important;\"> <p>繁体 :</p></td> "
                    //+ "  <td style=\"border-left-color: #e5e5e5!important;\"><div class=\"div_area\"><span  name=\"Traditional\"></span><textarea name=\"Traditional\" onkeyup=\"SwapTxt('" + windowsId + "','Traditional')\"></textarea></div></td>  "
                    + " </tr>"
                    + " <tr> "
                    //+ "  <td class=\"vtop t_c bg_eee\" style=\"border-right-color: #e5e5e5!important;\"><p>简体 :</p></td>  "
                    + "  <td style=\"border-left-color: #e5e5e5!important;\"><div class=\"div_area\"><span  name=\"Simplified\"></span><textarea name=\"Simplified\"  onkeyup=\"SwapTxt('" + windowsId + "','Simplified')\"></textarea></div></td>  "
                    + " </tr>"
                    + " <tr> "
                    //+ "  <td class=\"vtop t_c bg_eee\" style=\"border-right-color: #e5e5e5!important;\"><p>英文 :</p></td>  "
                    //+ "  <td style=\"border-left-color: #e5e5e5!important;\"><div class=\"div_area\"><span  name=\"English\" ></span><textarea name=\"English\"  onkeyup=\"SwapTxt('" + windowsId + "','English')\"></textarea></div></td>  "
                    + " </tr>"
                    + "</table> "
                    + "<p class=\"wrong_tips color_red\"></p> "
                    + "<div class=\"t_c pd_t20\"><a href=\"javascript:void(0)\" class=\"btn btn_ok\" onclick=\"SubmitTips('" + key + "','" + windowsId + "','" + type + "','" + addAmount + "','" + skutype + "'," + maxLength + "," + maxItems + ")\">确定</a><a href=\"javascript:void(0)\" class=\"btn btn_cancel mg_l5\" onclick=\"HideAddTips('" + windowsId + "')\">取消</a></div>  "
                    + "  </div> "
                    + " </div>  "
                    + "</div>";
    }
    else {
        html = "<div id=\"" + windowsId + "\" class=\"add_window\" style=\"display: none;\"> "
                 + "<i class=\"wind_jj\"></i>"
                 + "<div class=\"bor_green\">"
                 + "<div class=\"wrap_window wrap_window2 clearfix\">"
                 + "<input type=\"text\" class=\"f_l right\">"
                 + "<div class=\"t_c f_l\"><a href=\"javascript:void(0)\" class=\"btn btn_ok mg_l5\" onclick=\"SubmitTips('" + key + "','" + windowsId + "','" + type + "','" + addAmount + "','" + skutype + "'," + maxLength + "," + maxItems + ")\">确定</a>"
                 + "<a href=\"javascript:void(0)\" class=\"btn btn_cancel mg_l5\" onclick=\"HideAddTips('" + windowsId + "')\">取消</a></div>"
                 + "<p class=\"wrong_tips color_red\"></p> "
                 + "</div></div></div>";
    }

    var aObj = $(obj);
    var pop = $("#" + windowsId);
    if (pop.length < 1) {
        aObj.after(html);
        pop = $("#" + windowsId);
    }
    var msgObj = pop.find("div:eq(0)");

    var x = aObj.offset().left;
    var y = aObj.offset().top;

    pop.show();

    var msgW = msgObj.width();
    var msgH = msgObj.height();

    x -= (msgW - 145 - aObj.width() / 2);
    if (type == 1) {
        y -= (msgH - 135 - msgObj.height() / 2);
    }
    else {
        y -= (msgH - 65 - msgObj.height() / 2);
    }

    zIndex = zIndex + 1;
    pop.css("left", x).css("top", y);
    pop.css("z-index", zIndex);
};

function SwapTxt(windowsId, textarea) {
    var obj = $("#" + windowsId).find("textarea[name='" + textarea + "']");
    var txt = obj.val();
    obj.parent().find("span").html(txt);
}

function SubmitTips(key, windowsId, type, addAmount, skutype, maxLength, maxItems) {
    if (maxLength == undefined) {
        maxLength = 200;
    }
    var nameId = "";
    $("#" + windowsId).find("p.wrong_tips").html("");
    $("#" + windowsId).find(".error").removeClass("error");

    if (!$("#" + windowsId).parents("td").hasClass("Require") && $("#" + windowsId).find("textarea[name='Traditional']").val() == "") {
        var pop = $("#" + windowsId).hide();
        return true;
    }

    var name = "";
    if (type == 1) {
        //var traditional = $("#" + windowsId).find("textarea[name='Traditional']").val();
        //var simplified = $("#" + windowsId).find("textarea[name='Simplified']").val();
        //var english = $("#" + windowsId).find("textarea[name='English']").val();
        var traditional = "";
        var simplified = $("#" + windowsId).find("textarea[name='Simplified']").val();
        var english = "";


        name = traditional;
        //if (traditional == "") {
        //    $("#" + windowsId).find("p.wrong_tips").html("中文繁体不能为空");
        //    $("#" + windowsId).find("textarea[name='Traditional']").parent().parent().parent().addClass("error");
        //    $("#" + windowsId).find("div.wrap_window").addClass("error");
        //    return false;
        //}
        //else if (traditional.length > maxLength) {
        //    $("#" + windowsId).find("p.wrong_tips").html("最多可输入" + maxLength + "个字符");
        //    $("#" + windowsId).find("textarea[name='Traditional']").parent().parent().parent().addClass("error");
        //    $("#" + windowsId).find("div.wrap_window").addClass("error");
        //    return false;
        //}
        if (simplified == "") {
            $("#" + windowsId).find("p.wrong_tips").html("中文简体不能为空");
            $("#" + windowsId).find("textarea[name='Simplified']").parent().parent().parent().addClass("error");
            $("#" + windowsId).find("div.wrap_window").addClass("error");
            return false;
        }
        else if (simplified.length > maxLength) {
            $("#" + windowsId).find("p.wrong_tips").html("最多可输入" + maxLength + "个字符");
            $("#" + windowsId).find("textarea[name='Simplified']").parent().parent().parent().addClass("error");
            $("#" + windowsId).find("div.wrap_window").addClass("error");
            return false;
        }
        //else if (english == "") {
        //    $("#" + windowsId).find("p.wrong_tips").html("英文不能为空");
        //    $("#" + windowsId).find("textarea[name='English']").parent().parent().parent().addClass("error");
        //    $("#" + windowsId).find("div.wrap_window").addClass("error");
        //    return false;
        //}
        //else if (english.length > maxLength) {
        //    $("#" + windowsId).find("p.wrong_tips").html("最多可输入" + maxLength + "个字符");
        //    $("#" + windowsId).find("textarea[name='English']").parent().parent().parent().addClass("error");
        //    $("#" + windowsId).find("div.wrap_window").addClass("error");
        //    return false;
        //}
        nameId = windowsId + "-" + simplified + "-" + type;
        var html = '<div class="f_l add_success_li mg_r20" id="' + nameId + '">'
                   + ' <ul class="add_success_wrap f_l fff_bor mg_t5">'
                   //+ '    <li class=" clearfix icon_f"><span class="f_l left"></span>'
                   //+ '       <p class="f_l right traditional">' + traditional + '</p>'
                   //+ '     </li>'
                   + '      <li class=" clearfix icon_j">'
                   + '       <p class="f_l right simplified">' + simplified + '</p>'
                   + '   </li>'
                   //+ '   <li class=" clearfix icon_e"><span class="f_l left"></span>'
                   //+ '        <p class="f_l right english">' + english + '</p>'
                   //+ '      </li>'
                   + '   </ul>';
        if (key == "name") {
            html += '  <a href="javascript:void(0)" class="btn_edit" onclick="EditValue(this,' + addAmount + ',' + maxLength + ',' + maxItems + ')"></a>';
        }
        else {
            html += '  <a href="javascript:void(0)" class="close" onclick="DeleteValue(this,' + addAmount + ',' + maxItems + ')"></a>'
        }
        html += '</div>';
        if (addAmount > 1) {
            $("#" + windowsId).parent().parent().parent().find("div.add_success_list").append(html);
        }
        else {
            $("#" + windowsId).parent().parent().parent().find("div.add_success_list").append(html);
            if (key == "name") {
                $("#" + windowsId).parent().remove();
            }
        }
        var length = $("#" + windowsId).parent().parent().parent().find("div.add_success_li").length;
        if (length >= maxItems) {
            $("#" + windowsId).parent().hide();
        }
    }

    else {
        var value = $("#" + windowsId).find("input").val();
        var temp = /^\d+(\.\d+)?$/;
        nameId = windowsId + "-" + value + "-" + type;

        if (value == "") {
            $("#" + windowsId).find("p.wrong_tips").html("请输入正确的内容").show();
            $("#" + windowsId).find("div.wrap_window").addClass("error");
            return false;
        }
        if (value.length > maxLength) {
            $("#" + windowsId).find("p.wrong_tips").html("最多可输入" + maxLength + "位数字").show();
            $("#" + windowsId).find("div.wrap_window").addClass("error");
            return false;
        }
        else if (!temp.test(value)) {
            $("#" + windowsId).find("p.wrong_tips").html("请输入正确的数字").show();
            $("#" + windowsId).find("div.wrap_window").addClass("error");
            return false;
        }
        else {
            value = parseFloat(value).toFixed(2);
        }
        name = value;
        var html = '<label class="add_success f_l" id="' + nameId + '"><span class="block bor f_l fff_bor mg_l20 h25" >' + value + '</span><a href="javascript:void(0)" class="close f_l"  onclick="DeleteValue(this,' + addAmount + ',' + maxItems + ')"></a></label>';

        if (addAmount > 1) {
            $("#" + windowsId).parent().parent().find("div.add_success_label").find("div.add_success_li").remove();
            $("#" + windowsId).parent().parent().find("div.add_success_label").append(html);
        }
        else {
            $("#" + windowsId).parent().parent().find("div.add_success_label").append(html);
            $("#" + windowsId).parent().hide();
        }
        var length = $("#" + windowsId).parent().parent().parent().find("label.add_success").length;
        if (length >= maxItems) {
            $("#" + windowsId).parent().hide();
        }
    }

    var pop = $("#" + windowsId).hide();
    $("#" + windowsId + " input,#" + windowsId + " textarea").val("");

    // 如果是SKU 属性，则增加相关的sku
    if (skutype == "main" || skutype == "sku") {

        UpdateSkuInfo(nameId, name, true);
    }
}

function HideAddTips(windowsId) {
    var pop = $("#" + windowsId).hide();
}

function BindLiChange(ul, func) {
    ul.delegate("li", "click", function () {
        var li = $(this);
        var old;
        if (li.hasClass("current")) {
            return;
        }
        else {
            old = ul.find("li.current");
            old.removeClass("current");
            li.addClass("current");
        }
        func(li, old);
    });
}

function Save(type) {
    if (type == 1) {
        // 暂存 
        if (Validite(type)) {
            SaveSubmit(false);
        }
    }
    else {

        if (Validite(type)) {
            // 上传
            SaveSubmit(true);
        }
    }
}

// 上传
function SaveSubmit(isRelease) {
    var json = {
        SPU: getSpuBaseJson(),
        SKUS: getSkuJson(),
        SpuEx: getSpuExJson()
    }
    var action = $("#action").val();
    $.ajax({
        type: 'POST',
        url: "/Product/SaveProduct",
        data: { productInfo: JSON.stringify(json), isRelease: isRelease, action: action },
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.data == 1) {
                if (isRelease) {
                    window.location.href = "/Product/UploadSuccess?oper=" + $("#action").val();
                }
                else {
                    $.dialog({
                        width: 490,
                        title: "提示",
                        content: $("#saveDraftSuccess").html(),
                        buttons: [
                            {
                                text: "查看未上传商品列表",
                                width: "w178",
                                isWhite: 1,
                                onClick: function () {
                                    window.location.href = "/Product/ProductList";
                                    return true;
                                }
                            },
                            {
                                text: "继续编辑",
                                onClick: function () {
                                    var categoryId = $("#CategoryId").val();
                                    var spu = data.Spu

                                    window.location.href = "/Product/ProductUpload?categoryId=" + categoryId + "&spu=" + spu;
                                    return true;
                                }
                            }]
                    });
                }
            }
            else {
                if (isRelease) {
                    $.dialog({
                        width: 490,
                        title: "上传失败",
                        content: $("#uploadError").html()
                    });
                }
                else {
                    $.dialog({
                        width: 490,
                        title: "暂存失败",
                        content: $("#saveDraftError").html()
                    });
                }
            }
        }
    });
}

// 验证重复元素，有重复返回true；否则返回false
function isRepeat(arr) {
    var hash = {};
    for (var i in arr) {
        if (hash[arr[i]]) {
            return true;
        }
        // 不存在该元素，则赋值为true，可以赋任意值，相应的修改if判断条件即可
        hash[arr[i]] = true;
    }
    return false;
}

function isRepeatDB(barcodes, spu) {
    var result = false;
    $.ajax({
        type: 'POST',
        url: "/Product/BarCodeRepeat",
        data: { BarCode: barcodes, Spu: spu },
        async: false,
        dataType: "json",
        success: function (data) {
            result = data.data;
        }
    });
    return result;
}

function Validite(type) {
    if (type == 1) {
        var name = $("#name").find("div.add_success_list").find("p.traditional").html();
        if (name == "" || name == undefined) {
            $.dialog("请输入产品名称！");
            return false;
        }
        return true;
    }
    else {
        var result = true;
        if ($("#images").find("input[name='file']").length < 1) {
            result = false;
            $("#images").parent().find("p.wrong_tips").show();
        } else {
            $("#images").parent().find("p.wrong_tips").hide();
        }
        //if ($("#content1").val() == "" || $("#content2").val() == "" || $("#content3").val() == "") {
        //    result = false;
        //    $("#content1").parent().parent().find("p.wrong_tips").show();
        //}
        //1繁体  2简体  3英文
        if ($("#content2").val() == "") {
            result = false;
            $("#content2").parent().parent().find("p.wrong_tips").show();
        }
        else {
            $("#content2").parent().parent().find("p.wrong_tips").hide();
        }

        if ($("#BrandIdSel").val() == 0) {
            $("#BrandIdSel").parent().next().show();
        }
        else {
            $("#BrandIdSel").parent().next().hide();
        }

        $(".Require").each(function () {
            var obj = $(this);
            if (obj.find("input").length > 0 && obj.find("select").length < 1) {
                var objId = obj.find("input").attr("id");
                if (objId == "Price") {
                    var temp = /^\d+(\.\d+)?$/;
                    var price = $("#Price").val();
                    if (price == "") {
                        result = false;
                        obj.find("p.wrong_tips").show();
                        obj.find("input").parent().addClass("error");
                    }
                    else if (!temp.test(price) || price.length > 10) {
                        result = false;
                        obj.find("p.wrong_tips").show().html("商品价格必须为10位以下的数字");
                        obj.find("input").parent().addClass("error");
                    }
                    else {
                        obj.find("p.wrong_tips").hide();
                        obj.find("input").parent().removeClass("error");
                    }
                }
                else if (objId == "CommissionInHK") {
                    var display = $('#commissionHK').css('display');
                    if (display != 'none') {
                        if (obj.find("input").val() == "") {
                            result = false;
                            obj.find("p.wrong_tips").show();
                            obj.find("input").parent().addClass("error");
                        }
                        else {
                            obj.find("p.wrong_tips").hide();
                            obj.find("input").parent().removeClass("error");
                        }
                    }
                }
                else if (objId == "CommissionInCHINA") {
                    var display = $('#commissionCHINA').css('display');
                    if (display != 'none') {
                        if (obj.find("input").val() == "") {
                            result = false;
                            obj.find("p.wrong_tips").show();
                            obj.find("input").parent().addClass("error");
                        }
                        else {
                            obj.find("p.wrong_tips").hide();
                            obj.find("input").parent().removeClass("error");
                        }
                    }
                }
                else {
                    if (obj.find("input").val() == "") {
                        result = false;
                        obj.find("p.wrong_tips").show();
                        obj.find("input").parent().addClass("error");
                    }
                    else {
                        obj.find("p.wrong_tips").hide();
                        obj.find("input").parent().removeClass("error");
                    }
                }
            }
            else if (obj.find("select").length > 0) {
                var selecttext = obj.find("select").find("option:selected").text();
                var li = obj.find(".add_success_list").find("div.add_success_li").length;
                var label = obj.find(".add_success_list").find("label.add_success").length;
                if ((obj.find("select").val() == 0 || selecttext.indexOf("其它") > -1) && (li <= 0 && label <= 0)) {
                    result = false;
                    obj.find("p.wrong_tips").show();
                    obj.find("select").parent().addClass("error");
                }
                else {
                    obj.find("p.wrong_tips").hide();
                    obj.find("select").parent().removeClass("error");
                }
            }
            else {
                var isShow = true;
                obj.find("div.RequireValue").each(function () {
                    if ($(this).find("label.active,label.unactive").length > 0) {
                        isShow = false;
                        return;
                    }
                    if ($(this).find("select").length > 0) {
                        isShow = false;
                        return;
                    }
                    if ($(this).find("div.add_success_li").length > 0) {
                        isShow = false;
                        return;
                    }
                    if ($(this).find("label.add_success").length > 0) {
                        isShow = false;
                        return;
                    }
                })
                if (isShow) {
                    result = false;
                    obj.find("p.wrong_tips").show();
                }
                else {
                    obj.find("p.wrong_tips").hide();
                }
            }
        })

        var skutype = $("#skuInfoType").val();
        if (skutype == "0") {
            var spu = $("#Spu").val();
            var barcode = $("#BarCode").val();
            var temp = /^[A-Za-z0-9]+$/;
            if (barcode == undefined || barcode == "") {
                result = false;
                $("#BarCode").parent().find("p.wrong_tips").html("请输入条形码").show();
            }
            if (barcode.length > 20) {
                result = false;
                $("#BarCode").parent().find("p.wrong_tips").html("条形码的长度不能超过20位").show();
            }
            if (!temp.test(barcode)) {
                result = false;
                $("#BarCode").parent().find("p.wrong_tips").html("条形码必须由字母和数字组成").show();
            }
            if (isRepeatDB(barcode, spu)) {
                result = false;
                $("#BarCode").parent().find("p.wrong_tips").html("条形码重複,请重新输入").show();
            }
        }
        else {
            var arr = new Array();
            $("#skuInfo").find("input").each(function () {
                var obj = $(this);
                var name = obj.attr("name");
                if (name == "BarCode") {

                    var temp = /^[A-Za-z0-9]+$/;;
                    if (obj.val() == "") {
                        result = false;
                        obj.parent().find("p.BarCode").addClass("wrong_tips_h1").html("请输入条形码");
                    }
                    else if (obj.val().length > 20) {
                        result = false;
                        obj.parent().find("p.BarCode").addClass("wrong_tips_h1").html("条形码的长度不能超过20位");
                    }
                    else if (!temp.test(obj.val())) {
                        result = false;
                        obj.parent().find("p.BarCode").addClass("wrong_tips_h1").html("条形码必须由字母和数字组成").show();
                    }
                    else {
                        arr.push(obj.val());
                        obj.parent().find("p.BarCode").removeClass("wrong_tips_h1");
                    }
                }
                else if (name == "AlarmStockQty") {
                    var temp = /^[0-9]*[1-9][0-9]*$/;
                    if (obj.val() == "") {
                        result = false;
                        obj.parent().find("p.AlarmStockQty").addClass("wrong_tips_h1").html("请输入库存预警");
                    }
                    else if (!temp.test(obj.val()) || obj.val().length > 10) {
                        result = false;
                        obj.parent().find("p.AlarmStockQty").addClass("wrong_tips_h1").html("库存预警必须为10位以内的整数");
                    }

                    else {
                        obj.parent().find("p.AlarmStockQty").removeClass("wrong_tips_h1");
                    }
                }
            });


            var trs = $("#skuInfo table tbody").find("tr").length;
            $("#skuInfo").parent().parent().find("p.wrong_tips").html("");
            if (trs < 1) {
                result = false;
                $("#skuInfo").parent().parent().find("p.wrong_tips").html("请添加SKU").show();
            }

            if (arr.length > 0) {
                if (isRepeat(arr)) {
                    result = false;
                    $("#skuInfo").parent().parent().find("p.wrong_tips").html("条形码重複,请重新输入").show();
                }
                else {
                    var barcodes = arr.join(",");
                    var spu = $("#Spu").val();
                    if (isRepeatDB(barcodes, spu)) {
                        result = false;
                        $("#skuInfo").parent().parent().find("p.wrong_tips").html("条形码重複,请重新输入").show();
                    }
                    else {
                        $("#skuInfo").parent().parent().find("p.wrong_tips").hide();
                    }
                }
            }
        }
        return result;

    }
}

function getSpuBaseJson() {
    // 销售区域
    var salesterritory = 0;
    if ($("#SalesTerritory").find("label.active").length < 1) {
        salesterritory = 0;
    }
    if ($("#SalesTerritory").find("label.active").length == 1) {
        salesterritory = $("#SalesTerritory").find("label.active").attr("data-value");
    }
    if ($("#SalesTerritory").find("label.active").length >= 2) {
        salesterritory = 3;
    }
    // 产品图片
    var images = new Array();
    var sort = 0;
    $("#images").find("input[name='file']").each(function () {
        var image = {};
        image.path = $(this).val();
        image.SortId = sort + 1;

        images[sort] = image;

        sort = sort + 1;
    });

    var temp = /^\d+(\.\d+)?$/;
    var price = $("#Price").val() == "" || !temp.test($("#Price").val()) ? 0.00 : parseFloat($("#Price").val());
    // 商品标签
    var tags = new Array();
    $("#Tag").find("div.add_success_li").each(function () {
        var tag = {
            "Content_S": $(this).find("p.simplified").html(),
            "Content_T": $(this).find("p.traditional").html(),
            "Content_E": $(this).find("p.english").html()
        }

        tags.push(tag);
    });
    var isDutyOnSeller = $("#IsDutyOnSeller").hasClass("active") ? 1 : 0;

    var json = {
        "Spu": $("#Spu").val(),
        "CategoryId": $("#CategoryId").val(),
        "Name": {
            "Content_S": tranUndefined($("#name").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#name").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#name").find("div.add_success_list").find("p.english").html())
        },
        "Tag": tags,
        "Price": price,
        "Description": {
            "Content_S": $("#content2").val(),
            "Content_T": $("#content1").val(),
            "Content_E": $("#content3").val()
        },
        "BrandId": $("#BrandIdSel").val(),
        "CountryOfManufactureId": $("#CountryOfManufactureSel").val(),
        "SalesTerritory": salesterritory,
        "Unit": {
            "Key": $("#UnitSel").val(),
            "Other": {
                "Content_S": tranUndefined($("#Unit").find("div.add_success_list").find("p.simplified").html()),
                "Content_T": tranUndefined($("#Unit").find("div.add_success_list").find("p.traditional").html()),
                "Content_E": tranUndefined($("#Unit").find("div.add_success_list").find("p.english").html()),
            }
        },
        "IsExchangeInCHINA": $("#IsExchangeInCHINA").val(),
        "IsExchangeInHK": $("#IsExchangeInHK").val(),
        "IsReturn": $('input[name="IsReturn"]:checked').val(),
        "MinForOrder": $("#MinForOrder").val() == "" ? 0 : $("#MinForOrder").val(),
        "NetWeightUnitId": tranUndefined($("#NetWeightUnit").val()),
        "NetContentUnitId": tranUndefined($("#NetContentUnit").val()),
        "IsDutyOnSeller": isDutyOnSeller,
        "Images": images,
        "CommissionInCHINA": $("#CommissionInCHINA").val() == "" ? 0 : $("#CommissionInCHINA").val(),
        "CommissionInHK": $("#CommissionInHK").val() == "" ? 0 : $("#CommissionInHK").val(),
        "PreOnSaleTime": $("#PreOnSaleTime").val()
    }

    return json;

}

function getSpuExJson() {
    var languagekeys = new Array();
    $("#SupportedLanguage").find("label.active").each(function () {
        key = $(this).attr("data-value");
        key = key.split('-')[2];
        languagekeys.push(key);
    });
    var languageothers = new Array();
    $("#SupportedLanguage").find("div.add_success_li").each(function () {
        var other = {
            "Content_S": $(this).find("p.simplified").html(),
            "Content_T": $(this).find("p.traditional").html(),
            "Content_E": $(this).find("p.english").html()
        }

        languageothers.push(other);
    });

    var json = {
        "Materials": {
            "Key": $("#MaterialsSel").val() == "" || $("#MaterialsSel").val() == undefined ? 0 : $("#MaterialsSel").val(),
            "Other": {
                "Content_S": tranUndefined($("#Materials").find("div.add_success_list").find("p.simplified").html()),
                "Content_T": tranUndefined($("#Materials").find("div.add_success_list").find("p.traditional").html()),
                "Content_E": tranUndefined($("#Materials").find("div.add_success_list").find("p.traditional").html())
            }
        },
        "Pattern": {
            "Content_S": tranUndefined($("#Pattern").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#Pattern").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#Pattern").find("div.add_success_list").find("p.english").html())
        },
        "Flavour": {
            "Content_S": tranUndefined($("#Flavour").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#Flavour").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#Flavour").find("div.add_success_list").find("p.english").html())
        },
        "Ingredients": {
            "Content_S": tranUndefined($("#Ingredients").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#Ingredients").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#Ingredients").find("div.add_success_list").find("p.english").html())
        },
        "StoragePeriod": {
            "Content_S": tranUndefined($("#StoragePeriod").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#StoragePeriod").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#StoragePeriod").find("div.add_success_list").find("p.english").html())
        },
        "StoringTemperature": {
            "Content_S": tranUndefined($("#StoringTemperature").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#StoringTemperature").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#StoringTemperature").find("div.add_success_list").find("p.english").html())
        },
        "SkinType": {
            "Content_S": tranUndefined($("#SkinType").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#SkinType").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#SkinType").find("div.add_success_list").find("p.english").html())
        },
        "GenderId": $("#GenderIdSel").val() == "" || $("#GenderIdSel").val() == undefined ? 0 : $("#GenderIdSel").val(),
        "AgeGroup": {
            "Content_S": tranUndefined($("#AgeGroup").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#AgeGroup").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#AgeGroup").find("div.add_success_list").find("p.english").html())
        },
        "Model": {
            "Content_S": tranUndefined($("#Model").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#Model").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#Model").find("div.add_success_list").find("p.english").html())
        },
        "BatteryTime": {
            "Content_S": tranUndefined($("#BatteryTime").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#BatteryTime").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#BatteryTime").find("div.add_success_list").find("p.english").html())
        },
        "Voltage": {
            "Content_S": tranUndefined($("#Voltage").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#Voltage").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#Voltage").find("div.add_success_list").find("p.english").html())
        },
        "Power": {
            "Content_S": tranUndefined($("#Power").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#Power").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#Power").find("div.add_success_list").find("p.english").html())
        },
        "Warranty": {
            "Content_S": tranUndefined($("#Warranty").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#Warranty").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#Warranty").find("div.add_success_list").find("p.english").html())
        },
        "SupportedLanguage": {
            "keys": languagekeys,
            "Others": languageothers
        },
        "PetAgeUnit": {
            "Key": $("#PetAgeUnit").val() == "" || $("#PetAgeUnit").val() == undefined ? 0 : $("#PetAgeUnit").val()
        },
        "PetType": {
            "Key": $("#PetTypeSel").val() == "" || $("#PetTypeSel").val() == undefined ? 0 : $("#PetTypeSel").val(),
            "Other": {
                "Content_S": tranUndefined($("#PetType").find("div.add_success_list").find("p.simplified").html()),
                "Content_T": tranUndefined($("#PetType").find("div.add_success_list").find("p.traditional").html()),
                "Content_E": tranUndefined($("#PetType").find("div.add_success_list").find("p.english").html())
            }
        },
        "PetAge": {
            "Content_S": tranUndefined($("#PetAge").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#PetAge").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#PetAge").find("div.add_success_list").find("p.english").html())
        },
        "Location": {
            "Content_S": tranUndefined($("#Location").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#Location").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#Location").find("div.add_success_list").find("p.english").html())
        },
        "Weight": {
            "Content_S": tranUndefined($("#Weight").find("input").val()),
            "Content_T": tranUndefined($("#Weight").find("input").val()),
            "Content_E": tranUndefined($("#Weight").find("input").val())
        },
        "WeightUnit": {
            "Key": $("#WeightUnit").val() == "" || $("#WeightUnit").val() == undefined ? 0 : $("#WeightUnit").val()
        },
        "Volume": {
            "Content_S": tranUndefined($("#Volume").find("input").val()),
            "Content_T": tranUndefined($("#Volume").find("input").val()),
            "Content_E": tranUndefined($("#Volume").find("input").val())
        },
        "VolumeUnit": {
            "Key": $("#VolumeUnit").val() == "" || $("#VolumeUnit").val() == undefined ? 0 : $("#VolumeUnit").val()
        },
        "Length": {
            "Content_S": tranUndefined($("#Length").find("input").val()),
            "Content_T": tranUndefined($("#Length").find("input").val()),
            "Content_E": tranUndefined($("#Length").find("input").val())
        },
        "LengthUnit": {
            "Key": $("#LengthUnit").val() == "" || $("#LengthUnit").val() == undefined ? 0 : $("#LengthUnit").val()
        },
        "Width": {
            "Content_S": tranUndefined($("#Width").find("input").val()),
            "Content_T": tranUndefined($("#Width").find("input").val()),
            "Content_E": tranUndefined($("#Width").find("input").val())
        },
        "WidthUnit": {
            "Key": $("#WidthUnit").val() == "" || $("#WidthUnit").val() == undefined ? 0 : $("#WidthUnit").val()
        },
        "Height": {
            "Content_S": tranUndefined($("#Height").find("input").val()),
            "Content_T": tranUndefined($("#Height").find("input").val()),
            "Content_E": tranUndefined($("#Height").find("input").val())
        },
        "HeightUnit": {
            "Key": $("#HeightUnit").val() == "" || $("#HeightUnit").val() == undefined ? 0 : $("#HeightUnit").val()
        },
        "Flavor": {
            "Content_S": tranUndefined($("#Flavor").find("div.add_success_list").find("p.simplified").html()),
            "Content_T": tranUndefined($("#Flavor").find("div.add_success_list").find("p.traditional").html()),
            "Content_E": tranUndefined($("#Flavor").find("div.add_success_list").find("p.english").html())
        }
    }

    return json;
}

function getSkuJson() {
    var skutype = $("#skuInfoType").val();
    var skus = new Array();
    var netweight = $("#NetWeight").find("div.add_success_label").find("span").html();
    if (netweight == undefined || netweight == "") {
        netweight = 0;
    }
    var netcontent = $("#NetContent").find("div.add_success_label").find("span").html();
    if (netcontent == undefined || netcontent == "") {
        netcontent = 0;
    }
    if (skutype == "1") {
        $("#skuInfo tbody tr").each(function () {
            var objMain = $(this);
            var mainValue = objMain.attr("data-value");

            objMain.find("div.fuhe_color").each(function () {
                var objSku = $(this);
                var skuValue = objSku.attr("data-value");
                var price = objSku.find("input[name='Price']").val() == "" ? 0 : parseFloat(objSku.find("input[name='Price']").val());
                var alarmStockQty = objSku.find("input[name='AlarmStockQty']").val() == "" ? 0 : parseInt(objSku.find("input[name='AlarmStockQty']").val());
                var sku = {
                    "Sku": objSku.find("input[name='Sku']").val(),
                    "Price": price,
                    "BarCode": objSku.find("input[name='BarCode']").val(),
                    "AlarmStockQty": alarmStockQty,
                    "NetWeight": netweight,
                    "NetContent": netcontent,
                    "Color": {
                        "Key": tranUndefined($("#ColorSel").val()),
                        "Other": {
                            "Content_S": tranUndefined($("#Color").find("div.add_success_list").find("p.simplified").html()),
                            "Content_T": tranUndefined($("#Color").find("div.add_success_list").find("p.traditional").html()),
                            "Content_E": tranUndefined($("#Color").find("div.add_success_list").find("p.english").html())
                        }
                    },
                    "Size": {
                        "Key": tranUndefined($("#SizeSel").val()),
                        "Other": {
                            "Content_S": tranUndefined($("#Size").find("div.add_success_list").find("p.simplified").html()),
                            "Content_T": tranUndefined($("#Size").find("div.add_success_list").find("p.traditional").html()),
                            "Content_E": tranUndefined($("#Size").find("div.add_success_list").find("p.english").html())
                        }
                    },
                    "Specifications": {
                        "Content_S": tranUndefined($("#Specifications").find("div.add_success_list").find("p.simplified").html()),
                        "Content_T": tranUndefined($("#Specifications").find("div.add_success_list").find("p.traditional").html()),
                        "Content_E": tranUndefined($("#Specifications").find("div.add_success_list").find("p.english").html())
                    },
                    "AlcoholPercentage": {
                        "Content_S": tranUndefined($("#AlcoholPercentage").find("div.add_success_list").find("p.simplified").html()),
                        "Content_T": tranUndefined($("#AlcoholPercentage").find("div.add_success_list").find("p.traditional").html()),
                        "Content_E": tranUndefined($("#AlcoholPercentage").find("div.add_success_list").find("p.english").html())
                    },
                    "Smell": {
                        "Content_S": tranUndefined($("#Smell").find("div.add_success_list").find("p.simplified").html()),
                        "Content_T": tranUndefined($("#Smell").find("div.add_success_list").find("p.traditional").html()),
                        "Content_E": tranUndefined($("#Smell").find("div.add_success_list").find("p.english").html())
                    },
                    "CapacityRestriction": {
                        "Content_S": tranUndefined($("#CapacityRestriction").find("div.add_success_list").find("p.simplified").html()),
                        "Content_T": tranUndefined($("#CapacityRestriction").find("div.add_success_list").find("p.traditional").html()),
                        "Content_E": tranUndefined($("#CapacityRestriction").find("div.add_success_list").find("p.english").html())
                    },
                };

                mainValue = transforId(mainValue);
                skuValue = transforId(skuValue);
                if (mainValue.split("-").length < 4) {
                    sku.MainDicKey = mainValue.split('-')[1];
                    sku.MainKey = mainValue.split('-')[2];

                }
                else {
                    sku.MainDicKey = mainValue.split('-')[1];
                    var MainValue;
                    if (mainValue.split('-')[3] == "1") {
                        MainValue = {
                            "Content_S": tranUndefined($("#" + mainValue).find("p.simplified").html()),
                            "Content_T": tranUndefined($("#" + mainValue).find("p.traditional").html()),
                            "Content_E": tranUndefined($("#" + mainValue).find("p.english").html())
                        }
                    } else {
                        MainValue = {
                            "Content_S": tranUndefined($("#" + mainValue).find("span").html()),
                            "Content_T": tranUndefined($("#" + mainValue).find("span").html()),
                            "Content_E": tranUndefined($("#" + mainValue).find("span").html())
                        }
                    }
                    sku.MainKey = "";
                    sku.MainValue = MainValue;
                }
                if (skuValue.split("-").length < 4) {
                    sku.SubDicKey = skuValue.split('-')[1];
                    sku.SubKey = skuValue.split('-')[2];
                }
                else {
                    sku.SubDicKey = skuValue.split('-')[1];
                    sku.SubKey = "";
                    var SubValue;
                    if (skuValue.split('-')[3] == "1") {
                        SubValue = {
                            "Content_S": tranUndefined($("#" + skuValue).find("p.simplified").html()),
                            "Content_T": tranUndefined($("#" + skuValue).find("p.traditional").html()),
                            "Content_E": tranUndefined($("#" + skuValue).find("p.english").html())
                        }
                    } else {
                        SubValue = {
                            "Content_S": tranUndefined($("#" + skuValue).find("span").html()),
                            "Content_T": tranUndefined($("#" + skuValue).find("span").html()),
                            "Content_E": tranUndefined($("#" + skuValue).find("span").html())
                        }
                    }
                    sku.SubValue = SubValue;
                }

                skus.push(sku);
            })
        })
    }
    else if (skutype == "2") {
        $("#skuInfo tbody tr").each(function () {
            var objMain = $(this);
            var mainValue = objMain.attr("data-value");
            var MainDicKey = mainValue.split('-')[1];
            var MainKey = 0;

            objMain.find("div.fuhe_color").each(function () {
                var objSku = $(this);
                var skuValue = objSku.attr("data-value");
                var price = objSku.find("input[name='Price']").val() == "" ? 0 : parseFloat(objSku.find("input[name='Price']").val());
                var alarmStockQty = objSku.find("input[name='AlarmStockQty']").val() == "" ? 0 : parseInt(objSku.find("input[name='AlarmStockQty']").val());
                var sku = {
                    "Sku": objSku.find("input[name='Sku']").val(),
                    "SubDicKey": "",
                    "SubKey": "",
                    "Price": price,
                    "BarCode": objSku.find("input[name='BarCode']").val(),
                    "AlarmStockQty": alarmStockQty,
                    "NetWeight": netweight,
                    "NetContent": netcontent,
                    "Color": {
                        "Key": tranUndefined($("#ColorSel").val()),
                        "Other": {
                            "Content_S": tranUndefined($("#Color").find("div.add_success_list").find("p.simplified").html()),
                            "Content_T": tranUndefined($("#Color").find("div.add_success_list").find("p.traditional").html()),
                            "Content_E": tranUndefined($("#Color").find("div.add_success_list").find("p.english").html())
                        }
                    },
                    "Size": {
                        "Key": tranUndefined($("#SizeSel").val()),
                        "Other": {
                            "Content_S": tranUndefined($("#Size").find("div.add_success_list").find("p.simplified").html()),
                            "Content_T": tranUndefined($("#Size").find("div.add_success_list").find("p.traditional").html()),
                            "Content_E": tranUndefined($("#Size").find("div.add_success_list").find("p.english").html())
                        }
                    },
                    "Specifications": {
                        "Content_S": tranUndefined($("#Specifications").find("div.add_success_list").find("p.simplified").html()),
                        "Content_T": tranUndefined($("#Specifications").find("div.add_success_list").find("p.traditional").html()),
                        "Content_E": tranUndefined($("#Specifications").find("div.add_success_list").find("p.english").html())
                    },
                    "AlcoholPercentage": {
                        "Content_S": tranUndefined($("#AlcoholPercentage").find("div.add_success_list").find("p.simplified").html()),
                        "Content_T": tranUndefined($("#AlcoholPercentage").find("div.add_success_list").find("p.traditional").html()),
                        "Content_E": tranUndefined($("#AlcoholPercentage").find("div.add_success_list").find("p.english").html())
                    },
                    "Smell": {
                        "Content_S": tranUndefined($("#Smell").find("div.add_success_list").find("p.simplified").html()),
                        "Content_T": tranUndefined($("#Smell").find("div.add_success_list").find("p.traditional").html()),
                        "Content_E": tranUndefined($("#Smell").find("div.add_success_list").find("p.english").html())
                    },
                    "CapacityRestriction": {
                        "Content_S": tranUndefined($("#CapacityRestriction").find("div.add_success_list").find("p.simplified").html()),
                        "Content_T": tranUndefined($("#CapacityRestriction").find("div.add_success_list").find("p.traditional").html()),
                        "Content_E": tranUndefined($("#CapacityRestriction").find("div.add_success_list").find("p.english").html())
                    }
                };

                mainValue = transforId(mainValue);
                if (mainValue.split("-").length < 4) {

                    sku.MainDicKey = mainValue.split('-')[1];
                    sku.MainKey = mainValue.split('-')[2];

                }
                else {
                    sku.MainDicKey = mainValue.split('-')[1];
                    var MainValue = null;
                    if (mainValue.split('-')[3] == "1") {
                        MainValue = {
                            "Content_S": tranUndefined($("#" + mainValue).find("p.simplified").html()),
                            "Content_T": tranUndefined($("#" + mainValue).find("p.traditional").html()),
                            "Content_E": tranUndefined($("#" + mainValue).find("p.english").html())
                        }
                    } else {
                        MainValue = {
                            "Content_S": tranUndefined($("#" + mainValue).find("span").html()),
                            "Content_T": tranUndefined($("#" + mainValue).find("span").html()),
                            "Content_E": tranUndefined($("#" + mainValue).find("span").html())
                        }
                    }
                    sku.MainKey = "";
                    sku.MainValue = MainValue;
                }
                skus.push(sku);
            })
        });
    }
    else if (skutype == "0") {
        var temp = /^\d+(\.\d+)?$/;
        var price = $("#Price").val() == "" || !temp.test($("#Price").val()) ? 0.00 : parseFloat($("#Price").val());

        var sku = {
            "Sku": $("#Sku").val(),
            "MainDicKey": "",
            "SubDicKey": "",
            "MainKey": "",
            "SubKey": "",
            "Price": price,
            "BarCode": $("#BarCode").val(),
            "AlarmStockQty": $("#AlarmStockQty").val() == "" ? 0 : parseFloat($("#AlarmStockQty").val()),
            "NetWeight": netweight,
            "NetContent": netcontent,
            "Color": {
                "Key": $("#ColorSel").val(),
                "Other": {
                    "Content_S": tranUndefined($("#Color").find("div.add_success_list").find("p.simplified").html()),
                    "Content_T": tranUndefined($("#Color").find("div.add_success_list").find("p.traditional").html()),
                    "Content_E": tranUndefined($("#Color").find("div.add_success_list").find("p.english").html())
                }
            },
            "Size": {
                "Key": $("#SizeSel").val(),
                "Other": {
                    "Content_S": tranUndefined($("#Size").find("div.add_success_list").find("p.simplified").html()),
                    "Content_T": tranUndefined($("#Size").find("div.add_success_list").find("p.traditional").html()),
                    "Content_E": tranUndefined($("#Size").find("div.add_success_list").find("p.english").html())
                }
            },
            "Specifications": {
                "Content_S": tranUndefined($("#Specifications").find("div.add_success_list").find("p.simplified").html()),
                "Content_T": tranUndefined($("#Specifications").find("div.add_success_list").find("p.traditional").html()),
                "Content_E": tranUndefined($("#Specifications").find("div.add_success_list").find("p.english").html())
            },
            "AlcoholPercentage": {
                "Content_S": tranUndefined($("#AlcoholPercentage").find("div.add_success_list").find("p.simplified").html()),
                "Content_T": tranUndefined($("#AlcoholPercentage").find("div.add_success_list").find("p.traditional").html()),
                "Content_E": tranUndefined($("#AlcoholPercentage").find("div.add_success_list").find("p.english").html())
            },
            "Smell": {
                "Content_S": tranUndefined($("#Smell").find("div.add_success_list").find("p.simplified").html()),
                "Content_T": tranUndefined($("#Smell").find("div.add_success_list").find("p.traditional").html()),
                "Content_E": tranUndefined($("#Smell").find("div.add_success_list").find("p.english").html())
            },
            "CapacityRestriction": {
                "Content_S": tranUndefined($("#CapacityRestriction").find("div.add_success_list").find("p.simplified").html()),
                "Content_T": tranUndefined($("#CapacityRestriction").find("div.add_success_list").find("p.traditional").html()),
                "Content_E": tranUndefined($("#CapacityRestriction").find("div.add_success_list").find("p.english").html())
            }
        };
        skus.push(sku);
    }

    return skus;
}

function tranUndefined(val) {
    if (val == undefined) {
        return "";
    }
    else {
        return val;
    }
}


// 修改类目提示
function UpdateCategory() {
    $.dialog({
        width: 470,
        title: "提示",
        content: "修改类目将清空当前已编辑的商品内容，您确认要修改类目吗？",
        buttons: [{
            text: "取消",
            isWhite: 1
        },
            {
                text: "确定",
                onClick: function () {
                    window.location.href = "/Product/SelectCategory";
                    return true;
                }
            }]
    });
}


$(document).ready(function () {
    // 图片上传
    var btn = $("#addimage");

    new AjaxUpload(btn, {
        action: '/Utility/UploadImage?cutWidth=1',
        name: 'imgFile',
        fileType: '.jpg,.jpeg,.png',
        autoSubmit: true,
        responseType: "json",
        onSubmit: function (file, extension) {
            var ext = extension[0].toLowerCase();
            if (ext != "jpg" && ext != "jpeg" && ext != "png") {
                {
                    $.dialog("只支持上传PNG、JPG 或 JPEG格式的图片！");
                    return false;
                }
            }
        },
        onComplete: function (file, json) {
            if (json) {
                if (json.Error) {
                    $.dialog(json.Message);
                }
                else {
                    var li = $('<li class="bor f_l mg_r20 mg_b10" />')
                    li.append($('<input name="file" type="hidden"/>').attr("value", json.Path));
                    var img = $('<img/>').css("display", "inline");
                    imgWrapper = $('<div/>').append(img);
                    imgWrapper.css("line-height", "75px");
                    imgWrapper.css("overflow", "hidden");
                    var url = json.Url;
                    var index = url.lastIndexOf('.');
                    img.attr("src", url.substr(0, index) + '_180' + url.substr(index));
                    li.append(imgWrapper);
                    li.append($('<a href="javascript:void(0)" class="close" />'));
                    btn.before(li);
                    if (!btn.is(":hidden") && btn.index() >= 5) {
                        btn.hide();
                    }
                }
            } else {
                $.dialog("上传失败！");
            }
        }
    });
    // 删除图片
    btn.closest("ul").delegate("a.close", "click", function () {
        $(this).closest("li").remove();
        if (btn.is(":hidden") && btn.index() < 5) {
            btn.show();
        }
    })
    // 图片保持纵横比
    function OnImageLoad() {
        imgPreview = this;
        var size = GetSize(imgPreview.width, imgPreview.height);
        imgPreview.style.width = size.width + 'px';
        imgPreview.style.height = size.height + 'px';
    };

    function GetSize(width, height) {
        if (width > 80 || height > 80) {
            if (width == height) { width = height = 80; }
            else {
                if (height / width < 1) {
                    height = (height * 80 / width);
                    if (height < 1) { height = 1; }
                    width = 80;
                }
                else {
                    width = (width * 80 / height);
                    if (width < 1) { width = 1; }
                    height = 80;
                }
            }
        }
        return { width: width, height: height };
    }
});

