# ClockAndAlarm
Digital and Arrow Clock with simple alarm.
Это простая реализация электронных и аналоговых часов с возможностью установки 1 будильника. Программа на старте запрашивает точное время по API с двух серверов, один с открытым ключом, второй с API Key. 
Если разница во времени с 2 серверов более 1 минуты - выдается сообщение об ошибке. Далее программа повторяет запрос на оба сервера каждый час. Есть возможность установить 1 будильник, нажатием на кнопку "Set alarm".
После нажатия на кнопку аналоговые часы выключаются и пользователь может путем перетягивания стрелок выставить нужное время (Переключение AM/PM происходит автоматически при полном обороте часовой стрелки). 
Цифровые часы во время установки времени будильника продолжают показывать текущее время для того чтобы пользователь мог ориентироваться. Так же можно ввести время будильника вручную в 3 специальных поля, 
которые появляются вверху экрана. В эти же окна дублируется информация со стрелок, если их перетягивают. После нажатия на кнопку "Confirm" будет установлен будильник. При срабатывании будильника выскочит PopUp окно, 
которое можно закрыть кнопкой. Программа сверстана под корректное portrait и landscape оторбражение.
