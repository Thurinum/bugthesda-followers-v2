"use strict";

// JQuery-like selection utilities
function $(sel)  { return document.querySelector(sel); }
function $$(sel) { return document.querySelectorAll(sel); }

let main = $("main");

function registerLinks() {
	let buttons = $$("button.link");

	for (let i = 0; i < buttons.length; i++)
		buttons[i].addEventListener("click", (e) => {
			let target = e.currentTarget;
			let main = $("main");
			let dir = target.getAttribute("data-dir");

			if (!"top bottom left right".includes(dir))
				dir = "top"; // default

			main.style[dir] = "100vw";
			main.style.opacity = 0;

			setTimeout(window.location.href = target.getAttribute("data-href"), 1500);
		});
}
$("#ad").onmouseover = function (e) {
	$("#ad .geraltImg").src = "/images/shared/ui/popup/geralt2.png";
}

$("#ad").onmouseleave = function (e) {
	$("#ad .geraltImg").src = "/images/shared/ui/popup/geralt.png";
}