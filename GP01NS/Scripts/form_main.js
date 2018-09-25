$(document).ready(function () {
    const data = new Date();
    const valida_email = /[a-z0-9!#$ %& '*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&' * +/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/;
    const valida_senha = /"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"/;
    const limparCampos = _ => { $("input").val('') };
    $(".linkEsqueceu").click(limparCampos);

    /* Criação das Máscaras */
    $(".cpf").inputmask({ 'mask': '999.999.999-99' });
    $(".cnpj").inputmask({ 'mask': '99.999.999/9999-99' });
    $(".data").inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: "__/__/____" });
    $(".datepicker").inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: "__/__/____" });
    $(".datepicker").datepicker({ language: "pt-BR", format: "dd/mm/yyyy", autoclose: true, endDate: data.getDate().toString() });
    $(".cep").inputmask({ 'mask': '99999-999' });
    $(".celular").inputmask({ 'mask': '(99)[9]9999-9999' });

    /*Adicionar feedback inválido*/
    $("input").hasClass(".is-invalid") ? $(".invalid-feedback").show : $(".invalid-feedback").hide;

    /*Validação*/
    $("form").attr('novalidate', '');
    $("form").addClass("needs-validation");
    $(".needs-validation").on('submit', function (event) {
        if ($("input").hasClass("email")) { validarEmail };
        if ($("input").hasClass("cnpj")) { validarCNPJ($('.cnpj').val()) === false ? $(".cnpj").val('') : null };
        if ($("input").hasClass("cpf")) { validarCPF($('.cpf').val()) === false ? $(".cpf").val('') : null };
        if ($(".senha").val() != $(".confirmaSenha").val()) { $(".confirmaSenha").val('');}
        if ($(".needs-validation")[0].checkValidity() === false) {
            $(".needs-validation").addClass("was-validated");
            event.preventDefault();
        }
    });

    /*Funções De Validação*/
    const validarEmail = function () {
        if ($(".email").val().includes('@')) {
            let testa_email = valida_email.test($(".email").val());
            testa_email === false ? $(".email").val('') : null;
        }
    }

    const validarCNPJ = function(cnpj) {
        cnpj = cnpj.replace(/[^\d]+/g, '');
        if (cnpj == '' ||
            cnpj.length != 14 ||
            cnpj == "00000000000000" ||
            cnpj == "11111111111111" ||
            cnpj == "22222222222222" ||
            cnpj == "33333333333333" ||
            cnpj == "44444444444444" ||
            cnpj == "55555555555555" ||
            cnpj == "66666666666666" ||
            cnpj == "77777777777777" ||
            cnpj == "88888888888888" ||
            cnpj == "99999999999999")
            return false;
    
        let tamanho = cnpj.length - 2;
        let numeros = cnpj.substring(0, tamanho);
        let digitos = cnpj.substring(tamanho);
        let soma = 0;
        let pos = tamanho - 7;

        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }

        let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

        if (resultado != digitos.charAt(0))
            return false;
        
        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;

        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }

        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

        if (resultado != digitos.charAt(1))
            return false;
        return true;
    }

    const validarCPF = function(cpf) {
        cpf = cpf.replace(/[^\d]+/g, '');
        if (cpf == '' ||
            cpf.length != 11 ||
            cpf == "00000000000" ||
            cpf == "11111111111" ||
            cpf == "22222222222" ||
            cpf == "33333333333" ||
            cpf == "44444444444" ||
            cpf == "55555555555" ||
            cpf == "66666666666" ||
            cpf == "77777777777" ||
            cpf == "88888888888" ||
            cpf == "99999999999")
            return false;

        let soma;
        let resto;
        soma = 0;

        for (i = 1; i <= 9; i++) soma = soma + parseInt(cpf.substring(i - 1, i)) * (11 - i);
        resto = (soma * 10) % 11;

        if ((resto == 10) || (resto == 11)) resto = 0;
        if (resto != parseInt(cpf.substring(9, 10))) return false;

        soma = 0;
        for (i = 1; i <= 10; i++) soma = soma + parseInt(cpf.substring(i - 1, i)) * (12 - i);
        resto = (soma * 10) % 11;

        if ((resto == 10) || (resto == 11)) resto = 0;
        if (resto != parseInt(cpf.substring(10, 11))) return false;
        return true;
    }
});