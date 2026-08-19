
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VariousDemos.Models;

namespace VariousDemos.Services
{
    [Serializable]
    [DescriptionAnimal("Represents an animal")]
    //[DescriptionAnimal("AllowMultiple = true")]
    [JsonDerivedType(typeof(Dog), typeDiscriminator: "dog")] //type discriminator in JSON serialization, because Dog is derived from Animal
    [JsonDerivedType(typeof(Cat), typeDiscriminator: "cat")]
    public class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public virtual string Speak() { return string.Empty; }


        [Obsolete("This method is obsolete")]
        public virtual string SpeakOld() {
            return $"Animal {this.Name} speaks.";
        }


        [Log("Animal has been added")]
        public void Add()
        {
            //implement here add animal logic

            Console.WriteLine($"{DateTime.Now} Added animal {this.Name}.");
        }

        [Log("Animal has been deleted")]
        public void Delete() {
            //implement here delete animal logic

            Console.WriteLine($"{DateTime.Now} Deleted animal {this.Name}.");
        }
    }
}
