using System.Reflection;

string assemblyPath =
    @"C:\Endava\EndevLocal\App\Dot_Net\Reflection_and_Dynamic_Programming\Reflection\bin\Debug\net10.0\Reflection.dll";

if (!File.Exists(assemblyPath))
{
    Console.WriteLine($"The file was not found: {assemblyPath}");
    return;
}

Assembly asm = Assembly.LoadFrom(assemblyPath);

Type[] types = asm.GetTypes();
if (types.Length == 0)
{
    Console.WriteLine("No types found.");
    return;
}

Console.WriteLine("List of types from Reflection.dll");
foreach (Type i in types)
{
    Console.WriteLine("    " + i.Name);
}

Type? t = asm.GetType("Person");        // Reflection.Person
if (t == null)
{
    Console.WriteLine("Person type not found.");
    return;
}

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


object obj = Activator.CreateInstance(t);
if (obj == null)
{
    Console.WriteLine("Person object could not be created.");
    return;
}

Console.WriteLine("Method call:");
MethodInfo? method = t.GetMethod("SayHello");
if (method == null)
{
    Console.WriteLine("SayHello method not found.");
    return;
}
method.Invoke(obj, null);

PropertyInfo? property = t.GetProperty("Name");
if (property == null)
{
    Console.WriteLine("Name property not found.");
    return;
}

Console.WriteLine("Property access:");
var value = property.GetValue(obj);
Console.WriteLine($"Property name : {value}");
property.SetValue(obj, "John");
value = property.GetValue(obj);
Console.WriteLine($"Property name : {value}");
