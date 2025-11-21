const hotelsList = document.querySelector('.hotels__list');

if (hotelsList) {
    hotelsList.addEventListener('click', function (e) {
        const hotelNameEl = e.target.closest('.hotel-name');
        if (!hotelNameEl) return;

        e.preventDefault();

        try {
            const hotelId = parseInt(hotelNameEl.dataset.hotelid);
            if (isNaN(hotelId) || hotelId <= 0) {
                throw new Error('Некорректный HotelId');
            }

            const guests = window.getSelectedPeoples?.() || { adults: 1, childrenCount: 0 };
            const parentsCount = guests.adults || 1;
            const childrenCount = guests.childrenCount || 0;

            const nutrition = window.getSelectedMeal?.() || 'Любое';

            const dates = window.getSelectedDate?.();
            const start = dates?.start;
            const end = dates?.end;

            if (!start || !end) {
                throw new Error('Выберите даты заезда и выезда');
            }

            const startDateStr = start.toISOString().split('T')[0];

            const timeDiff = end.getTime() - start.getTime();
            const nightsCount = Math.ceil(timeDiff / (1000 * 60 * 60 * 24));
            if (nightsCount < 1) {
                throw new Error('Количество ночей должно быть не менее 1');
            }

            const params = new URLSearchParams({
                HotelId: hotelId,
                ParentsCount: parentsCount,
                ChildrenCount: childrenCount,
                StartDate: startDateStr,
                NightsCount: nightsCount,
                Nutrition: nutrition
            });

            window.location.href = `/Home/Hotel?${params.toString()}`;

        } catch (error) {
            console.error('Ошибка:', error);
            alert('Ошибка: ' + error.message);
        }

    });
}