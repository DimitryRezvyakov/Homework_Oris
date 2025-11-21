import { Editor } from 'https://esm.sh/@tiptap/core@2.5.0';
import StarterKit from 'https://esm.sh/@tiptap/starter-kit@2.5.0';
import Underline from 'https://esm.sh/@tiptap/extension-underline@2.5.0';

let editor = null;
let hotelId = null;
let existingImages = [];
let imagesToDelete = [];
let imagesToAdd = [];
let hotelData = null;

window.initHotelUpdateForm = (id) => {
    hotelId = id;
    document.getElementById('hotelId').value = id;

    imagesToAdd = [];
    imagesToDelete = [];
    existingImages = [];
    loadHotelData(id);
}

function fileNameFromUrl(url) {
    if (!url) return '';
    return url.split('/').pop();
}

function existingUrlForFileName(fileName) {
    return existingImages.find(img => img.endsWith(fileName)) || null;
}

function isMarkedForDeletion(url) {
    return imagesToDelete.includes(url);
}

function markForDeletion(url) {
    if (!imagesToDelete.includes(url)) imagesToDelete.push(url);
}

function unmarkDeletion(url) {
    const idx = imagesToDelete.indexOf(url);
    if (idx !== -1) imagesToDelete.splice(idx, 1);
}

function isInImagesToAdd(fileName) {
    return imagesToAdd.some(item => item.fileName === fileName);
}


function initEditorAndComponents() {
    const editorElement = document.querySelector('#editor');
    const toolbar = document.getElementById('toolbar');

    if (!editorElement || !toolbar) {
        console.error('Элементы редактора не найдены в DOM');
        return;
    }

    editor = new Editor({
        element: editorElement,
        extensions: [
            StarterKit.configure({
                heading: { levels: [2] },
            }),
            Underline,
        ],
        content: extractAndFillGeneralInfo(hotelData?.HtmlDescription) || '',
        editable: true,
    });

    initToolbar(toolbar);
    initComponents();

    loadAdditionalData();
}

async function loadAdditionalData() {
    try {

        await Promise.all([
            loadResorts(),
            loadTags()
        ]);

        setFormValues();

    } catch (error) {
        console.error('Ошибка загрузки дополнительных данных:', error);
    }
}

function setFormValues() {
    if (!hotelData) return;

    const resortSelect = document.getElementById('resortId');
    if (resortSelect && hotelData.ResortId) {
        resortSelect.value = hotelData.ResortId;
    }

    if (hotelData.hotelTags && hotelData.hotelTags.length > 0) {
        setHotelTags(hotelData.hotelTags);
    }
}

function setHotelTags(hotelTags) {
    hotelTags.forEach(tag => {
        const checkbox = document.querySelector(`input[name="tags"][value="${tag.Id}"]`);
        if (checkbox) {
            checkbox.checked = true;
        }
    });
}

