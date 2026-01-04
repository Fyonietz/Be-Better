using MySqlConnector;

public class Db
{
    private readonly string _connectionString;

    public Db(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MariaDb");
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}
