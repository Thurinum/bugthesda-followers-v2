"use strict";

import * as Shared from "./shared.mjs";

// Initialiser les "tooltips" de Bootstrap
// (https://getbootstrap.com/docs/4.0/components/tooltips/)
$(function () {
	$('[data-toggle="tooltip"]').tooltip()
})

Shared.fadeIn();

// Liens
let cards = document.querySelectorAll(".childCard");

for (let i = 0; i < cards.length; i++)
	cards[i].addEventListener("click", () => Shared.goto("/content/details.html"));
