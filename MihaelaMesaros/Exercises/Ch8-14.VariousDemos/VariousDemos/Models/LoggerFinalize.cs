using System;
using System.Collections.Generic;
using System.Text;

namespace VariousDemos.Models
{
    public class LoggerFinalize
    {
        private readonly StreamWriter _writer;

        public LoggerFinalize(string fileFullPath)
        {
            _writer = new StreamWriter(fileFullPath, append: true);
        }

        public void Log(string message)
        {
            _writer.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {message}");
            _writer.Flush(); //write to file immediately
        }


        //Finalizer = Destructor, nondeterministic cleanup
        //same as protected override void Finalize(), only GC calls it
        ~LoggerFinalize()
        {
            _writer.Dispose();
            Console.WriteLine("Finalizer executed (LoggerFinalize).");
        }

    }
}
