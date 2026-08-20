
namespace DemoILogger.Models
{
    public class Cat : Animal
    {
        public bool IsIndoor { get; set; }

        public override string Speak()
        {
            return $"Cat {this.Name}: Meow!";
        }
    }
}
