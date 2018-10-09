function InitCreateAndEndTime() {
    var endTime = new Date();
    var startTime = Date(endTime.setMonth(endTime.getMonth() - 6));

    $("#CreateTimeStart").val(FormatDate(startTime));
    $("#CreateTimeEnd").val(FormatDate(endTime));

    $("#startTime").val(FormatDate(startTime));
    $("#endTime").val(FormatDate(endTime));
}

function FormatDate(strTime) {
    var date = new Date(strTime);
    return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
}