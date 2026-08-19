using System;
using System.Collections.Generic;
using System.Text;

namespace VariousDemos.Services
{
    //event publisher class
    public class EventPublisher
    {
        //publisher declares the event and invokes it when the work is done
        public event Action? Finished;

        public void DoWork1()
        {
            Console.WriteLine("Working 1...");

            //publisher raises the 
            Finished?.Invoke();
        }

        public void DoWork2()
        {
            Console.WriteLine("Working 2...");

            //publisher raises the 
            Finished?.Invoke();
        }
    }
}
