$(function () {
    $("#btn-seguir").click(toggleSeguir);
});

function toggleSeguir() {
    var a = $(this).attr("data-musico");

    $.post("/inicio/toggleseguir/", { "ID": a }, function (s) {
        if (s == "ok") {
            var t = $("#btn-seguir").text();

            if (t == "Seguir") {
                $("#btn-seguir").removeClass("btn-primary");
                $("#btn-seguir").addClass("btn-danger");
                $("#btn-seguir").text("Deixar de Seguir");
            }
            else {
                $("#btn-seguir").addClass("btn-primary");
                $("#btn-seguir").removeClass("btn-danger");
                $("#btn-seguir").text("Seguir");
            }
        }
    });
}