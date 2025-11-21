/*



Creating database



*/

CREATE TABLE "Country" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "IconUri" TEXT
);

CREATE TABLE "Resort" (
    "Id" SERIAL PRIMARY KEY,
    "CountryId" INTEGER NOT NULL,
    "Name" VARCHAR(50) NOT NULL,
    "HasAirport" BOOLEAN NOT NULL DEFAULT FALSE,

    CONSTRAINT "FK_Resort_Country" FOREIGN KEY ("CountryId") REFERENCES "Country"("Id") ON DELETE CASCADE
);

CREATE TABLE "HotelTag" (
    "Id" SERIAL PRIMARY KEY,
    "Type" TEXT NOT NULL,
    "Name" TEXT NOT NULL
);

CREATE TABLE "Hotel" (
    "Id" SERIAL PRIMARY KEY,
    "ResortId" INTEGER NOT NULL,
    "Name" VARCHAR(50) NOT NULL,
    "HotelType" TEXT NOT NULL,
    "Price" NUMERIC(18,2) NOT NULL,
    "Stars" INTEGER NOT NULL CHECK ("Stars" BETWEEN 1 AND 5),
    "Raiting" NUMERIC(2,1) NOT NULL CHECK ("Raiting" >= 0 AND "Raiting" <= 5),
    "Nutrition" TEXT,
    "Description" VARCHAR(200),
    "HtmlDescription" TEXT,
    "Images" JSONB,

    CONSTRAINT "FK_Hotel_Resort" FOREIGN KEY ("ResortId") REFERENCES "Resort"("Id") ON DELETE CASCADE
);

