Требования:
  Для запуска вам поднадобится Docker и Docker Compose 
  https://docs.docker.com/get-docker/
  https://docs.docker.com/compose/install/
      
Настройка и установка:
  1) Клонировать репризиторий:
     ```bash
     git clone https://github.com/ogwayz/BookingApi
     cd BookingApi
     ```
  2) Создайте файл .env:
     Скопируйте .env.example в .env:
     ```
      cp .env.example .env
     ```
     Отредактируйте .env, заменив фейковые значения на реальные:
       DB_HOST, DB_PORT, DB_NAME, DB_USER, DB_PASSWORD: Учетные данные и имя базы данных для PostgreSQL


Запуск проекта:
  ```
  cd BookingApi
  docker-compose up --build -d
  ```

Документация

Эндпоинты
1) Регистрация (POST /api/Auth/register)
   Создает нового пользователя по заданным параметрам
   - username (обязательный, string)
   - password (обязательный, string)
2) Авторизация (POST /api/Auth/login)
   Выдает jwt пользователя по заданным данным
   - username (обязательный, string)
   - password (обязательный, string)
3) Получение информации о комнате (GET /api/Room/{id})
   Сообщает данные о комнате с указанным ID
4) Создание комнаты(Только для админа, POST /api/Room)
   Создает комнату по заданным данным:
   - id (обязательный, int)
   - class (обязательный, string)
   - price (обязательный, int)
   - description (обязательный, string)
   - IsAvailable (обязательный, bool)
5) Удаление комнаты(Только для амина, DELETE /api/Room/{id})
   Удаляет комнату с указанным Id
6) Получение информации о бронировании (Get /api/Booking/{id})
   Сообщает данные о бронировании по указанному id
7) Бронирование (POST /api/Booking)
   Создает бронирование, если оно возможно с указанными данными
   - roomID (обязательный, int)
   - startDate (обязательный, DateTime)
   - endDate (обязательный, DateTime)
  

