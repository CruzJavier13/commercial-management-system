using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CommercialSystem.Shared.Persistence.Database;

public class SqlDbContext : ISqlDbContext
{
    private readonly string _connectionString;

    public SqlDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration), "DefaultConnection no configurado en appsettings.json");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }


}