const budgetInput = document.getElementById('budgetInput');
const budgetDropdown = document.getElementById('budgetDropdown');
const budgetAmount = document.getElementById('budgetAmount');
const budgetSubheader = document.querySelector('.budget-subheader');

let currentCurrency = 'RUB';
let currentAmount = null;

const currencyShortNames = {
    'RUB': 'РУБ',
    'USD/EUR': 'USD/EUR',
    'BYN': 'BYN',
    'THG': 'THG',
    'UAH': 'UAH',
    'UZS': 'UZS',
    'KGZ': 'KGZ'
};

function updateBudgetDisplay() {
    const shortName = currencyShortNames[currentCurrency] || currentCurrency;
    if (currentAmount !== null && !isNaN(currentAmount)) {
        budgetInput.querySelector('span:first-child').textContent = `${currentAmount} ${shortName}`;
    } else {
        budgetInput.querySelector('span:first-child').textContent = `БЮДЖЕТ (${shortName})`;
    }
}

function updateSubheader() {
    const shortName = currencyShortNames[currentCurrency] || currentCurrency;
    budgetSubheader.textContent = `Максимальный бюджет (${shortName})`;
}

budgetInput.addEventListener('click', () => {
    budgetDropdown.classList.toggle('open');
    budgetInput.classList.toggle('open');

    if (budgetDropdown.classList.contains('open')) {
        budgetAmount.value = currentAmount !== null ? currentAmount : '';
        document.querySelectorAll('input[name="currency"]').forEach(radio => {
            radio.checked = radio.value === currentCurrency;
        });
        updateSubheader();
    }
});

budgetAmount.addEventListener('input', () => {
    const value = budgetAmount.value.trim();
    if (value === '') {
        currentAmount = null;
    } else {
        const num = parseFloat(value);
        currentAmount = !isNaN(num) && num >= 0 ? num : null;
    }
    updateBudgetDisplay();
    updateSubheader();
});

document.querySelectorAll('input[name="currency"]').forEach(radio => {
    radio.addEventListener('change', () => {
        if (radio.checked) {
            currentCurrency = radio.value;
            updateSubheader();
            updateBudgetDisplay();
        }
    });
});

document.addEventListener('click', (event) => {
    const isClickInsideInput = budgetInput.contains(event.target);
    const isClickInsideDropdown = budgetDropdown.contains(event.target);

    if (!isClickInsideInput && !isClickInsideDropdown) {
        budgetDropdown.classList.remove('open');
        budgetInput.classList.remove('open');
    }
});

const applyBudgetBtn = document.getElementById('applyBudget');
if (applyBudgetBtn) {
    applyBudgetBtn.addEventListener('click', () => {
        budgetDropdown.classList.remove('open');
        budgetInput.classList.remove('open');
    });
}

window.getSelectedBudget = () => {
    if (currentAmount !== null) {
        return { amount: currentAmount, currency: currentCurrency };
    }
    return null;
};

window.isBudgetSelected = () => {
    return currentAmount !== null;
};