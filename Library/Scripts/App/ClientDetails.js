/// <reference path="../jquery-3.7.0.js" />
/// <reference path="../jquery-3.7.0.intellisense.js" />

(
    function () {
        var ids = [['#borrow-btn', '#borrow-form'], ['#edit-client-btn', '#edit-client-form']];

        ids.forEach((elem) => {
            var $btn = $(elem[0]);
            var $form = $(elem[1]);

            $btn.on('click', function () {
                if ($form.css('display') === 'none') {
                    $form.css('display', 'block');
                }
                else {
                    $form.css('display', 'none');
                }
            });
        });
    }
)();