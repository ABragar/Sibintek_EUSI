$(function() {
    'use strict';

    // SHOW FORM (ANIMATION)
    $('.account').addClass('account--show'); 

    // TODO:
    // FORCE LABEL ACTIVATION ON BROWSER AUTOFILL
    //$('.input-field > input').each(function() {
    //    if (this.value.trim()) {
    //        $(this).siblings('label').addClass('active');
    //    }
    //});

    // HANDLE CHECKBOX VALUE CHANGE
    $('[type=checkbox]').change(function() {
        this.checked = this.value = $(this).prop('checked');
    });

    // HANDLE CHECKBOX INITIAL VALUE FROM "data-checked" ATTRIBUTE
    $('[type=checkbox][data-checked]').each(function() {
        var isChecked = this.getAttribute('data-checked') === 'true';
        this.value = this.checked = isChecked;
    });

    // SHOW FIELD VALIDATION MESSAGES
    $('[data-validation-for]').each(function() {
        var $validation = $(this);
        var sel = $validation.attr('data-validation-for');
        var msg = $validation.text().trim();

        if (!msg) return;

        $(this).closest('form').find('[name='+sel+"]")
            .addClass('validate invalid')
            .attr('placeholder', ' ')
            .siblings('label[for=' + sel + ']')
                .addClass('active')
                .attr('data-error', msg);
    });

    // SHOW SUMMARY VALIDATION MESSAGES
    $('[data-validation-summary] li').each(function() {
        var msg = $(this).text().trim();

        if (!msg) return;

        Materialize.toast(msg, 4500, 'error');
    });

    // LOADING ON FORM SUBMIT
    $('form').submit(onSubmit);

    // HANDLE LINK FOLLOW ANIMATIONS
    $('a[data-route]').click(function(e) {
        var href = this.getAttribute('href');

        var route = this.getAttribute('data-route');
        var isBack = route === 'back';
        var isForward = route === 'forward';
        var isExit = route === 'exit';

        if (!route || !isBack && !isForward && !isExit) return;

        if (isExit) {
            enableLoading(true);
            return;
        }

        e.preventDefault();

        $(this).closest('.account').addClass('account--fade');

        if (href !== '#') {
            setTimeout(function() {
                location.href = href;
            }, 500);
        }
    });

    // HANDLE CONFIRMATION OF FORM SENDING
    function confirmModal(title, message, callback) {
        var $modal = $('<div class="modal"><div class="modal-content"><h4>' + title + '</h4><p>' + message + '</p></div><div class="modal-footer"><a href="#" class="modal-action modal-close waves-effect btn-flat" data-answer="no">Нет</a><a href="#" class="modal-action modal-close waves-effect btn-flat" data-answer="yes">Да</a></div></div>')
            .appendTo('body');

        $modal.find('[data-answer]').click(function(e) {
            e.preventDefault();

            var answer = this.getAttribute('data-answer');

            callback(answer);
        });

        $modal.openModal({
            dismissible: false,
            complete: function() {
                $modal.remove();
            }
        });
    }
    $('[data-confirm]').submit(function(e) {
        if (this.hasAttribute('data-confirmed')) {
            return;
        }

        e.preventDefault();

        var title = this.getAttribute('data-confirm');
        var msg = this.getAttribute('data-message');
        var form = this;

        enableLoading(false);

        confirmModal(title, msg, function(answer) {
            if (answer === 'yes') {
                form.setAttribute('data-confirmed', '');
                $(form).submit();
            }
        });
    });

    function enableLoading(enable) {
        $('.account').toggleClass('account--loading', enable);
    }

    function onSubmit() {
        enableLoading(true);
    }
});
