
namespace VariousDemos.Services
{
    public class Dog : Animal
    {
        public bool IsTrained { get; set; }

        public override string Speak()
        {
            return $"Dog {this.Name}: Woof!";
        }

    }
}
