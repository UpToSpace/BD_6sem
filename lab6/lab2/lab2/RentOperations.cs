using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class RentOperations
    {
        private OracleConnection connection;

        public RentOperations(string connectionString)
        {
            connection = new OracleConnection(connectionString);
            connection.Open();
        }

        public List<Rent> GetAllRents()
{
    var rents = new List<Rent>();

    using (var command = new OracleCommand("SELECT * FROM rents", connection))
    {
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                DateTime dateBegin, dateEnd;
                if (DateTime.TryParse(reader["date_begin"].ToString(), out dateBegin) && DateTime.TryParse(reader["date_end"].ToString(), out dateEnd))
                {
                    var rent = new Rent
                    {
                        RentId = int.Parse(reader["rent_id"].ToString()),
                        ApartmentId = int.Parse(reader["apartment_id"].ToString()),
                        Status = reader["status"].ToString(),
                        ClientPassportNumber = reader["client_passport_number"].ToString(),
                        Type = reader["type"].ToString(),
                        DateBegin = dateBegin,
                        DateEnd = dateEnd
                    };
                    rents.Add(rent);
                }
            }
        }
    }

    return rents;
}

        public Rent FindRent(int rentId)
        {
            using (var command = new OracleCommand("SELECT * FROM rents WHERE rent_id = :rentId", connection))
            {
                command.Parameters.Add(":rentId", OracleDbType.Int32).Value = rentId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime dateBegin,dateEnd;
                        if (DateTime.TryParse(reader["date_begin"].ToString(), out dateBegin) && DateTime.TryParse(reader["date_end"].ToString(), out dateEnd))
                        {
                            return new Rent
                            {
                                DateBegin = dateBegin,
                                DateEnd = dateEnd,
                                RentId = int.Parse(reader["rent_id"].ToString()),
                                ApartmentId = int.Parse(reader["apartment_id"].ToString()),
                                Status = reader["status"].ToString(),
                                ClientPassportNumber = reader["client_passport_number"].ToString(),
                                Type = reader["type"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void AddRent(int apartmentId, string status, DateTime dateBegin, DateTime dateEnd, string clientPassportNumber, string type)
        {
            using (var command = new OracleCommand("INSERT INTO rents (apartment_id, status, date_begin, date_end, client_passport_number, type) VALUES (:apartmentId, :status, :dateBegin, :dateEnd, :clientPassportNumber, :type)", connection))
            {
                command.Parameters.Add(":apartment_id", OracleDbType.Int32).Value = apartmentId;
                command.Parameters.Add(":status", OracleDbType.Varchar2).Value = status;
                command.Parameters.Add(":dateBegin", OracleDbType.Date).Value = dateBegin;
                command.Parameters.Add(":dateEnd", OracleDbType.Date).Value = dateEnd;
                command.Parameters.Add(":clientPassportNumber", OracleDbType.Varchar2).Value = clientPassportNumber;
                command.Parameters.Add(":type", OracleDbType.Varchar2).Value = type;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateRent(int rentId, int apartmentId, string status, DateTime dateBegin, DateTime dateEnd, string clientPassportNumber, string type)
        {
            var oldRent = FindRent(rentId);
            if (oldRent == null)
            {
                Console.WriteLine("A rent with this ID doesn't exist");
                return;
            }

            using (var command = new OracleCommand("UPDATE rents SET apartment_id = :apartmentId, status = :status, date_begin = :dateBegin, date_end = :dateEnd, client_passport_number = :clientPassportNumber, type = :type WHERE rent_id = :rentId", connection))
            {
                command.Parameters.Add(":apartmentId", OracleDbType.Int32).Value = apartmentId;
                command.Parameters.Add(":status", OracleDbType.Varchar2).Value = status;
                command.Parameters.Add(":dateBegin", OracleDbType.Date).Value = dateBegin;
                command.Parameters.Add(":dateEnd", OracleDbType.Date).Value = dateEnd;
                command.Parameters.Add(":clientPassportNumber", OracleDbType.Varchar2).Value = clientPassportNumber;
                command.Parameters.Add(":type", OracleDbType.Varchar2).Value = type;
                command.Parameters.Add(":rentId", OracleDbType.Int32).Value = rentId;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteRent(int rentId)
        {
            var oldRent = FindRent(rentId);
            if (oldRent == null)
            {
                Console.WriteLine("A rent with this ID doesn't exist");
                return;
            }

            using (var command = new OracleCommand("DELETE FROM rents WHERE rent_id = :rentId", connection))
            {
                command.Parameters.Add(":rentId", OracleDbType.Int32).Value = rentId;
                command.ExecuteNonQuery();
            }
        }
    }
}