CREATE TABLE "HotelTagTable" (
    "HotelId" INTEGER NOT NULL,
    "TagId" INTEGER NOT NULL,

    CONSTRAINT "PK_HotelTagTable" PRIMARY KEY ("HotelId", "TagId"),
    CONSTRAINT "FK_HotelTagTable_Hotel" FOREIGN KEY ("HotelId") REFERENCES "Hotel"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_HotelTagTable_HotelTag" FOREIGN KEY ("TagId") REFERENCES "HotelTag"("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Hotel_ResortId" ON "Hotel" ("ResortId");

CREATE INDEX "IX_Resort_CountryId" ON "Resort" ("CountryId");

CREATE INDEX "IX_HotelTagTable_TagId" ON "HotelTagTable" ("TagId");

/*


filling database


*/

INSERT INTO "Country" ("Name", "IconUri") VALUES
('Россия', 'https://www.shutterstock.com/ru/image-photo/russia-tricolor-flag-illustration-suitable-banner-2127607031'),
('Турция', 'https://www.shutterstock.com/ru/image-photo/flag-turkey-1153566256'),
('Испания', 'https://www.google.com/imgres?q=%D1%84%D0%BB%D0%B0%D0%B3%20%D0%B8%D1%81%D0%BF%D0%B0%D0%BD%D0%B8%D0%B8&imgurl=https%3A%2F%2Fimg.freepik.com%2Fpremium-vector%2Fflat-illustration-spain-national-flag_1118204-811.jpg&imgrefurl=https%3A%2F%2Fru.freepik.com%2Fpremium-vector%2Fspain-national-flag-waving-realistic-vector-flag-spain_30656666.htm&docid=Rt6-7ykbrwF_KM&tbnid=kjicrS5o9Y3UcM&vet=12ahUKEwiIvdD4mvKQAxXB8LsIHb-IIXAQM3oECDMQAA..i&w=626&h=418&hcb=2&ved=2ahUKEwiIvdD4mvKQAxXB8LsIHb-IIXAQM3oECDMQAA'),
('Греция', 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAPoAAACnCAMAAAAPIrEmAAAANlBMVEUNXq////9woNAocLhmmcwxdrtdk8mCrNYWZLJ4pdIfarX2+fyUuNydvt/7/P47fb7Q3++/1epYl/f/AAABHUlEQVR4nO3ay24CMRBFwQGSQML7/3+WLWQ2jaKRO3Pr7FtyWV61PE2VjpvfnUpzKwgdHT0jdHT0jNDR0TNCR0fPCP1d+nXfuUXp29lYp9DR0dHR0V9DR0dHR0dvGzo6egj9WGk+d6mMnQ+dm5a813PtTY0KHR0dHR0dHR0dHR0dHb1twfRTpctMdauM7b87V7ufde7m0NHR0dHRX0JHR0dHR28bOjo6+p/ph5/OLUpfQ+jo6Bmho6NnhI6OnhE6Ovq/b1tp/o3oXppr3aKfx3qHnhh6YuiJoSeGnhh6YuiJoSeWTN/FNno1KEmSJEmS9NxHbMm7udEHGBd6YuiJoSeGnhh6YuiJoSeGntj0Gdvo1aAkSZIkSdJzX7E9AIqre1tJkARUAAAAAElFTkSuQmCC'),
('Италия', 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAPoAAACnCAMAAAAPIrEmAAAAHlBMVEUAkkbOKzf///+r28LvuLxUtoPLaWvH59ag1rrIvbO2lRRDAAAAzUlEQVR4nO3PyRWAIBAFsGERpf+G6YH3vSUdpCpv9LjZ8n6Yq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq6urq1/Vn7zvjdsr7wCzuKqVC083sAAAAABJRU5ErkJggg=='),
('Франция', 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAPoAAACnCAMAAAAPIrEmAAAAGFBMVEXOESb///8AJlTniJPeX230y89QXHzplJ7TPNm/AAAAyElEQVR4nO3PSQ0AIBAEMG78O8bEbMKjddA2cnrcbJXU1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXV1dXVP6mfnB13V6EHHf2dNlH3uyUAAAAASUVORK5CYII='),
('Хорватия', 'data:image/png;base64,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'),
('Болгария', 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASIAAACuCAMAAAClZfCTAAAAElBMVEX////WJhIAlm4AjF8AnHPgFQC7K9XWAAAA+ElEQVR4nO3QMQGAMAAEsYeCf8tIuI0pkZANAAAAAAAAAAAAAAAAAAAAgB8dwm6CoqQoKUqKkqKkKClKipKipCgpSoqSoqQoKUqKkqKkKClKipKipCgpSoqSoqQoKUqKkqKkKClKipKipCgpSoqSoqQoKUqKkqKkKClKipKipCgpSoqSoqQoKUqKkqKkKClKewh7CbsIipKipCgpSoqSoqQoKUqKkqKkKClKipKipCgpSoqSoqQoKUqKkqKkKClKipKipCgpSoqSoqQoKUqKkqKkKClKipKipCgpSoqSoqQoKUqKkqKkKClKipKipCgpSoqSoqQoKUofMGTNC8HkSxoAAAAASUVORK5CYII='),
('Египет', 'data:image/png;base64,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'),
('Мексика', 'data:image/png;base64,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');

WITH resort_data AS (
    SELECT 
        ROW_NUMBER() OVER () AS rn,
        name,
        has_airport
    FROM (VALUES
        ('Сочи', true),
        ('Красная Поляна', false),
        ('Анталья', true),
        ('Кемер', false),
        ('Барселона', true),
        ('Ибица', false),
        ('Афины', true),
        ('Санторини', false),
        ('Рим', true),
        ('Милан', false),
        ('Париж', true),
        ('Ницца', false),
        ('Дубровник', true),
        ('Загреб', false),
        ('София', true),
        ('Пампорово', false),
        ('Шарм-эль-Шейх', true),
        ('Хургада', false),
        ('Канкун', true),
        ('Пуэрто-Вальярта', false)
    ) AS t(name, has_airport)
),
country_with_index AS (
    SELECT 
        "Id" AS country_id,
        ROW_NUMBER() OVER () AS cn
    FROM "Country"
    ORDER BY "Id"
)
INSERT INTO "Resort" ("CountryId", "Name", "HasAirport")
SELECT 
    c.country_id,
    r.name,
    r.has_airport
FROM country_with_index c
JOIN resort_data r ON ((r.rn - 1) / 2 + 1) = c.cn;

INSERT INTO "HotelTag" ("Type", "Name") VALUES
-- Пляж
('Пляж', 'Галечный пляж'),
('Пляж', 'Первая береговая линия'),
('Пляж', 'Собственный пляж'),
('Пляж', 'Песчаный пляж'),

-- Территория
('Территория', 'Футбол'),
('Территория', 'Бассейн'),
('Территория', 'Новый отель'),
('Территория', 'Бассейн с подогревом'),
('Территория', 'Водные горки'),
('Территория', 'СПА-центр'),

-- Услуги
('Услуги', 'Анимация'),
('Услуги', 'Дискотека'),
('Услуги', 'Wi-Fi'),
('Услуги', 'Размещение одиноких мужчин'),
('Услуги', 'Только для взрослых'),

-- Номер
('Номер', 'Кухня в номере'),
('Номер', 'Балкон в номере'),
('Номер', 'Wi-Fi в номере'),
('Номер', 'Кондиционер'),
('Номер', 'Размещение с животными');

DO $$
DECLARE
    resort_id INT;
    hotel_name TEXT;
    hotel_type TEXT;
    price NUMERIC;
    stars INT;
    raiting NUMERIC;
    nutrition TEXT;
    description TEXT;
    html_description TEXT;
    images JSONB;
    i INT := 1;
BEGIN

    FOR resort_id IN SELECT "Id" FROM "Resort" ORDER BY "Id" LOOP

        FOR j IN 1..CASE WHEN i % 3 = 0 THEN 3 ELSE 2 END LOOP

            hotel_name := 'Отель ' || chr(65 + (i % 26)) || chr(65 + ((i / 26) % 26)) || ' ' || i;


            hotel_type := CASE (i % 3)
                WHEN 0 THEN 'отель'
                WHEN 1 THEN 'вилла'
                ELSE 'хостел'
            END;


            price := ROUND((RANDOM() * 19000 + 1000)::NUMERIC, 2);

			stars := FLOOR(RANDOM() * 5 + 1)::INT;

            raiting := ROUND((RANDOM() * 5)::NUMERIC, 1);

            nutrition := CASE (i % 6)
                WHEN 0 THEN 'AI'
                WHEN 1 THEN 'UAI'
                WHEN 2 THEN 'FB'
                WHEN 3 THEN 'HB'
                WHEN 4 THEN 'BB'
                ELSE 'Без питания'
            END;

            description := 'Отличный отель в ' || (SELECT "Name" FROM "Resort" WHERE "Id" = resort_id) || '. Идеально подходит для отдыха с семьёй или компанией друзей.';

           html_description := '<div class="general-info">
    <div class="field"><div class="field-label">Основан:</div><div class="field-value">' || (2000 - (i % 30))::TEXT || '</div></div>
    <div class="field"><div class="field-label">Последняя реновация:</div><div class="field-value">' || (2020 - (i % 5))::TEXT || '</div></div>
    <div class="field"><div class="field-label">Расположение:</div><div class="field-value">' || 
        (FLOOR(RANDOM() * 51)::INT)::TEXT || ' км от аэропорта, ' || 
        (FLOOR(RANDOM() * 501)::INT)::TEXT || ' м от центра.</div></div>
    <div class="field"><div class="field-label">Площадь:</div><div class="field-value">' || 
        (FLOOR(RANDOM() * 10001)::INT)::TEXT || ' кв м</div></div>
    <div class="field"><div class="field-label">Телефон:</div><div class="field-value">+' || (90 + (i % 10))::TEXT || ' 242 814 61 06</div></div>
    <div class="field"><div class="field-label">Сайт:</div><div class="field-value">www.' || LOWER(REPLACE(hotel_name, ' ', '')) || '.com</div></div>
    <div class="field"><div class="field-label">Адрес:</div><div class="field-value">Merkez mahallesi, Kavaklı cad.no:' || (i % 20 + 1)::TEXT || ', ' || (SELECT "Name" FROM "Resort" WHERE "Id" = resort_id) || '</div></div>
  </div>
<h2>Пляж</h2><ul class="checklist"><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Пляж' ORDER BY RANDOM() LIMIT 1) || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Пляж' ORDER BY RANDOM() LIMIT 1) || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Пляж' ORDER BY RANDOM() LIMIT 1) || '</p></li></ul><h2>Территория отеля</h2><ul class="checklist"><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Территория' ORDER BY RANDOM() LIMIT 1) || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Территория' ORDER BY RANDOM() LIMIT 1) || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Территория' ORDER BY RANDOM() LIMIT 1) || '</p></li></ul><h2>Питание</h2><ul class="checklist"><li><p>' || nutrition || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Услуги' ORDER BY RANDOM() LIMIT 1) || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Услуги' ORDER BY RANDOM() LIMIT 1) || '</p></li></ul><h2>В номере</h2><ul class="checklist"><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Номер' ORDER BY RANDOM() LIMIT 1) || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Номер' ORDER BY RANDOM() LIMIT 1) || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Номер' ORDER BY RANDOM() LIMIT 1) || '</p></li></ul><h2>Услуги</h2><ul class="checklist"><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Услуги' ORDER BY RANDOM() LIMIT 1) || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Услуги' ORDER BY RANDOM() LIMIT 1) || '</p></li><li><p>' || (SELECT "Name" FROM "HotelTag" WHERE "Type" = 'Услуги' ORDER BY RANDOM() LIMIT 1) || '</p></li></ul><h2>Для детей</h2><ul class="checklist"><li><p>краватка, по запросу</p></li><li><p>детский бассейн</p></li><li><p>лягушатник</p></li></ul><h2>Примечание</h2><ul class="checklist"><li><p>Администрация отеля оставляет за собой право вносить любые изменения в концепцию отеля, в том числе о наборе платных/бесплатных услуг без предварительного уведомления. Мы просим предварительно уточнять интересующую Вас информацию.</p></li></ul>';

            images := jsonb_build_array(
                'https://images.pexels.com/photos/338504/pexels-photo-338504.jpeg?auto=compress&cs=tinysrgb&w=600',
                'https://images.pexels.com/photos/1458457/pexels-photo-1458457.jpeg',
                'https://images.pexels.com/photos/258154/pexels-photo-258154.jpeg',
		'https://images.pexels.com/photos/261169/pexels-photo-261169.jpeg',
		'https://images.pexels.com/photos/774042/pexels-photo-774042.jpeg',
		'https://images.pexels.com/photos/161758/governor-s-mansion-montgomery-alabama-grand-staircase-161758.jpeg',
		'https://images.pexels.com/photos/2245411/pexels-photo-2245411.jpeg',
		'https://images.pexels.com/photos/933337/pexels-photo-933337.jpeg',
		'https://images.pexels.com/photos/2986231/pexels-photo-2986231.jpeg',
		'https://images.pexels.com/photos/1628086/pexels-photo-1628086.jpeg'
            );

            INSERT INTO "Hotel" (
                "ResortId", "Name", "HotelType", "Price", "Stars", "Raiting", "Nutrition", 
                "Description", "HtmlDescription", "Images"
            ) VALUES (
                resort_id, hotel_name, hotel_type, price, stars, raiting, nutrition, 
                description, html_description, images
            );

            i := i + 1;
            EXIT WHEN i > 50;
        END LOOP;
        EXIT WHEN i > 50;
    END LOOP;
END $$;

DO $$
DECLARE
    hotel_id INT;
    tag_id INT;
    tag_count INT;
    i INT := 1;
BEGIN
    FOR hotel_id IN SELECT "Id" FROM "Hotel" ORDER BY "Id" LOOP

        tag_count := FLOOR(RANDOM() * 3 + 3)::INT;

        FOR j IN 1..tag_count LOOP

            SELECT "Id" INTO tag_id FROM "HotelTag" ORDER BY RANDOM() LIMIT 1;


            INSERT INTO "HotelTagTable" ("HotelId", "TagId")
            VALUES (hotel_id, tag_id)
            ON CONFLICT DO NOTHING;
        END LOOP;
    END LOOP;
END $$;