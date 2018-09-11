function fieldsRequest() {
	$('[required]').each(function () {
		const textError = $(this).attr('placeholder') || 'Preencha esse campo';
        $(this).after("<div class='invalid-feedback'>Este campo é obrigatório</div>");

	});
}

function selectCustom() {
	$('.select-custom').each((idx, el) => {
		const placeholder = $(el).attr('placeholder') || '';

		setTimeout(function () {
			$(el).select2({
				placeholder
			});
		}, 30);		
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

	$('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
		if (!$(this).next().hasClass('show')) {
			$(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
		}
		var $subMenu = $(this).next(".dropdown-menu");
		$subMenu.toggleClass('show');

		$(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
			$('.dropdown-submenu .show').removeClass("show");
		});

		return false;
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
});
