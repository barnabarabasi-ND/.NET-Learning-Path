
using PetShelter.Interfaces;

namespace PetShelter.Models
{
    public class Reptile : Animal, IFeedable
    {
        public bool IsVenomous { get; set; }

        public override void Speak()
        {
            Console.WriteLine($"Reptile {this.Name}: Sss!");
        }

        public override decimal DailyCareCost()
        {
            return base.DailyCareCost() + 1m;
        }

        public void Feed()
        {
            Console.WriteLine($"Reptile {this.Name} has been fed.");
        }

    }
}
