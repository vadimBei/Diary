$(".textbox input").focusout(function () {
	if ($(this).val() == "") {
		$(this).siblings().removeClass("hiddenReq");
		$(this).css("background", "#554343");
	} else {
		$(this).siblings().addClass("hiddenReq");
		$(this).css("background", "#484848");
	}
})