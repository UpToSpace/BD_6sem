using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class Apartment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApartmentId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int RoomNumber { get; set; }
        public decimal DayCost { get; set; }
        public override string ToString()
        {
            return $"Apartment #{ApartmentId}: {City}, {Street}, {HouseNumber}, Room {RoomNumber}, Cost: {DayCost} per day";
        }
    }
}
