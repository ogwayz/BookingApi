Требования:
  Для запуска вам поднадобится Docker и Docker Compose 
  https://docs.docker.com/get-docker/
  https://docs.docker.com/compose/install/
      
Настройка и установка:
  1) Клонировать репризиторий:
     ```bash
     git clone https://github.com/fcvf-jmail/TicketsToSky
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
