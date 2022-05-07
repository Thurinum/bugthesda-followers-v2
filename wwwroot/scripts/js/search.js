"use strict";
const SEARCH_INPUT = $("#search");
const FORM = $("#searchForm");
$("#cardsWrapper").onwheel = function (e) {
    $("#cardsWrapper").scrollLeft += e.deltaY;
};
SEARCH_INPUT.onclick = () => {
    FORM.style.height = "30vh";
};
SEARCH_INPUT.onkeydown = (e) => {
    if (e.key === "Enter")
        FORM.submit();
};
window.onclick = (e) => {
    if (e.target === $("#searchBar") || e.target === $("#cardsWrapper"))
        FORM.style.height = "0";
};
//# sourceMappingURL=search.js.map