var regex = /^[0-9]{8}$/;

$(function () {
    $(".cep").on("change", function () {
        getEndereco();
    });
});

function getEndereco()  {
    $(".cep").attr("readonly", "readonly");

    var cep = $(".cep").val().replace(/\D/g, '');

    if (cep != "" && regex.test(cep)) {
        $.get('https://viacep.com.br/ws/' + cep + '/json/', function (json) {
            if (!("erro" in json)) {
                var e = eval(json);

                $("#Logradouro").val(e.logradouro);
                $("#Bairro").val(e.bairro);
                $("#Cidade").val(e.localidade);
                $(".UF").val(e.uf);
                $("#UF").val(e.uf);
                $("#IDMunicipio").val(e.ibge);

                $("#Logradouro").attr("readonly", "readonly");
                $("#Bairro").attr("readonly", "readonly");
                $("#Cidade").attr("readonly", "readonly");
                $("#Cidade").attr("readonly", "readonly");
                $(".UF").attr("disabled", "disabled");
            }
            else {
                limpar();
            }
        });
    }
    else {
        limpar();
    }

    $(".cep").removeAttr("readonly");
}

function limpar() {
    $("#Logradouro").val("");
    $("#Bairro").val("");
    $("#Cidade").val("");
    $(".UF").val("AC");
    $("#UF").val("");
    $("#IDMunicipio").val("");

    $("#Logradouro").removeAttr("readonly");
    $("#Bairro").removeAttr("readonly");
    $("#Cidade").removeAttr("readonly");
    $(".UF").removeAttr("disabled");
}