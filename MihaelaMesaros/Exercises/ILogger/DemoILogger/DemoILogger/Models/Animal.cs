
namespace DemoILogger.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public virtual string Speak() { return string.Empty; }

    }
}
