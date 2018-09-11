$(function () {
    if ($("#DataDe").val() == "01/01/0001") {
        $("#DataDe").val("");
    }

    if ($("#DataAte").val() == "01/01/0001") {
        $("#DataAte").val("");
    }

	$("#btn1").click(function () {
		$("#mRecebe").append("<div class='form-group col-md-3'><input type='file' class='form-control-file' id='exampleFormControlFile1'></div>");
	});

	$("#btn2").click(function () {
		$("input:file").appendTo("<div class='form-group col-md-3'><input type='file' class='form-control-file' id='exampleFormControlFile1'></div>");
	});
});