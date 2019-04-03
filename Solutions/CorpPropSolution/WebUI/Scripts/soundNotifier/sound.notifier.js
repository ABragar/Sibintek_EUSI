/**
 * @author Vadim Smirnov <smirnov.v.vadim@gmail.com>
 * Copyright (c) 2015 Vadim Smirnov - released under MIT License
**/
(function ($) {

    /**
     * Private key generation method
     * @returns {string} Simple guid string
     */
    var generateKey = function () {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();
    };

    /**
     * Default end event
     * @returns {} 
     */
    var onEnded = function() {
        $(this).remove();
    }

    $.extend({

        /**
         * Start playing sound.
         * @param {string} url The url of target sound file.
         * @param {string} [key] The custom key of audio player, would automatic generated if not defined.
         * @param {bool} [loop=false] The key for automatic repeat
         * @returns {string} The current key of audio player.
         */
        soundPlay: function (url, key, loop) {

            key = key ? key : generateKey();
            loop = loop ? 'loop' : '';

            var $player = $('<audio data-item="soundNotifier" ' + loop + ' autoplay="autoplay" id="soundNotifier-' + key + '" style="display:none;" controls="controls"><source src="' + url + '" /></audio>');

            $player[0].onended = onEnded;

            $player.appendTo('body');

            return key;
        },

        /**
         * Stop playing
         * @param {string} [key] The key of player need to stop, would stop last finded player if not defined
         */
        soundStop: function (key) {
            var player;

            if (key)
                player = $('#soundNotifier-' + arguments[0]);
            else
                player = $('audio[data-item="soundNotifier"]:last');

            if (player.length) {
                player[0].pause();
                player.remove();
            }
        }
    });

})(jQuery);
