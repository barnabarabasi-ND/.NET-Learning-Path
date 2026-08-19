using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Reflection.PortableExecutable;
using System.Text;

namespace VariousDemos.Demos
{
    public class FilesStreamsSerialization
    {
        private static string _samplePath = @"C:\Endava\EndevLocal\Learning\SamplePath.txt";
        private static string _fileFullPath = @"C:\Endava\EndevLocal\Learning\SampleFile.txt";
        private static string _fileFullPathStream = @"C:\Endava\EndevLocal\Learning\SampleLogStream.txt";
        private static string _fileFullPathBinary = @"C:\Endava\EndevLocal\Learning\SampleBinary.bin";
        private static string _fileFullPathBinarySeek = @"C:\Endava\EndevLocal\Learning\SampleBinarySeek.bin";
        private static string _fileLorem = @"C:\Endava\EndevLocal\Learning\LoremIpsum.txt";
        private static string _fileSpecialChar = @"C:\Endava\EndevLocal\Learning\FileSpecialChar.txt";

        public static void Run() {
        //public static async Task Run() {

            string fileName = Path.GetFileName(_samplePath);
            Console.WriteLine($"GetFileName: {fileName}");
            Console.WriteLine($"HasExtension: {Path.HasExtension(_samplePath)}");
            Console.WriteLine($"GetExtension: {Path.GetExtension(_samplePath)}");
            Console.WriteLine($"IsPathFullyQualified: {Path.IsPathFullyQualified(_samplePath)}");
            Console.WriteLine($"IsPathRooted: {Path.IsPathRooted(_samplePath)}");
            Console.WriteLine($"GetPathRoot: {Path.GetPathRoot(_samplePath)}");
            Console.WriteLine($"GetDirectoryName: {Path.GetDirectoryName(_samplePath)}");
            Console.WriteLine($"GetFileNameWithoutExtension: {Path.GetFileNameWithoutExtension(_samplePath)}");
            Console.WriteLine($"ChangeExtension: {Path.ChangeExtension(_samplePath, ".dll")}"); //just in string, not the file extension
            Console.WriteLine($"GetFileNameWithoutExtension: {Path.GetFileNameWithoutExtension(_samplePath)}");
            Console.WriteLine($"Exists: {Path.Exists(_samplePath)}");

            string newSamplePath = Path.Combine(@"c:\", "Endava", "EndevLocal", "Learning", "NewSamplePath.txt");
            Console.WriteLine($"newSamplePath: {newSamplePath}");

            Console.WriteLine($"GetTempPath: {Path.GetTempPath()}");
            Console.WriteLine($"GetRandomFileName: {Path.GetRandomFileName()}");
            Console.WriteLine($"GetTempPath: {Path.GetTempPath()}");
            Console.WriteLine($"GetRelativePath: {Path.GetRelativePath(@"c:\Endava\EndevLocal\Learning", @"c:\Endava\EndevLocal\Learning\SampleFolder\NewSamplePath.txt")}");
            Console.WriteLine($"TrimEndingDirectorySeparator: {Path.TrimEndingDirectorySeparator(@"c:\Endava\")}");

            Console.WriteLine($"GetCurrentDirectory: {Directory.GetCurrentDirectory()}"); //or Environment.CurrentDirectory, where the process runs now
            Console.WriteLine($"BaseDirectory: {AppContext.BaseDirectory}"); //where is the application -> better

            //display folder structure
            ShowDirectoryStructure(Path.Combine(AppContext.BaseDirectory, "..", ".."), 0);


            File.WriteAllText(_fileFullPath, $"First line{Environment.NewLine}");
            string[] lines = {"Line 1","Line 2","Line 3"};
            File.AppendAllLines(_fileFullPath, lines);
            byte[] data = { 65, 66, 67, 68 };
            File.AppendAllBytes(_fileFullPath, data.Concat(Encoding.UTF8.GetBytes(Environment.NewLine)).ToArray());

            ////or async:
            //await File.AppendAllTextAsync(_fileFullPath, $"First line async{Environment.NewLine}");
            //await File.AppendAllLinesAsync(_fileFullPath, lines);
            //string text = await File.ReadAllTextAsync(_fileFullPath);

            string text = File.ReadAllTextAsync(_fileFullPath).Result;
            Console.WriteLine($"File content: {text}");


            Console.WriteLine("=============================");
            Console.WriteLine($"Working with FileStream and BinaryReader/BinaryWriter");
            // FileStream works with bytes.
            using (FileStream fs = File.Create(_fileFullPathBinary))
            {
                // BinaryWriter converts C# primitive types to their binary representation.
                using (BinaryWriter writer = new BinaryWriter(fs, Encoding.UTF8, leaveOpen: true))
                {
                    writer.Write(25);
                    writer.Write(3.14);
                    writer.Write(true);
                    writer.Write("John");

                    writer.Flush(); //flush the buffer to the underlying stream
                }
                // writer is disposed, but the underlying stream is still open, so we can work with the same stream, but we'll need to reset the position to the beginning of the stream

                Console.WriteLine($"Position after writing: {fs.Position}");

                //go back to the beginning
                fs.Position = 0;

                //read raw bytes
                var buffer = new byte[fs.Length];
                fs.ReadExactly(buffer);
                fs.Seek(sizeof(int), SeekOrigin.Begin);
                Console.WriteLine($"Buffer: {string.Join(" ", buffer.Select(b => $"{b:X2}"))}");


                //go to the beginning
                fs.Position = 0;

                //set position in stream
                fs.Seek(sizeof(int), SeekOrigin.Begin); //relative to beginning of stream, position=4=no of bytes of int
                //fs.Seek(5, SeekOrigin.Current); //relative to current position
                //fs.Seek(-5, SeekOrigin.Current); //go back
                //fs.Seek(-10, SeekOrigin.End);

                //read the same data using BinaryReader
                //BinaryWriter disposes the stream, sowe need to use leaveOpen=true
                using (BinaryReader reader = new BinaryReader(fs, Encoding.UTF8, leaveOpen: true))
                {
                    //int age = reader.ReadInt32();
                    double value = reader.ReadDouble();
                    bool active = reader.ReadBoolean();
                    string name = reader.ReadString();

                    Console.WriteLine($"BinaryReader: {value}, {active}, {name}");
                }
                //stream is still open here too

                //verify what stream can do
                Console.WriteLine($"CanRead: {fs.CanRead}");
                Console.WriteLine($"CanWrite: {fs.CanWrite}");
                Console.WriteLine($"CanSeek: {fs.CanSeek}");

                fs.SetLength(100); //set stream length, will be truncated if larger
                fs.Flush(); //flush the buffer to the stream, usually it is done on dispose
            } //stream disposed


            Console.WriteLine("\nSeek:");

            //file stream with file mode and access
            using FileStream fsSeek = new(_fileFullPathBinarySeek, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //FileMode.Create / FileMode.CreateNew / FileMode.Open / FileMode.OpenOrCreate / FileMode.Truncate / FileMode.Append
            //FileAccess.Read / FileAccess.Write / FileAccess.ReadWrite

            //write some integers, BinaryWriter recognizes the type and writes the binary representation of the integer to the stream.
            using BinaryWriter writerSeek = new BinaryWriter(fsSeek, Encoding.UTF8, leaveOpen: true);
            
                writerSeek.Write(10); //bytes 0-3
                writerSeek.Write(20); //bytes 4-7
                writerSeek.Write(30); //bytes 8-11
                writerSeek.Write(40); //bytes 12-15
            

            Console.WriteLine($"Length: {fsSeek.Length}");
            Console.WriteLine($"Position: {fsSeek.Position}");

            //go to the beginning
            fsSeek.Seek(0, SeekOrigin.Begin);

            //read bytes until the end of the stream
            int valByte;
            while ((valByte = fsSeek.ReadByte()) != -1)
            {
                string binary = Convert.ToString(valByte, 2).PadLeft(8, '0');
                Console.WriteLine($"Decimal: {valByte,3} | Hex: {valByte:X2} | Binary: {binary}");
            }
            Console.WriteLine($"Position: {fsSeek.Position}");

            using BinaryReader readerSeek = new BinaryReader(fsSeek, Encoding.UTF8, leaveOpen: true);
            
                fsSeek.Seek(0, SeekOrigin.Begin);

                Console.WriteLine(readerSeek.ReadInt32()); // 10
                Console.WriteLine(readerSeek.ReadInt32()); // 20
                Console.WriteLine(readerSeek.ReadInt32()); // 30
                Console.WriteLine(readerSeek.ReadInt32()); // 40
                //readerSeek.ReadString, readerSeek.ReadDouble, readerSeek.ReadBoolean, etc.
                Console.WriteLine($"Position: {fsSeek.Position}");

                fsSeek.Seek(0, SeekOrigin.Begin);

                //go to 2nd integer
                fsSeek.Seek(sizeof(int) * 2, SeekOrigin.Begin);
                Console.WriteLine(readerSeek.ReadInt32()); // 30

                writerSeek.Write(99); //overwrite the 3rd integer with 99


            //read large files in chunks, not all at once
            Console.WriteLine("");
            Console.WriteLine("Read from large file:");
            
            using var fsLarge = new FileStream(_fileSpecialChar, FileMode.Open, FileAccess.Read);

            Console.WriteLine("----- Byte buffer:");
            //read bytes not ok for special characters, because they can be multi-byte and might split in the middle of a character
            byte[] bufferLarge = new byte[1024]; //read 1KB at a time
            int bytesRead;
            fsLarge.Position = 0;
            while ((bytesRead = fsLarge.Read(bufferLarge, 0, bufferLarge.Length)) > 0)
            {
                string textRead = Encoding.UTF8.GetString(bufferLarge, 0, bytesRead);
                Console.WriteLine($"---{textRead}");
            }

            //so we need to use StreamReader instead of FileStream
            using StreamReader readerLarge = new StreamReader(fsLarge);

            //with buffer:
            Console.WriteLine("----- Char buffer:");
            char[] charBufferLarge = new char[1024]; //read 1KB at a time
            int charsRead;
            fsLarge.Position = 0;
            while ((charsRead = readerLarge.Read(charBufferLarge, 0, charBufferLarge.Length)) > 0)
            {
                Console.WriteLine($"---{new string(charBufferLarge, 0, charsRead)}");
            }

            //read line by line:
            Console.WriteLine("----- By line:");
            string? lineLarge;
            fsLarge.Position = 0;
            while ((lineLarge = readerLarge.ReadLine()) != null)
            {
                Console.WriteLine($"---{lineLarge}");
            }

            

        }


        static void ShowDirectoryStructure(string path, int level = 0)
        {
            string indent = new string(' ', level * 10);

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            

            //current folder
            Console.WriteLine($"{indent}>{Path.GetFileName(path)}");
            Console.WriteLine($"{indent}Attributes: {dirInfo.Attributes}");
            Console.WriteLine($"{indent}CreationTime: {dirInfo.CreationTime}");
            Console.WriteLine($"{indent}LastWriteTime: {dirInfo.LastWriteTime}");
            //Console.WriteLine($"{indent}{dirInfo.Attributes}");

            //current files from current folder
            foreach (string file in Directory.GetFiles(path))
            {
                var fileInfo = new FileInfo(file);
                Console.WriteLine($"{indent} -{Path.GetFileName(file)}");
                Console.WriteLine($"{indent}  Attributes: {fileInfo.Attributes}");
                Console.WriteLine($"{indent}  Length: {(fileInfo.Length / 1024.0):F2} KB");
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                ShowDirectoryStructure(directory, level + 1);
            }
        }

    }
}
