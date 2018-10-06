$(document).ready(function () {
        //Pega o value do input de id JSON
        var response = $('#JSON').val();
        //Dá o parse nele
        response = $.parseJSON(response);

        $(function(){
            $.each(response, function (i, item) {
                //Anexe a tabela de acordo com o que foi separado, ID, Titulo, Data, Status e pra cada um deles no fim cria um a
                $('<tr>').append(
                    $('<th>').text(item.Data),
                    $('<td>').text(item.Evento),
                    $('<td>').text(item.Estabelecimento),
                    $('<td>').text(item.Endereco),
                    $('<td>').append($("<a class='btn btn-primary' href='/estabelecimento/evento/" + item.ID + "'><span class='fa fa-chevron-right'></span> Abrir</a>"))).appendTo('.table');
            });

            $("#tb-agenda").dataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.19/i18n/Portuguese-Brasil.json"
                },
                "order": [[0, 'desc']]
            });

            $('#JSON').remove();
        });
    });