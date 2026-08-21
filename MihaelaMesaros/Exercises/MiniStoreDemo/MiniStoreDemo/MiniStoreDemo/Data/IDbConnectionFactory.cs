using System.Data.Common;

namespace MiniStoreDemo.Data;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}