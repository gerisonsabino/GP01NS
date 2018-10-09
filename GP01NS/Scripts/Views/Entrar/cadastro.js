$(function () {
    $("#div-cadastro-2").hide();
    $("#div-cadastro-3").hide();
    $("#div-cadastro-1").show();

    init();
});

function init() {
    $("#div-buttons-tipo button").click(function () {
        $("#Tipo").val($(this).attr("data-tipo"));

        $("#erro").html("");
        $("#div-cadastro-1").hide();
        $("#div-cadastro-2").show();
    });

    $("#btn-proximo").click(function () {
        var regex = /[a-z0-9!#$ %& '*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&' * +/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/;

        if ($("#Nome").val() == "") {
            $("#Nome").focus();
            return;
        }
        else if ($("#Email").val() == "" || !regex.test($("#Email").val())) {
            $("#Email").val("");
            $("#Email").focus();
            return;
        }

        $("#div-cadastro-2").hide();
        $("#div-cadastro-3").show();
    });

    $("#a-voltar-1").click(function () {
        $("#div-cadastro-1").show();
        $("#div-cadastro-2").hide();
    });

    $("#a-voltar-2").click(function () {
        $("#div-cadastro-2").show();
        $("#div-cadastro-3").hide();
    });
}