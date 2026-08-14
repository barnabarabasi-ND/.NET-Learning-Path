
namespace VariousDemos.Demos
{
    internal class MultithreadingAsyncronous
    {

        public static void Run()
        {
            int counterRaceCond = 0;
            object locker = new object();

            var mainThreadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Main Thread #{mainThreadId}");
            Console.WriteLine("");

            Thread thread1 = new Thread(() =>
            {
                Console.WriteLine($"Created Thread1 #{Thread.CurrentThread.ManagedThreadId}.");
                //this is a thread created manually, not from the thread pool
                Console.WriteLine($"Thread1 IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");

                //some method here to run in this thread...

                for (int i = 0; i < 100000; i++)
                {
                    counterRaceCond++;
                }

                Thread.Sleep(2000);

                Console.WriteLine("Thread1 finished.");
            });

            //the following run on main thread
            var thread1Id = thread1.ManagedThreadId;
            thread1.Start(); // run the thread
            Console.WriteLine($"Start Thread1 #{thread1Id}");
            Console.WriteLine($"Thread1 IsAlive: {thread1.IsAlive}");

            // main thread waits for thread1 to finish
            thread1.Join();

            Console.WriteLine("");
            Thread thread2 = new Thread(() =>
            {
                Console.WriteLine($"Created Thread2 #{Thread.CurrentThread.ManagedThreadId}.");
                //some method here to run in this thread...

                for (int i = 0; i < 100000; i++)
                {
                    counterRaceCond++;
                }

                Console.WriteLine("Thread2 finished.");
            });

            //the following run on main thread
            var thread2Id = thread2.ManagedThreadId;
            thread2.Start();
            Console.WriteLine($"Start Thread2 #{thread2Id}");
            Console.WriteLine($"Thread2 IsAlive: {thread2.IsAlive}"); //can be false really fast if the thread finishes quickly

            // main thread waits for thread2 to finish
            thread2.Join();
            Console.WriteLine("Thread2 joined.");

            Thread.Sleep(1000);
            Console.WriteLine("");
            Console.WriteLine("Main Thread continues after sleep.");

            Console.WriteLine("Both manual threads finished.");
            Console.WriteLine($"No race condition because Thread1 and Thread2 run sequentially with Join: {counterRaceCond}");
            Console.WriteLine("");

            // put in queue of thread pool some work to do, it will be executed by an available thread from the pool
            ThreadPool.QueueUserWorkItem(param =>
            {
                //some method here to run in this thread pool thread...

                //this is another thread from the pool, not the main thread
                Console.WriteLine($"ThreadPool work on thread #{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Param={param}");

                //check if this thread is from the thread pool
                Console.WriteLine($"Is ThreadPool thread: {Thread.CurrentThread.IsThreadPoolThread}");

            }, "SomeDataToPassToThreadPool");

            // Keep console app alive long enough to see ThreadPool work
            Thread.Sleep(1000);

            Console.WriteLine("End");

            //ThreadPool.GetAvailableThreads
            //ThreadPool.ThreadCount
            //ThreadPool.RegisterWaitForSingleObject


            //Race condition
            counterRaceCond = 0;

            Thread thread3 = new Thread(() =>
            {
                Console.WriteLine(
                    $"Thread3 #{Thread.CurrentThread.ManagedThreadId} started."
                );

                for (int i = 0; i < 100000; i++)
                {
                    //Thread.Yield();
                    //counterRaceCond++;

                    ////fix race condition with lock
                    //lock (locker)
                    //{
                    //    //if there is another thread, give it chnce to run, for testing race condition
                    //    Thread.Yield();

                    //    counterRaceCond++;
                    //}

                    ////protect one operation - increment
                    Thread.Yield();
                    Interlocked.Increment(ref counterRaceCond);
                }

                Console.WriteLine("Thread3 finished.");
            });


            Thread thread4 = new Thread(() =>
            {
                Console.WriteLine(
                    $"Thread4 #{Thread.CurrentThread.ManagedThreadId} started."
                );

                for (int i = 0; i < 100000; i++)
                {
                    //Thread.Yield();
                    //counterRaceCond++;

                    ////fix race condition with lock
                    //lock (locker)
                    //{
                    //    //if there is another thread, give it chnce to run, for testing race condition
                    //    Thread.Yield();

                    //    counterRaceCond++;
                    //}

                    ////protect one operation - increment
                    Thread.Yield();
                    Interlocked.Increment(ref counterRaceCond);
                }

                Console.WriteLine("Thread4 finished.");
            });


            //start both threads at the same time, they will run concurrently and cause a race condition
            thread3.Start();
            thread4.Start();

            //wait for both threads to finish, otherwise the main thread will continue
            thread3.Join();
            thread4.Join();

            Console.WriteLine(
                $"Expected: 200000, Actual: {counterRaceCond}"
            );

            //lock(locker)
            //{
            //    counter++;
            //}


            //Task.Run(() =>
            //{
            //    Console.WriteLine("Task running");
            //});


        }
    }
}