function initToolbar(toolbar) {
    const icons = {
        bold: `<svg viewBox="0 0 24 24"><path d="M13.5,15.5H9v-3h4.5a1.5,1.5 0 1,1 0,3M13,11H9V7h4a2,2 0 0,1 0,4M17,11a4,4 0 0,0-2.5-7H7v16h7.5a4.5,4.5 0 0,0 2.5-8Z"/></svg>`,
        italic: `<svg viewBox="0 0 24 24"><path d="M10 4v3h2.21l-3.42 10H6v3h8v-3h-2.21l3.42-10H18V4z"/></svg>`,
        underline: `<svg viewBox="0 0 24 24"><path d="M12,17A5,5 0 0,0 17,12V4H15V12A3,3 0 0,1 12,15A3,3 0 0,1 9,12V4H7V12A5,5 0 0,0 12,17M5,20V22H19V20H5Z"/></svg>`,
        heading: `<svg viewBox="0 0 24 24"><path d="M4,4H6V10H14V4H16V20H14V12H6V20H4V4Z"/></svg>`,
        bulletList: `<svg viewBox="0 0 24 24"><path d="M7,5A2,2 0 0,1 9,7A2,2 0 0,1 7,9A2,2 0 0,1 5,7A2,2 0 0,1 7,5M7,11A2,2 0 0,1 9,13A2,2 0 0,1 7,15A2,2 0 0,1 5,13A2,2 0 0,1 7,11M7,17A2,2 0 0,1 9,19A2,2 0 0,1 7,21A2,2 0 0,1 5,19A2,2 0 0,1 7,17M10,6H21V8H10V6M10,12H21V14H10V12M10,18H21V20H10V18Z"/></svg>`,
    };

    function addToolButton(name, svg, tooltip, action) {
        const btn = document.createElement('button');
        btn.innerHTML = svg;
        btn.type = 'button';
        btn.setAttribute('data-tooltip', tooltip);
        btn.addEventListener('click', action);
        toolbar.appendChild(btn);
        editor.on('update', () => {
            if (editor.isActive(name)) btn.classList.add('active');
            else btn.classList.remove('active');
        });
    }

    addToolButton('bold', icons.bold, 'Полужирный', () => editor.chain().focus().toggleBold().run());
    addToolButton('italic', icons.italic, 'Курсив', () => editor.chain().focus().toggleItalic().run());
    addToolButton('underline', icons.underline, 'Подчёркнутый', () => editor.chain().focus().toggleUnderline().run());
    addToolButton('heading', icons.heading, 'Заголовок секции', () => editor.chain().focus().toggleHeading({ level: 2 }).run());
    addToolButton('bulletList', icons.bulletList, 'Маркированный список', () => editor.chain().focus().toggleBulletList().run());

    document.getElementById('addSectionBtn').addEventListener('click', () => {
        const content = `<h2>Новая секция</h2><ul><li>новый пункт</li></ul>`;
        const endPos = editor.state.doc.content.size;
        editor.commands.insertContentAt(endPos, content);
        editor.chain().focus().setTextSelection(endPos + 1).run();
    });

    document.getElementById('addListItemBtn').addEventListener('click', () => {
        const doc = editor.state.doc;
        let lastListPos = -1;
        let listNode = null;
        doc.descendants((node, pos) => {
            if (node.type.name === 'bulletList') {
                lastListPos = pos;
                listNode = node;
            }
        });
        if (lastListPos !== -1 && listNode) {
            const insertPos = lastListPos + listNode.nodeSize - 1;
            editor.commands.insertContentAt(insertPos, '<li>новый пункт</li>');
        } else {
            const content = `<h2>Новая секция</h2><ul><li>новый пункт</li></ul>`;
            const endPos = doc.content.size;
            editor.commands.insertContentAt(endPos, content);
        }
    });
}

function initComponents() {
    const starRating = document.getElementById('starRating');
    const starsInput = document.getElementById('stars');

    starRating.addEventListener('click', (e) => {
        if (e.target.classList.contains('star')) {
            const value = parseInt(e.target.getAttribute('data-value'));
            starsInput.value = value;

            document.querySelectorAll('.star').forEach((star, index) => {
                if (index < value) {
                    star.classList.add('active');
                } else {
                    star.classList.remove('active');
                }
            });
        }
    });

    initImageUpload();
    document.getElementById('hotelForm').addEventListener('submit', handleFormSubmit);
}

