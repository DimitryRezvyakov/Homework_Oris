import { Editor } from 'https://esm.sh/@tiptap/core@2.5.0';
import StarterKit from 'https://esm.sh/@tiptap/starter-kit@2.5.0';
import Underline from 'https://esm.sh/@tiptap/extension-underline@2.5.0';

let editor = null;

window.initHotelForm = () => {

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
        content: `
        <h2>Пляж</h2>
        <ul><li>городской галечный пляж (500 м от отеля)</li><li>шезлонги (платно)</li><li>зонтики (платно)</li></ul>
        <h2>Территория отеля</h2>
        <ul><li>1 открытый бассейн, 60 кв. м</li></ul>
        `,
        editable: true,
    });

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

    const addSectionBtn = document.getElementById('addSectionBtn');
    const addListItemBtn = document.getElementById('addListItemBtn');

    if (addSectionBtn) {
        addSectionBtn.addEventListener('click', () => {
            const content = `<h2>Новая секция</h2><ul><li>новый пункт</li></ul>`;
            const endPos = editor.state.doc.content.size;
            editor.commands.insertContentAt(endPos, content);
            editor.chain().focus().setTextSelection(endPos + 1).run();
        });
    }

    if (addListItemBtn) {
        addListItemBtn.addEventListener('click', () => {
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

    const starRating = document.getElementById('starRating');
    const starsInput = document.getElementById('stars');
    window.uploadedImages = initImageUpload();

    if (starRating && starsInput) {
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
    }

    const imageUpload = document.getElementById('imageUpload');
    const imagePreview = document.getElementById('imagePreview');

    if (imageUpload && imagePreview) {
        imageUpload.addEventListener('change', (e) => {
            const files = e.target.files;

            for (let i = 0; i < files.length; i++) {
                const file = files[i];

                if (file.type.match('image.*')) {
                    const reader = new FileReader();

                    reader.onload = (e) => {
                        const previewItem = document.createElement('div');
                        previewItem.className = 'image-preview-item';

                        const img = document.createElement('img');
                        img.src = e.target.result;

                        const removeBtn = document.createElement('div');
                        removeBtn.className = 'remove';
                        removeBtn.innerHTML = '×';
                        removeBtn.addEventListener('click', () => {
                            previewItem.remove();
                        });

                        previewItem.appendChild(img);
                        previewItem.appendChild(removeBtn);
                        imagePreview.appendChild(previewItem);
                    };

                    reader.readAsDataURL(file);
                }
            }
        });
    }

    const hotelForm = document.getElementById('hotelForm');
    if (hotelForm) {
        hotelForm.addEventListener('submit', handleFormSubmit);
    }

    const cancelBtn = document.getElementById('cancelBtn');
    if (cancelBtn) {
        cancelBtn.addEventListener('click', handleCancel);
    }

    loadResorts();
    loadTags();
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

async function handleFormSubmit(e) {
    e.preventDefault();

    if (!editor) {
        alert('Редактор не инициализирован');
        return;
    }

    const uploadedImages = window.uploadedImages || [];

    const imagePaths = await uploadImagesToServer(uploadedImages);

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

    const hotelData = {
        id: 0,
        resortId: parseInt(formData.get('resortId')),
        name: formData.get('name'),
        hotelType: formData.get('hotelType'),
        price: parseFloat(formData.get('price')),
        stars: parseInt(formData.get('stars')),
        raiting: parseFloat(formData.get('raiting')) || 0,
        nutrition: formData.get('nutrition'),
        description: formData.get('description'),
        htmlDescription: htmlDescription,
        images: imagePaths
    };

    const dataToSend = {
        hotel: hotelData,
        hotelTags: selectedTags
    };

    console.log('Отправляемые данные:', dataToSend);

    try {
        const response = await fetch('/api/Admin/CreateHotel', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataToSend)
        });

        if (response.ok) {
            alert('Отель успешно добавлен!');
            resetForm();
        } else {
            alert('Ошибка при добавлении отеля');
        }
    } catch (error) {
        console.error('Ошибка:', error);
        alert('Ошибка при добавлении отеля');
    }
}

function handleCancel() {
    if (confirm('Вы уверены, что хотите отменить создание отеля?')) {
        resetForm();
    }
}

function resetForm() {
    const hotelForm = document.getElementById('hotelForm');
    if (hotelForm) {
        hotelForm.reset();
    }
    if (editor) {
        editor.commands.setContent('');
    }
    document.querySelectorAll('.star').forEach(star => star.classList.remove('active'));
    const starsInput = document.getElementById('stars');
    if (starsInput) {
        starsInput.value = '0';
    }
    const imagePreview = document.getElementById('imagePreview');
    if (imagePreview) {
        imagePreview.innerHTML = '';
    }
    if (window.uploadedImages) {
        window.uploadedImages.length = 0;
    }
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
    } catch (error) {
        console.error('Ошибка загрузки курортов:', error);
        const resortSelect = document.getElementById('resortId');
        if (resortSelect) {
            resortSelect.innerHTML = '<option value="">Ошибка загрузки</option>';
        }
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
    } catch (error) {
        console.error('Ошибка загрузки тегов:', error);
        const tagsContainer = document.getElementById('tagsContainer');
        if (tagsContainer) {
            tagsContainer.innerHTML = '<p>Ошибка загрузки тегов</p>';
        }
    }
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

function initImageUpload() {
    const imageUpload = document.getElementById('imageUpload');
    const imagePreview = document.getElementById('imagePreview');
    const uploadedImages = [];

    if (imageUpload && imagePreview) {
        imageUpload.addEventListener('change', async (e) => {
            const files = e.target.files;

            for (let i = 0; i < files.length; i++) {
                const file = files[i];

                if (file.type.match('image.*')) {
                    try {
                        const safeFileName = generateSafeFileName(file);

                        const reader = new FileReader();

                        reader.onload = (e) => {
                            const previewItem = document.createElement('div');
                            previewItem.className = 'image-preview-item';
                            previewItem.setAttribute('data-filename', safeFileName);

                            const img = document.createElement('img');
                            img.src = e.target.result;

                            const fileNameSpan = document.createElement('span');
                            fileNameSpan.className = 'file-name';
                            fileNameSpan.textContent = safeFileName;

                            const removeBtn = document.createElement('div');
                            removeBtn.className = 'remove';
                            removeBtn.innerHTML = '×';
                            removeBtn.addEventListener('click', () => {
    
                                const index = uploadedImages.findIndex(img => img.fileName === safeFileName);
                                if (index !== -1) {
                                    uploadedImages.splice(index, 1);
                                }
                                previewItem.remove();
                            });

                            previewItem.appendChild(img);
                            previewItem.appendChild(fileNameSpan);
                            previewItem.appendChild(removeBtn);
                            imagePreview.appendChild(previewItem);
                        };

                        reader.readAsDataURL(file);


                        uploadedImages.push({
                            file: file,
                            fileName: safeFileName
                        });

                    } catch (error) {
                        console.error('Ошибка при обработке файла:', error);
                        alert(`Ошибка при обработке файла: ${file.name}`);
                    }
                }
            }

            imageUpload.value = '';
        });
    }

    return uploadedImages;
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