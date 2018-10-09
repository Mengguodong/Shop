
$(document).ready(function () {

    $("#IsOnSales").chosen({ disable_search: true, width: "153px" });

    $("#ProductStatus").chosen({ disable_search: true, width: "153px" });

    $("#InventoryStatus").chosen({ disable_search: true, width: "153px" });

    $("#SalesTerritory").chosen({ disable_search: true, width: "153px" });

    $("#SupplierId").chosen({ disable_search: true, width: "276px" });

    $("#fstCagegoryId").chosen({ disable_search: true, width: "125px" });

    $("#sndCagegoryId").chosen({ disable_search: true, width: "125px" });

    $("#trdCagegoryId").chosen({ disable_search: true, width: "125px" });

    $("#search").click(function () { $(this).closest("form").submit() });

    $("#fstCagegoryId").change(function () {
        var parentId = $(this).val();
        GetChild(parentId, 1, "sndCagegoryId");
    });
    $("#sndCagegoryId").change(function () {
        var parentId = $(this).val();
        GetChild(parentId, 2, "trdCagegoryId");
    });

});

function GetChild(parentId, level, selectId) {
    if (parentId != 0) {
        $.post("GetCategoryList", { level: level, parentID: parentId }, function (json) {
            $("#" + selectId).empty();
            $("#" + selectId).append("<option value=\"0\">全部</option>");
            $.each(json, function (i, data) {
                $("#" + selectId).append("<option value=\"" + data.Id + "\">" + data.Name + "</option>");
            });

            $("#" + selectId).trigger("chosen:updated");

        }, "json")
    }
}

function SystemOffShelf(spu, name) {
    $.dialog({
        title: "下架原因",
        content: $("#offShelf").html(),
        init: function (obj) {
            obj.find("#productName").html(name);
        },
        onConfirm: function (obj) {
            var reason = obj.find("#shelfReson").val();

            if (reason == "") {
                obj.find("#error").html("请说明下架原因").show();
                return false;
            }
            var result = false;
            $.ajax({
                type: 'POST',
                url: "/Product/SystemOffShelf",
                data: { spu: spu, reason: reason, status: 5 },
                async: false,
                success: function (data) {
                    if (data.data) {
                        $.dialog({
                            width: 470,
                            title: "提示",
                            content: "下架成功",
                            buttons: [
                                {
                                    text: "確定",
                                    onClick: function () {
                                        window.location.reload();
                                        return true;
                                    }
                                }]
                        });
                        result = true;
                    }
                    else {
                        $.dialog("下架失败!");
                        result = false;
                    }

                }
            });
         
            return result;
        }
    });
}


function AllowShelf(spu, name) {
    $.dialog({
        title: "允许上架",
        content: $("#allowShelf").html(),
        init: function (obj) {
            obj.find("#pname").html(name);
        },
        onConfirm: function (obj) {
            var result = false;
            $.ajax({
                type: 'POST',
                url: "/Product/SystemOffShelf",
                data: { spu: spu, reason: "", status: 4 },
                async: false,
                success: function (data) {
                    if (data.data) {
                        $.dialog({
                            width: 470,
                            title: "提示",
                            content: "允许上架成功",
                            buttons: [
                                {
                                    text: "確定",
                                    onClick: function () {
                                        window.location.reload();
                                        return true;
                                    }
                                }]
                        });
                        result = true;
                    }
                    else {
                        $.dialog("允许上架失败!");
                        result = false;
                    }

                }
            });
            return result;
        }
    });
}