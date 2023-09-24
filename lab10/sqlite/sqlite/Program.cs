using System;

namespace sqlite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connection = @"Data Source=D:\University\бд\sqlite\company.db;foreign keys=true;";
            SQLiteCRUD cRUD = new SQLiteCRUD(connection);

            while (true)
            {
                Console.WriteLine("Выберите операцию: \n" +
                        "1 - Добавить отделение \n" +
                        "2 - Изменить отделение \n" +
                        "3 - Удалить отделение \n" +
                        "4 - Вывести отделения \n" +
                        "5 - Добавить сотрудника\n" +
                        "6 - Изменить сотрудника\n" +
                        "7 - Удалить сотрудника\n" +
                        "8 - Вывести сотрудников\n");

                string operation = Console.ReadLine();
                string name, fullName;
                int id, unitId;

                switch (operation)
                {
                    case "1":
                        {
                            Console.WriteLine("Введите ид отделения");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите имя отделения");
                            name = Console.ReadLine();
                            cRUD.CreateUnit(id, name);
                            cRUD.ReadUnits();
                        }
                        break;

                    case "2":
                        {
                            Console.WriteLine("Введите ид отделения");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите новое имя отделения");
                            name = Console.ReadLine();
                            cRUD.UpdateUnitName(id, name);
                            cRUD.ReadUnits();
                        }
                        break;

                    case "3":
                        {
                            Console.WriteLine("Введите ид отделения для удаления");
                            id = int.Parse(Console.ReadLine());
                            cRUD.DeleteUnit(id);
                            cRUD.ReadUnits();
                        }
                        break;

                    case "4":
                        {
                            cRUD.ReadUnits();
                        }
                        break;

                    case "5":
                        {
                            Console.WriteLine("Введите ид сотрудника");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите ид отделения");
                            unitId = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите полное имя сотрудника");
                            fullName = Console.ReadLine();
                            cRUD.CreateWorker(id, unitId, fullName);
                            cRUD.ReadWorkers();
                        }
                        break;

                    case "6":
                        {
                            Console.WriteLine("Введите ид сотрудника для изменения");
                            id = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите новое полное имя сотрудника");
                            fullName = Console.ReadLine();
                            cRUD.UpdateWorkerFullName(id, fullName);
                            cRUD.ReadWorkers();
                        }
                        break;

                    case "7":
                        {
                            Console.WriteLine("Введите ид сотрудника для удаления");
                            id = int.Parse(Console.ReadLine());
                            cRUD.DeleteWorker(id);
                            cRUD.ReadWorkers();
                        }
                        break;

                    case "8":
                        {
                            cRUD.ReadWorkers();
                        }
                        break;
                }
            }
        }
    }
}
