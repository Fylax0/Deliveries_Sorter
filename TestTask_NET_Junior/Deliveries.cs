using System.Text.RegularExpressions;

namespace TestTask_NET_Junior
{
    class Deliveries
    {
        /// <summary>
        /// Исходная база данных(По умолчанию, находится в директории исполняемого файла программы.)
        /// </summary>
        public const string _sourceDataBase = "database.txt";

        /// <summary>
        /// Получение данных одной доставки из файла database.txt
        /// </summary>
        /// <param name="deliveryNumber">Порядковый номер доставки, по которому осуществляется поиск всех данных доставки.</param>
        /// <returns>Возвращает DeliveryData, содержащий параметры этой доставки.</returns>
        public static DeliveryData Get(string deliveryNumber)
        {
            string text = File.ReadAllText(_sourceDataBase);
            DeliveryData info = new DeliveryData()
            {
                deliveryNumber = deliveryNumber,
                deliveryWeight = Convert.ToDouble(Regex.Match(text, @$"(?<={deliveryNumber} deliveryWeight: )[0-9]+\,[0-9]+(?=)").Value),
                cityDistrict = Regex.Match(text, @$"(?<={deliveryNumber} cityDistrict: )[D-D1-4]+(?=)").Value,
                timeToDelivery = Convert.ToDateTime(Regex.Match(text, @$"(?<={deliveryNumber} timeToDelivery: )[0-9. :]+(?=)").Value),
            };
            return info;
        }

        /// <summary>
        /// Получение данных всех доставок из базы database.txt.
        /// </summary>
        /// <returns>Возвращает Dictionary<string, DeliveryData>, где string - порядковый номер доставки, а DeliveryData - набор параметров этой доставки.</returns>
        public static Dictionary<string, DeliveryData> GetAll()
        {
            Dictionary<string, DeliveryData> _deliveries = new Dictionary<string, DeliveryData>();
            string text = File.ReadAllText(_sourceDataBase);
            foreach (Match item in Regex.Matches(text, "(?<=deliveryNumber: )[A-Za-z0-9]+(?=)"))
            {
                string _deliveryNumber = item.Value;
                if (!_deliveries.ContainsKey(_deliveryNumber))
                {
                    _deliveries.Add(_deliveryNumber, Get(_deliveryNumber));
                }
            }
            return _deliveries;
        }

        /// <summary>
        /// Получение дат всех доставок по выбранному району
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, DateTime> GetAllDates(string _cityDistrict)
        {
            int datesCounter = 1;
            Dictionary<string, DeliveryData> allDeliveries = GetAll();
            Dictionary<int, DateTime> dates = new Dictionary<int, DateTime>();
            foreach (DeliveryData val in allDeliveries.Values)
            {
                if (val.cityDistrict != _cityDistrict)
                {
                    allDeliveries.Remove(val.deliveryNumber);
                }
            }
            foreach (DeliveryData value in allDeliveries.Values) 
            {
                dates.Add(datesCounter, value.timeToDelivery);
                datesCounter++;
            }
            return dates;
        }

        /// <summary>
        /// Получение наименований всех существующих районов
        /// </summary>
        /// <returns>List<string></returns>
        public static List<string> GetAllDistricts()
        {
            Dictionary<string, DeliveryData> allDeliveries = GetAll();
            List<string> districts = new List<string>(allDeliveries.Count);
            foreach(DeliveryData value in allDeliveries.Values)
            {
                if (!districts.Contains(value.cityDistrict))
                {
                    districts.Add(value.cityDistrict);
                }
            }
            return districts;
        }

        /// <summary>
        /// Сортировка по району города и времени(ближайшие полчаса от введённог полтзователем времени). Возвращает отсортированный список доставок вместе с их параметрами в консоль, а так же записывает этот список в _deliveryOrder.txt в директории исполняемого файла.
        /// </summary>
        /// <param name="_cityDistrict">Район города, выбираемый пользователем.</param>
        public static void Sort(string _cityDistrict, int deliveryDate)
        {
            Dictionary<string, DeliveryData> sorted = GetAll();
            Dictionary<int, DateTime> dates = GetAllDates(_cityDistrict);
            DateTime _firstDeliveryDateTime = DateTime.Now;
            foreach (int chosenDate in dates.Keys)
            {
                if(chosenDate == deliveryDate)
                {
                    _firstDeliveryDateTime = dates[chosenDate];
                } 
            }
            foreach (DeliveryData val in sorted.Values)
            {
                if (val.cityDistrict != _cityDistrict)
                {
                    sorted.Remove(val.deliveryNumber);
                }
            }
            foreach (DeliveryData val in sorted.Values)
            {
                if (val.timeToDelivery > _firstDeliveryDateTime.AddMinutes(30) || val.timeToDelivery < _firstDeliveryDateTime)
                {
                    sorted.Remove(val.deliveryNumber);
                }
            }
            sorted = sorted.OrderBy(sorted => sorted.Value.timeToDelivery).ToDictionary(sorted => sorted.Key, sorted => sorted.Value);

            if (sorted.Count > 0)
            {
                string _deliveryOrder = $"Сортировка по району {_cityDistrict} и времени {_firstDeliveryDateTime}:\n\n";
                foreach (var data in sorted)
                {
                    _deliveryOrder += $"Номер заказа: {data.Value.deliveryNumber}\nВес заказа: {data.Value.deliveryWeight}\nРайон доставки заказа: {data.Value.cityDistrict}\nВремя доставки заказа: {data.Value.timeToDelivery}\n\n";
                }
                _deliveryOrder += $"Сортировка по району {_cityDistrict} и времени {_firstDeliveryDateTime} окончена.\n\n";
                File.AppendAllText("_deliveryOrder.txt", _deliveryOrder);
                Console.WriteLine($"{_deliveryOrder}\nОтсортированный список доставок по выбранному району и времени записан в _deliveryOrder.txt в директории программы.\n\n");
            }
            else
            {
                Console.WriteLine("\nВ ближайшие полчаса после этого времени доставок в этом районе нет.\n");
            }
        }
    }
    /// <summary>
    /// Исходные параметры для каждой доставки:
    /// deliveryNumber - порядковый номер доставки; 
    /// deliveryWeight - вес посылки; 
    /// cityDistrict - район города, в который производится доставка; 
    /// timeToDelivery - время, в которое посылка должна быть доставлена.
    /// </summary>
    class DeliveryData()
    {
        public string deliveryNumber = "";
        public double deliveryWeight = 0;
        public string cityDistrict = "";
        public DateTime timeToDelivery = new DateTime();
    }
}
