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

                $("#Endereco_Logradouro").val(e.logradouro);
                $("#Endereco_Bairro").val(e.bairro);
                $("#Endereco_Cidade").val(e.localidade);
                $(".UF").val(e.uf);
                $("#Endereco_UF").val(e.uf);
                $("#Endereco_IDMunicipio").val(e.ibge);

                $("#Endereco_Logradouro").attr("readonly", "readonly");
                $("#Endereco_Bairro").attr("readonly", "readonly");
                $("#Endereco_Cidade").attr("readonly", "readonly");
                $("#Endereco_Cidade").attr("readonly", "readonly");
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
    $("#Endereco_Logradouro").val("");
    $("#Endereco_Bairro").val("");
    $("#Endereco_Cidade").val("");
    $(".UF").val("AC");
    $("#Endereco_UF").val("");
    $("#Endereco_IDMunicipio").val("");

    $("#Endereco_Logradouro").removeAttr("readonly");
    $("#Endereco_Bairro").removeAttr("readonly");
    $("#Endereco_Cidade").removeAttr("readonly");
    $(".UF").removeAttr("disabled");
}