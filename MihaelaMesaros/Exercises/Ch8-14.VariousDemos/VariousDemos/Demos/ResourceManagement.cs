using System;
using System.Collections.Generic;
using System.Text;
using VariousDemos.Models;
using VariousDemos.Services;
using static System.Net.Mime.MediaTypeNames;

namespace VariousDemos.Demos
{
    internal class ResourceManagement
    {
        public static void Run() 
        {
            Console.WriteLine("=== Resource Management ===");

            var dog = new Dog();
            dog = null; // no references, eligible for GC
            GC.Collect(); // force garbage collection

            GC.WaitForPendingFinalizers(); // wait for finalizers to complete


            //using - for objects which implement IDisposable; the object resources must be released deterministic
            //FileStream,StreamReader,StreamWriter,SqlConnection,SqlCommand,SqlDataReader,MemoryStream,HttpResponseMessage,CancellationTokenSource,SemaphoreSlim,Bitmap / Image
            //using var writer = new StreamWriter("log.txt");
            using (StreamReader reader = new StreamReader("C:\\Endava\\EndevLocal\\Learning\\SampleLog.txt"))
            {
                reader.ReadLine();
            }
            //reader.Dispose() called automatically, even with exception

            //finalizer
            var loggerFinalize = new LoggerFinalize("C:\\Endava\\EndevLocal\\Learning\\SampleLog2.txt");
            Console.WriteLine("LoggerFinalize created");
            loggerFinalize.Log("Some message from LoggerFinalize");
            loggerFinalize = null;
            GC.Collect(); //collect objects and store in queue for finalization
            GC.WaitForPendingFinalizers(); //the finalizer thread is blocked until is completed
            Console.WriteLine("LoggerFinalize end");
            GC.Collect();
            //file is still locked



            //using and dispose
            using var logger = new Logger("C:\\Endava\\EndevLocal\\Learning\\SampleLog.txt");
            Console.WriteLine("Logger created");
            logger.Log("Some message from Logger");
            //logger.Dispose when exiting the scope, even with exception


            



            //Platform Invoke (P/Invoke) is used to call unmanaged code from managed code
            MessageBoxInvoke.MessageBox(IntPtr.Zero, "This is a message box opened with P/Invoke", "P/Invoke sample", 0x00 | 0x30);

            //unsafe code needs to be compiled with /unsafe option, allows pointer manipulation and direct memory access
            //works with pointers directly, usually for native libraries c/c++, processing images, audio, video, etc. where performance is critical
            //unsafe
            //{
            //    int number = 10;
            //    int* pointer = &number; // pointer to the memory address of number
            //    Console.WriteLine($"*pointer: {*pointer}");
            //}

        }

    }
}
