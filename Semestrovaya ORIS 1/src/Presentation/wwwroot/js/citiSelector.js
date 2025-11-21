const input = document.getElementById('cityInput');
const suggestionsBox = document.getElementById('suggestions');

let countries = [];
let resorts = [];
let hotels = [];
let countryMap = new Map();
let resortMap = new Map();

let selectedCity = null;
let isConfirmedSelection = false;

async function initializeAutocomplete() {
    try {
        const data = await fetchData();
        countries = data.countries || [];
        resorts = data.resorts || [];
        hotels = data.hotels || [];

        countryMap = new Map(countries.map(c => [c.id, c]));
        resortMap = new Map(resorts.map(r => [r.id, r]));

        attachEventListeners();
    } catch (error) {
        console.error('Не удалось инициализировать автокомплит:', error);
    }
}

async function fetchData() {
    try {
        const response = await fetch('/api/Hotel/GetAllSearchData');
        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
        const data = await response.json();
        return {
            countries: data.countries || [],
            resorts: data.resorts || [],
            hotels: data.hotels || []
        };
    } catch (error) {
        console.error('Ошибка при получении данных поиска:', error);
        return { countries: [], resorts: [], hotels: [] };
    }
}

function setDestinationState(cityObj, confirmed = true) {
    selectedCity = cityObj;
    isConfirmedSelection = confirmed;

    if (cityObj) {
        const isSelected = (cityObj.type === 'country' || cityObj.type === 'resort');
        input.setAttribute('data-selected', isSelected ? 'true' : 'false');
    } else {
        input.setAttribute('data-selected', 'false');
    }
}

function showSuggestions(filtered) {
    suggestionsBox.innerHTML = '';
    if (filtered.length === 0) {
        suggestionsBox.classList.remove('open');
        return;
    }

    filtered.forEach(item => {
        const div = document.createElement('div');
        div.className = 'suggestion-item';

        let imgSrc = '';
        let primaryText = '';
        let secondaryText = '';

        if (item.type === 'country') {
            imgSrc = item.imageSrc;
            primaryText = item.name;
        } else if (item.type === 'resort') {
            imgSrc = 'images/point.png';
            primaryText = item.name;
            const country = countryMap.get(item.countryId);
            secondaryText = country ? country.name : '';
        } else if (item.type === 'hotel') {
            imgSrc = 'images/bed.png';
            primaryText = item.name;
            const resort = resortMap.get(item.resortId);
            const country = resort ? countryMap.get(resort.countryId) : null;
            secondaryText = resort && country ? `${resort.name}, ${country.name}` : '';
        }

        const secondaryHtml = secondaryText ? `<div class="suggestion-country">${secondaryText}</div>` : '';

        div.innerHTML = `
            <span class="suggestion-pin">
                <img src="${imgSrc}" alt="">
            </span>
            <div class="suggestion-text">
                <div class="suggestion-name">${primaryText}</div>
                ${secondaryHtml}
            </div>
        `;

        div.addEventListener('mousedown', () => {
            const cityObj = {
                name: primaryText,
                type: item.type,
                original: item
            };
            input.value = primaryText;
            setDestinationState(cityObj, true);
            suggestionsBox.classList.remove('open');
        });

        suggestionsBox.appendChild(div);
    });

    suggestionsBox.classList.add('open');
}

function attachEventListeners() {
    input.addEventListener('input', () => {
        const query = input.value.trim().toLowerCase();
        isConfirmedSelection = false;

        const allItems = [
            ...countries.map(c => ({ ...c, type: 'country' })),
            ...resorts.map(r => ({ ...r, type: 'resort' })),
            ...hotels.map(h => ({ ...h, type: 'hotel' }))
        ];

        const filtered = allItems
            .filter(item => item.name.toLowerCase().startsWith(query))
            .slice(0, 6);

        showSuggestions(filtered);
    });

    input.addEventListener('focus', () => {
        input.value = '';
        const allItems = [
            ...countries.map(c => ({ ...c, type: 'country' })),
            ...resorts.map(r => ({ ...r, type: 'resort' })),
            ...hotels.map(h => ({ ...h, type: 'hotel' }))
        ];
        const first6 = allItems.slice(0, 6);
        showSuggestions(first6);
    });

    input.addEventListener('blur', () => {
        setTimeout(() => {
            suggestionsBox.classList.remove('open');
        }, 150);

        const inputValue = input.value.trim();
        if (inputValue === '') isConfirmedSelection = false;

        if (isConfirmedSelection && selectedCity) return;

        if (selectedCity) {
            input.value = selectedCity.name;
            const isSelected = (selectedCity.type === 'country' || selectedCity.type === 'resort');
            input.setAttribute('data-selected', isSelected ? 'true' : 'false');
        } else {
            input.value = '';
            input.setAttribute('data-selected', 'false');
        }
    });

    document.addEventListener('click', (event) => {
        const isClickInsideInput = input.contains(event.target);
        const isClickInsideSuggestions = suggestionsBox.contains(event.target);
        if (!isClickInsideInput && !isClickInsideSuggestions) {
            suggestionsBox.classList.remove('open');
            input.blur();
        }
    });
}

if (input && suggestionsBox) {
    initializeAutocomplete();
} else {
    console.warn('Элементы cityInput или suggestions не найдены');
}

window.isCitySelected = () => input?.getAttribute('data-selected') === 'true';
window.getSelectedCity = () => selectedCity;