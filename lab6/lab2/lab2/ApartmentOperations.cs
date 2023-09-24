using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class ApartmentOperations
    {
        private OracleConnection connection;

        public ApartmentOperations(string connectionString)
        {
            connection = new OracleConnection(connectionString);
            connection.Open();
        }

        public Apartment FindApartment(int apartmentId)
        {
            using (var command = new OracleCommand("SELECT apartment_id, city, street, house_number, room_number, day_cost FROM apartments WHERE apartment_id = :apartmentId", connection))
            {
                command.Parameters.Add(":apartmentId", OracleDbType.Int32).Value = apartmentId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Apartment
                        {
                            ApartmentId = int.Parse(reader["apartment_id"].ToString()),
                            City = reader["city"].ToString(),
                            Street = reader["street"].ToString(),
                            HouseNumber = int.Parse(reader["house_number"].ToString()),
                            RoomNumber = int.Parse(reader["room_number"].ToString()),
                            DayCost = decimal.Parse(reader["day_cost"].ToString())
                        };
                    }
                }
            }
            return null;
        }

        public List<Apartment> GetAllApartments()
        {
            var apartments = new List<Apartment>();
            using (var command = new OracleCommand("SELECT apartment_id, city, street, house_number, room_number, day_cost FROM apartments", connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    apartments.Add(new Apartment
                    {
                        ApartmentId = int.Parse(reader["apartment_id"].ToString()),
                        City = reader["city"].ToString(),
                        Street = reader["street"].ToString(),
                        HouseNumber = int.Parse(reader["house_number"].ToString()),
                        RoomNumber = int.Parse(reader["room_number"].ToString()),
                        DayCost = decimal.Parse(reader["day_cost"].ToString())
                    });
                }
            }
            return apartments;
        }

        public void AddApartment(string city, string street, int houseNumber, int roomNumber, decimal dayCost)
        {
            using (var command = new OracleCommand("INSERT INTO apartments (city, street, house_number, room_number, day_cost) VALUES (:city, :street, :houseNumber, :roomNumber, :dayCost)", connection))
            {
                command.Parameters.Add(":city", OracleDbType.Varchar2).Value = city;
                command.Parameters.Add(":street", OracleDbType.Varchar2).Value = street;
                command.Parameters.Add(":houseNumber", OracleDbType.Int32).Value = houseNumber;
                command.Parameters.Add(":roomNumber", OracleDbType.Int32).Value = roomNumber;
                command.Parameters.Add(":dayCost", OracleDbType.Decimal).Value = dayCost;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateApartment(int apartmentId, string city, string street, int houseNumber, int roomNumber, decimal dayCost)
        {
            if (FindApartment(apartmentId) == null)
            {
                Console.WriteLine("no apartment with this id");
            }
            else
            {
                using (var command = new OracleCommand("UPDATE apartments SET city = :city, street = :street, house_number = :houseNumber, room_number = :roomNumber, day_cost = :dayCost WHERE apartment_id = :apartmentId", connection))
                {
                    command.Parameters.Add(":apartmentId", OracleDbType.Int32).Value = apartmentId;
                    command.Parameters.Add(":dayCost", OracleDbType.Decimal).Value = dayCost;
                    command.Parameters.Add(":city", OracleDbType.Varchar2).Value = city;
                    command.Parameters.Add(":street", OracleDbType.Varchar2).Value = street;
                    command.Parameters.Add(":houseNumber", OracleDbType.Int32).Value = houseNumber;
                    command.Parameters.Add(":roomNumber", OracleDbType.Int32).Value = roomNumber;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteApartment(int apartmentId)
        {
            if (FindApartment(apartmentId) == null)
            {
                Console.WriteLine("no apartment with this id");
            }
            else
            {
                using (var command = new OracleCommand("DELETE FROM apartments WHERE apartment_id = :apartmentId", connection))
                {
                    command.Parameters.Add(":apartmentId", OracleDbType.Int32).Value = apartmentId;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
