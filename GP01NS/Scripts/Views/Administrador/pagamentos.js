﻿$(document).ready(function () {
    //Pega o value do input de id JSON
    var response = $('#JSON').val();
    //Dá o parse nele
    response = $.parseJSON(response);

    $(function () {
        $.each(response, function (i, item) {
            //Anexe a tabela de acordo com o que foi separado, ID, Titulo, Data, Status e pra cada um deles no fim cria um a
            $('<tr>').append(
                $('<td>').text(item.REF),
                $('<td>').text(item.Valor),
                $('<td>').text(item.Status),
                $('<td>').text(item.DtPagamento),
                $('<td>').text(item.Vencimento),
                $('<td>').text("@" + item.Usuario + " (" + item.TipoUsuario + ")"),
                $("<td width='100px'>").html("<div class='btn-group btn-group'><a href='/administrador/XML/" + item.Transacao + "' target='_blank' class='btn btn-primary'>XML</a></div>")
            ).appendTo('#tb-pagamentos')
        });

        $("#tb-pagamentos").dataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.19/i18n/Portuguese-Brasil.json"
            }
        });

        $('#JSON').remove();
    });
});