using System.Data;

namespace Backend.Shared.Database;

public interface ISqlDbContext : IDisposable
{
    IDbConnection GetConnection();

    IDbTransaction BeginTransaction();

    void Commit();

    void Rollback();
}
