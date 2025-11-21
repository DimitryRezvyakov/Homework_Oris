document.addEventListener('DOMContentLoaded', () => {
    const responseEl = document.getElementById('response');

    async function getHotel() {
        const inputEl = document.getElementById('get-id');
        const id = Number(inputEl.value);

        if (!id || isNaN(id) || id <= 0) {
            alert('Пожалуйста, введите корректный ID (целое положительное число).');
            return;
        }

        const url = `/api/Admin/GetHotelById?id=${id}`;
        console.log('GET URL:', url);

        try {
            const response = await fetch(url, { method: 'GET', credentials: 'include' });
            let data;
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                data = await response.json();
            } else {
                data = await response.text();
            }

            if (!response.ok) throw new Error(`HTTP ${response.status}`);

            responseEl.textContent = JSON.stringify(data, null, 2);
        } catch (error) {
            responseEl.textContent = `Ошибка: ${error.message}`;
        }
    }

    async function deleteHotel() {
        const inputEl = document.getElementById('delete-id');
        const id = Number(inputEl.value);

        if (!id || isNaN(id) || id <= 0) {
            alert('Пожалуйста, введите корректный ID (целое положительное число).');
            return;
        }

        const url = `/api/Admin/DeleteHotel?id=${id}`;
        console.log('DELETE URL:', url);

        try {
            const response = await fetch(url, { method: 'DELETE', credentials: 'include' });
            let data;
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                data = await response.json();
            } else {
                data = await response.text();
            }

            if (!response.ok) throw new Error(`HTTP ${response.status}`);

            responseEl.textContent = JSON.stringify(data, null, 2);
        } catch (error) {
            responseEl.textContent = `Ошибка: ${error.message}`;
        }
    }

    document.getElementById('get-btn').addEventListener('click', getHotel);
    document.getElementById('delete-btn').addEventListener('click', deleteHotel);
});



document.addEventListener('DOMContentLoaded', function () {
    const createBtn = document.getElementById('create-btn');
    const deleteBtn = document.getElementById('delete-btn');
    const updateBtn = document.getElementById('update-btn');
    const modal = document.getElementById('modal');
    const modalContent = document.getElementById('modal-content');
    const closeModalBtn = document.getElementById('close-modal');
    const responseDiv = document.getElementById('response');

    createBtn.addEventListener('click', function () {
        openCreateModal();
    });

    updateBtn.addEventListener('click', function () {
        const hotelId = document.getElementById('update-id').value;
        if (!hotelId) {
            responseDiv.textContent = 'Пожалуйста, введите ID отеля для обновления';
            return;
        }
        openUpdateModal(parseInt(hotelId));
    });

    closeModalBtn.addEventListener('click', function () {
        closeModal();
    });

    modal.addEventListener('click', function (e) {
        if (e.target === modal) {
            closeModal();
        }
    });

    deleteBtn.addEventListener('click', function () {
        const hotelId = document.getElementById('delete-id').value;
        if (!hotelId) {
            responseDiv.textContent = 'Пожалуйста, введите ID отеля';
            return;
        }

        responseDiv.textContent = `Запрос на удаление отеля с ID: ${hotelId}`;
    });

    function openCreateModal() {
        modalContent.innerHTML = '<div class="loading">Загрузка формы...</div>';
        modal.style.display = 'block';

        fetch('/api/Admin/AdminCreateHotelPartial')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Ошибка загрузки формы');
                }
                return response.text();
            })
            .then(html => {

                modalContent.innerHTML = html;

                if (typeof window.initHotelForm === 'function') {
                    window.initHotelForm();
                } else {
                    console.error('Функция initHotelForm не найдена');
                }

                const form = modalContent.querySelector('form');
                if (form) {
                    form.addEventListener('submit', function (e) {
                        e.preventDefault();
                        handleFormSubmit(form);
                    });
                }
            })
            .catch(error => {
                modalContent.innerHTML = `<div class="error">Ошибка: ${error.message}</div>`;
                console.error('Ошибка загрузки формы:', error);
            });
    }

    function openUpdateModal(hotelId) {

        modalContent.innerHTML = '<div class="loading">Загрузка формы редактирования...</div>';
        modal.style.display = 'block';

        fetch('/api/Admin/AdminUpdateHotelPartial')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Ошибка загрузки формы редактирования');
                }
                return response.text();
            })
            .then(html => {
                modalContent.innerHTML = html;

                if (typeof window.initHotelUpdateForm === 'function') {
                    window.initHotelUpdateForm(hotelId);
                } else {
                    console.error('Функция initHotelUpdateForm не найдена');
                    modalContent.innerHTML = '<div class="error">Ошибка: функция инициализации не найдена</div>';
                }

                const form = modalContent.querySelector('form');

            })
            .catch(error => {
                modalContent.innerHTML = `<div class="error">Ошибка: ${error.message}</div>`;
                console.error('Ошибка загрузки формы редактирования:', error);
            });
    }

    function closeModal() {
        modal.style.display = 'none';
    }
});