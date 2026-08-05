Console.WriteLine("Hello, World!");

//File
File.WriteAllText("test.txt", "Hello, New file!");
var text = File.ReadAllText("test.txt");
Console.WriteLine(text);
File.Delete("test.txt");
Console.WriteLine("New file deleted.");
try
{
    text = File.ReadAllText("test.txt");
}
catch (FileNotFoundException)
{
    Console.WriteLine("[EX] File not found!");
}
if (!File.Exists("test.txt"))
{
    Console.WriteLine("File not found!");
}

// Directory
Directory.CreateDirectory("Data");
Console.WriteLine("Hello, Data folder!");
Console.WriteLine("Data folder exists : " + Directory.Exists("Data"));
Directory.Delete("Data");
Console.WriteLine("Data folder deleted.");
Console.WriteLine("Data folder exists : "+Directory.Exists("Data"));

string temp = Path.GetTempPath();
Console.WriteLine("Temp folder: "+ temp);

// Path
string path = Path.Combine(temp,"First", "file.txt");
Console.WriteLine("Temp folder with file: " + path);
Console.WriteLine("File name : " + Path.GetFileName(path));
Console.WriteLine("File name without extension : " + Path.GetFileNameWithoutExtension(path));
Console.WriteLine("File extension : " + Path.GetExtension(path));
Console.WriteLine("Directory name : " + Path.GetDirectoryName(path));


