// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Autocomplete functionality for book authors

window.addEventListener('DOMContentLoaded', event => {

    // Navbar shrink function
    var navbarShrink = function () {
        const navbarCollapsible = document.body.querySelector('#mainNav');
        if (!navbarCollapsible) {
            return;
        }
        navbarCollapsible.classList.add('navbar-shrink');
    };

    // Check if the user is logged in
    const isAuthenticated = document.body.dataset.isAuthenticated === 'true';
    if (isAuthenticated) {
        // Always apply navbar shrink if the user is logged in
        navbarShrink();
    } else {
        // Shrink the navbar when page is scrolled for non-logged-in users
        document.addEventListener('scroll', () => {
            if (window.scrollY === 0) {
                // Remove navbar shrink if scrolled to top
                document.body.querySelector('#mainNav').classList.remove('navbar-shrink');
            } else {
                // Apply navbar shrink if scrolled down
                navbarShrink();
            }
        });
    }

    // Collapse responsive navbar when toggler is visible
    const navbarToggler = document.body.querySelector('.navbar-toggler');
    const responsiveNavItems = [].slice.call(
        document.querySelectorAll('#navbarResponsive .nav-link')
    );
    responsiveNavItems.map(function (responsiveNavItem) {
        responsiveNavItem.addEventListener('click', () => {
            if (window.getComputedStyle(navbarToggler).display !== 'none') {
                navbarToggler.click();
            }
        });
    });

});
