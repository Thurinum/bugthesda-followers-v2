"use strict";
const SEARCH_INPUT = $("#searchInput");
const FORM = $("#searchForm");
$("#cardsWrapper").onwheel = function (e) {
    $("#cardsWrapper").scrollLeft += e.deltaY;
};
SEARCH_INPUT.onclick = () => {
    FORM.style.maxHeight = "100vh";
    isRedirectEnabled = false;
};
SEARCH_INPUT.onkeydown = (e) => {
    if (e.key === "Enter")
        FORM.submit();
};
window.onclick = (e) => {
    if (e.target != SEARCH_INPUT && !FORM.contains(e.target)) {
        FORM.style.maxHeight = "0";
        isRedirectEnabled = true;
    }
};
window.addEventListener("load", setTrivia);
function setTrivia() {
    fetch("/game/getrandomtrivia")
        .then(data => data.text())
        .then(trivia => {
        $("#trivia").innerText = trivia;
        setTimeout(setTrivia, 5000);
    });
}
//# sourceMappingURL=search.js.map