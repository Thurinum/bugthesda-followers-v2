"use strict";

let currentindex = 0;
let carouselenabled = true;

let currenttimeout;

// "CoverFlow"-like carousel system pour les parents
const Carousel = {
	// set card at index as active
	setActiveAt: (index) => {
		if (index == currentindex)
			return;

		let container = $(".carousel");

		if (!carouselenabled)
			return;

		let target = container.children[index];

		if (!target)
			return;

		// if an item is already active, make it inactive
		// also disables its inactive siblings (fade effect)
		let currentItem = container.querySelector(".active");
		if (currentItem) {
			let shown = currentItem.querySelectorAll(".showOnActive");
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

		let hidden = target.querySelectorAll(".showOnActive");
		for (let i = 0; i < hidden.length; i++) {
			hidden[i].style.opacity = "1";
			hidden[i].style.transform = "scale(1)";
		}
	   
		// center the new active card in container
		$("main").scrollLeft = target.offsetLeft - (document.body.offsetWidth / 2) + (target.offsetWidth / 2)

		let shortname = target.querySelector("input").value;
		$("#background").firstElementChild.src = `/videos/games/${shortname}/background.mp4`;
		$("#background").load();
		$("#background").play();

		currentindex = index;
	}
}

function showDetails(target) {
	function setThumbnails() {
		let previews = $$(".followerPreview");
		let id = target.id.slice(5);

		for (let i = 0; i < previews.length; i++) {
			previews[i].style.opacity = 0;
			fetch(`/${id}/randomthumbnail`)
				.then(data => data.text())
				.then(src => {
					previews[i].src = "";
					previews[i].src = src;
					previews[i].onload = () => { previews[i].style.opacity = 1; };					
				});
		}
	}

	// cacher les infos pour ne montrer que le logo en background
	target.querySelector("h1").style.fontSize = "100px";
	target.querySelector("h2").style.transform = "scaleY(0)";
	target.querySelector("h2").style.opacity = "0";
	target.querySelector("h2").style.height = "0";
	target.querySelector(".statsWrapper").style.flexDirection = "row";

	let hiddens = target.querySelectorAll(".statsWrapper .hiddenStat");
	
	for (let i = 0; i < hiddens.length; i++)
		hiddens[i].style.display = "block";

	setThumbnails()

	// empecher l'activation des items
	carouselenabled = false;

	setInterval(setThumbnails, 10000);

	// reactiver les items au mouseleave
	/*document.onclick = function (e) {
		if (firstClick) {
			firstClick = false;
			return;
		}

		carouselenabled = true;
		close();
	};*/

	function close() {
		details.style.opacity = 0;
		details.style.transform = "translate(-50%, -50%) scale(0.9)";
		setTimeout(function () {
			details.style.display = "none";
		}, 500);
	}
}

document.addEventListener("keydown", function (e) {
	if (e.key == "ArrowLeft")
		Carousel.setActiveAt(currentindex - 1);
	else if (e.key == "ArrowRight")
		Carousel.setActiveAt(currentindex + 1);
	else if (e.key == "Enter")
		showDetails($("card" + currentindex));
});

window.onload = function () {
	// rendre actif le dernier jeu selectionne en memoire
	// sinon, rendre actif le jeu du milieu
	let lastIndex = parseInt(window.localStorage.getItem("iLastSelectedGame"));
	if (!isNaN(lastIndex))
		Carousel.setActiveAt(lastIndex);
	else
		Carousel.setActiveAt(Math.round(($(".carousel").children.length - 1) / 2));
}