async function loadHotelData(id) {
    try {
        const response = await fetch(`/api/Hotel/GetHotelById?id=${id}`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        hotelData = await response.json();

        const hotelTags = await loadHotelTags(id);
        hotelData.hotelTags = hotelTags;

        populateFormWithHotelData(hotelData);

        initEditorAndComponents();

    } catch (error) {
        console.error('Ошибка загрузки данных отеля:', error);
        alert('Ошибка загрузки данных отеля');
    }
}

function populateFormWithHotelData(hotel) {
    document.getElementById('name').value = hotel.Name || '';
    document.getElementById('price').value = hotel.Price || '';
    document.getElementById('raiting').value = hotel.Raiting || '';
    document.getElementById('description').value = hotel.Description || '';

    const hotelType = hotel.HotelType ?
        hotel.HotelType.charAt(0).toUpperCase() + hotel.HotelType.slice(1).toLowerCase()
        : 'Отель';

    const hotelTypeRadio = document.querySelector(`input[name="hotelType"][value="${hotelType}"]`);
    if (hotelTypeRadio) {
        hotelTypeRadio.checked = true;
    }

    const nutrition = hotel.Nutrition || 'Без питания';
    const nutritionRadio = document.querySelector(`input[name="nutrition"][value="${nutrition}"]`);
    if (nutritionRadio) {
        nutritionRadio.checked = true;
    }

    if (hotel.Stars > 0) {
        document.getElementById('stars').value = hotel.Stars;
        document.querySelectorAll('.star').forEach((star, index) => {
            if (index < hotel.Stars) {
                star.classList.add('active');
            }
        });
    }

    if (hotel.Images && hotel.Images.length > 0) {
        existingImages = hotel.Images;
        displayExistingImages(hotel.Images);
    }
}

function displayExistingImages(images) {
    const imagePreview = document.getElementById('imagePreview');
    imagePreview.innerHTML = '';

    images.forEach((imageUrl) => {
        const previewItem = document.createElement('div');
        previewItem.className = 'image-preview-item';
        previewItem.setAttribute('data-image-url', imageUrl);

        const img = document.createElement('img');
        img.src = imageUrl;

        const fileNameSpan = document.createElement('span');
        fileNameSpan.className = 'file-name';
        const fileName = fileNameFromUrl(imageUrl);
        fileNameSpan.textContent = fileName;

        const actionBtn = document.createElement('button');
        actionBtn.type = 'button';
        actionBtn.className = 'image-action-btn';

        const setState = () => {
            if (isMarkedForDeletion(imageUrl)) {
                previewItem.classList.add('deleted');
                actionBtn.textContent = 'Restore';
                actionBtn.setAttribute('aria-pressed', 'true');
            } else {
                previewItem.classList.remove('deleted');
                actionBtn.textContent = 'Delete';
                actionBtn.setAttribute('aria-pressed', 'false');
            }
        };

        actionBtn.addEventListener('click', () => {
            if (!isMarkedForDeletion(imageUrl)) {

                markForDeletion(imageUrl);

                const addIndex = imagesToAdd.findIndex(item => item.fileName === fileName);
                if (addIndex !== -1) {
                    imagesToAdd.splice(addIndex, 1);
                    console.log('Removed from imagesToAdd because original was marked deleted:', fileName);
                }

                setState();
            } else {

                unmarkDeletion(imageUrl);
                setState();
            }
        });

        setState();

        previewItem.appendChild(img);
        previewItem.appendChild(fileNameSpan);
        previewItem.appendChild(actionBtn);
        imagePreview.appendChild(previewItem);
    });
}
async function loadHotelTags(id) {
    try {
        const response = await fetch(`/api/Hotel/GetHotelTags?id=${id}`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Ошибка загрузки тегов отеля:', error);
        return [];
    }
}

function initImageUpload() {
    const imageUpload = document.getElementById('imageUpload');
    const imagePreview = document.getElementById('imagePreview');

    imageUpload.addEventListener('change', (e) => {
        const files = e.target.files;

        for (let i = 0; i < files.length; i++) {
            const file = files[i];

            if (!file.type.match('image.*')) continue;

            const fileName = generateSafeFileName(file);
            const expectedUrl = `http://localhost:8888/wwwroot/images/${fileName}`;

            if (isInImagesToAdd(fileName)) {
                console.log('File already queued for upload, skipping:', fileName);
                continue;
            }

            const existingUrl = existingUrlForFileName(fileName);
            if (existingUrl && !isMarkedForDeletion(existingUrl)) {
                console.log('File already present on server and not deleted, skipping:', fileName);
                continue;
            }

            if (existingUrl && isMarkedForDeletion(existingUrl)) {
                unmarkDeletion(existingUrl);
                displayExistingImages(existingImages);
                console.log('Restored previously deleted existing image instead of re-adding:', fileName);
                continue;
            }

            const reader = new FileReader();
            reader.onload = (ev) => {
                const previewItem = document.createElement('div');
                previewItem.className = 'image-preview-item new-upload';
                previewItem.setAttribute('data-file-name', fileName);

                const img = document.createElement('img');
                img.src = ev.target.result;

                const fileNameSpan = document.createElement('span');
                fileNameSpan.className = 'file-name';
                fileNameSpan.textContent = file.name;

                const removeBtn = document.createElement('button');
                removeBtn.type = 'button';
                removeBtn.className = 'image-remove-btn';
                removeBtn.innerHTML = 'Remove';
                removeBtn.addEventListener('click', () => {

                    const addIndex = imagesToAdd.findIndex(item => item.fileName === fileName);
                    if (addIndex !== -1) {
                        imagesToAdd.splice(addIndex, 1);
                    }
                    previewItem.remove();
                });

                previewItem.appendChild(img);
                previewItem.appendChild(fileNameSpan);
                previewItem.appendChild(removeBtn);
                imagePreview.appendChild(previewItem);
            };
            reader.readAsDataURL(file);

            imagesToAdd.push({
                file: file,
                fileName: fileName
            });
            console.log('Queued for upload:', fileName);
        }

        imageUpload.value = '';
    });
}
function generateHash(str) {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
        const char = str.charCodeAt(i);
        hash = ((hash << 5) - hash) + char;
        hash = hash & hash;
    }
    return Math.abs(hash).toString(16).padStart(8, '0');
}
function extractHashInfo(filename) {
    const pattern = /^hotel_([a-f0-9]{8})\.([a-zA-Z0-9]+)$/;
    const match = filename.match(pattern);

    if (match) {
        return {
            isHashed: true,
            hash: match[1],
            extension: match[2]
        };
    } else {
        return {
            isHashed: false,
            hash: null,
            extension: filename.split('.').pop()
        };
    }
}
function generateSafeFileName(file) {
    const filename = file.name;

    const hashInfo = extractHashInfo(filename);

    if (hashInfo.isHashed) {
        return filename;
    }

    const hash = generateHash(filename);
    return `hotel_${hash}.${hashInfo.extension}`;
}

async function loadResorts() {
    try {
        const response = await fetch('/api/Hotel/GetAllResorts');
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const resorts = await response.json();

        const resortSelect = document.getElementById('resortId');
        if (resortSelect) {
            resortSelect.innerHTML = '<option value="">Выберите курорт</option>';
            resorts.forEach(resort => {
                const option = document.createElement('option');
                option.value = resort.resortId;
                option.textContent = resort.resortName;
                resortSelect.appendChild(option);
            });
        }
        return resorts;
    } catch (error) {
        console.error('Ошибка загрузки курортов:', error);
        const resortSelect = document.getElementById('resortId');
        if (resortSelect) {
            resortSelect.innerHTML = '<option value="">Ошибка загрузки</option>';
        }
        return [];
    }
}

async function loadTags() {
    try {
        const response = await fetch('/api/Hotel/GetAllTags');
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const tags = await response.json();

        const tagsContainer = document.getElementById('tagsContainer');
        if (tagsContainer) {
            tagsContainer.innerHTML = '';

            tags.forEach(tag => {
                const label = document.createElement('label');
                label.className = 'amenities-item';

                const input = document.createElement('input');
                input.type = 'checkbox';
                input.value = tag.tagId;
                input.name = 'tags';

                const span = document.createElement('span');
                span.textContent = tag.tagName;

                label.appendChild(input);
                label.appendChild(span);
                tagsContainer.appendChild(label);
            });
        }
        return tags;
    } catch (error) {
        console.error('Ошибка загрузки тегов:', error);
        const tagsContainer = document.getElementById('tagsContainer');
        if (tagsContainer) {
            tagsContainer.innerHTML = '<p>Ошибка загрузки тегов</p>';
        }
        return [];
    }
}

async function handleFormSubmit(e) {
    e.preventDefault();

    if (!editor) {
        alert('Редактор не инициализирован');
        return;
    }

    let newImagePaths = [];
    if (imagesToAdd.length > 0) {
        newImagePaths = await uploadImagesToServer(imagesToAdd);
    }

    const formData = new FormData(e.target);

    let htmlDescription = editor.getHTML();
    htmlDescription = htmlDescription.replace(/<ul>/g, '<ul class="checklist">');
    const generalInfo = document.querySelector('.general-info');
    if (generalInfo) {
        htmlDescription = generalInfo.outerHTML + '\n' + htmlDescription;
    }
    htmlDescription = removeContentEditable(htmlDescription);

    const selectedTags = Array.from(document.querySelectorAll('input[name="tags"]:checked'))
        .map(checkbox => parseInt(checkbox.value));

    const finalImages = [
        ...existingImages.filter(img => !imagesToDelete.includes(img)),
        ...newImagePaths
    ];

    const hotelData = {
        id: parseInt(document.getElementById('hotelId').value),
        resortId: parseInt(formData.get('resortId')),
        name: formData.get('name'),
        hotelType: formData.get('hotelType'),
        price: parseFloat(formData.get('price')),
        stars: parseInt(formData.get('stars')),
        raiting: parseFloat(formData.get('raiting')) || 0,
        nutrition: formData.get('nutrition'),
        description: formData.get('description'),
        htmlDescription: htmlDescription,
        images: finalImages
    };

    const dataToSend = {
        hotel: hotelData,
        hotelTags: selectedTags
    };

    dataToSend.imagesToDelete = imagesToDelete;

    console.log('Отправляемые данные:', dataToSend);

    try {
        const response = await fetch('/api/Admin/UpdateHotel', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataToSend)
        });

        if (response.ok) {
            alert('Отель успешно обновлен!');
            imagesToAdd = [];
            imagesToDelete = [];
            const modal = document.getElementById('modal');
            if (modal) modal.style.display = 'none';
        } else {
            alert('Ошибка при обновлении отеля');
        }
    } catch (error) {
        console.error('Ошибка:', error);
        alert('Ошибка при обновлении отеля');
    }
}

