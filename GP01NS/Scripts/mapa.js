$(function () {
	Mapa().Geolocalizacao();
});

function Mapa() {
    var m = {
        Mapa: {
            coordenadas: {
                latitude: NaN,
                longitude: NaN
            },
            mapa: null,
            iw: null,
            Criar: function () {
				this.mapa = new google.maps.Map(document.getElementById('mapa'), {					
					mapTypeControl: false,
					streetViewControl: false,
					rotateControl: true,
                    zoom: 14,
                    scrollwheel: false,
                    clickableIcons: true,
                    disableDefaultUI: false,
                    disableDoubleClickZoom: true,
                    draggable: true,
                    fullscreenControl: false,
                    keyboardShortcuts: false,
                    maxZoom: 16,
                    minZoom: 12,
                    streetViewControl: false,
                    scaleControl: false,
                    mapTypeControl: false,
                    //zoomControl: false,
                    //styles: [{ "stylers": [{ "hue": "#B01B8B" }, { "saturation": 250 } ] }, { "featureType": "road", "elementType": "geometry", "stylers": [ { "lightness": 50 }, { "visibility": "simplified" } ] }, { "featureType": "road", "elementType": "labels", "stylers": [ { "visibility": "off" }]}],
                    zoomControlOptions: { style: google.maps.ZoomControlStyle.LARGE },
                    center: { lat: this.coordenadas.latitude, lng: this.coordenadas.longitude }
                });

                var voce = new google.maps.Marker({
                    map: this.mapa,
                    clickable: true,
                    optimized: true,
                    position: { lat: this.coordenadas.latitude, lng: this.coordenadas.longitude }
                });

                var infowindow = new google.maps.InfoWindow({ content: "Você" });
                voce.addListener('mouseover', function () {
                    infowindow.open(voce.get('map'), voce);
                });

                voce.addListener('mouseout', function () {
                    infowindow.setMap(null);
                });

                this.Locais();
            },
            Locais: function () {
                var a = this.mapa;

                for (var i = 0; i < 3; i++) {
                    var marca = new google.maps.Marker({
                        position: { lat: parseFloat("-23.3" + (i - 1) + "963405"), lng: parseFloat("-46.7" + i + "911402") },
                        map: a
                    });

                    this.Teste(marca, i);
                }
            },
            Teste: function (marca, i) {
                var infowindow = new google.maps.InfoWindow({ content: "<h3>Teste " + (i + 1) + "</h3><br /><b style='font-size: 15px !important;'>Endereço:</b><br /><label style='font-size: 15px !important;'>Rua, Nº 0 - Bairro - Cidade, Estado</label>" });
                marca.addListener('mouseover', function () {
                    infowindow.open(marca.get('map'), marca);
                });

                marca.addListener('mouseout', function () {
                    //infowindow.setMap(null);                    
                });
            },
        },
        Posicao: function (p) {
            this.Mapa.coordenadas.latitude = p.coords.latitude;
            this.Mapa.coordenadas.longitude = p.coords.longitude;

            this.Mapa.Criar();
        },
        Geolocalizacao: function () {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(this.Posicao.bind(m), this.Erro);
            }
            else {
                var a = document.getElementById("mapa");
                a.innerHTML = "<h2>A Localização geográfica não é suportada por este navegador.</h2>";
            }
        },
        Erro: function (e) {
            //var a = document.getElementById("mapa");

            //switch (e.code) {
            //    case e.PERMISSION_DENIED:
            //        a.innerHTML = "<h5>O Usuário negou o pedido de localização geográfica.<br /><br /><a href='/Inicio/Index'>Por favor, recarregue a página.</a></h5>";
            //        break;
            //    case e.POSITION_UNAVAILABLE:
            //        a.innerHTML = "<h5>Informações sobre o local não estão disponíveis.<br /><br /><a href='/Inicio/Index'>Por favor, recarregue a página.</a></h5>";
            //        break;
            //    case e.TIMEOUT:
            //        a.innerHTML = "<h5>A solicitação para obter a localização do usuário expirou.<br /><br /><a href='/Inicio/Index'>Por favor, recarregue a página.</a></h5>";
            //        break;
            //    case e.UNKNOWN_ERROR:
            //        a.innerHTML = "<h5>Ocorreu um erro desconhecido.<br /><br /><a href='/Inicio/Index'>Por favor, recarregue a página.</a></h5>";
            //        break;
            //}

            endereco();
		}		
    };

    return m;
}

function endereco() {
    var m = new google.maps.Map(document.getElementById('mapa'), {
        mapTypeControl: false,
        streetViewControl: false,
        rotateControl: true,
        zoom: 14,
        scrollwheel: false,
        clickableIcons: true,
        disableDefaultUI: false,
        disableDoubleClickZoom: true,
        draggable: true,
        fullscreenControl: false,
        keyboardShortcuts: false,
        maxZoom: 16,
        minZoom: 12,
        streetViewControl: false,
        scaleControl: false,
        mapTypeControl: false,
        //zoomControl: false,
        //styles: [{ "stylers": [{ "hue": "#B01B8B" }, { "saturation": 250 }] }, { "featureType": "road", "elementType": "geometry", "stylers": [{ "lightness": 50 }, { "visibility": "simplified" }] }, { "featureType": "road", "elementType": "labels", "stylers": [{ "visibility": "off" }] }],
        zoomControlOptions: { style: google.maps.ZoomControlStyle.LARGE },
        center: { lat: 90, lng: 180 }
    });

    var e = "Rua Galvão Bueno, 868 - Liberdade - São Paulo, SP - 01506-000";
    var g = new google.maps.Geocoder();

    g.geocode({ 'address': e }, function (r, s) {
        if (s == google.maps.GeocoderStatus.OK) {
            m.setCenter(r[0].geometry.location);

            var marca = new google.maps.Marker({
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