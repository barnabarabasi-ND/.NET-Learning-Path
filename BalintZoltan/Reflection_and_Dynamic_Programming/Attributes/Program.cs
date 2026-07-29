using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

Person person = new Person();
person.Name = "John";
Print(person);


[Conditional("DEBUG")]
static void Print(Person person)
{
    Console.WriteLine($"The Person name: {person.Name}");
}


var attr = typeof(Person).GetCustomAttribute<AuthorAttribute>();

if (attr != null)
{
    Console.WriteLine($"The Attribute name:{attr.Name}");
    Console.WriteLine($"The Attribute name:{attr.Version}");
    Console.WriteLine($"The Attribute name:{attr.Company}");
}


[Obsolete]
//[Author("Peter", "2.1", Company = "Endava")]
[Author("Peter", Version = "2.1", Company = "Endava")]      // <- "Peter" is mandatory , because constructor : public AuthorAttribute(string name)
class Person
{
    [NonSerialized]
    //[Author("Peter")]                                     // Only for class type
    private int age;
    [Required]
    public string? Name { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
class AuthorAttribute : Attribute
{
    public string? Name;
    public string? Version;
    public string? Company
    {
        get;
        set;
    }

    public AuthorAttribute(string name)
    {
        Name = name;
    }
}