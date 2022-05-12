"use strict";

$("#cardsWrapper").onwheel = function (e) {
	$("#cardsWrapper").scrollLeft += e.deltaY;
}