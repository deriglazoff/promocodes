# Otus.Teaching.PromoCodeFactory
```mermaid
sequenceDiagram
participant customer as Таксист
participant Gateway
participant auth as Authorization
participant anti as Antiford
participant req as request
  customer->>Gateway:Авторизуется
  activate customer
  Gateway->>auth:Прокси
  auth->>customer:Успешная авторизация
  deactivate customer
  activate customer
  customer->>Gateway:Фото автомобиля
  Gateway->>anti:Прокси
  anti->>customer:Успех
  deactivate customer
    rect GoldenRod
    Note over customer, req:Повторяется с каждым заказом
    customer->>Gateway:В очередь
    Gateway->>req:Прокси
    req->>customer:Заказ найден
    customer->>Gateway:Поездка окончена
    Gateway->>req:Прокси
    end
```
Проект для домашних заданий и демо по курсу `C# ASP.NET Core Разработчик` от `Отус`.
Cистема `Promocode Factory` для выдачи промокодов партнеров для клиентов по группам предпочтений.

cicd src/Otus.Teaching.PromoCodeFactory.sln

Подробное описание проекта можно найти в [Wiki](https://gitlab.com/devgrav/otus.teaching.promocodefactory/-/wikis/Home)

Описание домашнего задания в [Homework Wiki](https://gitlab.com/devgrav/otus.teaching.promocodefactory/-/wikis/Homework-1)
