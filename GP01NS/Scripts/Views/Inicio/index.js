﻿$(function () {
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
                        html += "                Estabelecimento";
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
                        html += "                Evento";
                        html += "            </div>";
                        html += "        </div>";
                        html += "        <div class='resultado-item-endereco'>" + json[i].Endereco + "</div>";
                        html += "        <div class='resultado-item-tipo ml-2'>";
                        html += "           Evento de: <span style='color: #AAA;'>@" + json[i].Username + "</span>";
                        html += "        </div>";
                        break;

                    case "Músico":
                        html += "        <div class='resultado-item-nome flex'>";
                        html += "            <a href='/inicio/musico/" + json[i].Username + "'>" + json[i].Nome + "</a>";
                        html += "            <div class='resultado-item-tipo ml-2'>";
                        html += "                <span style='color: #AAA;'>@" + json[i].Username + "</span>";
                        html += "                Músico";
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
            html += "<div class='resultado-item flex'>";
            html += "   <div class='container-fluid text-center'>";
            html += "       <strong>Oops! Nenhum resultado encontrado.</strong>";
            html += "   </div > ";
            html += "</div>";
        }

        $("#resultset").html(html);

        $(".buscador-load").hide();
    });
}