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

    $("#btn-pesquisar").click(function () {

        if ($("[name='q']").val() == "") {
            $("[name='q']").focus();
            return;
        }

        pesquisar();
    });

    $("[name='q']").on("keydown", function (e) {
        $("#resultset").html("");
    });

    getAtracoes();
});

function pesquisar() {
    $("#resultset").html("");
    $(".buscador-load").show();

    var id = $("#ID").val();
    var q = $("[name='q']").val();

    $.post("/estabelecimento/pesquisarmusicos/", { "id": id, "q": q }, function (s) {
        var json = eval(s);

        var html = "";

        if (json != "") {
            for (var i = 0; i < json.length; i++) {
                html += "<div class='resultado-item container-fluid'>";
                html += "    <div class='usuario-item mb-3 flex'>";
                html += "        <div class='usuario-foto' style='background-image: url(http://nossoshow.gerison.net" + json[i].Imagem + ")'></div>";
                html += "        <div class='usuario-nome'>";
                html += "           <a href='/inicio/musico/" + json[i].Username + "'>" + json[i].Nome + "</a>";
                html += "           <label>@" + json[i].Username + "</label>";
                html += "        </div > ";
                html += "        <div class='usuario-botao text-right'>";
                html += "            <button type='button' class='btn btn-success btn-sm' data-id='" + json[i].ID + "' onclick='convidar(this);'>Enviar convite</button>";
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

function convidar(btn) {
    $(btn).closest(".usuario-item").remove();
    $.post("/estabelecimento/convitemusico/", {"idE": $("#ID").val(), "idM": $(btn).attr("data-id") }, function (s) {
        getAtracoes();
    });
}

function getAtracoes() {
    $("#lista-load").show();
    $(".usuario-lista").html("");

    $.post("/estabelecimento/getatracoes/", { "id": $("#ID").val() }, function (s) {
        var html = "";
        var json = JSON.parse(s);
        if (json.length) {
            for (var i = 0; i < json.length; i++) {
                html += "<div class='usuario-item mb-3 flex'>";
                html += "    <div class='usuario-foto' style='background-image: url(" + json[i].Imagem + ")'></div>";
                html += "        <div class='usuario-nome'>";
                html += "           <a href='/inicio/musico/" + json[i].Username + "'>" + json[i].Nome + "</a>";
                html += "           <label>@" + json[i].Username + "</label>";
                html += "        </div > ";
                html += "    <div class='usuario-botao text-right'>";
                if (!json[i].Recusado && !json[i].Confirmado) {
                    html += "        <button type='button' class='btn btn-light btn-sm' data-id='" + json[i].ID + "' onclick='convidar(this);'>Cancelar convite</button>";
                }
                else if (json[i].Recusado) {
                    html += "        <button type='button' class='btn btn-danger btn-sm'>Convite Recusado</button>";
                }
                else if (json[i].Confirmado) {
                    html += "        <button type='button' class='btn btn-success btn-sm' data-id='" + json[i].ID + "' onclick='convidar(this);'>Excluir atração</button>";
                }
                html += "    </div>";
                html += "</div>";
            }
        }
        else {
            html += "<div class='usuario-item mb-3 flex text-center'>";
            html += "    <div class='usuario-nome' style='width: 100% !important;'>Nenhuma atração foi convidada.</div>";
            html += "</div>";
        }

        $("#lista-load").hide();
        $(".usuario-lista").html(html);
    });
}