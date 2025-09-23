document.addEventListener("DOMContentLoaded", function () {
    const slides = document.querySelectorAll(".slide-image");
    const prevBtn = document.getElementById("prevBtn");
    const nextBtn = document.getElementById("nextBtn");
    let currentIndex = 0;
    let slideInterval;

    function showSlide(index) {
        slides.forEach((slide, i) => {
            slide.classList.toggle("active", i === index);
        });
    }

    function nextSlide() {
        currentIndex = (currentIndex + 1) % slides.length;
        showSlide(currentIndex);
    }

    function prevSlideFunc() {
        currentIndex = (currentIndex - 1 + slides.length) % slides.length;
        showSlide(currentIndex);
    }

    prevBtn.addEventListener("click", () => {
        prevSlideFunc();
        resetInterval();
    });

    nextBtn.addEventListener("click", () => {
        nextSlide();
        resetInterval();
    });

    function startInterval() {
        slideInterval = setInterval(nextSlide, 4000);
    }

    function resetInterval() {
        clearInterval(slideInterval);
        startInterval();
    }

    // Initialize
    showSlide(currentIndex);
    startInterval();
});
