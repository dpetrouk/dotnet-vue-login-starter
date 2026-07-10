using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public enum DatabaseType
{
    Postgres,
    Sqlite
}

public static class DbContextFactory
{
    public static Action<DbContextOptionsBuilder> Configure(DatabaseType dbType, string conn)
    {
        return dbType switch
        {
            DatabaseType.Postgres => o => o.UseNpgsql(conn),
            DatabaseType.Sqlite => o => o.UseSqlite(conn),
            _ => throw new ArgumentOutOfRangeException(nameof(dbType))
        };
    }
}
