const ratingInput = document.getElementById('ratingInput');
const ratingDropdown = document.getElementById('ratingDropdown');

ratingInput.classList.add('disabled');

const cityInput = document.getElementById('cityInput');

if (cityInput) {
    const observer = new MutationObserver((mutations) => {
        mutations.forEach((mutation) => {
            if (mutation.attributeName === 'data-selected') {
                const isSelected = cityInput.getAttribute('data-selected') === 'true';

                if (isSelected) {
                    ratingInput.classList.remove('disabled');
                } else {
                    ratingInput.classList.add('disabled');
                    ratingInput.dataset.selected = 'false';
                    ratingInput.querySelector('span:first-child').textContent = 'РЕЙТИНГ ОТЕЛЯ';

                    const radios = ratingDropdown.querySelectorAll('.rating-option input');
                    radios.forEach(radio => {
                        if (radio.value === 'Любой') {
                            radio.checked = true;
                        } else {
                            radio.checked = false;
                        }
                    });
                }
            }
        });
    });

    observer.observe(cityInput, {
        attributes: true,
        attributeFilter: ['data-selected']
    });

    if (cityInput.getAttribute('data-selected') === 'true') {
        ratingInput.classList.remove('disabled');
    }
}

ratingInput.addEventListener('click', () => {
    if (ratingInput.classList.contains('disabled')) return;

    ratingDropdown.classList.toggle('open');
    ratingInput.classList.toggle('open');
});

ratingDropdown.querySelectorAll('.rating-option input').forEach(radio => {
    radio.addEventListener('change', () => {
        if (radio.checked) {
            const value = radio.value;
            ratingInput.dataset.selected = 'true';
            ratingInput.querySelector('span:first-child').textContent = `Рейтинг ${value == "" ? "Любой" : value}`;
            ratingDropdown.classList.remove('open');
            ratingInput.classList.remove('open');
        }
    });
});

document.addEventListener('click', (event) => {
    const isClickInsideInput = ratingInput.contains(event.target);
    const isClickInsideDropdown = ratingDropdown.contains(event.target);

    if (!isClickInsideInput && !isClickInsideDropdown) {
        ratingDropdown.classList.remove('open');
        ratingInput.classList.remove('open');
    }
});

window.getSelectedRating = () => {
    if (ratingInput.dataset.selected !== 'true') return '';

    const text = ratingInput.querySelector('span:first-child').textContent;

    const match = text.match(/(\d+(?:\.\d+)?)/);

    return match ? match[1] : '';
};

window.isRatingSelected = () => ratingInput.dataset.selected === 'true';