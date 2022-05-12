"use strict";

const SEARCH_INPUT = $("#searchInput");
const FORM = $("#searchForm") as HTMLFormElement;

$("#cardsWrapper").onwheel = function (e) {
	$("#cardsWrapper").scrollLeft += e.deltaY;
}

SEARCH_INPUT.onclick = () => {
	FORM.style.maxHeight = "100vh";
	FORM.style.padding = "40px";
	FORM.style.borderWidth = "1px";
	isRedirectEnabled = false;
}

SEARCH_INPUT.onkeydown = (e: any) => {
	if (e.key === "Enter")
		FORM.submit();
}

window.onclick = (e) => {
	if (e.target != SEARCH_INPUT && !FORM.contains(e.target as HTMLElement)) {
		FORM.style.maxHeight = "0";
		FORM.style.padding = "0";
		FORM.style.borderWidth = "0";
		isRedirectEnabled = true;
	}
}

window.addEventListener("load", setTrivia);

function setTrivia() {
	fetch("/game/getrandomtrivia")
		.then(data => data.text())
		.then(trivia => {
			$("#trivia").innerText = trivia;
			setTimeout(setTrivia, 5000);
		});
}

if (!navigator.userAgent.match(/chrome|chromium|crios/i)) {
	FORM.style.backgroundColor = "rgba(20, 20, 20, 0.93)";
}