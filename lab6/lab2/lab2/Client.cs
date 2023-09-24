using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class Client
    {
        public string surname { get; set; }
        public string name { get; set; }
        public string passport_number { get; set; }

        public override string ToString()
        {
            return $"Номер паспорта: {passport_number}, Фамилия: {surname}, Имя: {name}";
        }
    }
}
