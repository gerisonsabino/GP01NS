$(function () {
    init();
});


function init() {
    var json = JSON.parse($("#JSON").val());
    setEstabelecimentos(json.Estabelecimentos);
    setEventos(json.Eventos);
    setMusicos(json.Musicos);
}

function setEstabelecimentos(s) {
    var json = eval(s);
    var html = "";

    html += "<div class='resultado' id='resultset'>";

    if (json != null) {
        for (var i = 0; i < json.length; i++) {
            html += "<div class='resultado-item flex mb-2'>";
            html += "    <div class='resultado-item-imagem arredondado mr-3' style='background-image: url(http://nossoshow.gerison.net" + json[i].Imagem + ")'></div>";
            html += "    <div class='resultado-item-texto'>";
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
    html += "</div>";

    $("#tab-estabelecimentos").html(html);
}

function setEventos(s) {
    var json = eval(s);
    var html = "";

    html += "<div class='resultado' id='resultset'>";

    if (json != null) {
        for (var i = 0; i < json.length; i++) {
            html += "<div class='resultado-item flex mb-2'>";
            html += "    <div class='resultado-item-imagem arredondado mr-3' style='background-image: url(http://nossoshow.gerison.net" + json[i].Imagem + ")'></div>";
            html += "    <div class='resultado-item-texto'>";
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

    html += "</div>";

    $("#tab-eventos").html(html);
}

function setMusicos(s) {
    var json = eval(s);
    var html = "";

    html += "<div class='resultado' id='resultset'>";

    if (json != null) {
        for (var i = 0; i < json.length; i++) {
            html += "<div class='resultado-item flex mb-2'>";
            html += "    <div class='resultado-item-imagem arredondado mr-3' style='background-image: url(http://nossoshow.gerison.net" + json[i].Imagem + ")'></div>";
            html += "    <div class='resultado-item-texto'>";
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

    html += "</div>";

    $("#tab-musicos").html(html);
}