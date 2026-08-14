using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using VariousDemos.Models;
using VariousDemos.Services;

namespace VariousDemos.Demos
{
    internal class ReflectionDynamicProgramming
    {
        public static void Run()
        {
            Console.WriteLine("=== Reflection and Dynamic Programming ===");

            Type t = typeof(Animal);
            Console.WriteLine($"typeof(Animal)={t.Name}");
            foreach (MethodInfo m in t.GetMethods())
            {
                Console.WriteLine($"Method: {m.Name}");
            }

            Assembly asm1 = Assembly.Load("System.Collections");
            Console.WriteLine($"Collections FullName={asm1.FullName}");
            foreach (Type t1 in asm1.GetTypes())
            {
                Console.WriteLine($"Collections Type: {t1.Name}");
            }

            //dynamic loading assembly during runtime, from file
            Assembly asm2 = Assembly.LoadFrom("VehicleManagement.dll");
            Console.WriteLine($"VehicleManagement FullName={asm2.FullName}");
            //or asm2.GetName().Name
            foreach (Type t2 in asm2.GetTypes()) //classes
            {
                Console.WriteLine($"VehicleManagement Type: {t2.Name}");

                foreach (MethodInfo m2 in t2.GetMethods())
                {
                    Console.WriteLine($"VehicleManagement Method: {m2.Name}");
                }
                foreach (PropertyInfo p2 in t2.GetProperties())
                {
                    Console.WriteLine($"VehicleManagement Property: {p2.Name}");
                }
                foreach (FieldInfo f2 in t2.GetFields())
                {
                    Console.WriteLine($"VehicleManagement Field: {f2.Name}");
                }
            }

            Type t3 = asm2.GetType("VehicleManagement.Models.Vehicle");
            object obj3 = Activator.CreateInstance(t3);
            MethodInfo methodInfoStartEngine = t3.GetMethod("StartEngine");
            Console.WriteLine($"Invoke StartEngine:");
            methodInfoStartEngine.Invoke(obj3, null); //Late Binding

            //using parameters:
            MethodInfo methodMath = typeof(Math).GetMethod("Max", new[] { typeof(double), typeof(double) }); //because Max is overloaded, we need to specify the parameter types
            double resultMath = (double)methodMath.Invoke(null, new object[] { 100.0, 200.0 }); //Late Binding
            Console.WriteLine($"Math Max (100,200): {resultMath}");


            //dynamic can change type during runtime, not type safe
            dynamic x = 10;
            Console.WriteLine($"1. int x={x}");
            x = "Helloo";
            Console.WriteLine($"2. string x={x}");
            x = true;
            Console.WriteLine($"3. bool x={x}");


            //System Attributes
            var typeAnimal = typeof(Animal);
            foreach (Attribute a in typeAnimal.GetCustomAttributes())
            {
                Console.WriteLine($"CustomAttributes: {a.GetType().Name}");
            }

            var animal = new Animal { Name = "Dog Spike" };
            animal.SpeakOld(); //obsolete

            //with serializable attribute
            string json = JsonSerializer.Serialize(animal);
            Console.WriteLine($"Serialize to JSON: {json}");
            //File.WriteAllText("animal.json", json); //to file

            //string json = File.ReadAllText("animal.json"); //from file
            Animal animal2 = JsonSerializer.Deserialize<Animal>(json);
            Console.WriteLine($"Deserialize from JSON: {animal2.Name}");
            

            // xml serialization
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Animal));
            //using StreamWriter writer = new StreamWriter("animal.xml"); //to file
            StringWriter writer = new StringWriter();
            xmlSerializer.Serialize(writer, animal);
            string xml = writer.ToString();
            Console.WriteLine($"Serialize to XML: {xml}");

            // xml deserialization
            //using StreamReader reader = new StreamReader("animal.xml"); //from file
            StringReader reader = new StringReader(xml);
            Animal animal3 = (Animal)xmlSerializer.Deserialize(reader);
            Console.WriteLine($"Deserialize from XML: {animal3.Name}");



            //custom attribute Log, needs to invoke the methods
            //Animal animal4 = new Animal { Name = "Rex" };
            
            //MethodInfo method5 = typeof(Animal).GetMethod("Add");
            //// get Log attribute from method
            //LogAttribute logAdd = (LogAttribute)Attribute.GetCustomAttribute(method5, typeof(LogAttribute));
            //if (logAdd != null)
            //{
            //    Console.WriteLine($"LOG: {logAdd.Message}");
            //}
            //method5.Invoke(animal4, null);

            Animal animal6 = new Animal { Name = "Rex" };
            foreach (MethodInfo method in typeof(Animal).GetMethods())
            {
                // get Log attribute from method
                LogAttribute log = (LogAttribute)Attribute.GetCustomAttribute(method, typeof(LogAttribute));

                if (log != null)
                {
                    Console.WriteLine($"LOG: {log.Message}");
                    method.Invoke(animal6, null);
                }
            }






        }
    }
}
