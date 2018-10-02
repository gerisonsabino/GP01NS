$(function () {
    $("#a-esqueci").click(esqueci);
});

function esqueci() {
    $("form").html("<a  target='_blank' href='/inicio/'><img src='/Imagens/Views/Entrar/logo.png'></a><h5 style='font-weight: bold;' >Seja Bem-Vindo(a)</h5><div class='separador'>&nbsp;</div><p style:margin-bottom:16px!important; >No campo abaixo informe seu usuário ou e-mail.</p><div id='erro'></div><div class='form-group'><div class='form-group'><input type='text' class='form-control' id='Esqueci' name='Esqueci' aria-describedby='emailHelp' required='required' placeholder='usuário ou e-mail' /><label class='invalid-feedback'>Nome de Usuário ou E-mail inválido!</label></div><button style='margin-bottom:25px!important; margin-top:5px!important;' type='submit' class='btn btn-danger'>Quero solicitar novo acesso!</button><a href='/entrar/'>&larr; voltar</a>");
}