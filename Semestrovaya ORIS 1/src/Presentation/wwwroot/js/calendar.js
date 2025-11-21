// === ГЛОБАЛЬНЫЕ ПЕРЕМЕННЫЕ ===
let startDate = null;
let endDate = null;
let currentOffset = 0; // текущее смещение месяцев от текущего

const datesInput = document.getElementById('datesInput');
const calendarPopup = document.getElementById('calendarPopup');

// === БЛОКИРОВКА РУЧНОГО ВВОДА ===
datesInput.addEventListener('keydown', (e) => e.preventDefault());
datesInput.addEventListener('paste', (e) => e.preventDefault());
datesInput.addEventListener('mousedown', (e) => {
  // Разрешаем только левый клик для открытия
  if (e.button !== 0) return;
  e.preventDefault();
  renderCalendar(currentOffset);
  calendarPopup.classList.add('open');
});

// === ФОРМАТЫ ===
function formatDate(date) {
  const months = ['янв', 'фев', 'мар', 'апр', 'май', 'июн', 'июл', 'авг', 'сен', 'окт', 'ноя', 'дек'];
  return `${date.getDate()} ${months[date.getMonth()]}`;
}

function formatRange() {
  if (!startDate || !endDate) return '';
  const daysDiff = Math.ceil((endDate - startDate) / (1000 * 60 * 60 * 24));
  return `${formatDate(startDate)} - ${formatDate(endDate)} (${daysDiff} нч)`;
}

function updateInputValue() {
  datesInput.value = formatRange();
}

function initDefaultRange() {
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  const tomorrow = new Date(today);
  tomorrow.setDate(today.getDate() + 1);

  startDate = today;
  endDate = tomorrow;

  updateInputValue();
}

// === РЕНДЕР КАЛЕНДАРЯ ===
function renderCalendar(monthOffset = 0) {
  currentOffset = monthOffset;

  const now = new Date();
  const currentMonth = new Date(now.getFullYear(), now.getMonth() + monthOffset, 1);
  const nextMonth = new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1);

  const months = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
                  'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];

  calendarPopup.innerHTML = '';

  // === ЗАГОЛОВОК С КНОПКАМИ ===
  const header = document.createElement('div');
  header.className = 'calendar-header';

  const prevButton = document.createElement('button');
  prevButton.className = 'nav-button prev';
  prevButton.textContent = '❮';
  prevButton.disabled = currentOffset <= 0;
  prevButton.style.opacity = currentOffset <= 0 ? '0.4' : '1';
  prevButton.style.cursor = currentOffset <= 0 ? 'not-allowed' : 'pointer';
  prevButton.addEventListener('click', (e) => {
    e.stopPropagation();
    if (currentOffset > 0) renderCalendar(currentOffset - 1);
  });

  const month1Div = document.createElement('div');
  month1Div.className = 'calendar-month';
  month1Div.innerHTML = `<h3>${months[currentMonth.getMonth()]} <span>${currentMonth.getFullYear()}</span></h3>`;

  const month2Div = document.createElement('div');
  month2Div.className = 'calendar-month';
  month2Div.innerHTML = `<h3>${months[nextMonth.getMonth()]} <span>${nextMonth.getFullYear()}</span></h3>`;

  const nextButton = document.createElement('button');
  nextButton.className = 'nav-button next';
  nextButton.textContent = '❯';
  nextButton.disabled = currentOffset >= 12;
  nextButton.style.opacity = currentOffset >= 12 ? '0.4' : '1';
  nextButton.style.cursor = currentOffset >= 12 ? 'not-allowed' : 'pointer';
  nextButton.addEventListener('click', (e) => {
    e.stopPropagation();
    if (currentOffset < 12) renderCalendar(currentOffset + 1);
  });

  header.appendChild(prevButton);
  header.appendChild(month1Div);
  header.appendChild(month2Div);
  header.appendChild(nextButton);
  calendarPopup.appendChild(header);

  const monthsContainer = document.createElement('div');
  monthsContainer.className = 'calendar-months-container';
  monthsContainer.style.display = 'flex';
  monthsContainer.style.gap = '20px';
  monthsContainer.style.justifyContent = 'space-between';

  const month1Block = renderSingleMonth(currentMonth, true);
  monthsContainer.appendChild(month1Block);

  const month2Block = renderSingleMonth(nextMonth, false);
  monthsContainer.appendChild(month2Block);

  calendarPopup.appendChild(monthsContainer);
}

