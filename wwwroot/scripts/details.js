"use strict";

// Initialiser les "tooltips" de Bootstrap
// (https://getbootstrap.com/docs/4.0/components/tooltips/)
$(function () {
	$('[data-toggle="tooltip"]').tooltip()
})

let childImage = document.querySelector(".childImage");

// Juste une petite demo avec la propriete object-fit
// Bouger la souris sur l'image "zoome" dessus
// Mais si l'image est trop petite elle est seulement deplacée
// Je n'ai pas redimensionne toutes les images, juste la premiere
// En vrai je combinerais avec transform scale pour zoomer
childImage.addEventListener("mouseover", function(e) {
	e.target.style.objectFit = "none";
});
childImage.addEventListener("mousemove", function(e) {
	// produit croise + limite
	let rect = e.target.getBoundingClientRect();
	let xLocal = e.clientX - rect.left;
	let yLocal = e.clientY - rect.top;

	let x = Math.min(xLocal * 100 / e.target.offsetWidth, 100);
	let y = Math.min(yLocal * 100 / e.target.offsetHeight, 100);

	e.target.style.objectPosition = `${x}% ${y}%`;
});
childImage.addEventListener("mouseleave", function(e) {
	e.target.style.objectFit = "fill";
});
