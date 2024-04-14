// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    var currentUrl = window.location.pathname;
    if (currentUrl === "/") {
        currentUrl = "/trang-chu";
    }
    var navLinks = document.querySelectorAll(".navbar-nav a");
    navLinks.forEach(function (link) {
        if (link.getAttribute("href") === currentUrl) {
            link.parentNode.classList.add("active");
        }
    });
});