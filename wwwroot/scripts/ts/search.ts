"use strict";

const SEARCH_INPUT = $("#search");
const FORM = $("#searchForm") as HTMLFormElement;

$("#cardsWrapper").onwheel = function (e) {
    $("#cardsWrapper").scrollLeft += e.deltaY;
}

SEARCH_INPUT.onclick = () => {
    FORM.style.height = "30vh";
}

SEARCH_INPUT.onkeydown = (e: any) => {
      if (e.key === "Enter")
            FORM.submit();
}

window.onclick = (e) => {
    if (e.target === $("#searchBar") || e.target === $("#cardsWrapper"))
        FORM.style.height = "0";
}