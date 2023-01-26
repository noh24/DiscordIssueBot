// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#floatingSearch').keyup(function() {  
  var value = $(this).val().toLowerCase();    
  $('ul li').filter(function() {
    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)});
    });


const main = $('html');

// The scrollTop function
// scrolls to the top
function scrollTop() {
    console.log('scrolling to top')
    main.animate({scrollTop: 0},1000,"linear",scrollBottom /* this is a callback it means when we are done scrolling to the top, scroll to the bottom */)
}

function scrollBottom() {
    console.log('scrolling to bottom')
    main.animate({scrollTop: document.body.offsetHeight},1000,"linear",scrollTop /* this is a callback it means when we are done scrolling to the bottom, scroll to the top */)
}

// this kicks it off
// again only running $(document).ready once to increase performance.
// Once scrollTop completes, it calls scrollBottom, which in turn calls scrollTop and so on
$(document).ready(scrollTop);