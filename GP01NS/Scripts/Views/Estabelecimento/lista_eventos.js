$(document).ready(function () {
    var response = $('#JSON').val();
    response = $.parseJSON(response);
    var lista = $('.listaeventos');

    $(function () {
        var html = "";

        $.each(response, function (i, item) {

            html += "<a href='/inicio/evento/" + item.ID + "' class='list-group-item list-group-item-action flex-column align-items-start'>";
            html += "    <div class='d-flex w-100 justify-content-between'>";
            html += "        <h6 class='mb-1'>" + item.Titulo +  "</h6>";
            html += "        <small class='text-muted'>" + item.Data + "</small>";
            html += "    </div>";
            html += "    <p class='mb-1'>" + $("#endereco").text() + "</p>";
            html += "    <small class='text-muted'>" + $("#estabelecimento").attr("data-nome")  + "</small>";
            html += "</a>";

        });

        if (html != "")
            $(html).appendTo(lista);
        else
            $("#row-agenda").remove();

        $('#JSON').remove();
    });
});