function renderSingleMonth(monthDate, isFirstMonth) 
{
    const container = document.createElement('div');
    container.className = 'single-month-block';

    const weekdays = ['ПН', 'ВТ', 'СР', 'ЧТ', 'ПТ', 'СБ', 'ВС'];
    const weekRow = document.createElement('div');
    weekRow.className = 'calendar-weekdays';
    weekdays.forEach(day => {
        const d = document.createElement('div');
        d.textContent = day;
        weekRow.appendChild(d);
    });
    container.appendChild(weekRow);

    const daysGrid = document.createElement('div');
    daysGrid.className = 'calendar-days';

    const firstDayOfMonth = monthDate.getDay();
    const adjustedFirstDay = (firstDayOfMonth === 0) ? 6 : firstDayOfMonth - 1;

    for (let i = 0; i < adjustedFirstDay; i++) 
        {
        const empty = document.createElement('div');
        empty.className = 'calendar-day disabled';
        daysGrid.appendChild(empty);
    }

    const daysInMonth = new Date(monthDate.getFullYear(), monthDate.getMonth() + 1, 0).getDate();
  for (let day = 1; day <= daysInMonth; day++) 
   {
        const date = new Date(monthDate.getFullYear(), monthDate.getMonth(), day);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        const dayEl = document.createElement('div');
        dayEl.className = 'calendar-day';
        dayEl.textContent = day;

        const isPast = date < today;

        if (isPast) {
            dayEl.classList.add('disabled');
            dayEl.style.pointerEvents = 'none';
            dayEl.style.color = '#ccc';         
        } else {
            dayEl.addEventListener('click', (e) => {
                e.stopPropagation();
                if (!startDate) {
                startDate = date;
                endDate = null;
                } else if (!endDate && date >= startDate) {
                endDate = date;
                } else {
                startDate = date;
                endDate = null;
                }
                renderCalendar(currentOffset);
                updateInputValue();
            });
        }

        if (startDate && date.getTime() === startDate.getTime()) {
            dayEl.classList.add('selected', 'start');
            if (isPast) {
                dayEl.style.color = '#fff';
            }
        }
        if (endDate && date.getTime() === endDate.getTime()) {
            dayEl.classList.add('selected', 'end');
        if (isPast) {
            dayEl.style.color = '#fff';
        }
        }
        if (startDate && endDate && date > startDate && date < endDate) {
        dayEl.classList.add('range');
        if (isPast) {
            dayEl.style.backgroundColor = '#cce5ff';
        }
        }

        daysGrid.appendChild(dayEl);
    } 

    const totalDays = adjustedFirstDay + daysInMonth;
    const rowsNeeded = Math.ceil(totalDays / 7);

    if (rowsNeeded < 6) {
        const remainingInLastRow = 7 - (totalDays % 7);
        if (remainingInLastRow < 7) {
            for (let i = 0; i < remainingInLastRow; i++) {
            const empty = document.createElement('div');
            empty.className = 'calendar-day disabled';
            daysGrid.appendChild(empty);
            }
        }
    } else {
        while (daysGrid.children.length < 42) {
            const empty = document.createElement('div');
            empty.className = 'calendar-day disabled';
            daysGrid.appendChild(empty);
        }
    }

    container.appendChild(daysGrid);
    return container;
}

document.addEventListener('click', (e) => {
  if (
    !datesInput.contains(e.target) &&
    !calendarPopup.contains(e.target) &&
    calendarPopup.classList.contains('open')
  ) {
    if (startDate && !endDate) {
      endDate = new Date(startDate);
      endDate.setDate(startDate.getDate() + 1);
      updateInputValue();
    }
    calendarPopup.classList.remove('open');
  }
});

window.getSelectedDate = () => {
    return {
        start: startDate,
        end: endDate
    };
};

initDefaultRange();