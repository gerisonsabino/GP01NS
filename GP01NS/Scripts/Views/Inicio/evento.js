$(function () {
    $("#btn-seguir").click(toggleSeguir);
    initMap();
    carouselMusicos();
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
    var es = document.getElementById("evento").innerHTML;
    var et = document.getElementById("estabelecimento").innerHTML;
    var e = document.getElementById("endereco").textContent;
    var d = document.getElementById("data").textContent;
    var g = new google.maps.Geocoder();

    g.geocode({ 'address': e }, function (r, s) {
        if (s == google.maps.GeocoderStatus.OK) {
            m.setCenter(r[0].geometry.location);

            var marca = new google.maps.Marker({
                position: r[0].geometry.location,
                map: m
            });

            var infowindow = new google.maps.InfoWindow({
                content: "<h6>" + es + "</h6><strong style='margin-bottom: 5px; display: block;'>" + d + "</strong><p style='margin-bottom: 0px;'>" + e + "</p>"
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

function carouselMusicos() {
    var json = JSON.parse($("#MusicosJSON").val());
    var html = "";
    var any = false;

    for (var i = 0; i < json.length; i++) {
        var m = json[i];

        if (m.Confirmado) {
            any = true;
            html += "<div class='carousel-item" + (i == 0 ? " active" : "") + "'>";
            html += "    <img class='d-block w-100' src='" + m.Imagem + "' alt='" + m.Nome + "' />";
            html += "    <div class='carousel-caption d-none d-md-block'>";
            html += "        <h5><a href='/inicio/musico/" + m.Username + "' style='color: #FFF !important;' target='_blank'>@" + m.Username + "</a></h5>";
            html += "    </div>";
            html += "</div>";
        }
    }

    $("#carouselMusicos .carousel-inner").html(html);

    if (!any) {
        $("#carouselMusicos").hide();
    }

    $("#MusicosJSON").remove();
}