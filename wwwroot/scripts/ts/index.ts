"use strict";

let firstTime = true;
let isCarouselEnabled = true;
let currentIndex = -1;
let currentTimeout;
let precedenceTimeout : any;
let items = $$(".carousel > div");

// carousel style coverflow pour les jeux (parents)
const Carousel = {
	// set card at index as active
	setActiveAt: (index: number) => {
		clearTimeout(precedenceTimeout);

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
		MAIN.scrollLeft = target.offsetLeft - (document.body.offsetWidth / 2) + (target.offsetWidth / 2)

		let shortname = ($("input.gameName", target) as HTMLInputElement).value;
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

		precedenceTimeout = setTimeout(() => MAIN.style.pointerEvents = "auto", 500);
	},

	// afficher plus de details sur un jeu specifique
	showDetails(target: HTMLElement) {
		isCarouselEnabled = false;

		// hide all carousel items except current
		for (let i = 0; i < items.length; i++) {
			let item: HTMLElement = items[i];
			if (item != target)
				item.style.transform = "scale(0)";
		}

		// hide all headers of current item except title
		let headers = $$(".showOnActive", target);
		for (let i = 0; i < headers.length; i++) {
			let header = headers[i];

			if (header.nodeName != "H1") {
				header.style.opacity = "0";
				header.style.height = "0";
			}
		}

		// fetch game name from hidden input and load partial view
		let input = $("input.gameName", target) as HTMLInputElement;
		let details = $("#details");

		fetch(`/game/${input.value}`)
			.then(data => data.text())
			.then(html => {
				details.innerHTML = html;
				details.style.transform = "translateY(-50%) scale(1)";
			});		

		// blur further background
		$("#background").style.filter = "blur(20px)";
		let img = $("img", target);
		img.style.animationPlayState = "running";
		img.style.transform = "scale(1.1)";

		// show button
		let button = $(".searchBtn", target);
		button.style.height = "3ex";
		button.style.transform = "scale(1)";
	},

	hideDetails() {
		isCarouselEnabled = true;

		// reset items display
		for (let i = 0; i < items.length; i++) {
			items[i].style.transform = "";
		}

		// reset headers
		let target = items[currentIndex];
		let headers = $$(".showOnActive", target);
		for (let i = 0; i < headers.length; i++) {
			let header = headers[i];

			if (header.nodeName != "H1") {
				header.style.opacity = "1";
				header.style.height = "3ex";
			}
		}

		// unblur bg
		$("#background").style.filter = "blur(5px)";
		let img = $("img", target);
		img.style.animationPlayState = "paused";
		img.style.transform = "";

		$("#details").style.transform = "translateY(-50%) scale(0)";
		$(".searchBtn", target).style.height = "0";
		$(".searchBtn", target).style.transform = "scale(0)";
	}
}

document.addEventListener("click", (e) => {
	if (!(e.target as HTMLElement).classList.contains("logo"))
		Carousel.hideDetails();
});

// naviguer dans le carousel avec les fleches du clavier + entree
document.addEventListener("keydown", function (e) {
	if (e.key == "ArrowLeft") {
		MAIN.style.pointerEvents = "none";
		Carousel.setActiveAt(currentIndex - 1);
	} else if (e.key == "ArrowRight") {
		MAIN.style.pointerEvents = "none";
		Carousel.setActiveAt(currentIndex + 1);
	} else if (e.key == "Enter") {
		let card = $("#card" + currentIndex);

		// si details montre, rediriger sinon montrer details
		if ($("#details").style.transform === "translateY(-50%) scale(1)") {
			Carousel.hideDetails();
			isCarouselEnabled = false;
			$(".searchBtn", card).click();
		} else {
			Carousel.showDetails(card);
		}
	}
});

// re-centrer la position des items dans le carousel on page resize
window.onresize = function () {
	let target = $("#card" + currentIndex);
	MAIN.scrollLeft = target.offsetLeft - (document.body.offsetWidth / 2) + (target.offsetWidth / 2)
}

// rendre actif le dernier jeu selectionne en memoire
// sinon, rendre actif le jeu du milieu
window.addEventListener("load", function () {
	let lastIndex = Number(window.localStorage.getItem("iLastSelectedGame"));

	if (!isNaN(lastIndex))
		Carousel.setActiveAt(lastIndex);
	else
		Carousel.setActiveAt(Math.round(($(".carousel").children.length - 1) / 2));

	// ajouter les evenements aux items
	for (let i = 0; i < items.length; i++) {
		items[i].onclick = (e) => {
			if (isCarouselEnabled) Carousel.showDetails(e.currentTarget as HTMLElement);
        }
	}

	registerLinks();

	MAIN.style.opacity = "1";
});
