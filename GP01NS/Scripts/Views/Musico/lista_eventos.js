$(document).ready(function () {
    var response = $('#JSON').val();
    response = $.parseJSON(response);
    var lista = $('.listaeventos');

    $(function () {
        $.each(response, function (i, item) {
            $('<li>').text("Data: " + item.Data + "; Evento: " + item.Evento + "; Estabelecimento: " + item.Estabelecimento + "; Endereço: " + item.Endereco).appendTo(lista);
         //Sem Chave   $('<li>').text(item.Data + "; " + item.Evento + "; " + item.Estabelecimento + "; " + item.Endereco).appendTo(lista);
        });

        $('#JSON').remove();
    });
});