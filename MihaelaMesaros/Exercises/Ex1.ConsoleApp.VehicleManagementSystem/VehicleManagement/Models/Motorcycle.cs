
namespace VehicleManagement.Models
{
    internal class Motorcycle : Vehicle
    {
		public bool HasSidecar { get; set; }
		public override void StartEngine()
		{
			Console.WriteLine($"The motorcycle {this.Brand} {this.Model} engine starts with a button.");
		}

	}
}
