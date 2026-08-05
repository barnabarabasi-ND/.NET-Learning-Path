Console.WriteLine("Hello, World!");

File.WriteAllText("test.txt", "Hello New File");

// FileStream
// Using . . . 
using (FileStream fs =
    new FileStream(
        "test.txt",
        FileMode.Open))
{
    byte[] buffer = new byte[100];
    int count = fs.Read(buffer, 0, buffer.Length);
    Console.WriteLine("The file contain " + count + " byte(s)");
}
//fs.Close();                                                           // Using automatically close the unused resource.
File.Delete("test.txt");

using StreamWriter writer =
    new StreamWriter("data.txt");
writer.WriteLine("Hello");
writer.WriteLine("World");
writer.Close();                                                         // Need Close() because the using part is not finished before next using
                                                                        // The stream remain opened.

using StreamReader reader =
    new StreamReader("data.txt");
string text = reader.ReadToEnd();
Console.WriteLine("The file contain: " + text);
reader.Close();                                                         // Need Close() because the using part is not finished before next using
                                                                        // The stream remain opened.

File.Delete("data.txt");


// MemoryStream
Console.WriteLine("MemoryStream");
Console.WriteLine("------------");
Console.WriteLine();

using MemoryStream ms = new MemoryStream();

byte[] data = { 1, 2, 3, 4, 5 };

ms.Write(data, 0, data.Length);

Console.Write("The MemoryStream contain:");
ms.Position = 0;
// OR ms.Seek(0, SeekOrigin.Begin);
foreach (byte b in ms.ToArray())
{
    Console.Write($"{b} ");
}
Console.Write("The MemoryStream contain:");
for (int i = 0;i<ms.Length/2+1;i++)
{
    Console.Write($"{ms.ReadByte()} ");
    ms.Seek(ms.Position+1, SeekOrigin.Begin);
    //ms.Seek(1, SeekOrigin.Current);
}
