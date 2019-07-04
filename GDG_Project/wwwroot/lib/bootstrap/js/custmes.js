$(document).ready(function() {
    $('.popup-with-form').magnificPopup({
        type: 'inline',
        preloader: false,
        focus: '#name',

        // When elemened is focused, some mobile browsers in some cases zoom in
        // It looks not nice, so we disable it:
        callbacks: {
            beforeOpen: function() {
                if ($(window).width() < 700) {
                    this.st.focus = false;
                } else {
                    this.st.focus = '#name';
                }
            }
        }
    });
});



$(document).ready(function() {
    $('.popup-gallery').magnificPopup({
        delegate: 'a',
        type: 'image',
        tLoading: 'Loading image #%curr%...',
        mainClass: 'mfp-img-mobile',
        gallery: {
            enabled: true,
            navigateByImgClick: true,
            preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
        },
        image: {
            tError: '<a href="%url%">The image #%curr%</a> could not be loaded.',
            titleSrc: function(item) {
                return item.el.attr('title');
            }
        }
    });
});




$(function() {
    'use strict';

    // $("html").niceScroll();

    $(".arrow1 i").click(function() {


        $("html, body").animate({
            scrollTop: $(".news").offset().top
        }, 1200);

    });


    $(".arrow2 i").click(function() {


        $("html, body").animate({
            scrollTop: $(".contact").offset().top
        }, 1200);

    });




    $("#header").click(function() {


        $("html, body").animate({
            scrollTop: $(".header").offset().top
        }, 1000);

    });

    $("#fast").click(function() {


        $("html, body").animate({
            scrollTop: $(".fast").offset().top
        }, 1000);

    });

    $("#news").click(function() {


        $("html, body").animate({
            scrollTop: $(".news").offset().top
        }, 1000);

    });
    $("#contact").click(function() {


        $("html, body").animate({
            scrollTop: $(".contact").offset().top
        }, 1000);

    });





    $(".n1c").hide();
    $(".show1").click(function() {
        $(".n1c").slideDown(1500),
            $(".n1").slideUp(1000);

    });
    $(".hide1").click(function() {
        $(".n1c").slideUp(1000),
            $(".n1").slideDown(1500);
    });
});

$('#exampleModal').on('show.bs.modal', function(event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever')
    var body_detal = button.data('content')
    var body_Img = button.data('animation')// Extract info from data-* attributes
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
    var modal = $(this)
    modal.find('.modal-title').text(recipient)
    modal.find('.modal-body input').val(recipient)
    modal.find('.modal-body  h2').text(recipient)
    modal.find('.modal-body  p').html(body_detal)
    modal.find('.modal-body  label').html(body_Img)
   // modal.
  
})