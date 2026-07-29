
using System.Text.Json.Serialization;

namespace VehicleManagement.Models
{
	//for using JSON poplymorfic serialization
	[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")] //discriminator type used in JSON, for derived objects, to be correctly restored on deserialization
	[JsonDerivedType(typeof(Car), "car")]
    [JsonDerivedType(typeof(Motorcycle), "motorcycle")]
    [JsonDerivedType(typeof(Truck), "truck")]
    [JsonDerivedType(typeof(ElecticCar), "electiccar")]
    public class Vehicle
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
		public virtual void StartEngine()
		{
			Console.WriteLine("The vehicle engine starts.");
		}

	}
}
