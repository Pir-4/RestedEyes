# RestedEyes
Программа - таймер. Отслеживает рабочее время и уведомляет пользователя о начале и конце времени отдыха.
Функционал:
* отображает время запуска программы, т.е. начало работы.
* отображает текущее время (обновляется каждую секунду).
* отображает прошедшее время работы, отсчет начинается с последнего отдыха.
* отображает прошедшее время отдыха.
* оповещает о начале отдыха сообщением, где отображается количество минут отведенное на отдых и сообщение из конфига.
* оповещает о конце отдыха, где призывает начать работать.
* имеется меню для добавиления(удаления) в атозапуск. Можно добавить в автозапуск через реест или добаление ярлыка в директорию Startup.
* имеется кнопка отдых(работы), при нажатии которой идет сигнла, что начался(закончился) отдых.
* имеется checkbox для скрытия всех сообщений.
* можно подгрузить свой конфиг.
* можно сохранить текущий конфиг.
* по умолчанию используется "зашитый" в код конфиг.
[{"Message":"Сделайте гимнастику для глаз","Rest":{"Number":15,"Sign":"m"},"Work":{"Number":1,"Sign":"h"}},{"Message":"Разомнитесь","Rest":{"Number":15,"Sign":"m"},"Work":{"Number":2,"Sign":"h"}},{"Message":"Передохните","Rest":{"Number":2,"Sign":"m"},"Work":{"Number":30,"Sign":"m"}}]

Рекомендации:
Рекомендуется организовывать перерывы на 10/15 минут через каждые 45/60 минут. 
Перерывы увеличиваются на 30% при работе в ночное время (не реализованно).
Так же каждые 2 часа необходимо делать гимнатсику. 
