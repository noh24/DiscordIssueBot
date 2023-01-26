// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#floatingSearch').keyup(function() {  
  var value = $(this).val().toLowerCase();    
  $('tr').filter(function() {
    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)});
    });


const main = $('.scrollable-tbody');

// The scrollTop function
// scrolls to the top
// function scrollBottom() {
//     console.log('scrolling to top')
//     main.animate({scrollTop: document.body.offsetHeight},3000,"linear",scrollTop)
// }

// function scrollTop() {
//     console.log('scrolling to bottom')
//     main.animate({scrollTop: 0},3000,"linear",scrollBottom)
// }


$(document).ready(function() {

  if ($('.scrollable-tbody').height() > $('.scrollable-tbl').height()) {
      setInterval(function () {
          start();
  }, 3000); 

  }
});

function animateContent(direction) {  
  var animationOffset = $('.scrollable-tbl').height() - $('.scrollable-tbody').height()-30;
  if (direction == 'up') {
      animationOffset = 0;
  }

  console.log("animationOffset:"+animationOffset);
  $('.scrollable-tbody').animate({ "marginTop": (animationOffset)+ "px" }, 5000);
}

function up(){
  animateContent("up")
}
function down(){
  animateContent("down")
}

function start(){
setTimeout(function () {
  down();
}, 2000);
  setTimeout(function () {
  up();
}, 2000);
  setTimeout(function () {
  console.log("wait...");
}, 5000);
}   

  var number = 1 + Math.floor(Math.random() * 20);
  $('.random').text(number);


// this kicks it off
// again only running $(document).ready once to increase performance.
// Once scrollTop completes, it calls scrollBottom, which in turn calls scrollTop and so on
// $(document).ready(scrollTop);