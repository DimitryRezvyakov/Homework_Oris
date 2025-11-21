document.addEventListener('DOMContentLoaded', () => {
    const cityInput = document.getElementById('cityInput');
    const starRating = document.getElementById('starRating');
    const stars = Array.from(document.querySelectorAll('.star-rating__star'));

    if (!cityInput || !starRating || stars.length === 0) {
        console.warn('Элементы звёздного рейтинга не найдены');
        return;
    }

    let selectedValue = 0;

    function isCitySelected() {
        return cityInput.getAttribute('data-selected') === 'true';
    }

    function renderStars(hoverValue = null) {
        const activeValue = hoverValue !== null ? hoverValue : selectedValue;
        stars.forEach((star, index) => {
            star.classList.toggle('active', index + 1 <= activeValue);
        });
    }

    function updateFormState() {
        const isActive = isCitySelected();
        starRating.classList.toggle('active', isActive);
        if (!isActive) {
            selectedValue = 0;
            renderStars();
        }
    }

    stars.forEach((star, index) => {
        const value = index + 1;

        star.addEventListener('mouseenter', () => {
            if (isCitySelected()) {
                renderStars(value);
            }
        });

        star.addEventListener('mouseleave', () => {
            if (isCitySelected()) {
                renderStars();
            }
        });

        star.addEventListener('click', () => {
            if (isCitySelected()) {
                selectedValue = value;
                renderStars();
            }
        });
    });

    const observer = new MutationObserver(updateFormState);
    observer.observe(cityInput, { attributes: true, attributeFilter: ['data-selected'] });

    updateFormState();

    window.getSelecetdStars = () => selectedValue;
});