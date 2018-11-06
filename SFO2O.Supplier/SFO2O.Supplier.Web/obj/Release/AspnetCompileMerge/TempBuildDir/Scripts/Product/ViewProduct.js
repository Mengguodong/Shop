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
function CreateTabs(ul) {
    BindLiChange(ul, function (li, old) {
        $("#" + old.data("for")).hide();
        $("#" + li.data("for")).show();
    });
}
CreateTabs($("#tabs"));
function ViewDescription(lang) {
    var title;
    switch (lang) {
        case 1:
            title = "商品详情";
            break;
        case 3:
            title = "Product Details";
            break;
        default:
            title = "商品詳情";
    }
    var html = $("#Description" + lang).html();
    var win = window.open("about:blank");
    var doc = win.document;
    doc.writeln('<!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">');
    doc.writeln('<html xmlns="http://www.w3.org/1999/xhtml">');
    doc.writeln('<head>');
    doc.writeln('    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />');
    doc.writeln('    <title>' + title + '</title>');
    $(document).find("head").find("link").each(function () {
        doc.writeln('    <link href="' + $(this).attr("href") + '" rel="stylesheet" />')
    });
    doc.writeln('</head>');
    doc.writeln('<body class="bg_f8f8f8">');
    doc.writeln('    <div class="img_xq_wrap">');
    doc.writeln('        <p class="t_c img_xq_title">' + title + '</p>');
    doc.writeln('        <div class="img_xq pd20">');
    doc.writeln(html);
    doc.writeln('        </div>');
    doc.writeln('    </div>');
    doc.writeln('</body>');
    doc.writeln('</html>');
    doc.close();
}
$(".img_wrap img").load(OnImageLoad);
// 图片保持纵横比
function OnImageLoad() {
    imgPreview = this;
    var size = GetThumbsSize(imgPreview.width, imgPreview.height);
    imgPreview.style.width = size.width + 'px';
    imgPreview.style.height = size.height + 'px';
};

function GetThumbsSize(width, height) {
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