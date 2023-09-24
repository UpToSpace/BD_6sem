using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace lab2
{
    public class ClientOperations
    {
        private OracleConnection connection;

        public ClientOperations(string connectionString)
        {
            connection = new OracleConnection(connectionString);
            connection.Open();
        }

        public Client FindClient(string passportNumber)
        {
            using (var command = new OracleCommand("SELECT * FROM clients WHERE passport_number = :passportNumber", connection))
            {
                command.Parameters.Add(":passportNumber", OracleDbType.Varchar2).Value = passportNumber;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Client
                        {
                            surname = reader["surname"].ToString(),
                            name = reader["name"].ToString(),
                            passport_number = reader["passport_number"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public List<Client> GetAllClients()
        {
            List<Client> clients = new List<Client>();
            using (var command = new OracleCommand("SELECT * FROM clients", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new Client
                        {
                            surname = reader["surname"].ToString(),
                            name = reader["name"].ToString(),
                            passport_number = reader["passport_number"].ToString()
                        });
                    }
                }
            }
            return clients;
        }

        public void AddClient(string surname, string name, string passportNumber)
        {
            if (FindClient(passportNumber) != null)
            {
                Console.WriteLine("A client with this passport number already exists");
                return;
            }

            using (var command = new OracleCommand("INSERT INTO clients (surname, name, passport_number, hid) VALUES (:surname, :name, :passportNumber, null)", connection))
            {
                command.Parameters.Add(":surname", OracleDbType.Varchar2).Value = surname;
                command.Parameters.Add(":name", OracleDbType.Varchar2).Value = name;
                command.Parameters.Add(":passportNumber", OracleDbType.Varchar2).Value = passportNumber;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateClient(string surname, string name, string passportNumber)
        {
            var oldClient = FindClient(passportNumber);
            if (oldClient == null)
            {
                Console.WriteLine("A client with this passport number doesn't exist");
                return;
            }

            using (var command = new OracleCommand("UPDATE clients SET surname = :surname, name = :name WHERE passport_number = :passportNumber", connection))
            {
                command.Parameters.Add(":surname", OracleDbType.Varchar2).Value = surname;
                command.Parameters.Add(":name", OracleDbType.Varchar2).Value = name;
                command.Parameters.Add(":passportNumber", OracleDbType.Varchar2).Value = passportNumber;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteClient(string passportNumber)
        {
            var oldClient = FindClient(passportNumber);
            if (oldClient == null)
            {
                Console.WriteLine("A client with this passport number doesn't exist");
                return;
            }

            using (var command = new OracleCommand("DELETE FROM clients WHERE passport_number = :passportNumber", connection))
            {
                command.Parameters.Add(":passportNumber", OracleDbType.Varchar2).Value = passportNumber;
                command.ExecuteNonQuery();
            }
        }

        public List<Rent> Services(DateTime dateBegin, DateTime dateEnd)
        {
            try
            {
                string sql = "SELECT * FROM rents WHERE date_begin > :dateBegin AND date_end < :dateEnd ORDER BY type";
                using (var command = new OracleCommand(sql, connection))
                {
                    command.Parameters.Add("dateBegin", OracleDbType.Date).Value = dateBegin;
                    command.Parameters.Add("dateEnd", OracleDbType.Date).Value = dateEnd;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            List<Rent> rents = new List<Rent>();
                            while (reader.Read())
                            {
                                Rent r = new Rent();
                                r.ClientPassportNumber = reader["client_passport_number"].ToString();
                                r.DateBegin = DateTime.Parse(reader["date_begin"].ToString());
                                r.DateEnd = DateTime.Parse(reader["date_end"].ToString());
                                r.Type = reader["type"].ToString();
                                rents.Add(r);
                            }
                            return rents;
                        }
                        Console.WriteLine("no data with these dates");
                        return null;
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                System.Console.Read();
                return null;
            }
        }
    }
}
