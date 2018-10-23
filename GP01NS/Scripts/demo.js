/**
 * demo.js
 * http://www.codrops.com
 *
 * Licensed under the MIT license.
 * http://www.opensource.org/licenses/mit-license.php
 * 
 * Copyright 2016, Codrops
 * http://www.codrops.com
 */
;(function(window) {

	'use strict';

	// taken from mo.js demos
	function isIOSSafari() {
		var userAgent;
		userAgent = window.navigator.userAgent;
		return userAgent.match(/iPad/i) || userAgent.match(/iPhone/i);
	};

	// taken from mo.js demos
	function isTouch() {
		var isIETouch;
		isIETouch = navigator.maxTouchPoints > 0 || navigator.msMaxTouchPoints > 0;
		return [].indexOf.call(window, 'ontouchstart') >= 0 || isIETouch;
	};
	
	// taken from mo.js demos
	var isIOS = isIOSSafari(),
		clickHandler = isIOS || isTouch() ? 'touchstart' : 'click';

	function extend( a, b ) {
		for( var key in b ) { 
			if( b.hasOwnProperty( key ) ) {
				a[key] = b[key];
			}
		}
		return a;
	}

	function Animocon(el, options) {
		this.el = el;
		this.options = extend( {}, this.options );
		extend( this.options, options );

		this.checked = false;

		this.timeline = new mojs.Timeline();
		
		for(var i = 0, len = this.options.tweens.length; i < len; ++i) {
			this.timeline.add(this.options.tweens[i]);
		}

		var self = this;
		this.el.addEventListener(clickHandler, function() {
			if( self.checked ) {
				self.options.onUnCheck();
			}
			else {
				self.options.onCheck();				
			}
			self.timeline.replay();
		});
	}

	Animocon.prototype.options = {
		tweens : [
			new mojs.Burst({})
		],
		onCheck : function() { return false; },
		onUnCheck : function() { return false; }
	};

	function init() {
		/* Icon nota */	
		var grupoCoracao = document.querySelectorAll('.nota span');

		grupoCoracao.forEach(function(coracao) {
			const coracao_icone = document.querySelector('span');

			new Animocon(coracao, {
				tweens : [
					// burst animation
					new mojs.Burst({
						parent: 			coracao,
						radius: 			{30:90},
						count: 				6,
						children : {
							fill: 			'#C0C1C3',
							opacity: 		0.6,
							radius: 		15,
							duration: 	1700,
							easing: 		mojs.easing.bezier(0.1, 1, 0.3, 1)
						}
					}),
					// ring animation
					new mojs.Shape({
						parent: 		coracao,
						type: 			'circle',
						radius: 		{0: 60},
						fill: 			'transparent',
						stroke: 		'#C0C1C3',
						strokeWidth: {20:0},
						opacity: 		0.6,
						duration: 	700,
						easing: 		mojs.easing.sin.out
					})					
				]
			});

		});
		/* Icon nota */
	}
	
	init();
})(window);