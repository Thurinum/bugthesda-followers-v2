"use strict";
const DEBUG = true;
const main = $("main");
const ad = $("#ad");
const adimg = $("#ad .geraltImg");
function $(sel, target) {
    let parent = target ? target : document;
    if (!DEBUG)
        return parent.querySelector(sel);
    let elem = parent.querySelector(sel);
    if (elem === null) {
        console.warn(`Attempt to access invalid element "${sel}" in "${target}".`);
        return document.createElement("invalid");
    }
    return elem;
}
function $$(sel, target) {
    return (target ? target : document).querySelectorAll(sel);
}
function $a(attr, target) {
    if (!DEBUG)
        return target.getAttribute(attr);
    let val = target.getAttribute(attr);
    if (val === null || val == undefined) {
        console.warn(`Attempt to access invalid attribute "${attr}" in "${target}".`);
        return "invalid";
    }
    return val;
}
function registerLinks() {
    let buttons = $$("button.link");
    for (let i = 0; i < buttons.length; i++) {
        buttons[i].addEventListener("click", (e) => {
            let target = e.currentTarget;
            main.style.top = "100vw";
            main.style.opacity = "0";
            setTimeout(window.location.href = $a("data-href", target), 1500);
        });
    }
}
function showPopup() {
    ad.style.right = "0";
    setTimeout(function () {
        ad.style.right = "-430px";
    }, 15000);
}
ad.onmouseover = function () { adimg.src = "/images/shared/ui/popup/geralt2.png"; };
ad.onmouseleave = function () { adimg.src = "/images/shared/ui/popup/geralt.png"; };
