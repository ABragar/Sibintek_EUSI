/* globals $, pbaAPI, globalapp */
(function() {
	'use strict';

	// ############
	// DEPENDENCIES
	// ############

	var socketService = globalapp.socketService;
	var events = globalapp.events;

	// #####################################
	// PRIVATE DATA STORAGES (the same data)
	// #####################################

    // hash object for fast counter finding by mnemonic
	var _mnemonics = {};

    // hash object for ensure that any counter of a given type is exists
	var _types = {};

    // for custom queries
	var _data = [];

	// ###############
	// PRIVATE HELPERS
	// ###############

	var createCounter = function(mnemonic) {
		return {
			mnemonic: mnemonic,
			type: null,
			updateRequired: false,
			isUpdating: false,
			value: 0,
		};
	};

	var parseCounterData = function(counterData) {
		return {
			mnemonic: counterData.Mnemonic,
			type: counterData.Type,
			value: counterData.Count,
		};
	};

	var deserializeCompound = function(attribute) {
		return attribute.split(/\s*,\s*/).reduce(function(result, attr) {
			var splitted = attr.split('=');
			var mnemonic = splitted[0];
			var value = parseInt(splitted[1]);

			if (mnemonic && !isNaN(value)) {
				result.push({
					mnemonic: mnemonic,
					value: value,
				});
			}

			return result;
		}, []);
	};

	var updateDom = function(counters) {
		counters.forEach(function(counter) {

			// CHANGE ELEMENTS TEXT VALUE FROM COUNTER DATA
			$('[data-counter-mnemonic="' + counter.mnemonic + '"]').each(function() {
				this.setAttribute('data-counter-mnemonic-value', counter.value);
				this.textContent = counter.value;
			});

			// CHANGE COMPOUND ELEMENTS TEXT VALUE FROM COUNTER DATA
			$('[data-counter-mnemonics*="' + counter.mnemonic + '"]').each(function() {

				// READ ATTRIBUTE VALUE
				var attribute = this.getAttribute('data-counter-mnemonics');

				// PARSE TO { mnemonic, value } OBJECT
				var data = deserializeCompound(attribute);

				// SUMM ALL MNEMONICS EXCEPT OLD VALUE OF CURRENT ITERATION COUNTER
				var value = data.reduce(function(result, item) {
					if (item.mnemonic !== counter.mnemonic) {
						result += item.value;
					}
					return result;
				}, 0);

				var totalValue = value + counter.value;

				// WRITE SUMM TO DOM
				this.textContent = totalValue;
				this.setAttribute('data-counter-mnemonic-value', totalValue);

				var rAttr = new RegExp('(' + counter.mnemonic + '=)\\d+');
				var newAttr = attribute.replace(rAttr, '$1' + counter.value);

				// APPLY NEW VALUE TO PARTICULAR MNEMONIC VALUE OF APPROPERIATE ATTRIBUTE
				this.setAttribute('data-counter-mnemonics', newAttr);
			});
		});
	};

	var updateCounters = pbaAPI.debounce(function() {

		// SELECT THOSE, WHICH ARE MARKED AS "TO UPDATE" BUT ARE NOT UPDATING NOW
		var counters = _data.filter(function(counter) {
		    return counter.updateRequired && !counter.isUpdating;
		});

		// IF NO SUCH COUNTERS
		if (!counters.length) {
			return;
		}

		// MARK SELECTED ONES AS THEY ARE UPDATING
		counters.forEach(function(counter) {
			counter.isUpdating = true;
		});

		// GET MNEMONICS OF SELECTED COUNTERS
		var mnemonics = counters.map(function(counter) {
			return counter.mnemonic;
		});

		// REQUEST DATA FROM SERVER
		$.getJSON('/communication/getmnemoniccounters', $.param({ mnemonics: mnemonics }, true), function(data) {

			// IF SERVER RETURNS A BAD DATA
			if (!data) {
				pbaAPI.errorMsg('Не удалось обновить мнемоники');
				return;
			}

			// MARK ALL REQUESTED ONES, THAT THEY ARE NOT UPDATING ANYMORE
			counters.forEach(function(counter) {
				counter.isUpdating = false;
			});

			// CONVERT SERVER FORMAT TO CLIENT FORMAT
			data = data.map(parseCounterData);

			// UPDATE COUNTERS
			data.forEach(function(item) {

				// EXTRACT COUNTER
			    var counter = _mnemonics[item.mnemonic];

				// UPDATE COUNTER DATA
			    counter.type = item.type;
			    counter.value = item.value;
				counter.updateRequired = false;

				// ENSURE TYPE
				_types[counter.type] = counter;
			});

			// UPDATE DOM
			updateDom(counters);

		});

	}, { time: 250 });

	var updateByType = function(type) {

		// IF NO SUCH TYPE
		if (!_types[type]) {
			return;
		}

		// SELECT COUNTERS OF THIS TYPE WHICH IS NOT UPDATING NOW
		var counters = _data.filter(function(counter) {
			return counter.type === type && !counter.isUpdating;
		});

		// IF NO SUCH COUNTERS
		if (!counters.length) {
			return;
		}

		// MARK SELECTED AS "TO UPDATE"
		counters.forEach(function(counter) {
		    counter.updateRequired = true;
		});

		// QUEUE UPDATE PROCESS
		updateCounters();
	};

	// ##########
	// MODULE API
	// ##########
	globalapp.mnemonicCounterService = {

		/**
		 * @param  {string} mnemonic Register given mnemonic if it's a non empty string.
		 * @return {number} Current counter value (if it's a new counter, returns 0).
		 */
		register: function(mnemonic) {

			// CHECK MNEMONIC TYPE AND FORMAT
			if (!/\S+/.test(mnemonic)) {
				throw new Error('mnemonic has wrong format');
			}

			// LOOKUP FOR COUNTER
			var counter = _mnemonics[mnemonic];

			// THIS COUNTER IS NOT REGISTERED
			if (!counter) {

				// CREATE NEW COUNTER
				counter = createCounter(mnemonic);

				// ADD TO HASH COLLECTION HELPER
				_mnemonics[mnemonic] = counter;

				// ADD TO ARRAY HELPER
				_data.push(counter);
			}

			// MARK THIS COUNTER AS "TO UPDATE"
			counter.updateRequired = true;

			// QUEUE UPDATE PROCESS IF COUNTER IS NOT UPDATING NOW
			if (!counter.isUpdating) {
				updateCounters();
			}

			return counter.value;
		},
	};

	// #################
	// LISTEN TO SOCKETS
	// #################

	socketService.on(events.updateCounters, function(type) {
		updateByType(type);
	});

}());
