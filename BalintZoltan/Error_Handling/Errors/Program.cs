using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

Console.WriteLine();
Console.WriteLine("*******************");
Console.WriteLine("  Exception datas   ");
Console.WriteLine("*******************");
Console.WriteLine();

try
{
    int zero = 0;
    Console.WriteLine("Start");
    int x = 10 / zero;
    Console.WriteLine("Error");
}
catch
{
    Console.WriteLine("Catched");
}

Console.WriteLine("Continue");

// Exeption as a Object
try
{
    try
    {
        int.Parse("abc");
    }
    catch (Exception ex)
    {
        throw new Exception("Parse error", ex);
    }
}
catch (Exception ex)
{
    Console.WriteLine("[Message]: "+ex.Message);
    Console.WriteLine("[StackTrace]: "+ex.StackTrace);
    Console.WriteLine("[Source]: "+ex.Source);
    Console.WriteLine("[Inner message]: "+ex.InnerException.Message);
}


// Exception filtering - when
Console.WriteLine();
Console.WriteLine("*******************");
Console.WriteLine("Exception filtering");
Console.WriteLine("*******************");
Console.WriteLine();
try
{
    //Console.WriteLine(@"int.Parse(""abc"");");
    //int.Parse("abc");

    Console.WriteLine(@"int.Parse(""999999999999999"");");
    int.Parse("999999999999999");

}
catch (FormatException ex) when (DateTime.Now.Hour < 12)
{
    Console.WriteLine("Error before 12:00");
}
catch (FormatException ex) when (ex.Message.Contains("format"))
{
    Console.WriteLine("Format error");
}
catch (FormatException)
{
    Console.WriteLine("General format error.");
}
catch (OverflowException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

// Thruw
Console.WriteLine();
Console.WriteLine("*******************");
Console.WriteLine("      Throw        ");
Console.WriteLine("*******************");
Console.WriteLine();



//Console.WriteLine("Start");
//throw new Exception("Something happend . . .");
//Console.WriteLine("End");                                                 // Never executed

//Withdraw(-10);                                                            // No .Net error. Self made Exception.

//string name = "Joe";
//throw new ArgumentException("");
//throw new ArgumentOutOfRangeException("");
//throw new ArgumentNullException("");
//throw new InvalidOperationException("");
//throw new NotImplementedException(nameof(name));                            // In development phase
                                                                            // nameof() usefull in case of variable rename , auto update the messsage text.            

void Withdraw(decimal amount)
{
    if (amount <= 0)
    {
        throw new ArgumentException("The amount is negative.");
    }
}