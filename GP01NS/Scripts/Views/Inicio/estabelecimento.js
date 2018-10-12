
$(function () {
    $("#btn-seguir").click(toggleSeguir);
    initMap();
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