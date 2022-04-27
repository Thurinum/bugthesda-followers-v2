"use strict";

const DEBUG = true;

const main = $("main");
const ad = $("#ad");
const adimg = $("#ad .geraltImg") as HTMLImageElement;

// JQuery-like selection utilities
function $(sel: string, target?: Element): HTMLElement {
	let parent = target ? target : document;

	if (!DEBUG)
		return parent.querySelector(sel)!;

	let elem = parent.querySelector(sel);

	if (elem === null) {
		console.warn(`Attempt to access invalid element "${sel}" in "${target}".`);
		return document.createElement("invalid");
    }

	return elem as HTMLElement;
}

function $$(sel: string, target?: Element): NodeListOf<HTMLElement> {
	return (target ? target : document).querySelectorAll(sel);
}

function $a(attr: string, target: HTMLElement): string {
	if (!DEBUG)
		return target.getAttribute(attr)!;

	let val = target.getAttribute(attr);

	if (val === null || val == undefined) {
		console.warn(`Attempt to access invalid attribute "${attr}" in "${target}".`);
		return "invalid";
    }

	return val;
}

// register les liens de la page (permet une transition)
function registerLinks() {
	let buttons = $$("button.link");

	for (let i = 0; i < buttons.length; i++) {
		buttons[i].addEventListener("click", (e) => {
			let target = e.currentTarget as HTMLElement;

			main.style.top = "100vw";
			main.style.opacity = "0";

			setTimeout(window.location.href = $a("data-href", target), 1500);
		});
    }
}

// annonce pas rapport
function showPopup() {
	ad.style.right = "0";

	setTimeout(function () {
		ad.style.right = "-430px";
	}, 15000);
}

ad.onmouseover  = function () { adimg.src = "/images/shared/ui/popup/geralt2.png"; }
ad.onmouseleave = function () { adimg.src = "/images/shared/ui/popup/geralt.png"; }