"use strict";
$(".picturePreview").addEventListener("mouseover", function (e) {
    e.target.style.objectFit = "none";
});
$(".picturePreview").addEventListener("mousemove", function (e) {
    let rect = e.target.getBoundingClientRect();
    let xLocal = e.clientX - rect.left;
    let yLocal = e.clientY - rect.top;
    let x = Math.min(xLocal * 100 / e.target.offsetWidth, 100);
    let y = Math.min(yLocal * 100 / e.target.offsetHeight, 100);
    e.target.style.objectPosition = `${x}% ${y}%`;
});
$(".picturePreview").addEventListener("mouseleave", function (e) {
    e.target.style.objectFit = "cover";
});
//# sourceMappingURL=create.js.map