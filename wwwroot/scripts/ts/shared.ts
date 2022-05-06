"use strict";

const DEBUG = true;

const MAIN  = $("main");
const AD    = $("#collabPopup");
const ADIMG = $("#collabPopup .geraltImg") as HTMLImageElement;

let canClosePopup: boolean = true; // si la popup peut fermer

enum Transition {
	ScaleDown,
	ScaleUp
}

// random debug util
function getFunInfoStr(target: Element) {
	return `${target.nodeName.toLowerCase()}${target.id != "" ? "#" + target.id : ""}${target.classList.length > 0 ? "." + target.classList.toString() : ""}`;
}

// JQuery-like selection utilities
function $(sel: string, parent?: Element): HTMLElement {
	if (!DEBUG)
		return (parent ? parent : document).querySelector(sel)!;

	let elem = (parent ? parent : document).querySelector(sel);

	if (elem === null) {
		if (parent)
			console.warn(`[WARN] No element matched selector "${sel}" inside "${getFunInfoStr(parent)}".`);
		else
			console.warn(`[WARN] No element matched selector "${sel}" in the document.`);

		return document.createElement("invalid");
	}

	return elem as HTMLElement;
}
function $$(sel: string, parent?: Element): NodeListOf<HTMLElement> {
	if (!DEBUG)
		(parent ? parent : document).querySelectorAll(sel);

	let elems : NodeListOf<HTMLElement> = (parent ? parent : document).querySelectorAll(sel);
	if (elems.length === 0)
		if (parent)
			console.warn(`[WARN] No collection matched selector "${sel}" in "${getFunInfoStr(parent)}".`);
		else
			console.warn(`[WARN] No collection matched selector "${sel}" in document.`);

	return elems;
}
function $a(attr: string, parent: HTMLElement): string {
	if (!DEBUG)
		return parent.getAttribute(attr)!;

	let val = parent.getAttribute(attr);

	if (val === null || val == undefined) {
		console.warn(`[WARN] No attribute "${attr}" was found in "${getFunInfoStr(parent)}".`);
		return "invalid";
	}

	return val;
}

// register les liens de la page (permet une transition)
function registerLinks() : void {
	let buttons = $$("button.link");

	for (let i = 0; i < buttons.length; i++) {
		buttons[i].addEventListener("click", (e) => {
			let target = e.currentTarget as HTMLElement;
			redirect($a("data-href", target));
		});
	}
}


function redirect(href: string) {
	MAIN.style.opacity = "0";

	setTimeout(() => {
		window.location.href = href;
	}, 2000);
}

// montrer la popup
function showPopup() {
	AD.style.right = "0";

	setTimeout(function () {
		if (canClosePopup)
		AD.style.right = "-430px";
	}, 15000);
}

// changer l'image du Witcher on hover
AD.onmouseover  = function () { ADIMG.src = "/images/shared/ui/popup/geralt2.png"; }
AD.onmouseleave = function () { ADIMG.src = "/images/shared/ui/popup/geralt.png"; }

// rediriger vers le site de Zachary sur click de la popup
$(".collabBtn", AD).onclick = function () {
	let foreground = $("#foreground_witcherCollab");
	foreground.style.display = "block";

	setTimeout(function () {
		foreground.style.opacity = "1";
		foreground.style.filter = "blur(100px) brightness(100)";
	}, 1000);
}

