using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class BillDetailsOperations
    {
        private OracleConnection connection;

        public BillDetailsOperations(string connectionString)
        {
            connection = new OracleConnection(connectionString);
            connection.Open();
        }
        public BillDetails FindBillDetails(int billId)
        {
            using (var command = new OracleCommand("SELECT * FROM bill_details WHERE bill_id = :billId", connection))
            {
                command.Parameters.Add(":billId", OracleDbType.Int32).Value = billId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new BillDetails
                        {
                            BillId = int.Parse(reader["bill_id"].ToString()),
                            RentId = int.Parse(reader["rent_id"].ToString()),
                            BillDate = DateTime.Parse(reader["bill_date"].ToString()),
                            Total = decimal.Parse(reader["total"].ToString())
                        };
                    }
                }
            }
            return null;
        }

        public List<BillDetails> GetAllBillDetails()
        {
            List<BillDetails> billDetails = new List<BillDetails>();
            using (var command = new OracleCommand("SELECT * FROM bill_details", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        billDetails.Add(new BillDetails
                        {
                            BillId = int.Parse(reader["bill_id"].ToString()),
                            RentId = int.Parse(reader["rent_id"].ToString()),
                            BillDate = DateTime.Parse(reader["bill_date"].ToString()),
                            Total = decimal.Parse(reader["total"].ToString())
                        });
                    }
                }
            }
            return billDetails;
        }

        public void AddBillDetails(int rentId, DateTime billDate, decimal total)
        {
            using (var command = new OracleCommand("INSERT INTO bill_details (rent_id, bill_date, total) VALUES (:rentId, :billDate, :total)", connection))
            {
                command.Parameters.Add(":rentId", OracleDbType.Int32).Value = rentId;
                command.Parameters.Add(":billDate", OracleDbType.Date).Value = billDate;
                command.Parameters.Add(":total", OracleDbType.Decimal).Value = total;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateBillDetails(int billId, int rentId, DateTime billDate, decimal total)
        {
            var oldBillDetails = FindBillDetails(billId);
            if (oldBillDetails == null)
            {
                Console.WriteLine("A bill details with this bill id doesn't exist");
                return;
            }

            using (var command = new OracleCommand("UPDATE bill_details SET rent_id = :rentId, bill_date = :billDate, total = :total WHERE bill_id = :billId", connection))
            {
                command.Parameters.Add(":rentId", OracleDbType.Int32).Value = rentId;
                command.Parameters.Add(":billDate", OracleDbType.Date).Value = billDate;
                command.Parameters.Add(":total", OracleDbType.Decimal).Value = total;
                command.Parameters.Add(":billId", OracleDbType.Int32).Value = billId;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteBillDetails(int billId)
        {
            var oldBillDetails = FindBillDetails(billId);
            if (oldBillDetails == null)
            {
                Console.WriteLine("A bill details with this bill id doesn't exist");
                return;
            }

            using (var command = new OracleCommand("DELETE FROM bill_details WHERE bill_id = :billId", connection))
            {
                command.Parameters.Add(":billId", OracleDbType.Int32).Value = billId;
                command.ExecuteNonQuery();
            }
        }
    }
}
