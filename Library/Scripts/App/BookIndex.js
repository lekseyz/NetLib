/// <reference path="../jquery-3.7.0.js" />
/// <reference path="../jquery-3.7.0.intellisense.js" />

(
    function () {
        var $btn = $('#add-book-btn');
        var $form = $('#new-book-form');

        $btn.on('click', function () {
            if ($form.css('display') === 'none') {
                $form.css('display', 'block')
            }
            else {
                $form.css('display', 'none')
            }
        })
    }
)();