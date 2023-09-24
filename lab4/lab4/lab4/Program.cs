using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new rentEntities();
            try
            {
                while (true)
                {
                    Console.WriteLine("\nВыберите операцию: \n" +
                        "1 - Вывести список апартаментов \n" +
                        "2 - Найти пересечение \n" +
                        "3 - Найти расстояние");
                    string operation = Console.ReadLine();

                    switch (operation)
                    {
                        case "1":
                            foreach (var item in context.apartments.ToList())
                            {
                                Console.WriteLine($"id: {item.apartment_id}\tcity:{item.city}\tstreet:{item.street}\thouse number:{item.house_number}\troom number:{item.room_number}\tlocation:{item.location}");
                            };
                            break;
                        case "2":
                            {
                                Console.WriteLine("Введите id апартмента 1");
                                var apartment_id1 = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите id апартмента 2");
                                var apartment_id2 = int.Parse(Console.ReadLine());

                                var apartment1 = context.apartments
                                    .Where(e => e.apartment_id == apartment_id1)
                                    .Select(e => e.location)
                                    .FirstOrDefault();

                                var apartment2 = context.apartments
                                    .Where(e => e.apartment_id == apartment_id2)
                                    .Select(e => e.location)
                                    .FirstOrDefault();

                                var union = apartment1.Union(apartment2);

                                var result = union != null ? union.ToString() : "No union";

                                Console.WriteLine(result);
                                break;
                            }
                        case "3":
                            {
                                Console.WriteLine("Введите id апартмента 1");
                                var apartment_id1 = int.Parse(Console.ReadLine());
                                Console.WriteLine("Введите id апартмента 2");
                                var apartment_id2 = int.Parse(Console.ReadLine());

                                var apartment1 = context.apartments
                                    .Where(e => e.apartment_id == apartment_id1)
                                    .Select(e => e.location)
                                    .FirstOrDefault();

                                var apartment2 = context.apartments
                                    .Where(e => e.apartment_id == apartment_id2)
                                    .Select(e => e.location)
                                    .FirstOrDefault();

                                var distance = apartment1.Distance(apartment2);

                                var result = distance != null ? distance.ToString() : "No distance";

                                Console.WriteLine(result);
                                break;
                            }
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
