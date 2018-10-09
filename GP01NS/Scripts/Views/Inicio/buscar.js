$(function () {
    getGeneros();

   htmlInputRange.options({
        output: '.buscador-filtro output',
        tooltip: false,
        posfix: ' km',
        max: 50,
        min: 0,
        value: 25
    });
});

function pesquisar() {
    $("#resultset").html("");
    $(".buscador-load").show();

    var q = $("[name='q']").val();
    var e = $("#e").val();
    var g = $("#g").val();
    var a = $("#a").val();
    var h = $("#h").val();

    $.post("/inicio/pesquisar/", { "q": q, "e": e, "g": g, "a": a, "h": h }, function (s) {
        var json = eval(s);

        var html = "";

        if (json != "") {
            for (var i = 0; i < json.length; i++) {
                html += "<div class='resultado-item flex'>";
                html += "    <div class='resultado-item-imagem arredondado mr-3' style='background-image: url(http://nossoshow.gerison.net" + json[i].Imagem + ")'></div>";
                html += "    <div class='resultado-item-texto'>";

                switch (json[i].Tipo) {
                    case "Estabelecimento":
                        html += "        <div class='resultado-item-nome flex'>";
                        html += "            <a href='/inicio/estabelecimento/" + json[i].Username + "'>" + json[i].Nome + "</a>";
                        html += "            <div class='resultado-item-tipo ml-2'>";
                        html += "                <span style='color: #AAA;'>@" + json[i].Username + "</span>";
                        html += "                <span class='badge badge-secondary'>Estabelecimento</span>";
                        html += "            </div>";
                        html += "        </div>";
                        html += "        <div class='resultado-item-endereco'>" + json[i].Endereco + "</div>";
                        html += "        <div class='resultado-item-endereco'>";
                        var badges = eval(json[i].Badges);
                        for (var j = 0; j < badges.length; j++) {
                            html += "        <span class='badge bg-primary' style='color: #FFF !important; margin-left: 3px;'>" + badges[j] + "</span>";
                        }
                        html += "        </div>";
                        break;

                    case "Evento":
                        html += "        <div class='resultado-item-nome flex'>";
                        html += "            <a href='/inicio/evento/" + json[i].ID + "'>" + json[i].Nome + "</a>";
                        html += "            <div class='resultado-item-tipo ml-2'>";
                        html += "                <span class='badge badge-secondary'>Evento</span>";
                        html += "            </div>";
                        html += "        </div>";
                        html += "        <div class='resultado-item-endereco'>" + json[i].Endereco + "</div>";
                        html += "        <div class='resultado-item-tipo'>";
                        html += "           <strong>Evento de: </strong> <span style='color: #AAA;'>@" + json[i].Username + "</span>";
                        html += "        </div>";
                        break;

                    case "Músico":
                        html += "        <div class='resultado-item-nome flex'>";
                        html += "            <a href='/inicio/musico/" + json[i].Username + "'>" + json[i].Nome + "</a>";
                        html += "            <div class='resultado-item-tipo ml-2'>";
                        html += "                <span style='color: #AAA;'>@" + json[i].Username + "</span>";
                        html += "                <span class='badge badge-secondary'>Músico</span>";
                        html += "            </div>";
                        html += "        </div>";
                        html += "        <div class='resultado-item-endereco'>";
                        var badges = eval(json[i].Badges);
                        for (var j = 0; j < badges.length; j++) {
                            html += "        <span class='badge bg-primary' style='color: #FFF !important; margin-left: 3px;'>" + badges[j] + "</span>";
                        }
                        html += "        </div>";
                        break;
                    default:
                }

                html += "        <div class='flex'>";
                //html += "            <div class='resultado-item-km mr-2'>";
                //html += "                9 km";
                //html += "            </div>";
                //html += "            <div class='nota-3 resultado-item-nota'>";
                //html += "                <span class='voto-1 oi oi-heart'></span>";
                //html += "                <span class='voto-2 oi oi-heart'></span>";
                //html += "                <span class='voto-3 oi oi-heart'></span>";
                //html += "                <span class='voto-4 oi oi-heart'></span>";
                //html += "                <span class='voto-5 oi oi-heart'></span>";
                //html += "            </div>";
                html += "        </div>";
                html += "    </div>";
                html += "</div>";
            }
        }
        else {
            html += "<div class='resultado-item p-4 flex'>";
            html += "   <div class='container-fluid text-center'>";
            html += "       <strong>Oops! Nenhum resultado encontrado.</strong>";
            html += "   </div > ";
            html += "</div>";
        }

        $("#resultset").html(html);

        if (json != "") {
            $(".resultado-item").click(function () {
                location.href = $(this).find("a").attr("href");
            });
        }

        $(".buscador-load").hide();
    });
}

