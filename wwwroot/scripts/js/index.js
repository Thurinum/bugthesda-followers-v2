"use strict";
let firstTime = true;
let isCarouselEnabled = true;
let currentIndex = -1;
let currentTimeout;
let precedenceTimeout;
let items = $$(".carousel > div");
const Carousel = {
    setActiveAt: (index) => {
        clearTimeout(precedenceTimeout);
        if (index == currentIndex || !isCarouselEnabled)
            return;
        let container = $(".carousel");
        let target = container.children[index];
        if (!target)
            return;
        let currentItem = container.querySelector(".active");
        if (currentItem) {
            let shown = $$(".showOnActive", currentItem);
            for (let i = 0; i < shown.length; i++) {
                shown[i].style.opacity = "0";
                shown[i].style.transform = "scale(0.9)";
            }
            let leftSibling = currentItem.previousElementSibling;
            let rightSibling = currentItem.nextElementSibling;
            if (leftSibling)
                leftSibling.className = "hidden";
            if (rightSibling)
                rightSibling.className = "hidden";
            currentItem.className = "hidden";
        }
        let leftSibling = target.previousElementSibling;
        let rightSibling = target.nextElementSibling;
        if (leftSibling)
            leftSibling.className = "inactive";
        if (rightSibling)
            rightSibling.className = "inactive";
        target.className = "active";
        let hidden = $$(".showOnActive", target);
        for (let i = 0; i < hidden.length; i++) {
            hidden[i].style.opacity = "1";
            hidden[i].style.transform = "scale(1)";
        }
        MAIN.scrollLeft = target.offsetLeft - (document.body.offsetWidth / 2) + (target.offsetWidth / 2);
        let shortname = $("input.gameName", target).value;
        let background = $("#background");
        let overlay = $("#foreground_mist");
        let dir = index < currentIndex ? -1 : 1;
        background.style.left = `${-dir * 100}vw`;
        overlay.style.transform = `scaleX(${dir})`;
        overlay.playbackRate = 2;
        overlay.style.opacity = "0.5";
        setTimeout(() => {
            background.style.transition = "";
            background.style.left = `${dir * 100}vw`;
            setTimeout(() => {
                background.src = `/videos/fallback/${shortname}.webp`;
                background.style.transition = "left 0.25s ease-out, transform 0.25s ease-out";
                background.style.left = "0";
                overlay.playbackRate = 0.9;
                overlay.style.opacity = "0.3";
            }, 100);
        }, 100);
        if (Math.random() < 0.35)
            showPopup();
        currentIndex = index;
        window.localStorage.setItem("iLastSelectedGame", `${currentIndex}`);
        precedenceTimeout = setTimeout(() => MAIN.style.pointerEvents = "auto", 500);
    },
    showDetails(target) {
        isCarouselEnabled = false;
        for (let i = 0; i < items.length; i++) {
            let item = items[i];
            if (item != target)
                item.style.transform = "scale(0)";
        }
        let headers = $$(".showOnActive", target);
        for (let i = 0; i < headers.length; i++) {
            let header = headers[i];
            if (header.nodeName != "H1") {
                header.style.opacity = "0";
                header.style.height = "0";
            }
        }
        let input = $("input.gameName", target);
        let details = $("#details");
        fetch(`/game/${input.value}`)
            .then(data => data.text())
            .then(html => {
            details.innerHTML = html;
            details.style.transform = "translateY(-50%) scale(1)";
        });
        $("#background").style.filter = "blur(20px)";
        let img = $("img", target);
        img.style.animationPlayState = "running";
        img.style.transform = "scale(1.1)";
        let button = $(".searchBtn", target);
        button.style.height = "3ex";
        button.style.transform = "scale(1)";
    },
    hideDetails() {
        isCarouselEnabled = true;
        for (let i = 0; i < items.length; i++) {
            items[i].style.transform = "";
        }
        let target = items[currentIndex];
        let headers = $$(".showOnActive", target);
        for (let i = 0; i < headers.length; i++) {
            let header = headers[i];
            if (header.nodeName != "H1") {
                header.style.opacity = "1";
                header.style.height = "3ex";
            }
        }
        $("#background").style.filter = "blur(5px)";
        let img = $("img", target);
        img.style.animationPlayState = "paused";
        img.style.transform = "";
        $("#details").style.transform = "translateY(-50%) scale(0)";
        $(".searchBtn", target).style.height = "0";
        $(".searchBtn", target).style.transform = "scale(0)";
    }
};
document.addEventListener("click", (e) => {
    if (!e.target.classList.contains("logo"))
        Carousel.hideDetails();
});
document.addEventListener("keydown", function (e) {
    if (e.key == "ArrowLeft") {
        MAIN.style.pointerEvents = "none";
        Carousel.setActiveAt(currentIndex - 1);
    }
    else if (e.key == "ArrowRight") {
        MAIN.style.pointerEvents = "none";
        Carousel.setActiveAt(currentIndex + 1);
    }
    else if (e.key == "Enter") {
        let card = $("#card" + currentIndex);
        if ($("#details").style.transform === "translateY(-50%) scale(1)") {
            Carousel.hideDetails();
            isCarouselEnabled = false;
            $(".searchBtn", card).click();
        }
        else {
            Carousel.showDetails(card);
        }
    }
});
window.onresize = function () {
    let target = $("#card" + currentIndex);
    MAIN.scrollLeft = target.offsetLeft - (document.body.offsetWidth / 2) + (target.offsetWidth / 2);
};
window.addEventListener("load", function () {
    let lastIndex = Number(window.localStorage.getItem("iLastSelectedGame"));
    if (!isNaN(lastIndex))
        Carousel.setActiveAt(lastIndex);
    else
        Carousel.setActiveAt(Math.round(($(".carousel").children.length - 1) / 2));
    for (let i = 0; i < items.length; i++) {
        items[i].onclick = (e) => {
            if (isCarouselEnabled)
                Carousel.showDetails(e.currentTarget);
        };
    }
    registerLinks();
    MAIN.style.opacity = "1";
});
//# sourceMappingURL=index.js.map