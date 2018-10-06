$(document).ready(function () {
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
                $("<th scope='row'>").text(item.IDEvento),
                $('<td>').text(item.Evento),
                $('<td>').text(item.Data),
                $('<td>').text(item.Estabelecimento),
                $("<td>").html("<button type='button' data-opc='1' class='btn btn-primary' title='ACEITAR'><span class='fa fa-thumbs-up'></span></button> <button type='button' data-opc='0' class='btn btn-secondary' title='RECUSAR'><span class='fa fa-thumbs-down'></span></button>")
            ).appendTo('.table')
        });

        $("#tb-convites").dataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.19/i18n/Portuguese-Brasil.json"
            }
        });

        $('#JSON').remove();
    });
});