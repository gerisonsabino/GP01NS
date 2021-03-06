﻿$(document).ready(function () {
    //Pega o value do input de id JSON
    var response = $('#JSON').val();
    //Dá o parse nele
    response = $.parseJSON(response);

    $(function () {
        $.each(response, function (i, item) {
            if (i == 0 && response.length > 0) {
                $(".table tbody").html("");
            }
            //Anexe a tabela de acordo com o que foi separado, ID, Titulo, Data, Status e pra cada um deles no fim cria um a
            $('<tr>').append(
                $('<th>').text(item.ID),
                $('<td>').text(item.Titulo),
                $('<td>').text(item.Data),
                $('<td>').text(item.Status),
                //Ia criar uma variável mas não apareceu o btn a partir do segundo registro, acho que pq a primeira já estava em uso
                $('<td>').append($("<a class='btn btn-primary' href='/estabelecimento/evento/" + item.ID + "'><span class='fa fa-chevron-right'></span> Abrir</a>"))).appendTo('.table');
        });

        $("#tb-eventos").dataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.19/i18n/Portuguese-Brasil.json"
            }
        });

        $('#JSON').remove();
    });
});