

using System.Reflection;

Type t = typeof(string);
MethodInfo[] methods = t.GetMethods();
PropertyInfo[] properties = t.GetProperties();
FieldInfo[] fields = t.GetFields();
ConstructorInfo[] constructors = t.GetConstructors();

Console.WriteLine($"Name of the type:{t.Name}");
Console.WriteLine($"Namespace of the type:{t.Namespace}");
Console.WriteLine($"BaseType of the type:{t.BaseType}");

Console.WriteLine("Methods:");
    for (int i = 0; i < methods.Length; i++)
    {
        if (i > 10)
        {
            Console.WriteLine("    ...");
            break;
        }
        Console.WriteLine("    " + methods[i].Name);
    }

Console.WriteLine("Properties:");
    for (int i = 0; i < properties.Length; i++)
    {
        if (i > 10)
        {
            Console.WriteLine("    ...");
            break;
        }
        Console.WriteLine("    " + properties[i].Name);
    }

Console.WriteLine("Fields:");
    for (int i = 0; i < fields.Length; i++)
    {
        if (i > 10)
        {
            Console.WriteLine("    ...");
            break;
        }
        Console.WriteLine("    " + fields[i].Name);
    }

Console.WriteLine("Constructors:");
    for (int i = 0; i < constructors.Length; i++)
    {
        if (i > 10)
        {
            Console.WriteLine("    ...");
            break;
        }
        Console.WriteLine("    " + constructors[i].Name);
    }

Type t1 = typeof(Person);
object obj = Activator.CreateInstance(t1);
MethodInfo method = t1.GetMethod("SayHello");
PropertyInfo property = t1.GetProperty("Name");
method.Invoke(obj, null);

var value = property.GetValue(obj);
Console.WriteLine($"Property name : {value}");
property.SetValue(obj, "John");
value = property.GetValue(obj);
Console.WriteLine($"Property name : {value}");


class Person
{
    public string Name { get; set; } = "Joe";
    public void SayHello()
    {
        Console.WriteLine("Hello");
    }
}