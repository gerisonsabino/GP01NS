$(function () {
    initMap();
});

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

function setMarkers(m){
    var e = "Rua Galvão Bueno, 868 - Liberdade - São Paulo, SP - 01506-000";
    var g = new google.maps.Geocoder();

    g.geocode({ 'address': e }, function (r, s) {
        if (s == google.maps.GeocoderStatus.OK) {
            m.setCenter(r[0].geometry.location);

            var marca = new google.maps.Marker({
                //icon: "http://www.nossoshow.gerison.net/Imagens/Views/Inicio/pin.png",
                position: r[0].geometry.location,
                map: m
            });

            var infowindow = new google.maps.InfoWindow({
                content: "Você"
            });

            infowindow.open(marca.get('map'), marca);
        }
        else {
            var a = document.getElementById("mapa");
            a.innerHTML = "<h5 class='text-center'>Houve um erro ao carregar o mapa. Por favor, recarregue a página.</h5>";
        }
    });
}