S.accordion = {
    load: function () {
        $('.accordion > .title').off('click').on('click', S.accordion.toggle);
    },

    toggle: function () {
        $(this).toggleClass('expanded');
        var box = $(this).parent().find('> .box, > .menu');
        box.toggleClass('expanded');
        if (box.hasClass('expanded')) {
            $('html, body').animate({
                scrollTop: $(this).offset().top
            }, 700);
        }
    }
};