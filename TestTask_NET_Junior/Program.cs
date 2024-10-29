using static TestTask_NET_Junior.Deliveries;

namespace TestTask
{
    class Program
    {
#nullable disable
        static void Main()
        {
            List<string> allDistricts = GetAllDistricts();

        DistrictChoosing:

            Console.WriteLine($"Какой район вас интересует?");
            foreach (string district in allDistricts)
            {
                Console.WriteLine(district);
            }
            string districtChoise = Console.ReadLine().ToUpper();

            Dictionary<int, DateTime> datesOfDeliveries = GetAllDates(districtChoise);

            Console.WriteLine($"Введите время первой доставки");
            int datecounter = 1;
            foreach (DateTime date in datesOfDeliveries.Values)
            {
                Console.WriteLine($"{datecounter}: {date}");
                datecounter++;
            }
            int _firstDeliveryDateTime = Convert.ToInt32(Console.ReadLine());
            if (datesOfDeliveries.ContainsKey(_firstDeliveryDateTime))
            {

                if (allDistricts.Contains(districtChoise))
                {
                    Sort(districtChoise, _firstDeliveryDateTime);
                    goto DistrictChoosing;
                }
                else
                {
                    Console.WriteLine("Неверный формат района.");
                    goto DistrictChoosing;
                }
            }
            else if (!datesOfDeliveries.ContainsKey(_firstDeliveryDateTime) || _firstDeliveryDateTime <= 0)
            {
                Console.WriteLine("Выберите верный номер.");
                goto DistrictChoosing;
            }
        }

    }
}    
    

