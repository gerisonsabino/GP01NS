$(document).ready(function(){
    $("form").attr("novalidate", "");
    $("form").addClass("needs-validation");
        //Coloca na variável tudo aquilo que precisar de validação
		var frmValidar = $(".needs-validation");
		//Para saber em que página estou
		var paginaAtual = window.location.href;
    	//Guarda todo meu form dentro de um array a fim de verificar os requireds
		var validation = Array.prototype.filter.call(frmValidar, function(form){
    	form.addEventListener('submit', function(event){
    		//Limpa o campo Confirme a Senha se os valores forem diferentes
            if ($("#senha").val() != $("#confirmacao").val()){
				$("#confirmacao").val('');
			}
    		//Faz a validação de tudo o que está com required, enquanto estiver, trava o submit
    		if(form.checkValidity() === false){
    				form.classList.add('was-validated');
    				event.preventDefault();
    			}
    		});
		}, false);
	
    	//Checa se algum input tem a classe inválido e se tiver mostra a div de alerta
    	if($("input").hasClass(".form-control:invalid")){
    		$(".invalid-feedback").show;
    	}
});
	
    	