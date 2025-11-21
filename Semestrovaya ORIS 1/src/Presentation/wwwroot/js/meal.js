const mealInput = document.getElementById('mealInput');
const mealDropdown = document.getElementById('mealDropdown');
let selectedMeal = 'Любое';

mealInput.addEventListener('click', () => {
    mealDropdown.classList.toggle('open');
    mealInput.classList.toggle('open');
});

mealDropdown.querySelectorAll('.meal-option input').forEach(radio => {
    radio.addEventListener('change', () => {
        if (radio.checked) {
            selectedMeal = radio.value;
            mealInput.querySelector('span:first-child').textContent = `Питание ${selectedMeal == "" ? "Любое" : selectedMeal}`;
            mealDropdown.classList.remove('open');
            mealInput.classList.remove('open');
        }
    });
});

document.addEventListener('click', (event) => {
    const isClickInsideMealInput = mealInput.contains(event.target);
    const isClickInsideMealDropdown = mealDropdown.contains(event.target);

    if (!isClickInsideMealInput && !isClickInsideMealDropdown) {
        mealDropdown.classList.remove('open');
        mealInput.classList.remove('open');
    }
});

window.getSelectedMeal = () => selectedMeal;