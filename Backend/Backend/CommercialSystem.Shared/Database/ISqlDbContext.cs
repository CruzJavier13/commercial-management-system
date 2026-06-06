using System.Data;

namespace CommercialSystem.Shared.Persistence.Database;

public interface ISqlDbContext
{
    IDbConnection CreateConnection();
}
