$(document).ready(function () {
    //Pega o value do input de id JSON
    var response = $('#JSON').val();
    //Dá o parse nele
    response = $.parseJSON(response);

    $(function () {
        $.each(response, function (i, item) {
            //Anexe a tabela de acordo com o que foi separado, ID, Titulo, Data, Status e pra cada um deles no fim cria um a
            $('<tr>').append(
                $('<th>').text(item.Codigo),
                $('<td>').text(item.Nome),
                $('<td>').text(item.Username),
                $('<td>').text(item.Status),
                $('<td>').text(item.Cadastro),
                $('<td>').text(item.Tipo),
                $("<td width='120px'>").html("<div class='btn-group btn-group'><a href='/inicio/musico/" + item.Username + "' target='_blank' class='btn btn-primary'>Perfil</a></div>")
            ).appendTo('#tb-musicos')
        });

        $("#tb-musicos").dataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.19/i18n/Portuguese-Brasil.json"
            }
        });

        $('#JSON').remove();
    });
});