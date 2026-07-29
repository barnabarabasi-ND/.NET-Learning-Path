using Ch6.Generics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ch6.Generics.Interfaces
{
    //variant generic interface, with contravariant type parameter "in T", allows assignment of a less derived type (Animal) to a more derived type (Dog)
    public interface IWriteRepository<in T> where T : Animal
    {
        void AddAnimal(T animal);
    }
}
