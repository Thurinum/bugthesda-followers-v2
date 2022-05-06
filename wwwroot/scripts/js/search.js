"use strict";
const SEARCH_BTN = $("#search");
const FORM = $("#searchForm");
$("#cardsWrapper").onwheel = function (e) {
    $("#cardsWrapper").scrollLeft += e.deltaY;
};
SEARCH_BTN.onclick = () => {
    FORM.style.height = "30vh";
};
window.onclick = (e) => {
    if (e.target === $("#searchBar") || e.target === $("#cardsWrapper"))
        FORM.style.height = "0";
};
//# sourceMappingURL=search.js.map