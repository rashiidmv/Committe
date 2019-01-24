using System;
using System.Data.SqlClient;

namespace DBScriptsForSetup {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Script Execution Started..!");
            String connectionString = @"Data Source=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Server=localhost\SQLEXPRESS04";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            Console.WriteLine("Connection open");

            string createDB = @"IF NOT EXISTS (SELECT DB_NAME(database_id) AS [Database] FROM sys.databases WHERE DB_NAME(database_id)='MahalluDatabase')
            BEGIN
            CREATE DATABASE MahalluDatabase;
            PRINT 'Database is created successfully.!';
            END
            ELSE
            PRINT 'Database already exists.!'; ";
            SqlCommand sqlCommand = new SqlCommand(createDB, connection);
            int rowCount = sqlCommand.ExecuteNonQuery();
            Console.WriteLine("Database is created successfully.!");
            connection.Close();


            Console.WriteLine("\n\nSchema creation Started..!");
            connectionString = @"Data Source=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Server=localhost\SQLEXPRESS04;Initial Catalog=MahalluDatabase;";
            connection = new SqlConnection(connectionString);
            connection.Open();
            Console.WriteLine("Connection opened for schema creation..!");
            connection.Close();


            Console.Read();
        }
    }
}
