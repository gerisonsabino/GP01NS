$(document).ready(function () {
    /* Cria��o das M�scaras */

    //Pega a data atual do sistema, apenas a data (DD)
    var data = new Date();

    //Express�o Regular Para Validar os Campos E-mail
    var valida_email = /[a - z0 - 9!#$ %& '*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&' * +/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/;

    $(".cpf").inputmask({ 'mask': '999.999.999-99' });
    $(".cnpj").inputmask({ 'mask': '99.999.999/9999-99' });
    $(".data").inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: "__/__/____" });
    $(".datepicker").inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: "__/__/____" });
    $(".datepicker").datepicker({ language: "pt-BR", format: "dd/mm/yyyy", autoclose: true, endDate: data.getDate().toString() });
    $(".cep").inputmask({ 'mask': '99999-999' });
    $(".celular").inputmask({ 'mask': '(99)9999-9999[9]' });

    /*Valida��o dos Campos*/
    $("form").attr("novalidate", " ");
    $("form").addClass("needs-validation");
   
    //Checa se algum input tem a classe inv�lido e se tiver mostra a div de alerta
    if ($("input").hasClass(".is-invalid")) {
        $(".invalid-feedback").show;
    }
     //Coloca na vari�vel tudo aquilo que precisar de valida��o
    $(".needs-validation").on('submit', function (event) {
        //Limpa o campo E-mail se o email n�o tiver pelo menos um .com
            //if (valida_email.test($(".email").val()) === false) {
            //    $(".email").val('');   
            //}
            ////Limpa o campo Confirme a Senha se os valores forem diferentes
            //if ($(".senha").val() != $(".confirmaSenha").val()) {
            //    $(".confirmaSenha").val('');
            //}
        //Faz a valida��o de tudo o que est� com required, enquanto estiver, trava o submit
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
});