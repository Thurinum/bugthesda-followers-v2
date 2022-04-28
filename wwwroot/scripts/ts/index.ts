"use strict";

let currentIndex = -1;
let isCarouselEnabled = true;
let firstTime = true;
let currentTimeout;
let items = $$(".carousel > div");

// carousel style coverflow pour les jeux (parents)
const Carousel = {
	// set card at index as active
	setActiveAt: (index: number) => {
		if (index == currentIndex || !isCarouselEnabled)
			return;

		let container = $(".carousel");
		let target = container.children[index] as HTMLElement;

		if (!target)
			return;

		// if an item is already active, make it inactive
		// also disables its inactive siblings (fade effect)
		let currentItem = container.querySelector(".active");
		if (currentItem) {
			let shown = $$(".showOnActive", currentItem);
			for (let i = 0; i < shown.length; i++) {
				shown[i].style.opacity = "0";
				shown[i].style.transform = "scale(0.9)";
			}

			let leftSibling = currentItem.previousElementSibling;
			let rightSibling = currentItem.nextElementSibling;

			if (leftSibling) leftSibling.className = "hidden";
			if (rightSibling) rightSibling.className = "hidden";

			currentItem.className = "hidden";
		}

		// set target as active
		// set target's sibling as inactive
		let leftSibling  = target.previousElementSibling;
		let rightSibling = target.nextElementSibling;

		if (leftSibling)  leftSibling.className = "inactive";
		if (rightSibling) rightSibling.className = "inactive";

		target.className = "active";

		let hidden = $$(".showOnActive", target);
		for (let i = 0; i < hidden.length; i++) {
			hidden[i].style.opacity = "1";
			hidden[i].style.transform = "scale(1)";
		}
	   
		// center the new active card in container
		main.scrollLeft = target.offsetLeft - (document.body.offsetWidth / 2) + (target.offsetWidth / 2)

		let shortname = ($("input", target) as HTMLInputElement).value;
		let background = $("#background") as HTMLVideoElement;
		let overlay = $("#foreground_mist") as HTMLVideoElement;
		let dir = index < currentIndex ? -1 : 1;

		background.style.left = `${-dir * 100}vw`;
		overlay.style.transform = `scaleX(${dir})`;
		overlay.playbackRate = 2;
		overlay.style.opacity = "0.5";

		setTimeout(() => {
			background.style.transition = "";
			background.style.left = `${dir * 100}vw`;
			setTimeout(() => {
				background.src = `/videos/games/${shortname}/background.mp4#t=15`;
				background.load();

				background.style.transition = "left 0.25s ease-out, transform 0.25s ease-out";
				background.style.left = "0";
				overlay.playbackRate = 0.9;
				overlay.style.opacity = "0.3";
			}, 100);
		}, 100);

		// show clickbait popup on first launch and then every 1/10 times
		if (Math.random() < 0.35)
			showPopup();

		currentIndex = index;
		window.localStorage.setItem("iLastSelectedGame", `${currentIndex}`);
	},

	// afficher plus de details sur un jeu specifique
	showDetails(target: HTMLElement) {
		function setThumbnails() {
			let previews = $$(".followerPreview");
			let id = target.id.slice(5);

			for (let i = 0; i < previews.length; i++) {
				let preview = previews[i] as HTMLImageElement;
				preview.style.opacity = "0";
				fetch(`/${id}/randomthumbnail`)
					.then(data => data.text())
					.then(src => {
						preview.src = "";
						preview.src = src;
						preview.onload = () => { preview.style.opacity = "1"; };
					});
			}
		}

		setThumbnails();

		isCarouselEnabled = false;

		for (let i = 0; i < items.length; i++) {
			let item: HTMLElement = items[i];
			if (item != target)
				item.style.transform = "scale(0)";
		}

		let headers = $$(".showOnActive", target);
		for (let i = 0; i < headers.length; i++) {
			headers[i].style.opacity = "0";
		}

		let input = $("input", target) as HTMLInputElement;
		console.log(input.value)
		fetch(`/${input.value}`)
			.then(data => data.text())
			.then(html => {
				$("#details").innerHTML = html;
				$("#details").style.transform = "translateY(-50%) scale(1)";
			});		
	}
}

document.addEventListener("click", function (e) {
	if ((e.target as HTMLElement).classList.contains("logo"))
		return;

	isCarouselEnabled = true;

	for (let i = 0; i < items.length; i++) {
		let item: HTMLElement = items[i];
		item.style.transform = "";
	}

	$("#details").style.transform = "translateY(-50%) scale(0)";
	Carousel.setActiveAt(currentIndex);
});

// naviguer dans le carousel avec les fleches du clavier + entree
document.addEventListener("keydown", function (e) {
	if (e.key == "ArrowLeft")
		Carousel.setActiveAt(currentIndex - 1);
	else if (e.key == "ArrowRight")
		Carousel.setActiveAt(currentIndex + 1);
	else if (e.key == "Enter")
		Carousel.showDetails($("card" + currentIndex));
});

// re-centrer la position des items dans le carousel on page resize
window.onresize = function () {
	let target = $("#card" + currentIndex);
	main.scrollLeft = target.offsetLeft - (document.body.offsetWidth / 2) + (target.offsetWidth / 2)
}

// ajouter les evenements aux items
for (let i = 0; i < items.length; i++)
	items[i].onclick = (e) => Carousel.showDetails(e.currentTarget as HTMLElement);

// rendre actif le dernier jeu selectionne en memoire
// sinon, rendre actif le jeu du milieu
window.addEventListener("load", function () {
	let lastIndex = Number(window.localStorage.getItem("iLastSelectedGame"));

	if (!isNaN(lastIndex))
		Carousel.setActiveAt(lastIndex);
	else
		Carousel.setActiveAt(Math.round(($(".carousel").children.length - 1) / 2));

	main.style.opacity = "1";
});