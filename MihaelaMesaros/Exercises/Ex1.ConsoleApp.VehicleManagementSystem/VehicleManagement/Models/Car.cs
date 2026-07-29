
using VehicleManagement.Interfaces;

namespace VehicleManagement.Models
{
    public class Car : Vehicle, IDriveable
    {
		public int NumberOfDoors { get; set; }

		public override void StartEngine()
		{
			Console.WriteLine($"The car {this.Brand} {this.Model} starts with a key.");
		}

		public void Drive() 
		{
			Console.WriteLine($"The car {this.Brand} {this.Model} is driving on the road.");
		}
	}
}
