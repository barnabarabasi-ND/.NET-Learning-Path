
namespace PetShelter.Models
{
    public abstract class Animal
    {
        public int Id { get; internal set; } // readable from anywhere, settable only within the same assembly
        public required string Name { get; set; }
        public int Age { get; set; }
        public DateTime IntakeDate { get; set; }

        public abstract void Speak();
        public virtual decimal DailyCareCost() => 5m;
    }
}
