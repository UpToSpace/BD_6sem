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
                string connection = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));Password=1111;User Id=migr;";
                ClientOperations clientOperations = new ClientOperations(connection);
                RentOperations rentOperations = new RentOperations(connection);
                ApartmentOperations apartmentOperations = new ApartmentOperations(connection);
                BillDetailsOperations billDetailsOperations= new BillDetailsOperations(connection);

                //string connStr = @"Data Source=D:\University\бд\sqlite\company.db;foreign keys=true;";

                while (true)
                {
                    Console.WriteLine("Выберите операцию: \n" +
                        "1 - Добавить клиента \n" +
                        "2 - Изменить клиента \n" +
                        "3 - Удалить клиента \n" +
                        "4 - Вывести список оказанных услуг за определенный период, отсортированных по видам услуг \n" +
                        "5 - Добавить апартамент\n" +
                        "6 - Изменить апартамент\n" +
                        "7 - Удалить апартамент\n" +
                        "8 - Добавить аренду\n" +
                        "9 - Изменить аренду\n" +
                        "10 - Удалить аренду\n" +
                        "11 - Добавить информацию о платеже\n" +
                        "12 - Изменить информацию о платеже\n" +
                        "13 - Удалить информацию о платеже\n" +
                        "14 - Вывести клиентов\n" +
                        "15 - Вывести апартаменты\n" +
                        "16 - Вывести аренды\n" +
                        "17 - Вывести платежы\n");
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
                        case "4": //2023-02-03  2023-02-19
                            {
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
                                List<Rent> rents = clientOperations.Services(new DateTime(yearBegin, monthBegin, dayBegin), new DateTime(yearEnd, monthEnd, dayEnd));
                                if (rents != null)
                                    foreach (Rent rent in rents)
                                    {
                                        Console.WriteLine(rent);
                                        Console.WriteLine("------------------------------------------");
                                    }
                            }
                            break;
                        case "5":
                            {
                                Console.WriteLine("Введите город");
                                string city = Console.ReadLine();
                                Console.WriteLine("Введите улицу");
                                string street = Console.ReadLine();
                                Console.WriteLine("Введите номер дома");
                                int houseNumber = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите номер комнаты");
                                int roomNumber = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите стоимость дня");
                                decimal dayCost = decimal.Parse(Console.ReadLine());
                                apartmentOperations.AddApartment(city, street, houseNumber, roomNumber, dayCost);
                            }
                            break;
                        case "6":
                            {
                                Console.WriteLine("Введите id apartment");
                                int id = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите город");
                                string city = Console.ReadLine();
                                Console.WriteLine("Введите улицу");
                                string street = Console.ReadLine();
                                Console.WriteLine("Введите номер дома");
                                int houseNumber = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите номер комнаты");
                                int roomNumber = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите новую стоимость дня в формате 1,1");
                                decimal dayCost = decimal.Parse(Console.ReadLine());
                                apartmentOperations.UpdateApartment(id, city, street, houseNumber, roomNumber, dayCost);
                            }
                            break;
                        case "7":
                            {
                                Console.WriteLine("Введите id apartment");
                                int id = int.Parse(Console.ReadLine());
                                apartmentOperations.DeleteApartment(id);
                            }
                            break;
                        case "8":
                            {
                                string status = "not paid";
                                Console.WriteLine("Введите ид апартамента");
                                int apartmentId = int.Parse(Console.ReadLine());
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
                                DateTime dateBegin = new DateTime(yearBegin, monthBegin, dayBegin);
                                DateTime dateEnd = new DateTime(yearEnd, monthEnd, dayEnd);
                                Console.WriteLine("Введите номер паспорта");
                                passportNumber = Console.ReadLine();
                                Console.WriteLine("Введите тип аренда");
                                string type = Console.ReadLine();
                                rentOperations.AddRent(apartmentId, status, dateBegin, dateEnd, passportNumber, type);
                            }
                            break;
                        case "9":
                            {
                                Console.WriteLine("Введите id rent");
                                int rentId = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите id apartment");
                                int apartmentId = int.Parse(Console.ReadLine());
                                string status = "not paid";
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
                                DateTime dateBegin = new DateTime(yearBegin, monthBegin, dayBegin);
                                DateTime dateEnd = new DateTime(yearEnd, monthEnd, dayEnd);
                                Console.WriteLine("Введите номер паспорта");
                                passportNumber = Console.ReadLine();
                                Console.WriteLine("Введите тип аренда");
                                string type = Console.ReadLine();
                                rentOperations.UpdateRent(rentId, apartmentId, status, dateBegin, dateEnd, passportNumber, type);
                            }
                            break;
                        case "10":
                            {
                                Console.WriteLine("Введите id rent");
                                int rentId = int.Parse(Console.ReadLine());
                                rentOperations.DeleteRent(rentId);
                            }
                            break;
                        case "11":
                            {
                                Console.WriteLine("Введите id rent");
                                int rentId = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите день");
                                int dayEnd = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите месяц");
                                int monthEnd = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите год");
                                int yearEnd = int.Parse(Console.ReadLine());
                                DateTime dateEnd = new DateTime(yearEnd, monthEnd, dayEnd);
                                Console.WriteLine("Введите стоимость в формате 1,1");
                                decimal dayCost = decimal.Parse(Console.ReadLine());
                                billDetailsOperations.AddBillDetails(rentId, dateEnd, dayCost);
                            }
                            break;
                        case "12":
                            {
                                Console.WriteLine("Введите id bill");
                                int billId = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите id rent");
                                int rentId = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите день");
                                int dayEnd = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите месяц");
                                int monthEnd = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите год");
                                int yearEnd = int.Parse(Console.ReadLine());
                                DateTime dateEnd = new DateTime(yearEnd, monthEnd, dayEnd);
                                Console.WriteLine("Введите новую стоимость");
                                decimal dayCost = decimal.Parse(Console.ReadLine());
                                billDetailsOperations.UpdateBillDetails(billId, rentId, dateEnd, dayCost);
                            }
                            break;
                        case "13":
                            {
                                Console.WriteLine("Введите id bill");
                                int billId = int.Parse(Console.ReadLine());
                                billDetailsOperations.DeleteBillDetails(billId);
                            }
                            break;
                        case "14":
                            {
                                List<Client> clients = clientOperations.GetAllClients();
                                foreach (var item in clients)
                                {
                                    Console.WriteLine(item);
                                }
                            }
                            break;
                        case "15":
                            {
                                List<Apartment> apartments = apartmentOperations.GetAllApartments();
                                foreach (var item in apartments)
                                {
                                    Console.WriteLine(item);
                                }
                            }
                            break;
                        case "16":
                            {
                                List<Rent> rents = rentOperations.GetAllRents();
                                foreach (var item in rents)
                                {
                                    Console.WriteLine(item);
                                }
                            }
                            break;
                        case "17":
                            {
                                List<BillDetails> apartments = billDetailsOperations.GetAllBillDetails();
                                foreach (var item in apartments)
                                {
                                    Console.WriteLine(item);
                                }
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
            } finally
            {
                Console.ReadLine();
            }
        }
    }
}
