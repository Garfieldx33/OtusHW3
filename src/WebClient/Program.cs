using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    static class Program
    {
        static string _url = "https://localhost:5001/customers";
        static bool _isNeedToExit = false;

        static List<string> _namesList= new List<string> 
        {
            "Дмитрий",
            "Сергей",
            "Алексей",
            "Федор",
            "Андрей",
            "Иван",
            "Кирилл",
            "Иван",
            "Евгений",
            "Глеб"

        };

        static List<string> _surnamesList = new List<string>
        {
            "Колобков",
            "Казипопов",
            "Драчунов",
            "Верхнеполкин",
            "Околобанный",
            "Шпак",
            "Эмобоев",
            "Шматко",
            "Тиабалдин",
            "Зазаборный"

        };
        static async Task Main(string[] args)
        {
            while (!_isNeedToExit)
            {
                Console.WriteLine("Что вы хотите сделать?");
                Console.WriteLine("1 - вывод информации о клиенте по идентификатору");
                Console.WriteLine("2 - добавление нового клиента");
                Console.WriteLine("3 - выйти");

                var key = Console.ReadLine();
                switch (key)
                {
                    case ("1"):
                        Console.WriteLine($"Введите Id пользвателя");
                        long id = long.Parse(Console.ReadLine());
                        await GetCustomerById(id);
                        break;
                    case ("2"):
                        PostRandomCustomer();
                        break;
                    case ("3"):
                        _isNeedToExit = true;
                        break;
                    default:
                        Console.WriteLine("Неопознанная команда");
                        break;
                }
            }
            Console.ReadKey();
        }

        private static CustomerCreateRequest RandomCustomer()
        {
             Random rnd = new Random();
            string fName = _namesList[rnd.Next(1, 10)];
            string sName = _surnamesList[rnd.Next(1, 10)];
            return new CustomerCreateRequest(fName, sName);
        }

        private static async Task GetCustomerById(long CustomerId)
        {
            HttpClient client = new HttpClient();
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_url}/{CustomerId}");
            using var response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Пользователь с Id {CustomerId} обнаружен");
                Customer c = JsonConvert.DeserializeObject<Customer>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine($"Пользователь Id {c.Id} Имя:{c.Firstname} Фамилия:{c.Lastname}");
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Пользователь с ID {CustomerId} не найден");
            }
        }

        private static void PostRandomCustomer()
        {
            CustomerCreateRequest newCustomer = RandomCustomer();

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string customerStr = JsonConvert.SerializeObject(newCustomer, serializerSettings);
            var content = new StringContent(customerStr, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            using var response = client.PostAsync(_url, content).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Пользователь {newCustomer.Firstname} {newCustomer.Lastname} с Id {response.Content.ReadAsStringAsync().Result} создан успешно");
                Console.WriteLine("Проверяем");
                GetCustomerById(int.Parse(response.Content.ReadAsStringAsync().Result)).Wait();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                Console.WriteLine($"Пользователь с {newCustomer.Firstname} {newCustomer.Lastname} уже зарегистрирован");
            }
        }
    }
}