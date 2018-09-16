const nota = {
	inicio() {
		const
			nota = 1,
			$this = this;

		$('.nota').each((indice, el) => {
			$(el)
				.addClass(`nota-${nota}`)
				.children('span')
				.on('click', function () { $this.mudaNota(el, this) })
				.each(
					(indiceCoracao, coracao) => $(coracao).data('nota', indiceCoracao + 1)
				)
		});
	},
	mudaNota(el, coracao) {
		const nota = $(coracao).data('nota');
		$(el).attr('class', `nota-${nota}`);
	}
}

$(function () {
	nota.inicio();
})