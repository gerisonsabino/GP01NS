const inputRange = $('#html-input-range');
let htmlInputRange = {
  idNotThere : '<p>Note: Missing input tag id name e.g html-input-range</p>',
  init () {
    if (inputRange.length > 0) {
      let inputRanges = $('#html-input-range');
      inputRanges.parent().addClass('html-inupt-range');
      // if too many input ranges there for now hiding this part.
      // inputRanges.each(function( index ) {
      //   let $this = $(this);
      // });
      let $this = inputRanges;
        // default input range starts at 0 and ends at 100
        $this.attr({
          min: 0,
          max: 100,
          value: 0,
          step: 1
        });
    } else {
      $('input[type=range]').parent().append(htmlInputRange.idNotThere);
    }
  },
  options (inputs) {
	htmlInputRange.init();
	let options = inputs;
	// custom tracker
	$('input[type=range]').parent().addClass('html-input-range-custom');
	$('input[type=range]').parent().append('<div class="hir-tracker-bg"></div><div class="hir-tracker-thumb"></div>');

	let min = options.min || 0;
	let max = options.max || 100;
	let value = options.value || 0;
	let posfix = options.posfix || '';
	let output = options.output;

	if (output) {
		$(output).text(`${min}${posfix}`);
		$(inputRange).data('output', output);
	}

	posfix && $(inputRange).data('posfix', posfix);

	$(inputRange).attr({
		max,
		min,
		value
	});

	options.tooltip === true &&  $('input[type=range]').parent().append(`<div class="tooltip">${min}${posfix}</div>`);
  }
}

$(inputRange).on('input change', inputRange, function (e) {
	/*
	* splitting 100 by input range max value
	* for active tracker and tooltip position.
	*/
	let inputMax = inputRange.attr('max');
	let inputMin = inputRange.attr('min');
	let posfix = inputRange.data('posfix') || '';
	let value = inputRange.val();
	let output = inputRange.data('output');	
	let trackerTooltipMove = (inputRange.val() - inputMin) / (inputMax - inputMin) * 100;

	$('.html-inupt-range .tooltip').css('left', trackerTooltipMove + '%');
	$('.html-input-range-custom .hir-tracker-thumb').css('width', trackerTooltipMove + '%');
	// updating tooltip value based on the range from to.
	$('.html-inupt-range .tooltip').text(`${value}${posfix}`);
	output && $(output).text(`${value}${posfix}`);
});
