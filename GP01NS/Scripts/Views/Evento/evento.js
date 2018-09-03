$(function () {
	$("#btn1").click(function () {
		$("#mRecebe").append("<div class='form-group col-md-3'><input type='file' class='form-control-file' id='exampleFormControlFile1'></div>");
	});

	$("#btn2").click(function () {
		$("input:file").appendTo("<div class='form-group col-md-3'><input type='file' class='form-control-file' id='exampleFormControlFile1'></div>");
	});
});