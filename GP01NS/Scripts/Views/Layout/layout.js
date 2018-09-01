function fieldsRequest() {
	$('[required]').each(function () {
		const textError = $(this).attr('placeholder') || 'Preencha esse campo';
		$(this).after(`<div class="invalid-feedback">${textError}!</div>`)

	});
}

function selectCustom() {
	$('.select-custom').each((idx, el) => {
		const placeholder = $(el).attr('placeholder') || '';

		$(el).select2({
			placeholder
		});
	});
}


function textareaAutoresize() {
	$('textarea').each(function() {
		const
			it = this,
			heigthInit = $(it).height(),
			heigthEnd = 500;

		$(it)
			.on('change cut paste drop keydown', function() {				
				setTimeout(() => {
					$(it).height('auto');
					const scrollHeight = $(it).prop('scrollHeight');
					if (scrollHeight > heigthInit && scrollHeight < heigthEnd) {
						$(it).height(scrollHeight);
					} else {
						$(it).height(heigthEnd);
					}
				}, 0);
			});
	});	
}

$(function () {
	fieldsRequest()
	selectCustom();
	textareaAutoresize();
});