function getGeneros() {
    $.post("/inicio/getgeneros/", function (s) {
        try {
            var Generos = eval(s);
            var html = "";

            for (var i = 0; i < Generos.length; i++) {
                html += "<option value='" + Generos[i].ID + "'>" + Generos[i].Descricao + "</option>";
            }

            $("#Generos").html(html);

            getAmbientacoes();
        }
        catch (e) { }
    });
}

function getAmbientacoes() {
    $.post("/inicio/getambientacoes/", function (s) {
        try
        {
            var Ambientacoes = JSON.parse(s);
            var html = "";

            for (var i = 0; i < Ambientacoes.length; i++) {
                html += "<option value='" + Ambientacoes[i].ID + "'>" + Ambientacoes[i].Descricao + "</option>";
            }

            $("#Ambientacoes").html(html);

            getHabilidades();
        }
        catch (e) { }
    });
}

function getHabilidades() {
    var Tipos = "";
    var Habis = "";

    $.post("/inicio/gettipohabilidades/", function (s) {
        try {
            Tipos = JSON.parse(s);
            $.post("/inicio/gethabilidades/", function (_s) {
                Habis = JSON.parse(_s);
                var html = "";

                for (var i = 0; i < Tipos.length; i++) {
                    var tipo = Tipos[i];
                    html += "<optgroup label='" + tipo.Descricao + "'>";

                    for (var j = 0; j < Habis.length; j++) {
                        if (Habis[j].TipoHab == tipo.ID) {
                            html += "<option value='" + Habis[j].ID + "'>" + Habis[j].Descricao + "</option>";

                        }
                    }
                    html += "</optgroup>";
                }

                $("#Habilidades").html(html);
                init();
            });
        }
        catch (e) {
            init();
        }
    });
}

function init() {
    $("#btn-menu-pesquisar").click(function () {
        $('.buscador .buscador-botao, .buscador .buscador-filtro').toggleClass('aberto');
    });

    $("#btn-pesquisar").click(function () {

        if ($("[name='q']").val() == "") {
            $("[name='q']").focus();
            return;
        }

        var e = new Array();

        $(".custom-checkbox[data-checked='true']").each(function () {
            e.push(parseInt($(this).val()));
        });

        $("#e").val(JSON.stringify(e));

        pesquisar();
    });

    $("[name='q']").on("keydown", function (e) {
        $("#resultset").html("");

        if (e.keyCode == 13 && $(this).val() != "") {
            $("#btn-pesquisar").click();
        }
    });

    $(".custom-checkbox").click(function () {
        if ($(this).attr("data-checked") == "true") {
            $(this).attr("data-checked", "false");
        }
        else {
            $(this).attr("data-checked", "true");
        }
    });

    $('#Ambientacoes').val(eval($("#a").val())).trigger('change');
    $('#Ambientacoes').change(function () {
        $("#a").val("[" + $('#Ambientacoes').val() + "]");
    });

    $('#Generos').val(eval($("#g").val())).trigger('change');
    $('#Generos').change(function () {
        $("#g").val("[" + $('#Generos').val() + "]");
    });

    $('#Habilidades').val(eval($("#h").val())).trigger('change');
    $('#Habilidades').change(function () {
        $("#h").val("[" + $('#Habilidades').val() + "]");
    });
}


function toggleFiltro() {
    $('.campo-generos').toggle($('#tipo-evento').prop('checked') || $('#tipo-musico').prop('checked'));
    $('.campo-ambientacao').toggle($('#tipo-estabelecimento').prop('checked'));
    $('.campo-habilidades').toggle($('#tipo-musico').prop('checked'));
}