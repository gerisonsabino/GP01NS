
$(function () {
    $("#btn-seguir").click(toggleSeguir);
    initMap();

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
    var a = $(this).attr("data-estabelecimento");

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

function initMap() {
    var m = new google.maps.Map(document.getElementById('mapa'), {
        mapTypeControl: false,
        streetViewControl: false,
        rotateControl: true,
        zoom: 16,
        scrollwheel: false,
        clickableIcons: true,
        disableDefaultUI: false,
        disableDoubleClickZoom: true,
        draggable: true,
        fullscreenControl: false,
        keyboardShortcuts: false,
        maxZoom: 18,
        minZoom: 14,
        streetViewControl: false,
        scaleControl: false,
        mapTypeControl: false,
        zoomControlOptions: { style: google.maps.ZoomControlStyle.LARGE },
    });

    setMarkers(m);
}

function setMarkers(m) {
    var es = document.getElementById("estabelecimento").innerHTML;
    var e = document.getElementById("endereco").textContent;
    var g = new google.maps.Geocoder();

    g.geocode({ 'address': e }, function (r, s) {
        if (s == google.maps.GeocoderStatus.OK) {
            m.setCenter(r[0].geometry.location);

            var marca = new google.maps.Marker({
                position: r[0].geometry.location,
                map: m
            });

            var infowindow = new google.maps.InfoWindow({
                content: "<h6>" + es + "</h6><p style='margin-bottom: 0px;'>" + e + "</p>"
            });

            marca.addListener('mouseover', function () {
                infowindow.open(marca.get('map'), marca);
            });

            infowindow.open(marca.get('map'), marca);
        }
        else {
            var a = document.getElementById("mapa");
            a.innerHTML = "<h5 class='text-center'>Houve um erro ao carregar o mapa. Por favor, recarregue a página.</h5>";
        }
    });
}

function replaceModal() {
    $(".PerfilVertical__Nome").text($("#estabelecimento").attr("data-nome"));
    $(".FotoUsuario").attr("style", $(".perfil-imagem").attr("style"));
}

function getAvaliacoes() {
    $(".comentarios").html("");
    $(".comentario-lista .load").show();

    $.post("/inicio/getavaliacoes/", { "id": $(".comentario-lista").attr("data-estabelecimento") }, function (s) {
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
        else
        {

            html += "<div class='comentario-item mb-3 flex'>";
            html += "    <div class='comentario-mensagem text-center' style='width: 100%;'>Ninguém avaliou este estabelecimento.</div>";
            html += "</div> ";
        }

        $(".comentario-lista .load").hide();

        $(".comentarios").html(html);
    });


}