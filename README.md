# Deliveries_Sorter
Сортировщик использует документ database.txt, находящийся в директории исполняемого файла программы.
Вид каждой из доставок в этом документе: 
deliveryNumber: {string deliveryNumber}
{
{string deliveryNumber} deliveryWeight: {double deliveryWeight}
{string deliveryNumber} cityDistrict: {string cityDistrict}
{string deliveryNumber} timeToDelivery: {DateTime timeToDelivery}
}
*Для примера, 100 случайных доставок уже загружены в документ database.txt*

Для использования приложения, необходимо запустить исполняемый файл по адресу TestTask_NET_Junior\TestTask_NET_Junior\bin\Debug\net8.0 
При запуске программа предложит выбор среди районов города. При этом регистр букв в названии района при вводе с консоли не учитывается. 
Далее, после выбора района, программа выведет пронумерованный список дат доставок и нужно будет выбрать время одной из доставок, посчитая это время за время первой доставки _firstDeliveryDateTime. Выбор осуществляется путём ввода порядкового номера доставки, выведенной в консоль.
После ввода пользователя произойдет вывод в консоль отсортированного списка доставок по району и времени(ближайшие полчаса от выбранного времени), а так же этот вывод сохранится в документ _deliveryOrder.txt в директории исполняемого файла.
