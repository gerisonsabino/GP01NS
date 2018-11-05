$(function () {
    $(".avaliacao > div").removeAttr("class");
    $(".avaliacao > div").addClass("nota-" + $("#Avaliacao_Nota").val());

    $("#btn-salvar-avaliacao").click(function () {
        var av = new AvaliacaoVM();

        av.Nota = parseInt($(".avaliacao > div").attr("class").replace("nota-", ""));
        av.Comentario = $("#Avaliacao_Comentario").val();
        av.IDAvaliado = parseInt($("#Avaliacao_IDAvaliado").val());
        av.TipoAvaliado = parseInt($("#Avaliacao_TipoAvaliado").val());
        av.IDElogio = parseInt($("#Avaliacao_IDElogio").val());
        $('#modal').modal('toggle');

        $.post("/inicio/avaliacao/", { "json": JSON.stringify(av), "href": $("#Href").val() }, function (s) {
            getAvaliacoes();

            $("#modal-avaliar").removeAttr("class");
            $("#modal-avaliar").addClass($(".avaliacao > div").attr("class"));
        });
    });

});

function AvaliacaoVM() {
    var a = {
        Nota: NaN,
        Comentario: "",
        IDAvaliado: NaN,
        TipoAvaliado: NaN,
        IDElogio: NaN
    };

    return a;
}