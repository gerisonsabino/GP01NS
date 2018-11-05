$(function () {
    $("#btn-seguir").click(toggleSeguir);
    $(".comentario-lista .load").hide();

    $("#modal-avaliar > span").click(function () {
        $('#modal').modal('toggle');

        $(".avaliacao > div").removeAttr("class");
        $(".avaliacao > div").addClass($("#modal-avaliar").attr("class"));
    });

    replaceModal();

    getAvaliacoes();
});

$(function () {
    $(".foto-imagem").click(function () {
        var img = $(this).attr("data-img").toString();

        if (img != "") {
            $("#img-modal").attr("src", img);
            $("#modal-imagem").modal('show');
        }
    });
});


function toggleSeguir() {
    var a = $(this).attr("data-musico");

    $.post("/inicio/toggleseguir/", { "ID": a }, function (s) {
        if (s == "ok") {
            var t = $("#btn-seguir").text();

            if (t == "Seguir") {
                $("#btn-seguir").removeClass("btn-primary");
                $("#btn-seguir").addClass("btn-danger");
                $("#btn-seguir").text("Deixar de Seguir");
            }
            else {
                $("#btn-seguir").addClass("btn-primary");
                $("#btn-seguir").removeClass("btn-danger");
                $("#btn-seguir").text("Seguir");
            }
        }
    });
}

function replaceModal() {
    $(".PerfilVertical__Nome").text($("#musico").attr("data-nome"));
    $(".FotoUsuario").attr("style", $(".perfil-imagem").attr("style"));
}

function getAvaliacoes() {
    $(".comentarios").html("");
    $(".comentario-lista .load").show();

    var id = parseInt($(".comentario-lista").attr("data-musico"));

    $.post("/inicio/getavaliacoes/", { "id":  id }, function (s) {
        var json = JSON.parse(s);

        var html = "";

        if (json != "[]" && json != "") {
            for (var i = 0; i < json.length; i++) {
                var av = json[i];

                html += "<div class='comentario-item mb-3 flex'>";
                html += "    <div class='comentario-foto' style='background-image: url(http://nossoshow.gerison.net" + av.ImagemPerfil + ")'></div>";
                html += "    <div class='comentario-nome'>" + av.Usuario + "</div>";
                html += "    <div class='comentario-mensagem text-left'>";
                html += "        <div class='nota-" + av.Nota + "'>";
                html += "            <span class='voto-1 oi oi-heart'></span>";
                html += "            <span class='voto-2 oi oi-heart'></span>";
                html += "            <span class='voto-3 oi oi-heart'></span>";
                html += "            <span class='voto-4 oi oi-heart'></span>";
                html += "            <span class='voto-5 oi oi-heart'></span>";
                html += "        </div>";

                if (av.Elogio != "")
                    html += "        <strong>" + av.Elogio + "</strong><br />";

                if (av.Comentario != "")
                    html += av.Comentario;

                html += "    </div>";
                html += "</div> ";
            }
        }
        else {
            html += "<div class='comentario-item mb-3 flex'>";
            html += "    <div class='comentario-mensagem text-center' style='width: 100%;'>Ninguém avaliou este músico.</div>";
            html += "</div> ";
        }

        $(".comentario-lista .load").hide();

        $(".comentarios").html(html);
    });
}