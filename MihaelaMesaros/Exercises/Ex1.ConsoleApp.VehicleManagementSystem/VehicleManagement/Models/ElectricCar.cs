
namespace VehicleManagement.Models
{
    public class ElecticCar : Car
    {
		public int BatteryRange { get; set; }

		public override void StartEngine()
		{
			Console.WriteLine($"The electric car {this.Brand} {this.Model} starts from button.");
		}
	}
}
