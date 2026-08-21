
namespace DemoILogger.Models
{
    public class Animal
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int Age { get; set; }

        public virtual string Speak() { return string.Empty; }

    }
}
