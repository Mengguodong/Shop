$(function () {

    $("#tradinale").click(function () { switchTab("tradinale") });
    $("#sample").click(function () { switchTab("sample") });
    $("#english").click(function () { switchTab("english") });
});

function switchTab(liId) {
    $("#" + liId).parent().children().each(function () {
        $(this).removeClass("current");
    });

    $("#traDiv").hide();
    $("#samDiv").hide();
    $("#engDiv").hide();

    $("#" + liId).addClass("current");

    if (liId == "tradinale") {
        $("#traDiv").show();
    }
    else if (liId == "sample") {
        $("#samDiv").show();
    }
    else {
        $("#engDiv").show();
    }
}