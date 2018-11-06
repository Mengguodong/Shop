
    window.ReturnUrl = unescape(getReturnUrl("return_url"));

    $(function () {
        var RegionCode;
        var RegionCodeId;

        // 如果是编辑地址，初始化“通关有效证件”的select选择状态
        function numberTypeSelect() {
            var sDocType = $("#docNumberType").attr('val');
            $("#areaCode").find(".radio").removeClass('checked').end().find('li').filter(function (index) {
                return $(this).attr('val') == sDocType
            }).find('.radio').addClass('checked');
        }
        numberTypeSelect();

        // 点击设为默认地址
        $("#addressList").on('click', '.setDefault', function () {
            var id = $(this).parents('.item').attr('aid');
            var _this = $(this);
            FSH.Ajax({
                type: 'get',
                url: window.hostname + '/buy/SetDefaultAddress?id=' + id,
                success: function (msg) {
                    if (msg.Type == 1) {
                        $("#addressList .setDefault i").removeClass('checked');
                        _this.find('i').addClass('checked');
                    } else {
                        FSH.commonDialog(1, [msg.Content])
                    }
                }
            })

        }).on('click', '.delete', function () { // 删除地址
            var id = $(this).parents('.item').attr('aid');
            var _this = $(this);
            var flag = _this.siblings('.setDefault').find('i').hasClass('checked'); // 记录是否是默认地址
            FSH.Ajax({
                type: 'get',
                url: window.hostname + '/buy/DeleteAddress?id=' + id,
                success: function (msg) {
                    if (msg.Type == 1) {
                        FSH.smallPrompt(msg.Content);
                        _this.parents('.item').remove();
                        if (flag) { // 如果删除的是默认地址，则选择最新添加的地址为默认地址(aid最大)
                            var aidArr = [];
                            $("#addressList .item").each(function () {
                                aidArr.push($(this).attr('aid'));
                            })

                            var latest = Math.max.apply(null, aidArr);
                            $("#addressList .item").filter(function () {
                                return $(this).attr('aid') == String(latest);
                            }).find('.setDefault').click();

                        }
                    } else {
                        FSH.commonDialog(1.[msg.Content]);
                    }
                }
            })
        })

        // 选择地址中的添加地址按钮
        $("#addInChoose").on('click', function () {
            window.location.href = "/buy/addaddress" + "?return_url=" + window.ReturnUrl
        })
        // 添加收货地址页面，如果是从我的收货地址页面过来的，则不显示顶部快捷入口
        if (window.location.href.toUpperCase().indexOf('/buy/addaddress'.toUpperCase()) > 0 && window.location.href.toUpperCase().indexOf('buy/AddressList'.toUpperCase()) > 0) {
            $(".flowerMenu").hide();
            $("#saveInfo").text('保存');
        }

        // 添加新地址
        $("#addBtn").on('click', function () {
            if ($("#addressList .item").length >= 20) {
                FSH.smallPrompt('最多只能添加20个收货地址');
            } else {
                var currentUrl = window.location.href;
                window.location.href = window.hostname + '/buy/addaddress?return_url=' + currentUrl;
            }
        })

        // 选择证件类型
        $("#select").on('click', function () {
            numberTypeSelect();
            $(this).popShow({ positionY: "center", openAnimationName: "bottomShow", closeAnimationName: "bottomHide", targetEl: "#areaCode", closeBtnName: "#cancel,#sure" });

        })

        // 选择有效证件
        $("#areaCode").on('click', 'li', function () {
            $("#areaCode .radio").removeClass('checked');
            $(this).find('.radio').addClass('checked');
            RegionCode = $(this).text();
            RegionCodeId = $(this).attr('val');
            var targetSetMax = $("#docNumber");
            if (RegionCodeId == 1) {
                targetSetMax.attr('maxlength', 18);
            } else if (RegionCodeId == 2) {
                targetSetMax.attr('maxlength', 80);
            } else {
                targetSetMax.attr('maxlength', 200);
            }

        }).on('click', '#sure', function () {
            $("#select span").text(RegionCode).attr('val', RegionCodeId);

        })

        // 所在地区
        $("#selectAddress").on('click', function () { // 显示省份
            $(".secPage").remove();
            var titleText = $.trim($("header").eq(0).find('b').text());
            FSH.Ajax({
                type: 'get',
                url: window.hostname + '/buy/GetProvince',
                dataType: 'json',
                jsonpCallback: "success_jsonpCallback",
                success: function (msg) {
                    if (msg.Type == 1) {
                        $(renderProvinceHtml(msg.Data, 'province', titleText)).appendTo("#MContainer").show();
                    } else {
                        FSH.commonDialog(1, [msg.Content]);
                    }
                },
                error: function () {
                    alert('error');
                }
            })

        })

        $("#MContainer").on("click", "#province .content li", function () { // 显示城市
            var provinceId = $(this).attr('provinceid');
            $("#provincename").val($(this).text()).attr('provinceid', provinceId);
            var titleText = $.trim($("header").eq(0).find('b').text());
            FSH.Ajax({
                type: 'get',
                url: window.hostname + '/buy/GetCity?province=' + provinceId,
                success: function (msg) {
                    if (msg.Type == 1) {
                        $(renderCityHtml(msg.Data, 'city', titleText)).appendTo("#MContainer").show();
                        $("#province").hide();
                    } else {
                        FSH.commonDialog(1, [msg.Content]);
                    }
                },
                error: function () {
                    alert('error')
                }
            })
        })

        // 省份页面返回
        $("#MContainer").on('click', "#province .returnBtn", function () {
            $("#province").hide();
        })

        $("#MContainer").on("click", "#city .content li", function () {
            // $("#city").hide();
            // $("#areaName").show();
            var cityId = $(this).attr('cityid');
            $("#cityname").val($(this).text()).attr('cityid', cityId);
            var titleText = $.trim($("header").eq(0).find('b').text());
            FSH.Ajax({
                type: 'get',
                url: '/buy/GetArea?city=' + cityId,
                success: function (msg) {
                    if (msg.Type == 1) {
                        $(renderAreaHtml(msg.Data, 'areaName', titleText)).appendTo("#MContainer").show();
                        $("#city").hide()
                    } else {
                        FSH.commonDialog(1, [msg.Content]);
                    }
                },
                error: function () {
                    alert('出错啦!');
                }
            })
        })

        // 城市页面返回
        $("#MContainer").on('click', "#city .returnBtn", function () {
            $("#city").hide();
            $("#province").show();
        })

        $("#MContainer").on("click", "#areaName .content li", function () {
            // alert(0);
            $("#areaname").val($(this).text()).attr('areaid', $(this).attr('AreaId'));
            var address = $("#provincename").val() + $("#cityname").val() + $("#areaname").val();
            // alert(address);
            $("#area").text(address);
            $("#areaName").hide();
        })

        // 区域页面返回
        $("#MContainer").on('click', "#areaName .returnBtn", function () {
            $("#areaName").hide();
            $("#city").show();
        })

        // 证件类型为“身份证”时，证件号输入完成时将value值加到trueid上
        $("#MContainer").on("keyup", "#docNumber", function () {
            $(this).attr('trueid', $(this).val());
        })



        // 点击“保存”
        $("#saveInfo").on('click', function () {
            var consigneeName = $("#consigneeName").val();
            var tel = $("#tel").val();
            var area = $("#area").text();
            var address = $("#address").val();
            var zipCode = $("#zipCode").val();
            var docNumberType = $("#docNumberType").text();
            var docNumberTypeValue = $("#docNumberType").attr('val');
            var docNumber = $("#docNumber").attr("trueid");
            var provinceid = $("#provincename").attr('provinceid');
            var cityid = $("#cityname").attr('cityid');
            var areaid = $("#areaname").attr('areaid');
            var isAdd = $("#addressId").val();

            if (consigneeName == '') {
                FSH.smallPrompt('请输入收货人姓名');
                return;
            };

            if (tel == '') {
                FSH.smallPrompt('请输入手机号码');
                return;
            };

            if (area == '') {
                FSH.smallPrompt('请选择所在地区');
                return;
            };

            if (address == '') {
                FSH.smallPrompt('请输入详细地区');
                return;
            };

            //if (zipCode == '') {
            //    FSH.smallPrompt('请输入邮政编码');
            //    return;
            //};

            //if (docNumberType == '') {
            //    FSH.smallPrompt('请选择通关有效证件');
            //    return;
            //};

            //if (docNumber == '') {
            //    FSH.smallPrompt('请输入证件号');
            //    return;
            //};


            if (!FSH.tools.isPhone(tel)) {
                FSH.smallPrompt('请输入正确的手机号');
                return;
            }

            //if (!FSH.tools.isZipCode(zipCode)) {
            //    FSH.smallPrompt('请输入正确的邮编');
            //    return false;
            //};

            //if (docNumberTypeValue == 1) {
            //    if (!FSH.tools.IDCardCheck(docNumber)) {
            //        FSH.smallPrompt('请输入正确的证件号');
            //        return false;
            //    }
            //}

            var postData = {
                Receiver: consigneeName,
                Phone: tel,
                ProvinceId: provinceid,
                CityId: cityid,
                AreaId: areaid,
                Address: address,
                PostCode: zipCode,
                PapersType: docNumberTypeValue,
                PapersCode: docNumber,
                Id: isAdd
            };
            FSH.Ajax({
                type: 'post',
                data: { model: JSON.stringify(postData) },
                url: '/buy/SaveAddress',
                success: function (msg) {
                    if (msg.Type == 1) {
                        if (window.location.href.indexOf('return_url') > 0) {
                            //两种途径会添加之后直接返回到-确认订单页（购物车提交订单OrderSumit.html，直接提交订单buy/buynow）
                            //console.log(msg);
                            if (window.location.href.indexOf('OrderSumit.html') > 0) {
                                if (ReturnUrl.indexOf("aid") > 0) {
                                    window.location.href = window.ReturnUrl.replace(window.ReturnUrl.substring(window.ReturnUrl.indexOf('aid'), window.ReturnUrl.length), 'aid=' + msg.aid);
                                } else {
                                    window.location.href = window.ReturnUrl + '?aid=' + msg.aid;
                                }
                            } else if (window.location.href.indexOf('buynow?') > 0) {
                                if (ReturnUrl.indexOf("aid") > 0) {
                                    window.location.href = window.ReturnUrl.replace(window.ReturnUrl.substring(window.ReturnUrl.indexOf('aid'), window.ReturnUrl.length), 'aid=' + msg.aid);
                                } else {
                                    window.location.href = window.ReturnUrl + '&aid=' + msg.aid;
                                }
                            } else {
                                window.location.href = window.ReturnUrl;
                            }
                        } else {
                            window.location.href = msg.LinkUrl;
                        }
                    } else {
                        FSH.commonDialog(1, [msg.Content])
                    }
                }
            })
        })
    })

    function renderProvinceHtml(data, eleId, titleText) {
        if ($("#" + eleId).length > 0) {
            $("#" + eleId).remove();
        }
        var start = '<div class="MContainer secPage" id="' + eleId + '">\
        <header class="pageHeader w100 pr tc">\
            <a class="returnBtn"></a>\
            <b class="f36 FontColor1">'+ titleText + '</b>\
        </header>\
        <div class="content">\
            <ul>'
        var lists = '';
        for (var i in data) {
            lists = lists + '<li ProvinceId = "' + data[i].ProvinceId + '">' + data[i].ProvinceName + '</li>';
        }
        var end = '</ul>\
        </div>\
    </div>';
        return start + lists + end;
    }

    function renderCityHtml(data, eleId, titleText) {
        if ($("#" + eleId).length > 0) {
            $("#" + eleId).remove();
        }

        var start = '<div class="MContainer secPage" id="' + eleId + '">\
        <header class="pageHeader w100 pr tc">\
            <a class="returnBtn"></a>\
            <b class="f36 FontColor1">'+ titleText + '</b>\
        </header>\
        <div class="content">\
            <ul>'
        var lists = '';
        for (var i in data) {
            lists = lists + '<li CityId = "' + data[i].CityId + '">' + data[i].CityName + '</li>';
        }
        var end = '</ul>\
        </div>\
    </div>';
        return start + lists + end;
    }

    function renderAreaHtml(data, eleId, titleText) {
        if ($("#" + eleId).length > 0) {
            $("#" + eleId).remove();
        }
        var start = '<div class="MContainer secPage" id="' + eleId + '">\
        <header class="pageHeader w100 pr tc">\
            <a class="returnBtn"></a>\
            <b class="f36 FontColor1">'+ titleText + '</b>\
        </header>\
        <div class="content">\
            <ul>'
        var lists = '';
        for (var i in data) {
            lists = lists + '<li AreaId = "' + data[i].AreaId + '">' + data[i].AreaName + '</li>';
        }
        var end = '</ul>\
        </div>\
    </div>';
        return start + lists + end;
    }

    function getReturnUrl(paras) { /*获取return_url，条件是return_url必须是最后一项*/
        var url = location.href;
        var nIndex = url.indexOf('return_url');
        var paraString = url.substring(nIndex);
        var returnValue = paraString.substring(11);
        return returnValue;
    }

    // 选择收货地址页面
    $(function () {
        if (location.href.indexOf('aid') > 0) { // 如果之前有除了默认地址之外的选中地址，则给该地址加上选中状态
            var hasSelectAid = FSH.tools.request('aid');
            $("#selectAddList .radio").removeClass('checked');
            $("#selectAddList .item").filter(function (index) {
                return $(this).attr('aid') == hasSelectAid;
            }).find('.radio').addClass('checked');
        }

        $("#selectAddList .item").on('click', function () {
            $("#selectAddList .radio").removeClass('checked');
            $(this).find('.radio').addClass('checked');
            var aid = $(this).attr('aid');
            if (window.ReturnUrl.indexOf('?') > 0) {
                if (window.ReturnUrl.indexOf('aid') > 0) { // 如果参数中含有aid,则把aid替换成新选中的aid
                    var url = window.ReturnUrl.replace(window.ReturnUrl.substring(window.ReturnUrl.indexOf('aid'), window.ReturnUrl.length), 'aid=' + aid)
                    window.location.href = url;
                } else {
                    window.location.href = window.ReturnUrl + "&aid=" + aid;
                }
            } else {
                window.location.href = window.ReturnUrl + "?aid=" + aid;
            }
        })
    })
