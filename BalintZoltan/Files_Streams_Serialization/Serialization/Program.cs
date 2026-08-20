using System.Xml.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

Person p = new Person
{
    Name = "Anna",
    Age = 25
};
Console.WriteLine(@"
/**************/
/*    XML     */
/**************/
");

// Save object to XML file
XmlSerializer serializer =
    new XmlSerializer(typeof(Person));                      // Working only with FileStream

using FileStream fs =
    new FileStream("person.xml", FileMode.Create);

serializer.Serialize(fs, p);

fs.Close();

// Restore object from XML file

using FileStream fs1 =
    new FileStream("person.xml", FileMode.Open);

Person p1 =
    (Person)serializer.Deserialize(fs1);                    //  Casting from object to Person

fs1.Close();

Console.WriteLine($"Original Person name: {p.Name}, Original Person age:{p.Age}");
Console.WriteLine($"Restored Person name: {p1.Name}, Restored Person age:{p1.Age}");

string text = File.ReadAllText("person.xml");
Console.WriteLine("XML file contain:");
Console.WriteLine(text);

File.Delete("person.xml");


Console.WriteLine(@"
/**************/
/*    JSON    */
/**************/
");

string json =
    JsonConvert.SerializeObject(p);
    //string json1 =
    //    JsonSerializer.Serialize(person);
Console.WriteLine("JSON from object:");
Console.WriteLine(json);
File.WriteAllText("person.json", json);

string jsonFromFile =
    File.ReadAllText("person.json");

Person person =
    JsonConvert.DeserializeObject<Person>(jsonFromFile);                // No cast needed. Generic DeserializeObject<> used
                                                                        //Person person =
                                                                        //    JsonSerializer.Deserialize<Person>(json);
Console.WriteLine("JSON from file:");
Console.WriteLine(jsonFromFile);


public class Person
{
    public string? Name { get; set; }                       // public for serialization
    public int Age { get; set; }                            // public for serialization
}