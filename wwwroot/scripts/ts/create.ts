

// Juste une petite demo avec la propriete object-fit
// Bouger la souris sur l'image "zoome" dessus
// Mais si l'image est trop petite elle est seulement deplacée
// Je n'ai pas redimensionne toutes les images, juste la premiere
// En vrai je combinerais avec transform scale pour zoomer
$(".picturePreview").addEventListener("mouseover", function (e: any) {
	e.target.style.objectFit = "none";
});
$(".picturePreview").addEventListener("mousemove", function (e: any) {
	// produit croise + limite
	let rect = e.target.getBoundingClientRect();
	let xLocal = e.clientX - rect.left;
	let yLocal = e.clientY - rect.top;

	let x = Math.min(xLocal * 100 / e.target.offsetWidth, 100);
	let y = Math.min(yLocal * 100 / e.target.offsetHeight, 100);

	e.target.style.objectPosition = `${x}% ${y}%`;
});
$(".picturePreview").addEventListener("mouseleave", function (e: any) {
	e.target.style.objectFit = "cover";
});