document.addEventListener('DOMContentLoaded', function() {
    const mainImage = document.getElementById('mainImage');
    const thumbnailsContainer = document.querySelector('.slider__thumbnails');
    const counter = document.querySelector('.slider__counter');
    const showMoreBtn = document.querySelector('.slider__show-more-btn');

    const imageElements = Array.from(document.querySelectorAll('.slider__images img'));
    const totalImages = imageElements.length;

    if (totalImages === 0) {
        counter.textContent = "Нет фото";
        return;
    }

    let currentImageIndex = 0;

    function updateMainImage(index) {
        const img = imageElements[index];
        mainImage.src = img.src;
        mainImage.alt = img.alt || `Фото ${index + 1}`;
        counter.textContent = `${index + 1}/${totalImages}`;
    }

    const prevButton = document.querySelector('.slider__nav-button--prev');
    const nextButton = document.querySelector('.slider__nav-button--next');

    function goToNext() {
        currentImageIndex = (currentImageIndex + 1) % totalImages;
        updateMainImage(currentImageIndex);
        updateActiveThumbnail();
    }

    function goToPrev() {
        currentImageIndex = (currentImageIndex - 1 + totalImages) % totalImages;
        updateMainImage(currentImageIndex);
        updateActiveThumbnail();
    }

    function updateActiveThumbnail() {
        document.querySelectorAll('.slider__thumbnail').forEach((thumb, index) => {
            if (index === currentImageIndex) {
                thumb.classList.add('active');
            } else {
                thumb.classList.remove('active');
            }
        });
    }

    prevButton.addEventListener('click', goToPrev);
    nextButton.addEventListener('click', goToNext);

    document.addEventListener('keydown', function(e) {
        if (e.key === 'ArrowLeft') {
            goToPrev();
        } else if (e.key === 'ArrowRight') {
            goToNext();
        }
    });

    function renderThumbnails() {
        thumbnailsContainer.innerHTML = '';

        const maxThumbsToShow = Math.min(12, totalImages);

        for (let i = 0; i < maxThumbsToShow; i++) {
            const thumb = document.createElement('img');
            thumb.src = imageElements[i].src;
            thumb.alt = imageElements[i].alt || `Миниатюра ${i + 1}`;
            thumb.className = 'slider__thumbnail';
            thumb.dataset.index = i;

            if (i === currentImageIndex) {
                thumb.classList.add('active');
            }

            thumb.addEventListener('click', function() {
                currentImageIndex = parseInt(this.dataset.index);
                updateMainImage(currentImageIndex);
                document.querySelectorAll('.slider__thumbnail').forEach(el => el.classList.remove('active'));
                this.classList.add('active');
            });

            thumbnailsContainer.appendChild(thumb);
        }
    }

    updateMainImage(currentImageIndex);
    renderThumbnails();

    showMoreBtn.addEventListener('click', function() {
        alert(`Пока нельзя`);
    });

    document.querySelectorAll('.comments__raiting').forEach(ratingElement => {

        const rating = parseFloat(ratingElement.textContent.trim());


        const riTitle = ratingElement.nextElementSibling?.querySelector('.ri-title');

        if (riTitle && !isNaN(rating)) {

            if (rating >= 4) {
                riTitle.textContent = 'Хорошо';
            } else if (rating >= 3) {
                riTitle.textContent = 'Нормально';
            } else {
                riTitle.textContent = 'Плохо';
            }
        }
    });
});