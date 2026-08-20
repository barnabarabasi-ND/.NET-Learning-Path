
[TestClass]
public class CalculatorTests
{
    [TestMethod]
    public void Add_2And3_Returns5()
    {
        // Arrange
        Calculator calc = new Calculator();

        // Act
        int result = calc.Add(2, 3);

        // Assert
        Assert.AreEqual(5, result);
    }
    [TestMethod]
    public void Add_2And3_Returns6()
    {
        // Arrange
        Calculator calc = new Calculator();

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