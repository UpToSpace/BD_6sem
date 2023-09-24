using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class ClientOperations
    {
        rentEntities rent;

        public ClientOperations()
        {
            rent = new rentEntities();
        }

        public client FindClient(string passportNumber)
        {
            if(rent.clients.Where(e => e.passport_number == passportNumber).Any())
            {
                return rent.clients.Where(e => e.passport_number == passportNumber).First();
            }
            return null;
        }

        public void AddClient(string surname, string name, string passportNumber)
        {
            if(rent.clients.Where(e => e.passport_number == passportNumber).Any())
            {
                Console.WriteLine("A client with this passport number already exists");
                return;
            }
            client client = new client();
            client.surname = surname;
            client.name = name;
            client.passport_number = passportNumber;
            rent.clients.Add(client);
            rent.SaveChanges();
        }

        public void UpdateClient(string surname, string name, string passportNumber)
        {
            if (!rent.clients.Where(e => e.passport_number == passportNumber).Any())
            {
                Console.WriteLine("A client with this passport number doesnt exists");
                return;
            }

            client oldClient = FindClient(passportNumber);
            rent.clients.Remove(oldClient);

            client client = new client();
            client.surname = surname;
            client.name = name;
            client.passport_number = passportNumber;
            rent.clients.Add(client);
            rent.SaveChanges();
        }

        public void DeleteClient(string passportNumber)
        {
            if (!rent.clients.Where(e => e.passport_number == passportNumber).Any())
            {
                Console.WriteLine("A client with this passport number doesnt exists");
                return;
            }
            client oldClient = rent.clients.Where(e => e.passport_number == passportNumber).First();
            rent.clients.Remove(oldClient);
            rent.SaveChanges();
        }

        //Список оказанных услуг за определенный период, отсортированных по видам услуг
        public List<rent> Services(DateTime dateBegin, DateTime dateEnd)
        {
            return rent.rents.Where(e => e.date_begin > dateBegin && e.date_end < dateEnd).OrderBy(e => e.type).ToList();
        }
    }
}
