$(function () {
    $('#editType').chosen({ disable_search: true });
    $('#reportStatus').chosen({ disable_search: true });
    $('#salesTerritory').chosen({ disable_search: true });
    $('#supplierNames').chosen({ disable_search: true });

    $("#errorBtn").click(function () {
        $("#importError").hide();
        hideOverlay();
    });

    $("#importSuccessBtn").click(function () {
        $("#importSuccess").hide();
        hideOverlay();
    });

    $(".win_close").click(function () {
        $("#importError").hide();
        $("#importSuccess").hide();
        hideOverlay();
    });


    $("#search").click(function () {
        Query(1);
    });

    Query(1);

    new AjaxUpload("updateFile", {
        action: '/product/ImportCustomReport',//提交post
        fileType: "*.xls",
        autoSubmit: true,
        onSubmit: function (file, extension) {

            if (extension[0].toLowerCase() != "xls") {
                {
                    showOverlay();
                    $("#importError").show();
                    adjust("#importError");
                    $("#errorMessage").text("文件格式不正確，只支持XLS類型文件");
                    return false;
                }
            }
        },
        onComplete: function (file, response) {
            var retValue = JSON.parse(JSON.stringify(response).replace("<pre>", "").replace("</pre>", ""));
            if (retValue != null && retValue.Success == true) {
                showOverlay();
                $("#importSuccess").show();
                adjust("#importSuccess");

                $("#success").text(retValue.SuccessCount + " 條SKU記錄");
                $("#fail").text(retValue.FailCount + " 條SKU記錄");

                if (retValue.FailCount > 0) {
                    $("#showErrors").show();
                    $("#showErrors").attr("href", "/Product/GetImportError?batchNo=" + retValue.BatchNo);
                }

            } else {
                showOverlay();
                $("#importError").show();
                adjust("#importError");
                $("#errorMessage").text(retValue.Message);
                return false;
            }
        }
    });
});



function Query(pageIndex) {
    if (pageIndex == undefined)
        pageIndex = 1;

    $.ajax({
        type: 'POST',
        url: "/product/ProductAuditingList",
        data: $('#queryForm').serialize() + "&PageSize=" + 50 + "&PageNo=" + pageIndex,
        async: true,
        success: function (data) {
            $("#products").html(data);
            displayPage(Query, $("#recordCount").val(), $("#rowCount").val(), $("#pageIndex").val(), 50);
        }
    });
}

function Export() {
    window.location.href = "/product/ExcportProductAuditingList?startTime=" + $("#CreateTimeStart").val() + "&endTime=" + $("#CreateTimeEnd").val() + "&spu=" + $("#spuNo").val() + "&sku=" + $("#skuNo").val() + "&productName=" + $("#ProductName").val() + "&editType=" + $("#editType").val() + "&reportStatus=" + $("#reportStatus").val() + "&supplierid=" + $("#supplierNames").val() + "&salesTerritory=" + $("#salesTerritory").val();
}

function auditingProduct(spu) {
    window.location.href = "/product/AuditingProduct?spu=" + spu + "&StartTime=" + $("#CreateTimeStart").val() + "&EndTime=" + $("#CreateTimeEnd").val() + "&SpuNo=" + $("#spuNo").val() + "&Sku=" + $("#skuNo").val() + "&ProductName=" + $("#ProductName").val() + "&EditType=" + $("#editType").val() + "&ReportStatus=" + $("#reportStatus").val() + "&Supplierid=" + $("#supplierNames").val() + "&SalesTerritory=" + $("#salesTerritory").val();
}