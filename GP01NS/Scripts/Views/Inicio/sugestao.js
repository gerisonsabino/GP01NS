const sugestao = {
	tipos: ['evento', 'musico', 'estabelecimento', 'enteresse'],
	pagina: {},
	quantidadeItens: {},
	loading: false,
	todosItensCarregados: {},

	componente: item => 
		`<a class="sugestao-item" href="${sugestao.urlPerfil(item)}">
			<div class="sugestao-item-imagem" style="background-image: url(${item.Imagem})">
				<div class="col-12 pb-2 flex">
					<div class="nota-${item.Nota} sugestao-item-nota ml-auto">
						<span class="voto-1 oi oi-heart"></span>
						<span class="voto-2 oi oi-heart"></span>
						<span class="voto-3 oi oi-heart"></span>
						<span class="voto-4 oi oi-heart"></span>
						<span class="voto-5 oi oi-heart"></span>
					</div>
				</div>
			</div>
			<div class="sugestao-item-texto">
				<div class="sugestao-item-nome">
					${item.Nome}
				</div>
				<div class="sugestao-item-endereco">
					${(item.Endereco != null && item.Endereco != "") ? item.Endereco : "&nbsp;" }
				</div>
				<div class="sugestao-item-tipo">
					${ (item.Badges != null && item.Badges != "") ? sugestao.badges(item.Badges) + "<br />" : "" }
					<span class="badge badge-secondary">${item.Tipo.toUpperCase()}</span>
				</div>
			</div>
		</a>`,

    urlPerfil: function (item) {
		switch (item.Tipo.toLowerCase()) {
			case 'evento':
				return '/inicio/evento/' + item.ID;
				break;
			case 'músico':
				return '/inicio/musico/' + item.Username;
				break;
			default:
				return '/inicio/estabelecimento/' + item.Username;
		}
    },

    badges: function (str) {
        const json = eval(str);
        let html = "";

        for (i of json) {
            html += "<span class='badge bg-primary'>" + i.toUpperCase() + "</span>";
        }

        return html;
	},

	verificaScroll: {
		top: function (tipo) {
			const win = $(`.sugestao-lista-${tipo}`);
			const screenTop = win.scrollTop();

			const elementBottom = $(`.sugestao-lista-${tipo} > .flex`).height();

			return (elementBottom - 300) < screenTop;
		},

		left: function (tipo) {
			const janela = $(`.sugestao-lista-${tipo}`);
			const janelaLargura = janela.width();
			const janelaScrollFim = janelaLargura + janela.scrollLeft();

			const sugestoesLargura = $(`.sugestao-lista-${tipo} > .flex`).width();
			return (sugestoesLargura - 200) < janelaScrollFim;
		},
	},

	sugestoesAcabando: function(tipo) {
		return $('body').width() <= 955 ? sugestao.verificaScroll.left(tipo) : sugestao.verificaScroll.top(tipo);
	},

	ajustarLarguraJanela: function (tipo) {
		const larguraJanela = $('body').width();
		const larguraSugestao = larguraJanela <= 955 ? 271 : 0;
		$(`.sugestao-lista-${tipo} > .flex`).css({ 'min-width': sugestao.quantidadeItens[tipo] * larguraSugestao });
	},

	lista: function (tipo) {
		sugestao.pagina[tipo]++;
		sugestao.loading = true;
		$(`.sugestao-lista-${tipo} .load`).toggle(sugestao.loading);

		$.getJSON(`/inicio/getsugestoes/?page=${sugestao.pagina[tipo]}&tipo=${tipo}`, function (resp) {
			let itens = '';
			
            resp = JSON.parse(resp);

			sugestao.todosItensCarregados[tipo] = resp.length === 0;
			sugestao.quantidadeItens[tipo] += resp.length;

			for (item of resp) {
				itens += sugestao.componente(item);
			}

			$(`.sugestao-lista-${tipo} > .flex`).append(itens);
			sugestao.ajustarLarguraJanela(tipo);

			$(`.sugestao-lista-${tipo}`).prev('.sugestao-titulo').show();

			sugestao.loading = false;
			$(`.sugestao-lista-${tipo} .load`).toggle(sugestao.loading);
		});
	},

	lazyLoad: function () {
		$('.sugestao-lista > .custom-scrollbar').on("scroll", function () {			
			const tipo = $(this).data('tipo');
			!sugestao.todosItensCarregados[tipo] && !sugestao.loading && sugestao.sugestoesAcabando(tipo) && sugestao.lista(tipo);
		});
	},

	mudaTipo: function (tipo) {
		$('.sugestao-lista').removeClass('show');
		$(`.sugestao-lista-${tipo}`).parent().addClass('show');
	},

	inicio: function () {
		for (tipo of sugestao.tipos) {
			sugestao.pagina[tipo] = 0;
			sugestao.quantidadeItens[tipo] = 0;
			sugestao.todosItensCarregados[tipo] = false;
			sugestao.lista(tipo);
		}

		sugestao.lazyLoad();

		$(window).resize(function () {
			for (tipo of sugestao.tipos) {
				sugestao.ajustarLarguraJanela(tipo);
			}
		});
	}
}

$(function () {
	sugestao.inicio();
})