$(function(){
    var slickOpt = {
    infinite: true,
    autoplaySpeed: 3000,
    slidesPerRow: 1,
    arrows: true,
    autoplay: true,
    prevArrow: $('.prev'),
    nextArrow: $('.next'),
    };
    $('.carousel').slick(slickOpt);
});

$(window).on('scroll',function(){
    if($(window).scrollTop()){
        $('nav').addClass('dark');
        $('#logo').addClass('invis');
    }
    else{
        $('nav').removeClass('dark');
        $('#logo').removeClass('invis');
    }
});


document.onreadystatechange = function () {
    var state = document.readyState
    if (state == 'interactive') {
         document.getElementById('contents').style.visibility="hidden";
    } else if (state == 'complete') {
        setTimeout(function(){
           document.getElementById('interactive');
           document.getElementById('load').style.visibility="hidden";
           document.getElementById('contents').style.visibility="visible";
        },1000);
    }
  }