function removeContentEditable(html) {
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, 'text/html');

    const editableElements = doc.querySelectorAll('[contenteditable="true"]');
    editableElements.forEach(el => {
        el.removeAttribute('contenteditable');
    });

    return doc.body.innerHTML;
}

async function uploadImagesToServer(images) {
    const uploadedImagePaths = [];

    for (const imageInfo of images) {
        try {
            const base64String = await readFileAsBase64(imageInfo.file);

            const imageData = {
                fileName: imageInfo.fileName,
                fileData: base64String,
                mimeType: imageInfo.file.type
            };

            const response = await fetch('/api/Admin/UploadHotelImage', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(imageData)
            });

            if (response.ok) {
                uploadedImagePaths.push(`http://localhost:8888/wwwroot/images/${imageInfo.fileName}`);
            } else {
                console.error('Ошибка загрузки изображения:', await response.text());
            }
        } catch (error) {
            console.error('Ошибка сети при загрузке изображения:', error);
        }
    }

    return uploadedImagePaths;
}

function readFileAsBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onload = (e) => {
            const base64 = e.target.result.split(',')[1];
            resolve(base64);
        };

        reader.onerror = (error) => reject(error);
        reader.readAsDataURL(file);
    });
}

function extractAndFillGeneralInfo(html) {
    if (!html) return '';

    const parser = new DOMParser();
    const doc = parser.parseFromString(html, 'text/html');

    const generalInfoFromHtml = doc.querySelector('.general-info');

    if (generalInfoFromHtml) {
        const fields = generalInfoFromHtml.querySelectorAll('.field');

        fields.forEach(fieldFromHtml => {
            const labelElement = fieldFromHtml.querySelector('.field-label');
            if (labelElement) {
                const labelText = labelElement.textContent.trim();
     
                const mainField = findFieldByLabel(labelText);
                if (mainField) {
                    const valueElement = mainField.querySelector('.field-value');
                    const valueFromHtml = fieldFromHtml.querySelector('.field-value');
                    if (valueElement && valueFromHtml) {
                        valueElement.textContent = valueFromHtml.textContent;

                        valueElement.setAttribute('contenteditable', 'true');
                    }
                }
            }
        });

        generalInfoFromHtml.remove();
    }

    return doc.body.innerHTML;
}

function findFieldByLabel(labelText) {
    const mainGeneralInfo = document.querySelector('.general-info');
    if (!mainGeneralInfo) return null;

    const fields = mainGeneralInfo.querySelectorAll('.field');
    for (let field of fields) {
        const labelElement = field.querySelector('.field-label');
        if (labelElement && labelElement.textContent.trim() === labelText) {
            return field;
        }
    }
    return null;
}