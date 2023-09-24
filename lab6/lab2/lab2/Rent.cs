using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class Rent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RentId { get; set; }
        public int ApartmentId { get; set; }
        public string Status { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string ClientPassportNumber { get; set; }
        public string Type { get; set; }

        public virtual Apartment Apartment { get; set; }

        public virtual Client Client { get; set; }

        public override string ToString()
        {
            return $"ид: {RentId}, client_passport: {ClientPassportNumber}, date begin: {DateBegin}, date_end: {DateEnd}, type: {Type}, status: {Status}";
        }
    }
}
