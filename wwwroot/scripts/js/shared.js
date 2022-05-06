"use strict";
const DEBUG = true;
const MAIN = $("main");
const AD = $("#collabPopup");
const ADIMG = $("#collabPopup .geraltImg");
let canClosePopup = true;
var Transition;
(function (Transition) {
    Transition[Transition["ScaleDown"] = 0] = "ScaleDown";
    Transition[Transition["ScaleUp"] = 1] = "ScaleUp";
})(Transition || (Transition = {}));
function getFunInfoStr(target) {
    return `${target.nodeName.toLowerCase()}${target.id != "" ? "#" + target.id : ""}${target.classList.length > 0 ? "." + target.classList.toString() : ""}`;
}
function $(sel, parent) {
    if (!DEBUG)
        return (parent ? parent : document).querySelector(sel);
    let elem = (parent ? parent : document).querySelector(sel);
    if (elem === null) {
        if (parent)
            console.warn(`[WARN] No element matched selector "${sel}" inside "${getFunInfoStr(parent)}".`);
        else
            console.warn(`[WARN] No element matched selector "${sel}" in the document.`);
        return document.createElement("invalid");
    }
    return elem;
}
function $$(sel, parent) {
    if (!DEBUG)
        (parent ? parent : document).querySelectorAll(sel);
    let elems = (parent ? parent : document).querySelectorAll(sel);
    if (elems.length === 0)
        if (parent)
            console.warn(`[WARN] No collection matched selector "${sel}" in "${getFunInfoStr(parent)}".`);
        else
            console.warn(`[WARN] No collection matched selector "${sel}" in document.`);
    return elems;
}
function $a(attr, parent) {
    if (!DEBUG)
        return parent.getAttribute(attr);
    let val = parent.getAttribute(attr);
    if (val === null || val == undefined) {
        console.warn(`[WARN] No attribute "${attr}" was found in "${getFunInfoStr(parent)}".`);
        return "invalid";
    }
    return val;
}
function registerLinks() {
    let buttons = $$("button.link");
    for (let i = 0; i < buttons.length; i++) {
        buttons[i].addEventListener("click", (e) => {
            let target = e.currentTarget;
            redirect($a("data-href", target));
        });
    }
}
function redirect(href) {
    MAIN.style.opacity = "0";
    setTimeout(() => {
        window.location.href = href;
    }, 2000);
}
function showPopup() {
    AD.style.right = "0";
    setTimeout(function () {
        if (canClosePopup)
            AD.style.right = "-430px";
    }, 15000);
}
AD.onmouseover = function () { ADIMG.src = "/images/shared/ui/popup/geralt2.png"; };
AD.onmouseleave = function () { ADIMG.src = "/images/shared/ui/popup/geralt.png"; };
$(".collabBtn", AD).onclick = function () {
    let foreground = $("#foreground_witcherCollab");
    foreground.style.display = "block";
    setTimeout(function () {
        foreground.style.opacity = "1";
        foreground.style.filter = "blur(100px) brightness(100)";
    }, 1000);
};
$(".closeBtn").onclick = function () {
    const PROGRESS_TEXTS = [
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
    const MAX_ITERATIONS = DEBUG ? 0 : 5;
    let nbIterations = 0;
    let progress = $("progress", AD);
    let progressText = $(".progressText", AD);
    let progressTextIndex = 0;
    let throbber = $(".throbber", AD);
    let progressAmount = 1;
    let decreaseMode = false;
    let decreaseModeIterations = -1;
    let decreaseModeCurrentIterations = 0;
    let progressHeader = $("#collabPopup .progressHeader");
    progressHeader.innerText = "Processing request...";
    let nbEllipsis = 3;
    $(".overlay", AD).style.opacity = "1";
    canClosePopup = false;
    function ellipsis() {
        if (nbEllipsis === 3) {
            progressHeader.innerText = progressHeader.innerText.slice(0, -3);
            nbEllipsis = 0;
        }
        else {
            progressHeader.innerText += ".";
            nbEllipsis++;
        }
    }
    function setProgressTextIndex() {
        progressText.style.opacity = "0";
        setTimeout(() => {
            progressTextIndex = Math.round(Math.random() * (PROGRESS_TEXTS.length - 1));
            progressText.style.opacity = "1";
        }, 300);
    }
    function setProgress() {
        if (decreaseMode) {
            progress.value -= progressAmount;
            decreaseModeCurrentIterations++;
        }
        else {
            progress.value += progressAmount;
        }
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
        progressText.innerText = PROGRESS_TEXTS[progressTextIndex] + " " + Math.round(progress.value) + "%";
        if (Math.random() < 0.1)
            progressAmount = Math.random() * Math.random();
        if (Math.random() < 0.005) {
            if (!decreaseMode) {
                decreaseMode = true;
                decreaseModeIterations = Math.random() * 200;
                progress.style.animation = "ad-progress-rainbow 1s linear infinite";
                throbber.style.animation = "ad-throbber-reverse 1s ease-in-out infinite";
            }
        }
        if (decreaseModeCurrentIterations >= decreaseModeIterations) {
            decreaseMode = false;
            decreaseModeCurrentIterations = 0;
            progress.style.animation = "none";
            throbber.style.animation = "ad-throbber 1s ease-in-out infinite";
        }
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
};
//# sourceMappingURL=shared.js.map