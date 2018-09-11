$(document).ready(function () {
    /* Criação das Máscaras */

    //Pega a data atual do sistema, apenas a data (DD)
    var data = new Date();

    //Expressão Regular Para Validar os Campos E-mail e Senha
    var valida_email = /[a - z0 - 9!#$ %& '*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&' * +/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/;
   /* var valida_senha = /"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"/; Minimo 8 caracteres, 1 letra, 1 numero, 1 caracter especial*/

    $(".cpf").inputmask({ 'mask': '999.999.999-99' });
    $(".cnpj").inputmask({ 'mask': '99.999.999/9999-99' });
    $(".data").inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: "__/__/____" });
    $(".datepicker").inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: "__/__/____" });
    $(".datepicker").datepicker({ language: "pt-BR", format: "dd/mm/yyyy", autoclose: true, endDate: data.getDate().toString() });
    $(".cep").inputmask({ 'mask': '99999-999' });
    $(".celular").inputmask({ 'mask': '(99)9999-9999[9]' });

    /*Adicionar classes de Validação*/
    $("form").attr('novalidate', '');
    $("form").addClass("needs-validation");
   
    //Checa se algum input tem a classe inválido e se tiver mostra a div de alerta
    if ($("input").hasClass(".is-invalid")) {
        $(".invalid-feedback").show;
    }

    $(".needs-validation").on('submit', function (event) {
        //Limpa o campo E-mail se não for válido
        if (valida_email.test($(".email").val()) === false) {
                $(".email").val('');   
        }
        //Limpa o campo CNPJ se não for válido
        //if (!validarCNPJ($('.cnpj').val())) {
        //    $(".cnpj").val('');
        //}
        //Limpa o campo CPF se não for válido
        //if (!validarCPF($('.cpf').val())) {
        //    $(".cpf").val('');
        //}
        //Limpa o campo Confirme a Senha se os valores forem diferentes
        if ($(".senha").val() != $(".confirmaSenha").val()) {
            $(".confirmaSenha").val('');
        } /*else if (valida_senha.test($(".senha").val()) === false) {
            $(".senha").val('');
        } */
        //Faz a validação de tudo o que está com required, enquanto estiver, trava o submit
        //Por default a validação com maior prioridade é o campo vazio, por isso que eu sempre limpo os campos, pra que ele continue inválido
        if ($(".needs-validation")[0].checkValidity() === false) {
            $(".needs-validation").addClass("was-validated");
            event.preventDefault();
        }
    });

    /*Limpa os campos caso clique*/
    $(".linkEsqueceu").click(function () {
        $(".email").val('');
        $(".senha").val('');
    });

    /*Menu Fixo
        Última vez editado 09/09/2018 - Lucas Lima*/
    $(window).scroll(function () {
        if ($(window).scrollTop() > 52) {
            $('header').addClass('menu_fixo');
        }
        else {
            $('header').removeClass('menu_fixo');
        }
    });
    
    //function validarCNPJ(cnpj) {

    //    //Se vc der um console.log($('.cnpj').val()) vai ver que ele vem com ponto e barra, daí é necessário remover com esse Regex
    //    cnpj = cnpj.replace(/[^\d]+/g, '');
    //    //Se o campo estiver vazio, retorne falso pra essa função
    //    if (cnpj == '') return false;
    //    //Se for menor que 14 digitos, retorne falso
    //    if (cnpj.length != 14) return false;
    //    //Lista de CNPJs que são inválidos, assim, já removemos aqui, sem precisar calcular nada, até pq em certos desses casos o digito verificador vai dar válido
    //    if (cnpj == "00000000000000" ||
    //        cnpj == "11111111111111" ||
    //        cnpj == "22222222222222" ||
    //        cnpj == "33333333333333" ||
    //        cnpj == "44444444444444" ||
    //        cnpj == "55555555555555" ||
    //        cnpj == "66666666666666" ||
    //        cnpj == "77777777777777" ||
    //        cnpj == "88888888888888" ||
    //        cnpj == "99999999999999")
    //        return false;

    //    //Cálculo que valida o Digito Verificador
    //     tamanho = cnpj.length - 2
    //     numeros = cnpj.substring(0, tamanho);
    //     digitos = cnpj.substring(tamanho);
    //     soma = 0;
    //     pos = tamanho - 7;
    //    for (i = tamanho; i >= 1; i--) {
    //        soma += numeros.charAt(tamanho - i) * pos--;
    //        if (pos < 2)
    //            pos = 9;
    //    }
    //     resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    //    //Se o resultado da conta do DV não for o primeiro DV informado, ou seja, os que foram separados na variável digitos, 
    //    //então retorne falso a função
    //    if (resultado != digitos.charAt(0))
    //        return false;
        
    //    tamanho = tamanho + 1;
    //    numeros = cnpj.substring(0, tamanho);
    //    soma = 0;
    //    pos = tamanho - 7;
    //    for (i = tamanho; i >= 1; i--) {
    //        soma += numeros.charAt(tamanho - i) * pos--;
    //        if (pos < 2)
    //            pos = 9;
    //    }
    //    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    //    ///Se o resultado da conta do DV não for o segundo DV informado, ou seja, os que foram separados na variável digitos, 
    //    //então retorne falso a função
    //    if (resultado != digitos.charAt(1))
    //        return false;
    //    //Caso contrário o CNPJ é válido
    //    return true;
    //}

    //function validarCPF(cpf) {
    //    //Se vc der um console.log($('.cpf').val()) vai ver que ele vem com ponto e traço, daí é necessário remover com esse Regex
    //    cpf = cpf.replace(/[^\d]+/g, '');
    //    //Se o campo estiver vazio, retorne falso pra essa função
    //    if (cpf == '') return false;
    //    //Se for menor que 11 digitos, retorne falso
    //    if (cpf.length != 11) return false;
    //    //Lista de CPFs que são inválidos
    //    if (cpf == "00000000000" ||
    //        cpf == "11111111111" ||
    //        cpf == "22222222222" ||
    //        cpf == "33333333333" ||
    //        cpf == "44444444444" ||
    //        cpf == "55555555555" ||
    //        cpf == "66666666666" ||
    //        cpf == "77777777777" ||
    //        cpf == "88888888888" ||
    //        cpf == "99999999999")
    //        return false;

    //    var soma;
    //    var resto;
    //    soma = 0;

    //    for (i = 1; i <= 9; i++) soma = soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
    //    resto = (soma * 10) % 11;

    //    if ((resto == 10) || (resto == 11)) resto = 0;
    //    if (resto != parseInt(strCPF.substring(9, 10))) return false;

    //    soma = 0;
    //    for (i = 1; i <= 10; i++) soma = soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
    //    resto = (soma * 10) % 11;

    //    if ((resto == 10) || (resto == 11)) resto = 0;
    //    if (resto != parseInt(strCPF.substring(10, 11))) return false;
    //    return true;
    //}
});