using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ClientOperations clientOperations = new ClientOperations();

                while (true)
                {
                    Console.WriteLine("Выберите операцию: \n" +
                        "1 - Добавить клиента \n" +
                        "2 - Изменить клиента \n" +
                        "3 - Удалить клиента \n" +
                        "4 - Вывести список оказанных услуг за определенный период, отсортированных по видам услуг");
                    string operation = Console.ReadLine();
                    string passportNumber;
                    string surname;
                    string name;

                    switch (operation)
                    {
                        case "1":
                            Console.WriteLine("Введите номер паспорта");
                            passportNumber = Console.ReadLine();
                            Console.WriteLine("Введите фамилию клиента");
                            surname = Console.ReadLine();
                            Console.WriteLine("Введите имя клиента");
                            name = Console.ReadLine();
                            clientOperations.AddClient(surname, name, passportNumber);
                            Console.WriteLine(clientOperations.FindClient(passportNumber));
                            break;
                        case "2":
                            Console.WriteLine("Введите номер паспорта");
                            passportNumber = Console.ReadLine();
                            Console.WriteLine("Введите фамилию клиента");
                            surname = Console.ReadLine();
                            Console.WriteLine("Введите имя клиента");
                            name = Console.ReadLine();
                            clientOperations.UpdateClient(surname, name, passportNumber);
                            Console.WriteLine(clientOperations.FindClient(passportNumber));
                            break;
                        case "3":
                            Console.WriteLine("Введите номер паспорта");
                            passportNumber = Console.ReadLine();
                            clientOperations.DeleteClient(passportNumber);
                            Console.WriteLine(clientOperations.FindClient(passportNumber));
                            break;
                        case "4":
                            Console.WriteLine("Введите день начала");
                            int dayBegin = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите месяц начала");
                            int monthBegin = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите год начала");
                            int yearBegin = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите день конца");
                            int dayEnd = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите месяц конца");
                            int monthEnd = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите год конца");
                            int yearEnd = int.Parse(Console.ReadLine());
                            List<rent> rents = clientOperations.Services(new DateTime(yearBegin, monthBegin, dayBegin), new DateTime(yearEnd, monthEnd, dayEnd));
                            foreach (rent rent in rents)
                            {
                                Console.WriteLine(rent);
                                Console.WriteLine("------------------------------------------");
                            }
                            break;
                        default:
                            Console.WriteLine("Введите корректную команду");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
