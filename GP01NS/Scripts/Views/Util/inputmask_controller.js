$(document).ready(function(){
	var data = new Date();
	
	//Adicionar máscara aos campos
	$(".cpf").inputmask({'mask': '999.999.999-99'});
	$(".cnpj").inputmask({'mask': '99.999.999/9999-99'});
	$(".datepicker").inputmask({alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: "__/__/____"});
	$(".datepicker").datepicker({language: "pt-BR", format: "dd/mm/yyyy", autoclose: true, endDate: data.getDate().toString()});
	$(".cep").inputmask({'mask': '99999-999'});
	$(".celular").inputmask({'mask': '(99)9999-9999[9]'});
});