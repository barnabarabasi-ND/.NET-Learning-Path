using System.Data.Common;

namespace MiniStoreDemo.Infrastructure.Data;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}