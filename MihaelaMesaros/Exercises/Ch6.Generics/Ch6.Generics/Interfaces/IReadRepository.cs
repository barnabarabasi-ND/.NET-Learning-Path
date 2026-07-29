using Ch6.Generics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ch6.Generics.Interfaces
{
    //variant generic interface, with covariant type parameter "out T", allows assignment of a more derived type (Dog) to a less derived type (Animal)
    public interface IReadRepository<out T> where T : Animal
    {
        IReadOnlyList<T> GetAnimals();
    }
}
