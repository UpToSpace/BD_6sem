using System;
using System.Data.SQLite;

namespace sqlite
{
public class SQLiteCRUD
{
    private SQLiteConnection connection;

    public SQLiteCRUD(string databasePath)
    {
        connection = new SQLiteConnection(databasePath);
    }

    public void CreateUnit(int id, string name)
    {
        string query = "INSERT INTO units (id, name) VALUES (@id, @name)";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void CreateWorker(int id, int unitId, string fullName)
    {
        string query = "INSERT INTO workers (id, unit_id, full_name) VALUES (@id, @unitId, @fullName)";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@unitId", unitId);
            command.Parameters.AddWithValue("@fullName", fullName);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void ReadUnits()
    {
        string query = "SELECT id, name FROM units";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);

                    Console.WriteLine("Unit ID: " + id + ", Name: " + name);
                }
            }

            connection.Close();
        }
    }

    public void ReadWorkers()
    {
        string query = "SELECT id, unit_id, full_name FROM workers";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int unitId = reader.GetInt32(1);
                    string fullName = reader.GetString(2);

                    Console.WriteLine("Worker ID: " + id + ", Unit ID: " + unitId + ", Full Name: " + fullName);
                }
            }

            connection.Close();
        }
    }

    public void UpdateWorkerFullName(int id, string fullName)
    {
        string query = "UPDATE workers SET full_name = @fullName WHERE id = @id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@fullName", fullName);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

        public void UpdateUnitName(int id, string name)
        {
            string query = "UPDATE units SET name = @name WHERE id = @id";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeleteUnit(int id)
    {
        string query = "DELETE FROM units WHERE id = @id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void DeleteWorker(int id)
    {
        string query = "DELETE FROM workers WHERE id = @id";

        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}

}
