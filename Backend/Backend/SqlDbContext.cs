using System.Data;
using Microsoft.Data.SqlClient;

namespace CommercialSystem.Shared.Database;

public class SqlDbContext : ISqlDbContext
{
    private readonly string _connectionString;
    private SqlConnection? _connection;
    private SqlTransaction? _transaction;
    private bool _isDisposed;

    public SqlDbContext(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection GetConnection()
    {
        if (_connection == null)
        {
            _connection = new SqlConnection(_connectionString);
        }

        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }

        return _connection;
    }

    public IDbTransaction BeginTransaction()
    {
        if (_transaction != null)
        {
            return _transaction;
        }

        var connection = (SqlConnection)GetConnection();
        _transaction = connection.BeginTransaction();
        return _transaction;
    }

    public void Commit()
    {
        try
        {
            _transaction?.Commit();
        }
        finally
        {
            ResetTransactionState();
        }
    }

    public void Rollback()
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            ResetTransactionState();
        }
    }

    private void ResetTransactionState()
    {
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                ResetTransactionState();
                _connection?.Dispose();
                _connection = null;
            }
            _isDisposed = true;
        }
    }
}