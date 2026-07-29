
using PetShelter.Interfaces;

namespace PetShelter.Models
{
    public class Bird : Animal, IFeedable, IFlyable
    {
        public double WingSpanCm { get; set; }

        public override void Speak()
        {
            Console.WriteLine($"Bird {this.Name}: Chrip!");
        }

        public override decimal DailyCareCost()
        {
            return base.DailyCareCost() + 1m;
        }

        public void Feed()
        {
            Console.WriteLine($"Bird {this.Name} has been fed.");
        }

        public void Fly()
        {
            Console.WriteLine($"Bird {this.Name} flies with wing span {this.WingSpanCm} cm.");
        }

    }
}
