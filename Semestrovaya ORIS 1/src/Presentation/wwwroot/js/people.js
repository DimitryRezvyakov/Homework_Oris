const guestsInput = document.getElementById('guestsInput');
const guestsDropdown = document.getElementById('guestsDropdown');
const adultsCountEl = document.getElementById('adultsCount');
const adultsMinus = document.getElementById('adultsMinus');
const adultsPlus = document.getElementById('adultsPlus');
const childrenPlus = document.getElementById('childrenPlus');
const childrenList = document.getElementById('childrenList');
const ageSelector = document.getElementById('ageSelector');
const ageOptions = document.getElementById('ageOptions');
const applyBtn = document.getElementById('applyBtn');

let adults = 1;
let children = [];

function formatChildAge(age) {
  if (age <= 1) return 'До 2 лет';
  if (age >= 2 && age <= 4) return `${age} года`;
  return `${age} лет`;
}

function updateInputText() {
  let text = '';

  if (adults === 1) {
    text = '1 взрослый';
  } else if (adults >= 2 && adults <= 4) {
    text = `${adults} взрослых`;
  } else {
    text = `${adults} взрослых`;
  }

  const childCount = children.filter(age => age !== null).length;
  if (childCount > 0) {
    let childText = '';
    if (childCount === 1) {
      childText = '1 ребёнок';
    } else if (childCount >= 2 && childCount <= 4) {
      childText = `${childCount} ребенка`;
    } else {
      childText = `${childCount} детей`;
    }
    text += `, ${childText}`;
  }

  guestsInput.value = text || 'Сколько гостей';
}

function renderChildrenList() {
  childrenList.innerHTML = '';
  children.forEach((age, index) => {
    if (age !== null) {
      const childDiv = document.createElement('div');
      childDiv.className = 'added-child';

      const label = formatChildAge(age);
      childDiv.innerHTML = `
        <span>${label}</span>
        <button class="remove-child" data-index="${index}">×</button>
      `;

      childrenList.appendChild(childDiv);
    }
  });

  document.querySelectorAll('.remove-child').forEach(btn => {
    btn.onclick = () => {
      const idx = parseInt(btn.dataset.index, 10);
      children.splice(idx, 1);
      renderChildrenList();
      renderAgeSelector();
      updateInputText();
      updateChildrenButton();
    };
  });
}

function renderAgeSelector() {
  const unfinishedIndex = children.indexOf(null);
  if (unfinishedIndex === -1) {
    ageSelector.style.display = 'none';
    return;
  }

  ageSelector.style.display = 'block';
  ageOptions.innerHTML = '';

  for (let age = 1; age <= 15; age++) {
    const option = document.createElement('div');
    option.className = 'age-option';
    option.textContent = formatChildAge(age);

    option.onclick = () => {
      children[unfinishedIndex] = age;
      renderChildrenList();
      renderAgeSelector();
      updateInputText();
      updateChildrenButton();
    };

    ageOptions.appendChild(option);
  }
}

function updateChildrenButton() {
  const canAdd = children.length < 3 && !children.includes(null);
  childrenPlus.disabled = !canAdd;
}

adultsPlus.onclick = () => {
  if (adults < 6) {
    adults++;
    adultsCountEl.textContent = adults;
    adultsMinus.disabled = adults <= 1;
    updateInputText();
  }
};

adultsMinus.onclick = () => {
  if (adults > 1) {
    adults--;
    adultsCountEl.textContent = adults;
    adultsMinus.disabled = adults <= 1;
    updateInputText();
  }
};

childrenPlus.onclick = () => {
  if (children.length < 3 && !children.includes(null)) {
    children.push(null);
    renderChildrenList();
    renderAgeSelector();
    updateInputText();
    updateChildrenButton();
  }
};

guestsInput.onclick = () => {
  guestsDropdown.classList.toggle('open');
};

document.addEventListener('click', (e) => {
  if (!guestsInput.contains(e.target) && !guestsDropdown.contains(e.target)) {
    guestsDropdown.classList.remove('open');
  }
});

applyBtn.onclick = () => {
  guestsDropdown.classList.remove('open');
};

window.getSelectedPeoples = () => {
    return {
        adults,
        childrenCount: children.length,
        childrenAges: children
    }
}
updateInputText();
updateChildrenButton();