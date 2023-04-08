$(document).ready(function () {
    $(".alert").addClass("in").fadeOut(4500);

    /* swap open/close side menu icons */
    $('[data-toggle=collapse]').click(function () {
        // toggle icon
        $(this).find("i").toggleClass("glyphicon-chevron-right glyphicon-chevron-down");
    });
    

    $(window).resize(function () {
        var h = $('.navbar-fixed-bottom').offset().top - $('.navbar-fixed-bottom').outerHeight() - 10;
        var $containerWidth = $(window).width();
        if ($containerWidth > 880) {
            $('#sidebar').css('height', h + 'px');
            $('#main-content').css('height', h + 'px');
        }
    });
    $(window).resize();
});