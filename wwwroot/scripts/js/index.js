"use strict";
let currentIndex = -1;
let isCarouselEnabled = true;
let firstTime = true;
let currentTimeout;
let items = $$(".carousel > div");
const Carousel = {
    setActiveAt: (index) => {
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
        main.scrollLeft = target.offsetLeft - (document.body.offsetWidth / 2) + (target.offsetWidth / 2);
        let shortname = $("input", target).value;
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
                background.src = `/videos/games/${shortname}/background.mp4#t=15`;
                background.load();
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
    },
    showDetails(target) {
        function setThumbnails() {
            let previews = $$(".followerPreview");
            let id = target.id.slice(5);
            for (let i = 0; i < previews.length; i++) {
                let preview = previews[i];
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
            let item = items[i];
            if (item != target)
                item.style.transform = "scale(0)";
        }
        let headers = $$(".showOnActive", target);
        for (let i = 0; i < headers.length; i++) {
            headers[i].style.opacity = "0";
        }
        let input = $("input", target);
        console.log(input.value);
        fetch(`/${input.value}`)
            .then(data => data.text())
            .then(html => {
            $("#details").innerHTML = html;
            $("#details").style.transform = "translateY(-50%) scale(1)";
        });
    }
};
document.addEventListener("click", function (e) {
    if (e.target.classList.contains("logo"))
        return;
    isCarouselEnabled = true;
    for (let i = 0; i < items.length; i++) {
        let item = items[i];
        item.style.transform = "";
    }
    $("#details").style.transform = "translateY(-50%) scale(0)";
    Carousel.setActiveAt(currentIndex);
});
document.addEventListener("keydown", function (e) {
    if (e.key == "ArrowLeft")
        Carousel.setActiveAt(currentIndex - 1);
    else if (e.key == "ArrowRight")
        Carousel.setActiveAt(currentIndex + 1);
    else if (e.key == "Enter")
        Carousel.showDetails($("card" + currentIndex));
});
window.onresize = function () {
    let target = $("#card" + currentIndex);
    main.scrollLeft = target.offsetLeft - (document.body.offsetWidth / 2) + (target.offsetWidth / 2);
};
for (let i = 0; i < items.length; i++)
    items[i].onclick = (e) => Carousel.showDetails(e.currentTarget);
window.addEventListener("load", function () {
    let lastIndex = Number(window.localStorage.getItem("iLastSelectedGame"));
    if (!isNaN(lastIndex))
        Carousel.setActiveAt(lastIndex);
    else
        Carousel.setActiveAt(Math.round(($(".carousel").children.length - 1) / 2));
    main.style.opacity = "1";
});
//# sourceMappingURL=index.js.map