$(document).ready(function () { mapa(); });

function mapa() {

    var m = new google.maps.Map(document.getElementById('mapa'), {
        zoom: 16,
        scrollwheel: false,
        clickableIcons: true,
        disableDefaultUI: false,
        disableDoubleClickZoom: true,
        fullscreenControl: false,
        keyboardShortcuts: false,
        mapTypeControl: false,
        zoomControl: false,
        streetViewControl: false,
        scaleControl: false,
        styles: [{ "stylers": [{ "hue": "#007bff" }, { "saturation": 250 }] }, { "featureType": "road", "elementType": "geometry", "stylers": [{ "lightness": 50 }, { "visibility": "simplified" }] }, { "featureType": "road", "elementType": "labels", "stylers": [{ "visibility": "off" }] }],
        zoomControlOptions: { style: google.maps.ZoomControlStyle.LARGE },
        center: { lat: 90, lng: 180 }
    });


    endereco(m);
}

function endereco(m) {
    var es = document.getElementById("estabelecimento").textContent;
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
                content: "<span style='display: block;'><h6>" + es + "</h6><label style='font-size: 15px !important;'>" + e + "</label></span>"
            });

            infowindow.open(marca.get('map'), marca);
        }
        else {
            var a = document.getElementById("mapa");
            a.innerHTML = "<h5 class='text-center'>Houve um erro ao carregar o mapa. Por favor, recarregue a página.</h5>";
        }
    });
}