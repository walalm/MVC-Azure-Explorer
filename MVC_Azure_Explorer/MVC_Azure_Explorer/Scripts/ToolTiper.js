$(document).ready(function () {
    var elemento = $(".tt");
    var _strTip = $(elemento).attr("title") == null ? $(elemento).attr("alt") : $(elemento).attr("title");
    var _strHtml = "<span class=\"tooltip\"><span class=\"top\"></span><span class=\"middle\">" + _strTip + "</span><span class=\"bottom\"></span></span>";
    $(elemento).append(_strHtml);
});