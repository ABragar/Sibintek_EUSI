(function() {
    'use strict';

    pbaAPI.getMnemonicCounter = function(mnemonic) {

        if (!mnemonic || typeof mnemonic !== 'string') {
            return '';
        }

        var currentValue = globalapp.mnemonicCounterService.register(mnemonic);

        var classAttr = 'class="counter-mnemonic"';

        var dataAttrs = [
            'data-counter-mnemonic="' + mnemonic + '"',
            'data-counter-mnemonic-value="' + currentValue + '"',
        ];

        return '<span ' + classAttr + ' ' + dataAttrs.join(' ') + '>' + currentValue + '</span>';
    };

    pbaAPI.getCompoundMnemonicCounter = function(mnemonics) {

        // not array is noop
        if (!pbaAPI.isArray(mnemonics)) {
            return '';
        }

        mnemonics = pbaAPI.distinct(mnemonics);

        // total mnemonics count
        var summ = 0;

        var dataAttrValue = mnemonics.filter(function(mnemonic) {

            return mnemonic && typeof mnemonic === 'string';

        }).map(function(mnemonic) {

            var value = globalapp.mnemonicCounterService.register(mnemonic);

            summ += value;

            return mnemonic + '=' + value;

        }).join(',');

        var classAttr = 'class="counter-mnemonic counter-mnemonic--compound"';

        var dataAttrs = [
            'data-counter-mnemonics="' + dataAttrValue + '"',
            'data-counter-mnemonic-value="' + summ + '"',
        ];

        return '<span ' + classAttr + ' ' + dataAttrs.join(' ') + '>' + summ + '</span>';
    };

}());
