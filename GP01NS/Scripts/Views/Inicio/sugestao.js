const sugestao = {
	pagina: 0,
	loading: false,
	todosItensCarregados: false,

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

	sugestoesAcabando: function() {
		const win = $(window);
		const screenTop = win.scrollTop();
		const screenBottom = screenTop + win.height();

		const elementTop = $('.sugestao-lista').offset().top;
		const elementBottom = elementTop + $('.sugestao-lista').height();

		return (elementBottom - 500) < screenBottom;
	},

	lista: function () {
		sugestao.pagina++;
		sugestao.loading = true;
		$('.sugestao .load').toggle(sugestao.loading);

		$.getJSON("/inicio/getsugestoes/?page=" + sugestao.pagina, function (resp) {
            let itens = '';
            resp = JSON.parse(resp);

			sugestao.todosItensCarregados = resp.length === 0;

			for (item of resp) {
				itens += sugestao.componente(item);
			}

			$('.sugestao-lista').append(itens);

			sugestao.loading = false;
			$('.sugestao .load').toggle(sugestao.loading);
		});
	},

	lazyLoad: function () {
        $(document).on("scroll", function () {
            try {
				!sugestao.todosItensCarregados && !sugestao.loading && sugestao.sugestoesAcabando() && sugestao.lista();
            }
            catch (e) { console.log(e); }
		});
	},

	inicio: function () {
		sugestao.lista();
		sugestao.lazyLoad();
	}
}

$(function () {
	sugestao.inicio();
})