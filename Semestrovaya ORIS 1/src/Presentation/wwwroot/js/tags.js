function buildTagMap() {
    const tagMap = new Map();

    document.querySelectorAll('.amenities-category').forEach(categoryEl => {
        const type = categoryEl.querySelector('.category-header')?.textContent?.trim() || 'Unknown';
        const items = categoryEl.querySelectorAll('.amenities-item');

        items.forEach(item => {
            const input = item.querySelector('input[type="checkbox"]');
            const span = item.querySelector('span');
            if (input && span) {
                const id = Number(input.value);
                const name = span.textContent.trim();
                if (!isNaN(id) && name) {
                    tagMap.set(id, { id, name, type });
                }
            }
        });
    });

    return tagMap;
}

const tagMap = buildTagMap();

let selectedTags = new Set();
let isCitySelected = false;

const amenitiesInput = document.getElementById('amenitiesInput');
const amenitiesDropdown = document.getElementById('amenitiesDropdown');
const tabButtons = document.querySelectorAll('.tab-btn');
const tabPanes = document.querySelectorAll('.tab-pane');
const selectedItemsContainer = document.getElementById('selectedItems');
const noSelectedMessage = document.getElementById('noSelectedMessage');
const selectedCountEl = document.getElementById('selectedCount');
const applyButton = document.getElementById('applyAmenities');
const confirmReset = document.getElementById('confirmReset');
const cancelReset = document.getElementById('cancelReset');

amenitiesInput.classList.add('disabled');

function renderSelectedItems() {
    selectedItemsContainer.innerHTML = '';
    if (selectedTags.size === 0) {
        noSelectedMessage.style.display = 'block';
        return;
    }
    noSelectedMessage.style.display = 'none';

    selectedTags.forEach(id => {
        const tag = tagMap.get(id);
        if (!tag) return;

        const item = document.createElement('div');
        item.className = 'selected-item';
        item.innerHTML = `
            <span>${tag.name}</span>
            <button data-id="${id}">✕</button>
        `;
        item.querySelector('button').addEventListener('click', () => {
            selectedTags.delete(id);
            updateSelectedCount();
            renderSelectedItems();

            const checkbox = document.querySelector(`input[value="${id}"]`);
            if (checkbox) checkbox.checked = false;
        });
        selectedItemsContainer.appendChild(item);
    });
}

function updateSelectedCount() {
    selectedCountEl.textContent = selectedTags.size;
}

document.querySelectorAll('.amenities-item input[type="checkbox"]').forEach(input => {
    input.addEventListener('change', (e) => {
        const id = Number(e.target.value);
        if (e.target.checked) {
            selectedTags.add(id);
        } else {
            selectedTags.delete(id);
        }
        updateSelectedCount();
        renderSelectedItems();
    });
});

function switchTab(tabId) {
    tabButtons.forEach(btn => btn.classList.remove('active'));
    document.querySelector(`[data-tab="${tabId}"]`).classList.add('active');

    tabPanes.forEach(pane => pane.classList.add('hidden'));
    document.getElementById(`tab-${tabId}`).classList.remove('hidden');

    if (tabId === 'selected') {
        renderSelectedItems();
    }
}

tabButtons.forEach(btn => {
    btn.addEventListener('click', () => {
        switchTab(btn.dataset.tab);
    });
});

applyButton.addEventListener('click', () => {
    amenitiesDropdown.classList.remove('open');
    amenitiesInput.classList.remove('open');
    console.log('Выбранные теги (ID):', Array.from(selectedTags));
});

confirmReset.addEventListener('click', () => {
    selectedTags.clear();
    updateSelectedCount();
    renderSelectedItems();

    document.querySelectorAll('.amenities-item input[type="checkbox"]').forEach(cb => {
        cb.checked = false;
    });
    switchTab('all');
});

cancelReset.addEventListener('click', () => {
    switchTab('all');
});

amenitiesInput.addEventListener('click', () => {
    if (amenitiesInput.classList.contains('disabled')) return;
    amenitiesDropdown.classList.toggle('open');
    amenitiesInput.classList.toggle('open');
    switchTab('all');
});

document.addEventListener('click', (event) => {
    const isClickInsideInput = amenitiesInput.contains(event.target);
    const isClickInsideDropdown = amenitiesDropdown.contains(event.target);
    if (!isClickInsideInput && !isClickInsideDropdown) {
        amenitiesDropdown.classList.remove('open');
        amenitiesInput.classList.remove('open');
    }
});

if (cityInput) {
    const observer = new MutationObserver((mutations) => {
        mutations.forEach((mutation) => {
            if (mutation.attributeName === 'data-selected') {
                const isSelected = cityInput.getAttribute('data-selected') === 'true';
                if (!isSelected) {
                    selectedTags.clear();
                    updateSelectedCount();
                    renderSelectedItems();
                    document.querySelectorAll('.amenities-item input[type="checkbox"]').forEach(cb => {
                        cb.checked = false;
                    });
                    amenitiesInput.classList.add('disabled');
                } else {
                    amenitiesInput.classList.remove('disabled');
                }
                isCitySelected = isSelected;
            }
        });
    });

    observer.observe(cityInput, {
        attributes: true,
        attributeFilter: ['data-selected']
    });

    isCitySelected = cityInput.getAttribute('data-selected') === 'true';
    if (isCitySelected) {
        amenitiesInput.classList.remove('disabled');
    } else {
        amenitiesInput.classList.add('disabled');
    }
}

window.getSelectedAmenities = () => {
    return isCitySelected ? Array.from(selectedTags) : [];
};

window.isAmenitiesSelected = () => isCitySelected && selectedTags.size > 0;
