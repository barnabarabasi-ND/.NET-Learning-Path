
using VehicleManagement.Interfaces;

namespace VehicleManagement.Models
{
    internal class Truck : Vehicle, IRefuelable
    {
		public decimal CargoCapacity { get; set; }
		public override void StartEngine()
		{
			Console.WriteLine($"The truck {this.Brand} {this.Model} engine rumbles to life.");
		}

		public void Refuel()
		{
            Console.WriteLine($"The truck {this.Brand} {this.Model} is being refueled.");
		}

    }
}
