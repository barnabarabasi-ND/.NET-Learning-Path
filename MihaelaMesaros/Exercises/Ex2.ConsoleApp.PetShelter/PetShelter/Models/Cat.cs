
using PetShelter.Interfaces;

namespace PetShelter.Models
{
    public class Cat : Animal, IFeedable
    {
        public bool IsIndoor { get; set; }

        public override void Speak()
        {
            Console.WriteLine($"Cat {this.Name}: Meow!");
        }

        public override decimal DailyCareCost()
        {
            return base.DailyCareCost() + 2m;
        }

        public void Feed()
        {
            Console.WriteLine($"Cat {this.Name} has been fed.");
        }

    }
}
