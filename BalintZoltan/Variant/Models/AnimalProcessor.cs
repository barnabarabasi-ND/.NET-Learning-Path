using Variant.Interfaces;
namespace Variant.Models;

public class AnimalProcessor : IAnimalProcessor<Animal>
{
    public void Process(Animal animal)
    {
        Console.WriteLine(animal.Name);
    }
}