using System.Data;

namespace CommercialSystem.Shared.Persistence.Database;

public interface ISqlDbContext : IDisposable
{
    IDbConnection GetConnection();

    IDbTransaction BeginTransaction();

    void Commit();

    void Rollback();
}
