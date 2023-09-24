using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read)]
    public static int SqlStoredProcedure1 ()
    {
        string sql = @"select (sum(day_cost) - min(day_cost) - max(day_cost)) / (count(day_cost) - 2) from [dbo].[apartments]";

        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                int result = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return result;
            }
        }
    }
}
