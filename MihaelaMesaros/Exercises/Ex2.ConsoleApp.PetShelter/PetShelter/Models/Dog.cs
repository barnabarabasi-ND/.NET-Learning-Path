
using PetShelter.Interfaces;

namespace PetShelter.Models
{
    public class Dog : Animal, IFeedable
    {
        public bool IsTrained { get; set; }

        public override void Speak()
        {
            Console.WriteLine($"Dog {this.Name}: Woof!");
        }

        public override decimal DailyCareCost()
        {
            return base.DailyCareCost() + 3m;
        }

        public void Feed()
        {
            Console.WriteLine($"Dog {this.Name} has been fed.");
        }

    }
}
