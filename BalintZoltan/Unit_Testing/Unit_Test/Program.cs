[TestClass]
public class CalculatorTests
{
    [TestMethod]
    public void Add_2And3_Returns5()
    {
        // Arrange
        var calc = new Calculator();

        // Act
        int result = calc.Add(2, 3);

        // Assert
        Assert.AreEqual(5, result);
    }
    [TestMethod]
    public void Add_2And3_NotReturn6()
    {
        // Arrange
        var calc = new Calculator();

        // Act
        int result = calc.Add(2, 3);

        // Assert
        Assert.AreNotEqual(6, result);
    }
}

[TestClass]
public class BankAccountTests
{
    private BankAccount? account;

    [TestInitialize]
    public void Setup()
    {
        account = new BankAccount(500);
    }

    [TestMethod]
    public void DepositTest()
    {
        account.Deposit(100);

        Assert.AreEqual(600, account.Balance);
    }

    [TestMethod]
    public void WithdrawTest()
    {
        account.Withdraw(100);

        Assert.AreEqual(400, account.Balance);
    }
}

[TestClass]
public class BankAccountDataRowTests
{
    private BankAccount? account;

    [TestInitialize]
    public void Setup()
    {
        account = new BankAccount(500);
    }

    [TestMethod]
    [DataRow(-100, 500)]
    [DataRow(100, 600)]
    [DataRow(200, 700)]
    [DataRow(500, 1000)]
    public void DepositTest(int amount, int expectedBalance)
    {
        account!.Deposit(amount);

        Assert.AreEqual(expectedBalance, account.Balance);
    }

    [TestMethod]
    [DataRow(100, 400)]
    [DataRow(200, 300)]
    [DataRow(500, 0)]
    [DataRow(600, 500)]
    [DataRow(-100, 500)]
    [DataRow(0, 500)]
    public void WithdrawTest(int amount, int expectedBalance)
    {
        account!.Withdraw(amount);

        Assert.AreEqual(expectedBalance, account.Balance);
    }
}