function collectSearchFilters() {
    const payload = {};

    const city = window.getSelectedCity?.();
    if (city) {
        payload.destination = {
            type: city.type,
            id: city.original.id
        };
    }

    const budget = window.getSelectedBudget?.();
    if (budget && budget.amount !== null) {
        payload.budget = {
            amount: budget.amount,
            currency: budget.currency
        };
    }

    const meal = window.getSelectedMeal?.();
    if (meal && meal !== 'Любое') {
        payload.meal = meal;
    }

    const ratingStr = window.getSelectedRating?.();
    if (ratingStr && ratingStr !== 'Любой') {
        const match = ratingStr.match(/^(\d+(?:\.\d+)?)/);
        if (match) {
            payload.minRating = parseFloat(match[1]);
        }
    }

    const stars = window.getSelecetdStars?.();
    if (stars && stars > 0) {
        payload.minStars = stars;
    }

    const selectedTagIds = Array.from(window.getSelectedAmenities() || []);
    if (selectedTagIds.length > 0) {
        payload.amenities = selectedTagIds;
    }

    return payload;
}

async function applyFilters() {
    const hotelsList = document.querySelector('.hotels__list');
    if (!hotelsList) return;

    try {
        const filters = collectSearchFilters();

        const response = await fetch('/api/Home/ApplyFilters', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(filters)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const html = await response.text();
        hotelsList.innerHTML = html;

        document.querySelectorAll('.hotel-rating').forEach(ratingBlock => {
            const ratingValueElement = ratingBlock.querySelector('.rating-value');

            if (ratingValueElement) {
                const rating = parseFloat(ratingValueElement.textContent.trim());

                if (!isNaN(rating)) {
                    if (rating < 2) {
                        ratingValueElement.style.color = '#ff4444';
                        ratingValueElement.style.fontWeight = 'bold';
                    } else if (rating <= 3.5) {
                        ratingValueElement.style.color = '#ffaa00';
                        ratingValueElement.style.fontWeight = 'bold';
                    } else {
                        ratingValueElement.style.color = '#00aa00';
                        ratingValueElement.style.fontWeight = 'bold';
                    }
                }
            }
        });

    } catch (error) {
        console.error('Ошибка при применении фильтров:', error);
        hotelsList.innerHTML = '<p>Не удалось загрузить отели. Попробуйте позже.</p>';
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const buttons = document.querySelectorAll('.main__form__button, .header__form__button');
    buttons.forEach(button => {
        button.addEventListener('click', (e) => {
            e.preventDefault();
            applyFilters();
        });
    });
});

document.querySelectorAll('.hotel-rating').forEach(ratingBlock => {
    const ratingValueElement = ratingBlock.querySelector('.rating-value');

    if (ratingValueElement) {
        const rating = parseFloat(ratingValueElement.textContent.trim());

        if (!isNaN(rating)) {
            if (rating < 2) {
                ratingValueElement.style.color = '#ff4444';
                ratingValueElement.style.fontWeight = 'bold';
            } else if (rating <= 3.5) {
                ratingValueElement.style.color = '#ffaa00';
                ratingValueElement.style.fontWeight = 'bold';
            } else {
                ratingValueElement.style.color = '#00aa00';
                ratingValueElement.style.fontWeight = 'bold';
            }
        }
    }
});