// "fermer" la popup lorsque le bouton X est clique :3
$(".closeBtn").onclick = function () {
	const PROGRESS_TEXTS: string[] = [
		"Checking for null pointers...",
		"Loading closure data...",
		"Ensuring academical integrity...",
		"Analysing TS sourcemap concordance...",
		"Checking on Ciri...",
		"Loading Havok Behavior(TM)...",
		"Generating Kevin Leduc...",
		"Garbage collecting JS code...",
		"Validating microtransactions...",
		"Polishing Gerald's silver sword...",
		"Fetching available essays...",
		"Handling exceptions too hard...",
		"Handshaking IIS Express Server...",
		"Contacting Microsoft support...",
		"Checking Visual Studio profiles...",
		"Establishing connection over SSMS...",
		"No shutdowning Cisco connection...",
		"Switching interface to VLAN1...",
		"Answering Zachary's questions...",
		"Collecting legal telemetry data...",
		"Auto-generating XML profiles...",
		"Asking Lydia to carry burdens...",
		"Brushing Ciri's hair...",
		"Sending Geralt to beat up furries...",
		"Generating initial SSL definitions...",
		"Purchasing something from Billy Mays...",
		"Checking your HDD for suspicious data...",
		"Downloading optional furry theme...",
		"Have you heard of the high elves?",
		"Parsing YAML markup data...",
		"Initiating nomming of a burger...",
		"Sending Ciri in mission with Ezio...",
		"Calling tech support...",
		"Searching for Dog Meat...",
		"Mapping Altair's furrydom...",
		"Contacting Abstergo's tech support...",
		"Exposing Theo's pyramid scheme...",
		"Analysing average Pokemon repartition...",
		"Testing for misclored fur strands...",
		"Finding the escaped fox (he's sneaky!)...",
		"Sharpening the Edge of Glory...",
		"Praising our Master, Todd Howard...",
		"May the Father of Understanding Guide Us.",
		"Replacing spaces in Python file...",
		"Studying for maths exam (I should be)...",
		"Stealing the Boots of Springheel Jack...",
		"Fetching 16 times the details...",
		"Sketching anthropomorphic foxes...",
		"Consolidating the Linux foundations...",
		"6-spaces tabs are superior cmm."
	];

	const MAX_ITERATIONS = DEBUG ? 0 : 5; // how many dummy tasks to run
	let nbIterations: number = 0;
	let progress = $("progress", AD) as HTMLProgressElement;
	let progressText = $(".progressText", AD);
	let progressTextIndex: number = 0;
	let throbber = $(".throbber", AD);
	let progressAmount: number = 1;
	let decreaseMode: boolean = false;
	let decreaseModeIterations: number = -1;
	let decreaseModeCurrentIterations = 0;

	let progressHeader = $("#collabPopup .progressHeader");
	progressHeader.innerText = "Processing request...";
	let nbEllipsis = 3;

	// show obnoxious loading overlay
	$(".overlay", AD).style.opacity = "1";

	// prevent auto-closing of popup
	canClosePopup = false;

	// typewriter ellipsis
	function ellipsis() {
		if (nbEllipsis === 3) {
			progressHeader.innerText = progressHeader.innerText.slice(0, -3);
			nbEllipsis = 0;
		} else {
			progressHeader.innerText += ".";
			nbEllipsis++;
		}
	}

	// set progress bar text
	function setProgressTextIndex() {
		progressText.style.opacity = "0";

		setTimeout(() => {
			progressTextIndex = Math.round(Math.random() * (PROGRESS_TEXTS.length - 1));
			progressText.style.opacity = "1";
		}, 300);
	}

	// randomly increase progress on dummy progress bar
	function setProgress() {
		// increase or decrease progress
		if (decreaseMode) {
			progress.value -= progressAmount;
			decreaseModeCurrentIterations++;
		} else {
			progress.value += progressAmount;
		}

		// change current dummy task when progress bar full
		if (progress.value >= 100) {
			setProgressTextIndex();
			progress.value = 0;
			nbIterations++;
		}

		if (progress.value === 0 && decreaseMode) {
			decreaseMode = false;
			decreaseModeCurrentIterations = 0;
			progress.style.animation = "none";
			throbber.style.animation = "ad-throbber 1s ease-in-out infinite";
		}

		// set progress text
		progressText.innerText = PROGRESS_TEXTS[progressTextIndex] + " " + Math.round(progress.value) + "%";

		// randomly choose a new progress amount
		if (Math.random() < 0.1)
			progressAmount = Math.random() * Math.random();

		// sometimes we decide to reverse progress... :3
		if (Math.random() < 0.005) {
			if (!decreaseMode) {
				decreaseMode = true;
				decreaseModeIterations = Math.random() * 200;
				progress.style.animation = "ad-progress-rainbow 1s linear infinite";
				throbber.style.animation = "ad-throbber-reverse 1s ease-in-out infinite";
			}
		}

		// disable decrease mode when iterations count reached
		if (decreaseModeCurrentIterations >= decreaseModeIterations) {
			decreaseMode = false;
			decreaseModeCurrentIterations = 0;
			progress.style.animation = "none";
			throbber.style.animation = "ad-throbber 1s ease-in-out infinite";
		}

		// stop progress and redirect to Zachary's website after enough iterations
		if (nbIterations === MAX_ITERATIONS) {
			nbIterations = 0;
			   
			progressHeader.innerText = "Closing popup...";
			progressText.innerText = "Request approved. Have a nice day!";

			setTimeout(() => {
				$(".overlay", AD).style.opacity = "0";
				AD.style.right = "-450px";
			}, 1000);

			return;
		}

		setTimeout(setProgress, 10);
	}

	setProgressTextIndex();
	setProgress();

	setInterval(() => {
		ellipsis();
	}, 500);
}

MAIN.style.opacity = "0";
window.addEventListener("load", function () {
	MAIN.style.opacity = "1";

	registerLinks();
});