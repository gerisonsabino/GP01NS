$(document).ready(function () {
    //Pega o value do input de id JSON
    var response = $('#JSON').val();
    //Dá o parse nele
    response = $.parseJSON(response);

    $(function () {
        $.each(response, function (i, item) {
            //Anexe a tabela de acordo com o que foi separado, ID, Titulo, Data, Status e pra cada um deles no fim cria um a
            $('<tr>').append(
                $('<th>').text(item.ID),
                $('<td>').text(item.Titulo),
                $('<td>').text(item.Data),
                $('<td>').text(item.Status),
                //Ia criar uma variável mas não apareceu o btn a partir do segundo registro, acho que pq a primeira já estava em uso
                $('<td>').append($('<a type="button" class="btn btn-primary" href="/estabelecimento/evento/ID">Editar</a>'))).appendTo('.table');
        });
        $('#JSON').remove();
    });

   
});