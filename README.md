# Web Messaging Service - Тестовое задание
## Описание
- Задача: Написать простой web-сервис обмена сообщениями
## Структура
- Сервис состоит из трех компонентов:
  - Web-сервер
  - SQL БД - Postgresql
  - Слой DAL без использования ORM
  - 3 клиента
    - Первый клиент пишет потоком произвольные сообщения на сервер через API
    - Второй клиент читывает по веб-сокету поток сообщений от сервера и отображает их в порядке прихода с сервера
    - Третий клиент позволяет отобразить историю сообщений за последние 10 минут
  - Все три клиентские части реализованы как одно приложение c разделение клиентов по url
- Каждое сообщение состоит из текста до 128 символов, метки даты/времени (устанавливается на сервере) и порядкового номера (приходит от клиента)
- Серверная часть имеет REST API c 2 методами:
  - Отправить сообщение
  - Получить список сообщений за диапазон дат
- Серверная и клиентская части используют ASP.NET Core
- Серверная часть использует EF Core, Npgsql и AutoMapper
- Для написания кода использовалась Visual Studio
## Запуск
- Перед запуском нужно создать базу данных в PostgreSQL и указать строку подключения к ней в переменных среды в формате: `ASPNETCORE_WEBSERVER_CONNECTIONSTRING: Host=your-database-host; Username=your-username; Password=your-password; Database=your-database-name`
- В базе данных запустить файл `web-messaging-service\db\init.sql`
- Запустить сервер и приложение можно через `dotnet run` или непосредственно в Visual Studio