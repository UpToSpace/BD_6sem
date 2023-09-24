using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class BillDetails
    {
        public int BillId { get; set; }
        public int RentId { get; set; }
        public DateTime BillDate { get; set; }
        public decimal Total { get; set; }

        public override string ToString()
        {
            return $"Bill ID: {BillId}, Rent ID: {RentId}, Bill Date: {BillDate.ToString("dd/MM/yyyy")}, Total: {Total:C}";
        }
    }
}
