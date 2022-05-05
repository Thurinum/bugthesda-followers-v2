"use strict";

MAIN.style.opacity = "0";

window.onload = function () {
    MAIN.style.opacity = "1";

    registerLinks();
}

$("#cardsWrapper").onwheel = function (e) {
    $("#cardsWrapper").scrollLeft += e.deltaY;
}