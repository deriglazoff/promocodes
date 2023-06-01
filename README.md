Такси

Общая архитектура сервисов
![Схема-Страница 16 drawio](https://github.com/deriglazoff/promocodes/assets/99898300/8ee3d453-32bb-409f-bd56-bc43939321e5)

На схеме все взаимодействия между сервисами осуществляются по HTTP кроме взаимодействия с платежной системой, там асинхронно через брокер.

Диаграмма последовательности для водителя
```mermaid
sequenceDiagram
participant customer as Driver
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
    rect GoldenRod
    Note over customer, req:Повторяется с каждым заказом
    customer->>Gateway:В очередь
    Gateway->>req:Прокси
    req->>Сalc:Расчет заказа
    Сalc->>req:Заказ найден
    req->>customer:Заказ найден
    customer->>Gateway:Поездка окончена
    Gateway->>req:Прокси
    end
```
Диаграмма последовательности для клиента
```mermaid
sequenceDiagram
participant customer as Client
participant Gateway
participant auth as Clients
participant req as request
participant calc as Calc
participant pay as Payment
    customer->>Gateway:Авторизуется
    activate customer
    Gateway->>auth:Прокси
    auth->>customer:Успешная авторизация

    deactivate customer
    customer->>Gateway:Создание заказа
    Gateway->>req:Прокси
    req->>calc:Расчет заказа
    calc->>req:Заказ расчитан
    req->>customer:Возвращает тарифы и их стоимость
    customer->>Gateway:Выбирает тариф
    Gateway->>req:Прокси
    req->>calc:Поиск водителя
    calc->>req:Водитель найден
    req->>pay:Блокировка суммы
    req->>customer:Заказ принят, машина в пути
    customer->>req:Заказ завершен
    req->>pay:Списание ДС
```

Stranger Gateway 

|этап   | кол-во участников	критерии отбора  | условия перехода на этап  |План на случаи проблем	   |   
|---|---|---|---|
| 1  | 10% клиентов |регионы отдаленные от центра(магадан, бурятия) |сервис готов, протестирова и задеплоен на прод, получены все зазрешения от ИТ, СБ и прочих	   | Откат изменений, оповещения клиентов о повторной попытке
| 2  | 60% клиентов   |Все регионы, кроме городов миллионников(Москва, Питер)   | Сутки без проблем   |Откат изменений, оповещения клиентов о повторной попытке
| 3  | 60% клиентов   | Все регионы  | Трое суток без проблем  |Откат изменений, оповещения клиентов о повторной попытке
| 4  | Отключение старого функционала   | Неделя без проблем |   |
