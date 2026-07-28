using Variant.Models;

namespace Variant.Interfaces;
public interface IAnimalProcessor<in T>
{
    void Process(T animal);
}
