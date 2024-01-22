using Dapper;
using MySql.Data.MySqlClient;

namespace bookingservice.db;

public class DbConnection
{
    protected readonly ILogger Logger;
    protected MySqlConnection Conn { get; }

    protected DbConnection(ILogger logger)
    {
        try
        {
            Logger = logger;
            string? dbName = Environment.GetEnvironmentVariable("PLATFORM_DB_NAME");
            string? host = Environment.GetEnvironmentVariable("PLATFORM_DB_HOST");
            string? port = Environment.GetEnvironmentVariable("PLATFORM_DB_PORT");
            string? user = Environment.GetEnvironmentVariable("PLATFORM_DB_USER");
            string? password = Environment.GetEnvironmentVariable("PLATFORM_DB_PASSWORD");
            if (string.IsNullOrEmpty(dbName))
            {
                throw new Exception("PLATFORM_DB_NAME property cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(host))
            {
                throw new Exception("PLATFORM_DB_HOST property cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(port))
            {
                throw new Exception("PLATFORM_DB_PORT property cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(user))
            {
                throw new Exception("PLATFORM_DB_USER property cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("PLATFORM_DB_PASSWORD property cannot be null or empty!");
            }

            var connString =
                $"server={host};database={dbName};uid={user};pwd={password};port={port};Pooling=true;SslMode=Required;";
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
            Conn = new MySqlConnection(connString);
        }
        catch (Exception ex)
        {
            Logger!.LogError("{ExMessage}", ex.Message);
            throw;
        }
        
    }
    
    
}