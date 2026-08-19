using System;
using System.Collections.Generic;
using System.Text;

namespace VariousDemos.Models
{
    public class Logger : IDisposable
    {
        private readonly StreamWriter _writer;

        public Logger(string fileFullPath)
        {
            _writer = new StreamWriter(fileFullPath, append: true);
        }

        public void Log(string message)
        {
            _writer.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {message}");
            _writer.Flush(); //write to file immediately
        }

        public void Dispose()
        {
            _writer.Dispose();
            Console.WriteLine("Resources released (Logger).");

            GC.SuppressFinalize(this); //standard pattern; suppress finalizer, because we already released resources
        }

        //Finalizer = Destructor, nondeterministic cleanup
        //same as protected override void Finalize(), only GC calls it
        ~Logger()
        {
            _writer.Dispose();
            Console.WriteLine("Finalizer executed (Logger).");
        }

    }
}
