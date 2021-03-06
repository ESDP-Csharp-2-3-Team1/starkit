#Status of Last Deployment:<br>
<img src="https://github.com/ESDP-Csharp-2-3-Team1/starkit/workflows/.NET Core/badge.svg?branch=master"><br>

# Starkit 
Программное решение **Starkit** предоставляет возможность владельцам ресторанов и пунктов питания быстро создать стандартизированный сайт-визитку и получить мобильное приложение для перевода своего бизнеса в режим онлайн.

# Возможности приложения
* Автоматизация заведения объектов общепита и создание для них сайтов – визиток с представлением на них собственного меню и возможностью удаленного заказа продукции с последующей доставкой.
* Управление акциями заведения типа 1+1=3 или 1+1=1
* Управление бронированием посадочных мест.
* Управление скинами для сайтов-визиток.
* Предоставление мобильной версии для каждого сайта-визитки.
* Интеграция со службой доставки GLOVO
* Управление оплатой через карты банков эмитентов РК.
* Упрпавление заказами клиентов.

# Краткий порядок работы с приложением
* Пользователь входит на наш ресурс и проходит регистрацию (на первоначальном этапе мы выдаем несколько тестовых доменных имен, в дальнейшем будет реализована интеграция с хостер провайдером и реализация приобретения доменных имен для отдельных заведений)
* Пользователь заполняет регистрационные данные своей организации и логотип
* Пользователь входит в зарегистрированный кабинет и настраивает меню своего заведения вбивая данные по блюдам и стоимости, внесенный товар отображается на сайте и в мобильном приложение после публикации.
* Пользователь заполняет действующие акции заведения
* Покупатель, выбирая товар на сайте или в мобильном приложение перемещает его в корзину и оформляет заказ выбирая способ оплаты и доставки.

# Приступая к работе
Инструкция о том, как получить копию этого ПО и запустить его на локальном компьютере с целью разработки и тестирования. Подробную информацию о развертывании ПО в условиях эксплуатации см. в разделе «Развертывание».

## Предварительные условия
Предварительно вам необходимо сделать следующее:
* Скачать и установить <a href="https://visualstudio.microsoft.com/ru/vs/">Visual Studio Community 2019</a>. Предоставляется бесплатно для самостоятельных разработчиков, образовательных учреждений и проектов с открытым кодом, инструкции по установке дополнительных компонентов.
* Скачать и установить <a href="https://www.postgresql.org/download/">сервер БД PostgreSQL</a>. Предоставляется бесплатно для самостоятельных разработчиков, образовательных учреждений и проектов с открытым кодом.
* Скачать и установить <a href="https://www.pgadmin.org/download/pgadmin-4-windows/">Графический клиент pgAdmin</a>

## Установка и настройка окружения
until finished
End with an example of getting some data out of the system or using it for a little demo
Пошаговая инструкция, которая поможет войти в среду разработки.
Примеры
В конце на примере объясните, как извлечь данные из системы.

# Тестирование
Объясните как запустить автоматическое тестирование этой системы.
## Сквозное тестирование
Объясните, что проверяют эти тесты и зачем они нужны.
Пример
## Тестирование стандартов оформления кода
Объясните, что проверяют эти тесты и зачем они нужны.
Пример

# Развертывание
Более подробно расскажите, как развертывать ПО в условиях эксплуатации

# Создано с помощью
Проект разработан на ASP.NET Core на языке C# в среде разработки JetBrains Rider. Для реализации функционала проекта использовались библиотеки:
* Microsoft.AspNetCore.Identity.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Design
* Microsoft.EntityFrameworkCore.Proxies
* Npgsql.EntityFrameworkCore.PostgreSQL
* MailKit

В качестве базы данных используются PostgreSQL

Для реализации интерфейса в проекте также использованы HTML/CSS, язык JavaScript/JQuery и технология AJAX.

# Внесение правок
Здесь должен быть текст.

# Управление версиями
Для управления версиями мы используем GitHub. Информацию о доступных версиях см. в тегах в этом репозитории.

# Авторы
### Состав команды:
* <a href="https://hh.kz/resume/43807f53ff040e36910039ed1f524977745261">Алмат Сейдералы</a>
* <a href="https://hh.kz/resume/e7681825ff084c85aa0039ed1f516c51754b48">Самат Мұрат</a>
* <a href="https://www.facebook.com/samal.zhex">Самал Жексемалиева</a>
* <a href="https://www.instagram.com/rashitnurzhanov/">Рашит Нуржанов</a>

### Наставник:
* Майлюбаев Ернар

### Заказчик:
* Старостенко Константин

# Лицензия
См. условия лицензирования в <a href="https://github.com/ESDP-Csharp-2-3-Team1/starkit/blob/master/LICENSE">LICENSE</a>.

# Благодарности
Команда выражает свои благодарности Майлюбаеву Ернару и Старостенко Константину за вдохновение и мотивацию в реализации проекта.
