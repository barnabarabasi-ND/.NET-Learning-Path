using Microsoft.Extensions.Configuration;
using MiniStoreDemo.Infrastructure.Data;
using Moq;

namespace MiniStoreDemo.UnitTests.Infrastructure.Data;

public sealed class SqlConnectionFactoryTests
{
    private const string ValidConnectionString = "Server=localhost;Database=TestDb;Trusted_Connection=True;";

    #region Constructor - Happy Path Tests

    [Fact]
    public void Constructor_WithValidConfiguration_DoesNotThrow()
    {
        // Arrange
        var config = CreateConfiguration(ValidConnectionString);

        // Act & Assert
        var factory = new SqlConnectionFactory(config);
        Assert.NotNull(factory);
    }

    [Fact]
    public void Constructor_WithValidConnectionString_StoresConnectionString()
    {
        // Arrange
        var config = CreateConfiguration(ValidConnectionString);

        // Act
        var factory = new SqlConnectionFactory(config);
        var connection = factory.CreateConnection();

        // Assert
        Assert.Equal(ValidConnectionString, connection.ConnectionString);
    }

    #endregion

    #region Constructor - Null Configuration Tests

    [Fact]
    public void Constructor_WithNullConfiguration_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SqlConnectionFactory(null!));
    }

    #endregion

    #region Constructor - Missing Connection String Tests

    [Fact]
    public void Constructor_WithMissingConnectionString_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = new ConfigurationBuilder().Build();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => new SqlConnectionFactory(config));
        Assert.Contains("MiniStoreDemoConnStr", ex.Message);
    }

    [Fact]
    public void Constructor_WithNullConnectionString_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = CreateConfiguration(null);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => new SqlConnectionFactory(config));
        Assert.Contains("MiniStoreDemoConnStr", ex.Message);
    }

    [Fact]
    public void Constructor_WithEmptyConnectionString_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = CreateConfiguration("");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => new SqlConnectionFactory(config));
        Assert.Contains("MiniStoreDemoConnStr", ex.Message);
    }

    [Fact]
    public void Constructor_WithWhitespaceConnectionString_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = CreateConfiguration("   ");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => new SqlConnectionFactory(config));
        Assert.Contains("MiniStoreDemoConnStr", ex.Message);
    }

    [Fact]
    public void Constructor_WithTabsOnlyConnectionString_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = CreateConfiguration("\t\t\t");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => new SqlConnectionFactory(config));
        Assert.Contains("MiniStoreDemoConnStr", ex.Message);
    }

    #endregion

    #region CreateConnection - Happy Path Tests

    [Fact]
    public void CreateConnection_ReturnsNonNullConnection()
    {
        // Arrange
        var config = CreateConfiguration(ValidConnectionString);
        var factory = new SqlConnectionFactory(config);

        // Act
        var connection = factory.CreateConnection();

        // Assert
        Assert.NotNull(connection);
    }

    [Fact]
    public void CreateConnection_ReturnsSqlConnection()
    {
        // Arrange
        var config = CreateConfiguration(ValidConnectionString);
        var factory = new SqlConnectionFactory(config);

        // Act
        var connection = factory.CreateConnection();

        // Assert
        Assert.IsType<Microsoft.Data.SqlClient.SqlConnection>(connection);
    }

    [Fact]
    public void CreateConnection_ReturnsConnectionWithCorrectConnectionString()
    {
        // Arrange
        var config = CreateConfiguration(ValidConnectionString);
        var factory = new SqlConnectionFactory(config);

        // Act
        var connection = factory.CreateConnection();

        // Assert
        Assert.Equal(ValidConnectionString, connection.ConnectionString);
    }

    [Fact]
    public void CreateConnection_CalledMultipleTimes_ReturnsNewConnectionEachTime()
    {
        // Arrange
        var config = CreateConfiguration(ValidConnectionString);
        var factory = new SqlConnectionFactory(config);

        // Act
        var connection1 = factory.CreateConnection();
        var connection2 = factory.CreateConnection();

        // Assert
        Assert.NotSame(connection1, connection2);
    }

    [Fact]
    public void CreateConnection_ReturnsClosedConnection()
    {
        // Arrange
        var config = CreateConfiguration(ValidConnectionString);
        var factory = new SqlConnectionFactory(config);

        // Act
        var connection = factory.CreateConnection();

        // Assert
        Assert.Equal(System.Data.ConnectionState.Closed, connection.State);
    }

    #endregion

    #region CreateConnection - Different Connection String Formats

    [Theory]
    [InlineData("Server=localhost;Database=Test;")]
    [InlineData("Data Source=.;Initial Catalog=MyDb;Integrated Security=True;")]
    [InlineData("Server=192.168.1.1,1433;Database=Prod;User Id=sa;Password=pwd;")]
    [InlineData("Server=tcp:myserver.database.windows.net,1433;Database=mydb;")]
    public void CreateConnection_WithVariousConnectionStringFormats_Succeeds(string connectionString)
    {
        // Arrange
        var config = CreateConfiguration(connectionString);
        var factory = new SqlConnectionFactory(config);

        // Act
        var connection = factory.CreateConnection();

        // Assert
        Assert.NotNull(connection);
        Assert.Equal(connectionString, connection.ConnectionString);
    }

    #endregion

    #region CreateConnection - Edge Cases

        [Fact]
        public void CreateConnection_WithVeryLongConnectionString_Succeeds()
        {
            // Arrange - Database name limit is 128 chars, so we use a valid length
            var longConnectionString = $"Server=localhost;Database={new string('x', 100)};";
            var config = CreateConfiguration(longConnectionString);
            var factory = new SqlConnectionFactory(config);

            // Act
            var connection = factory.CreateConnection();

            // Assert
            Assert.NotNull(connection);
        }

    [Fact]
    public void CreateConnection_WithSpecialCharactersInConnectionString_Succeeds()
    {
        // Arrange
        var connectionString = "Server=localhost;Database=Test;Password=P@ss!w0rd#$%;";
        var config = CreateConfiguration(connectionString);
        var factory = new SqlConnectionFactory(config);

        // Act
        var connection = factory.CreateConnection();

        // Assert
        Assert.NotNull(connection);
    }

    [Fact]
    public void CreateConnection_AfterManyCreations_StillWorks()
    {
        // Arrange
        var config = CreateConfiguration(ValidConnectionString);
        var factory = new SqlConnectionFactory(config);

        // Act & Assert
        for (int i = 0; i < 100; i++)
        {
            var connection = factory.CreateConnection();
            Assert.NotNull(connection);
            connection.Dispose();
        }
    }

    #endregion

    #region IDbConnectionFactory Interface Tests

    [Fact]
    public void SqlConnectionFactory_ImplementsIDbConnectionFactory()
    {
        // Arrange & Act
        var factory = new SqlConnectionFactory(CreateConfiguration(ValidConnectionString));

        // Assert
        Assert.IsAssignableFrom<IDbConnectionFactory>(factory);
    }

    #endregion

    #region Helper Methods

    private static IConfiguration CreateConfiguration(string? connectionString)
    {
        var configData = new Dictionary<string, string?>();

        if (connectionString is not null)
        {
            configData["ConnectionStrings:MiniStoreDemoConnStr"] = connectionString;
        }

        return new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();
    }

    #endregion
}
